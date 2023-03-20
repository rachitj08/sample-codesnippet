using Common.Model;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentService;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.EmailHelper;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class PaymentIntentService : IPaymentIntentService
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IPaymentDetailsRepository _paymentDetailsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly ISMSHelper _smsHelper;
        private readonly PaymentConfig _paymentConfig;
        private readonly SMSConfig _smsConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="paymentServices"></param>
        public PaymentIntentService(IPaymentServices paymentServices, IPaymentDetailsRepository paymentDetailsRepository,
            IUserRepository userRepository, IEmailHelper emailHelper, ISMSHelper smsHelper,
            IOptions<PaymentConfig> paymentConfig, IOptions<SMSConfig> smsConfig)
        {
            Check.Argument.IsNotNull(nameof(paymentServices), paymentServices);
            Check.Argument.IsNotNull(nameof(paymentDetailsRepository), paymentDetailsRepository);
            Check.Argument.IsNotNull(nameof(userRepository), userRepository);

            Check.Argument.IsNotNull(nameof(emailHelper), emailHelper);
            Check.Argument.IsNotNull(nameof(smsHelper), smsHelper);

            Check.Argument.IsNotNull(nameof(paymentConfig), paymentConfig);
            Check.Argument.IsNotNull(nameof(smsConfig), smsConfig);

            _paymentServices = paymentServices;
            _paymentDetailsRepository = paymentDetailsRepository;
            _userRepository = userRepository;

            _emailHelper = emailHelper;
            _smsHelper = smsHelper;

            _paymentConfig = paymentConfig.Value;
            _smsConfig = smsConfig.Value;
        }

        /// <summary>
        /// Create Payment Intent
        /// </summary>
        /// <param name="modelData">Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100)</param>
        public async Task CreatePaymentIntent(string modelData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelData))
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }

                var model = JsonConvert.DeserializeObject<CreatePaymentIntentRequest>(modelData);
                if (model == null || model.ReservationId < 1 || model.Amount < 1 || model.UserId < 1)
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }

                var user = await _userRepository.GetUserDetails(model.UserId, model.AccountId);
                if (user == null)
                {
                    Console.WriteLine("Invalid user");
                    return;
                }

                var userFirstName = user.FirstName;
                var emailAddress = user.EmailAddress;
                var userMobileNo = (!string.IsNullOrWhiteSpace(user.MobileCode) ? user.MobileCode : "+1") + user.Mobile;

                if (!string.IsNullOrWhiteSpace(user.StripeCustomerId))
                {
                    var customerId = user.StripeCustomerId;
                    var paymentMethods = _paymentServices.GetPaymentMethodList(customerId, _paymentConfig.IsTestStripe);
                    if (paymentMethods != null && paymentMethods.Data.Count > 0)
                    {
                        var paymentDescription = user.FirstName + " " + model.ReservationId;
                        var finalAmount = model.Amount + _paymentConfig.MarginAmountForPaymentIntent;
                        long applicationFeeAmount = CalculateApplicationFeeAmount(finalAmount);

                        var paymentMethod = paymentMethods.Data.FirstOrDefault();
                        var paymentMethodId = paymentMethod.Id;
                        var connectedAccountId = "";
                        var result = await _paymentServices.CreatePaymentIntentAsync(finalAmount, applicationFeeAmount, emailAddress, customerId, paymentDescription, paymentMethodId, connectedAccountId, _paymentConfig.ApplicationName, _paymentConfig.IsTestStripe);
                        if (result != null)
                        {
                            if (_paymentServices.ConfirmPaymentIntent(result.Id, _paymentConfig.IsTestStripe) != null)
                            {
                                var paymentDetails = new PaymentDetails()
                                {
                                    AccountId = model.AccountId,
                                    IntentAmount = model.Amount,
                                    ApplicationFeeAmount = model.Amount,
                                    ReservationId = model.ReservationId,
                                    PaymentIntentId = result.Id,
                                    PaymentMethodId = result.PaymentMethodId,
                                    ReceiptEmail = result.ReceiptEmail,
                                    CustomerId = result.CustomerId,
                                    MarginAmount = _paymentConfig.MarginAmountForPaymentIntent,
                                    ConnectedAccountId = connectedAccountId
                                };

                                var saveResult = await _paymentDetailsRepository.CreatePaymentDetails(paymentDetails, model.AccountId, model.UserId);
                                if (saveResult > 0) return;
                                else Console.WriteLine("Could not able to save payment details.");
                            }
                            else
                            {
                                Console.WriteLine("Could not able to Confirm Payment Intent.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Could not able to Create Payment Intent.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("User does not have any payment method.");
                    }
                }
                else
                {
                    Console.WriteLine("Stripe Customer Id is blank.");
                }

                #region [Send Email and SMS to user]
                var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
                // Send SMS for Failed case
                if (!string.IsNullOrWhiteSpace(userMobileNo))
                {
                    var smsFilePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "PaymentIntentFailed.html");
                    var smsFileInfo = provider.GetFileInfo(smsFilePath);
                    if (smsFileInfo.Exists)
                    {
                        var smsText = "";
                        using (var fs = smsFileInfo.CreateReadStream())
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                smsText = sr.ReadToEnd();
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(smsText))
                        {
                            smsText = smsText.Replace("##UserName##", userFirstName).Replace("##ReservationCode##", model.ReservationCode);
                            if (!_smsHelper.SendSMS(_smsConfig, userMobileNo, smsText))
                            {
                                Console.WriteLine("Could not able to send SMS to customer.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Payment intent failed sms template file is blank.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Payment intent failed sms template file is missing.");
                    }
                }

                // Send Email for Failed case
                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    Console.WriteLine("Email Address is blank");
                    return;
                }

                // Get Email Template
                var filePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "PaymentIntentFailed.html");
                var fileInfo = provider.GetFileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    Console.WriteLine("Payment intent failed email template file is missing.");
                    return;
                }

                var bodyHTML = "";
                using (var fs = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        bodyHTML = sr.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(bodyHTML))
                {
                    Console.WriteLine("Payment intent failed email template file is blank.");
                    return;
                }

                bodyHTML = bodyHTML.Replace("##UserName##", userFirstName).Replace("##ReservationCode##", model.ReservationCode).Replace("##FinalAmount##", model.Amount.ToString("00.00"));
                if (!_emailHelper.SendMail(emailAddress, "Sample Car:Payment", bodyHTML))
                {
                    Console.WriteLine("Could not able to send Email to customer.");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message:- {ex.Message};");
                Console.WriteLine($"InnerException:- {ex.InnerException};");
                Console.WriteLine($"StackTrace:- {ex.StackTrace};");
            }
        }

        /// <summary>
        /// Cancel Payment Intent
        /// </summary>
        /// <param name="modelData"></param>
        public void CancelPaymentIntent(string modelData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelData))
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }

                var model = JsonConvert.DeserializeObject<CancelPaymentIntentRequest>(modelData);

                if (model == null || string.IsNullOrWhiteSpace(model.PaymentIntentId))
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }
                _paymentServices.CancelPaymentIntent(model.PaymentIntentId, _paymentConfig.IsTestStripe);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message:- {ex.Message};");
                Console.WriteLine($"InnerException:- {ex.InnerException};");
                Console.WriteLine($"StackTrace:- {ex.StackTrace};");
            }
        }


        /// <summary>
        /// Update Payment Intent
        /// </summary>
        /// <param name="modelData">Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100)</param>
        public async Task UpdatePaymentIntent(string modelData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelData))
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }

                var model = JsonConvert.DeserializeObject<UpdatePaymentIntentRequest>(modelData);

                if (model == null || string.IsNullOrWhiteSpace(model.PaymentIntentId))
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }

                long finalAmount = model.Amount + _paymentConfig.MarginAmountForPaymentIntent;
                long applicationFeeAmount = CalculateApplicationFeeAmount(finalAmount);
                var result = _paymentServices.UpdatePaymentIntent(finalAmount, applicationFeeAmount, model.EmailAddress, model.PaymentIntentId, _paymentConfig.IsTestStripe);
                if (result != null)
                {
                    var paymentDetails = new PaymentDetails()
                    {
                        IntentAmount = model.Amount,
                        ApplicationFeeAmount = applicationFeeAmount,
                        PaymentMethodId = result.PaymentMethodId,
                        ReceiptEmail = result.ReceiptEmail,
                        CustomerId = result.CustomerId,
                        PaymentDetailId = model.PaymentDetailId,
                        MarginAmount = _paymentConfig.MarginAmountForPaymentIntent
                    };
                    var saveResult = await _paymentDetailsRepository.UpdatePaymentDetails(paymentDetails, model.AccountId, model.UserId);
                    if (saveResult > 0) return;
                    else Console.WriteLine("Could not able to save payment details.");
                }
                else
                {
                    Console.WriteLine("Could not able to update Payment intent.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message:- {ex.Message};");
                Console.WriteLine($"InnerException:- {ex.InnerException};");
                Console.WriteLine($"StackTrace:- {ex.StackTrace};");
            }
        }

        /// <summary>
        /// Capture final amount for the placed order
        /// </summary>
        /// <param name="modelData">Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100)</param>
        public async Task CapturePaymentIntent(string modelData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelData))
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }

                var model = JsonConvert.DeserializeObject<CapturePaymentIntentRequest>(modelData);
                if (model == null || string.IsNullOrWhiteSpace(model.PaymentIntentId) || model.Amount < 1)
                {
                    Console.WriteLine("Invalid Request");
                    return;
                }
                long applicationFeeAmount = CalculateApplicationFeeAmount(model.Amount);


                var result = _paymentServices.CapturePaymentIntent(model.PaymentIntentId, model.Amount, applicationFeeAmount, _paymentConfig.IsTestStripe);

                if (result != null)
                {

                    var paymentDetails = new PaymentDetails()
                    {
                        IntentAmount = -1,
                        FinalAmount = model.Amount,
                        ApplicationFeeAmount = model.Amount,
                        PaymentDate = DateTime.UtcNow,
                        PaymentMethodId = result.PaymentMethodId,
                        ReceiptEmail = result.ReceiptEmail,
                        CustomerId = result.CustomerId,
                        PaymentDetailId = model.PaymentDetailId
                    };
                    var saveResult = await _paymentDetailsRepository.UpdatePaymentDetails(paymentDetails, model.AccountId, model.UserId);
                    if (saveResult > 0) return;
                    else Console.WriteLine("Could not able to save payment details.");
                }
                else
                {
                    Console.WriteLine("Could not able to Capture Final Payment.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message:- {ex.Message};");
                Console.WriteLine($"InnerException:- {ex.InnerException};");
                Console.WriteLine($"StackTrace:- {ex.StackTrace};");
            }

        }

        private long CalculateApplicationFeeAmount(long amount)
        {
            long applicationFeeAmount = 0;
            if (_paymentConfig.ApplicationFeePercentage > 0) applicationFeeAmount = Convert.ToInt64((amount * _paymentConfig.ApplicationFeePercentage) / 100);

            return applicationFeeAmount;

        }
    }


}
