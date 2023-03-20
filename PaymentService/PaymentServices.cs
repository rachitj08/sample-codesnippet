using PaymentService.Card;
using PaymentService.Customer;
using PaymentService.DisputeModel;
using PaymentService.RefundModel;
using Stripe;
using Stripe.Terminal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService
{
    public class PaymentServices : IPaymentServices
    {
        private readonly PaymentConfig _Config;
        public PaymentServices(PaymentConfig Config)
        {
            _Config = Config;
            StripeConfiguration.ApiKey = _Config.SecretKey;
            //StripeConfiguration.MaxNetworkRetries = 2;
        }

        #region [ConnectedAccount]

        /// <summary>
        /// Create new connected account only with provided email ID
        /// </summary>
        /// <returns></returns>
        public Account CreateConnectedAccount(string email, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var options = new AccountCreateOptions
                {
                    Type = "express",// Required
                    Country = "US", //Required
                    Email = email, //Required
                    RequestedCapabilities = new List<string>//Required
                    {
                        "card_payments",
                        "transfers",
                    },
                };

                var service = new AccountService();
                var response = service.Create(options);

                return response;
            }
            catch (StripeException ex)
            {
                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);

            }

        }

        /// <summary>
        /// Setup connected account with full details
        /// </summary>
        /// <param name="email"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Account SetupConnectedAccount(string email, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var options = new AccountCreateOptions
                {
                    Type = "express",// Required
                    Country = "US", //Required
                    Email = email, //Required
                    RequestedCapabilities = new List<string>//Required
                    {
                        "card_payments",
                        "transfers",
                    },
                    BusinessType = "", //Required
                    DefaultCurrency = "", //Required
                    Individual = new PersonCreateOptions
                    {
                        Address = new AddressOptions
                        { //Required
                            Line1 = "",
                            Line2 = "",
                            City = "",
                            State = "",
                            Country = "",
                            PostalCode = ""
                        },
                        Email = email,//Required
                        Dob = new DobOptions //Required
                        {
                            Day = 2,
                            Month = 2,
                            Year = 2020
                        },
                        FirstName = "", //Required
                        LastName = "", //Required
                        Gender = "",
                        MaidenName = "",
                        Relationship = new PersonRelationshipOptions
                        {//Required
                            Title = "",
                            Director = false,

                        },
                        Verification = new PersonVerificationOptions
                        { //Required
                            AdditionalDocument = new PersonVerificationDocumentOptions
                            {
                                Back = "",
                                Front = ""
                            },
                            Document = new PersonVerificationDocumentOptions
                            {
                                Back = "",
                                Front = ""
                            }
                        },
                        Phone = "", //Required
                        SSNLast4 = "", //Required
                        IdNumber = "",


                    },

                    BusinessProfile = new AccountBusinessProfileOptions //Required
                    {
                        Mcc = "",
                        Name = "Tejveer",
                        ProductDescription = "Test Product",
                        SupportEmail = "",
                        SupportPhone = "",
                        Url = "",
                        SupportUrl = "",
                        PrimaryColor = ""
                    },
                    Company = new AccountCompanyOptions //Required
                    {
                        Address = new AddressOptions
                        {
                            Country = "US",
                            Line1 = "Line1",
                            Line2 = "Line2",
                            City = "City",
                            State = "",
                            PostalCode = ""

                        },
                        Name = "",
                        Phone = "",
                        TaxId = "",
                        VatId = "",
                        OwnersProvided = false,
                        DirectorsProvided = false,
                        Structure = "",
                        Verification = new AccountCompanyVerificationOptions
                        {
                            Document = new AccountCompanyVerificationDocumentOptions
                            {

                            }
                        }
                    },
                    ExternalAccount = new AccountCardOptions //Required
                    {
                        Name = "Test",
                        Number = "",
                        ExpMonth = 02,
                        ExpYear = 2020,
                        Cvc = "123",
                        AddressLine1 = "",
                        AddressLine2 = "",
                        AddressCity = "",
                        AddressState = "",
                        AddressZip = "",
                        AddressCountry = ""
                    },
                    //ExternalAccount= new AccountBankAccountOptions
                    //{
                    //    AccountHolderName="",
                    //    AccountHolderType="",
                    //    AccountNumber="",
                    //    Country="",
                    //    Currency="",
                    //    RoutingNumber=""
                    //},
                    TosAcceptance = new AccountTosAcceptanceOptions //Required
                    {
                        Date = System.DateTime.UtcNow,
                        Ip = "",//Required
                        UserAgent = ""//Required
                    },
                    Settings = new AccountSettingsOptions
                    {
                        Payments = new AccountSettingsPaymentsOptions
                        {

                        },
                        Dashboard = new AccountSettingsDashboardOptions
                        {

                        },
                        CardPayments = new AccountSettingsCardPaymentsOptions
                        {

                        },
                        Payouts = new AccountSettingsPayoutsOptions
                        {

                        },
                        Branding = new AccountSettingsBrandingOptions
                        {
                            Icon = "",
                            Logo = "",
                            PrimaryColor = ""
                        }
                    },

                    Metadata = new Dictionary<string, string>
                    {

                    },

                };

                var service = new AccountService();
                var response = service.Create(options);

                return response;
            }
            catch (StripeException ex)
            {
                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);

            }

        }

        /// <summary>
        /// Fetch Connected Account Details
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Account GetConnectedAccountDetails(string connectedAccountId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var service = new AccountService();

                var response = service.Get(connectedAccountId);

                return response;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Fetch List of Connected Accounts 
        /// </summary>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public StripeList<Account> GetConnectedAccountList(bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new AccountListOptions
                {
                    Limit = 3,
                };
                var service = new AccountService();
                StripeList<Account> accounts = service.List(
                  options
                );

                return accounts;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get pending required information that will cause payment failure
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <param name="baseUrl"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public AccountLink GetConnectedAccountPendingInformation(string connectedAccountId, string baseUrl, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            //"Collect=currently_due : requirements request only the user information needed for verification"
            //"Collect=eventually_due: requirements include a more complete set of questions that we’ll eventually need to collect"
            //"Type=custom_account_update: Use it when you’re onboarding a new user, or when an existing user has new requirements; such as when a user had already provided enough information, but you requested a new capability that needs additional info"
            //"Type=custom_account_update: displays the fields that are already populated on the account object, and allows your user to edit previously provided information. Provide an access point in your platform’s dashboard to a type=custom_account_update Account Link, for users to initiate updates themselves (e.g., when their address changes). Consider framing this as “edit my profile” or “update my verification information"

            try
            {
                var options = new AccountLinkCreateOptions
                {
                    Account = connectedAccountId,
                    FailureUrl = baseUrl + "connect/failure",
                    SuccessUrl = baseUrl + "connect/oauth",
                    Type = "custom_account_update",
                    Collect = "eventually_due",

                };
                var service = new AccountLinkService();
                var accountLink = service.Create(options);

                return accountLink;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Authorize created Connected Account
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public OAuthToken AuthorizeConnectedAccount(string code, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                string connected_account_id = string.Empty;

                var options = new OAuthTokenCreateOptions
                {
                    GrantType = "authorization_code",
                    Code = code,
                    AssertCapabilities = new List<string>
                {
                "card_payments",
                "transfers",
                }

                };

                var service = new OAuthTokenService();
                var response = service.Create(options);

                // Access the connected account id in the response
                connected_account_id = response.StripeUserId;

                return response;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Update details of connected account
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <param name="clientIPAddress"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Account UpdateConnectedAccount(string connectedAccountId, string clientIPAddress, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var options = new AccountUpdateOptions
                {

                    RequestedCapabilities = new List<string>
                {
                "card_payments",
                "transfers",
                },
                    //Metadata = new Dictionary<string, string>
                    //  {
                    //    { "internal_id", "42" },
                    //  },
                };
                var service = new AccountService();
                var response = service.Update(connectedAccountId, options);

                return response;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <param name="email"></param>
        /// <param name="isTestBusiness"></param>
        /// <returns></returns>
        public Account UpdateConnectedAccountEmail(string connectedAccountId, string email, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var options = new AccountUpdateOptions
                {
                    Email = email                   
                };
                var service = new AccountService();
                var response = service.Update(connectedAccountId, options);

                return response;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }


        }
        /// <summary>
        /// DeAuthorize created Connected Account
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public OAuthDeauthorize DeAuthorizeConnectedAccount(string connectedAccountId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                string connected_account_id = string.Empty;

                var options = new OAuthDeauthorizeOptions
                {
                    ClientId = _Config.ClientId,
                    StripeUserId = connectedAccountId,
                };

                var service = new OAuthTokenService();
                var response = service.Deauthorize(options);

                // Access the connected account id in the response
                connected_account_id = response.StripeUserId;

                return response;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }


        }

        /// <summary>
        /// Create a login link for Express Connected Account only
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <returns></returns>
        public LoginLink CreateConnectedAccountLoginLink(string connectedAccountId, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var service = new LoginLinkService();
                var loginLink = service.Create(connectedAccountId);

                return loginLink;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get link to connected account
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <param name="baseUrl"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public AccountLink CreateConnectedAccountLink(string connectedAccountId, string baseUrl, string currentUrl, string seoName, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            //"Collect=currently_due : requirements request only the user information needed for verification"
            //"Collect=eventually_due: requirements include a more complete set of questions that we’ll eventually need to collect"
            //"Type=custom_account_update: Use it when you’re onboarding a new user, or when an existing user has new requirements; such as when a user had already provided enough information, but you requested a new capability that needs additional info"
            //"Type=custom_account_update: displays the fields that are already populated on the account object, and allows your user to edit previously provided information. Provide an access point in your platform’s dashboard to a type=custom_account_update Account Link, for users to initiate updates themselves (e.g., when their address changes). Consider framing this as “edit my profile” or “update my verification information"
            // account_update: Displays the fields that are already populated on the account object, and allows your user to edit previously provided information. Consider framing this as “edit my profile” or “update my verification information”.
            try
            {
                if (string.IsNullOrEmpty(currentUrl))
                    currentUrl = baseUrl + "connect/success/" + seoName;
                else
                    currentUrl = baseUrl + "onboarding/success/" + seoName;

                var options = new AccountLinkCreateOptions
                {
                    Account = connectedAccountId,
                    FailureUrl = baseUrl + "connect/failure/" + seoName,
                    //SuccessUrl = baseUrl + "connect/success/"+ seoName,
                    SuccessUrl = currentUrl,
                    Type = "account_onboarding",
                    //Collect = "currently_due",

                };
                var service = new AccountLinkService();
                var accountLink = service.Create(options);

                return accountLink;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Reject already created connected account 
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <returns></returns>
        public Account RejectConnectedAccount(string connectedAccountId, string reason, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new AccountRejectOptions
                {
                    //Reason = "fraud",
                    Reason = reason,
                };
                var service = new AccountService();
                var account = service.Reject(connectedAccountId, options);

                return account;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        #endregion

        #region [Customer]

        /// <summary>
        /// Register customer with stripe payment gateway
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Stripe.Customer CreateCustomer(CustomerRequest customerRequest, bool isTestBusiness = false)
        {
            if (isTestBusiness || _Config.IsTestStripe)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var address = new AddressOptions
                {
                    City = customerRequest.City,
                    Line1 = customerRequest.Line1,
                    Line2 = customerRequest.Line2,
                    State = customerRequest.State,
                    PostalCode = customerRequest.PostalCode,
                    Country = customerRequest.Country
                };

                Dictionary<string, string> metdata = new Dictionary<string, string>();


                var options = new CustomerCreateOptions
                {
                    Name = customerRequest.Name,
                    Address = address,
                    Metadata = metdata,
                    Email = customerRequest.Email,
                    Phone = customerRequest.Phone,
                    Description = "A aBitNow Customer " + customerRequest.Name,
                    //PaymentMethod = "card"
                };
                var service = new CustomerService();
                var customer = service.Create(options);

                return customer;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get customer details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Stripe.Customer GetCustomer(string customerId, bool isTestBusiness = false)
        {
            if (isTestBusiness || _Config.IsTestStripe)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var service = new CustomerService();
                var customer = service.Get(customerId);

                return customer;
            }
            catch (StripeException ex)
            {
                throw Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Update customer details
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerRequest"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Stripe.Customer UpdateCustomer(string customerId, CustomerRequest customerRequest, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var address = new AddressOptions
                {
                    City = customerRequest.City,
                    Line1 = customerRequest.Line1,
                    Line2 = customerRequest.Line2,
                    State = customerRequest.State,
                    PostalCode = customerRequest.PostalCode,
                    Country = customerRequest.Country
                };

                Dictionary<string, string> metdata = new Dictionary<string, string>();

                var options = new CustomerUpdateOptions
                {
                    Name = customerRequest.Name,
                    Address = address,
                    Metadata = metdata,
                    Email = customerRequest.Email,
                    Phone = customerRequest.Phone,
                    Description = "A aBitNow Customer " + customerRequest.Name,
                };
                var service = new CustomerService();
                var customer = service.Update(customerId, options);

                return customer;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }


        }

        /// <summary>
        /// Set Customers Default Payment Method
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Stripe.Customer SetCustomersDefaultPaymentMethod(string customerId, string paymentMethodId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {

                var options = new CustomerUpdateOptions
                {
                    DefaultSource = paymentMethodId,
                };
                var service = new CustomerService();
                var customer = service.Update(customerId, options);

                return customer;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }


        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool DeleteCustomer(string customerId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var service = new CustomerService();
                var customer = service.Delete(customerId);

                return Convert.ToBoolean(customer.Deleted);
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        #endregion

        #region [PaymentMethod]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardDetailsModel"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public PaymentMethod CreatePaymentMethod(CardRequest cardDetailsModel, bool isTestBusiness = false)
        {
            if (isTestBusiness || _Config.IsTestStripe)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            
            try
            {
                Dictionary<string, string> metdata = new Dictionary<string, string>();
                metdata.Add("CardPersonalName", cardDetailsModel.CardPersonalName);
                metdata.Add("CardHolderName", cardDetailsModel.CardHolderName);
                var options = new PaymentMethodCreateOptions
                {
                    
                    Metadata = metdata,
                    Type = "card",
                    Card = new PaymentMethodCardCreateOptions
                    {
                        Number = cardDetailsModel.CardNumber,
                        ExpMonth = cardDetailsModel.ExpirationMonth,
                        ExpYear = cardDetailsModel.ExpirationYear,
                        Cvc = cardDetailsModel.CVVNumber,
                        
                    }, 

                };
                var service = new PaymentMethodService();
                var cardResult = service.Create(options);

                return cardResult;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }
        }

        /// <summary>
        /// get list of all payment 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public StripeList<PaymentMethod> GetPaymentMethodList(string customerId, bool isTestBusiness = false)
        {
            if (isTestBusiness || _Config.IsTestStripe)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var options = new PaymentMethodListOptions
                {
                    Customer = customerId,
                    Type = "card",
                };
                var service = new PaymentMethodService();
                var cardResult = service.List(options);

                return cardResult;
            }
            catch (StripeException ex)
            {
                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);

            }

        }


        /// <summary>
        /// get details of payment method
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public PaymentMethod GetPaymentMethod(string paymentMethodId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var service = new PaymentMethodService();
                var cardResult = service.Get(paymentMethodId);
                return cardResult;
            }
            catch (StripeException ex)
            {
                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }
        }


        /// <summary>
        /// Update payment method details
        /// </summary>
        /// <param name="cardDetailsModel"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public PaymentMethod UpdatePaymentMethod(CardRequest cardDetailsModel, string paymentMethodId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var options = new PaymentMethodUpdateOptions
                {
                    Metadata = new Dictionary<string, string>
                              {
                                { "order_id", "6735" },
                              },
                };
                var service = new PaymentMethodService();
                var cardResult = service.Update(paymentMethodId, options);

                return cardResult;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }
        }

        /// <summary>
        /// Attach a registered payment method.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public PaymentMethod AttachPaymentMethod(string customerId, string paymentMethodId, bool isTestBusiness = false)
        {
            if (isTestBusiness || _Config.IsTestStripe)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new PaymentMethodAttachOptions
                {
                    Customer = customerId,
                };
                var service = new PaymentMethodService();
                var resultCard = service.Attach(paymentMethodId, options);

                return resultCard;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }


        }

        /// <summary>
        /// Attach Payment method with connected account
        /// </summary>
        /// <param name="connectedAccountId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public PaymentMethod AttachPaymentMethodWithConnectedAccount(string connectedAccountId, string paymentMethodId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new PaymentMethodCreateOptions
                {
                    //Customer = "cus_HmdxypMRgnuHRG",
                    PaymentMethod = paymentMethodId,
                };
                var requestOptions = new RequestOptions
                {
                    StripeAccount = connectedAccountId,
                };

                var service = new PaymentMethodService();
                var paymentMethod = service.Create(options, requestOptions);

                return paymentMethod;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }


        }

        /// <summary>
        /// detach a payment method
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public PaymentMethod DetachPaymentMethod(string paymentMethodId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var service = new PaymentMethodService();
                var resultCard = service.Detach(paymentMethodId);
                return resultCard;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        #endregion

        #region [Cards]

        /// <summary>
        /// Save card for a customer
        /// </summary>
        /// <param name="cardDetailsModel"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string SaveCard(CardRequest cardDetailsModel, string customerId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                Dictionary<string, string> metdata = new Dictionary<string, string>();
                metdata.Add("Description", "card Desc");

                var options = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {
                        Number = cardDetailsModel.CardNumber,
                        ExpYear = cardDetailsModel.ExpirationYear,
                        ExpMonth = cardDetailsModel.ExpirationMonth,
                        Cvc = cardDetailsModel.CVVNumber,
                        Metadata = metdata,
                    }
                };

                var service = new TokenService();
                Token stripeToken = service.Create(options);
                var serviceRegister = new CardService();
                if (!string.IsNullOrEmpty(stripeToken.Id))
                {
                    var option = new CardCreateOptions
                    {

                        Source = stripeToken.Id,

                    };

                    serviceRegister.Create(customerId, option);
                }
                return stripeToken.Id;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get card deatils for customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cardTokenId"></param>
        /// <returns></returns>
        public Stripe.Card GetCardDetails(string customerId, string cardTokenId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var service = new CardService();
                var card = service.Get(customerId, cardTokenId);

                return card;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get list of cards of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<Stripe.Card> GetListofCards(string customerId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var service = new CardService();
                var options = new CardListOptions
                {
                    Limit = 5,
                };
                var cards = service.List(customerId, options);
                return cards.Data;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Update card details of a customer
        /// </summary>
        /// <param name="cardDetailsModel"></param>
        /// <param name="customerId"></param>
        /// <param name="cardTokenId"></param>
        /// <returns></returns>
        public Stripe.Card UpdateCardDetails(CardRequest cardDetailsModel, string customerId, string cardTokenId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var options = new CardUpdateOptions
                {
                    ExpYear = cardDetailsModel.ExpirationYear,
                    ExpMonth = cardDetailsModel.ExpirationMonth,
                };
                var service = new CardService();
                var card = service.Update(customerId, cardTokenId, options);

                return card;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Delete card of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cardTokenId"></param>
        /// <returns></returns>
        public bool DeleteCard(string customerId, string cardTokenId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var service = new CardService();
                var card = service.Delete(customerId, cardTokenId);

                return Convert.ToBoolean(card.Deleted);
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }


        /// <summary>
        /// Validate card details before saving into application database
        /// </summary>
        /// <param name="cardDetailsModel"></param>
        /// <returns></returns>
        public CardValidationResponse ValidateCardDetails(CardRequest cardDetailsModel, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            CardValidationResponse cardStatus = new CardValidationResponse();
            cardStatus.IsSuccess = false;


            try
            {
                var options = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {

                        Number = cardDetailsModel.CardNumber,
                        ExpYear = cardDetailsModel.ExpirationYear,
                        ExpMonth = cardDetailsModel.ExpirationMonth,
                        Cvc = cardDetailsModel.CVVNumber
                    }
                };

                var service = new TokenService();
                Token stripeToken = service.Create(options);
                if (!string.IsNullOrEmpty(stripeToken.Id))
                {
                    cardStatus.IsSuccess = true;
                    cardStatus.Message = "Card Verified Successfully";
                    cardStatus.CardStripeVerificationToken = stripeToken.Id;
                    cardStatus.CardStripeId = stripeToken.Card.Id;
                    cardStatus.CustomerId = stripeToken.Card.CustomerId;
                    cardStatus.Exp_Year = Convert.ToInt32(stripeToken.Card.ExpYear);
                    cardStatus.Exp_Month = Convert.ToInt32(stripeToken.Card.ExpMonth);
                    cardStatus.Country = stripeToken.Card.Country;
                    cardStatus.Brand = stripeToken.Card.Brand;
                }
            }
            catch (StripeException ex)
            {
                cardStatus.Message = ex.Message;
                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

            return cardStatus;

        }

        #endregion

        #region [Payment]

        public PaymentIntent GetPaymentIntent(string paymentIntentId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var service = new PaymentIntentService();
                var intent = service.Get(paymentIntentId);
                return intent;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }
        /// <summary>
        /// Pre authrize an amount before making payment 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public PaymentIntent CreatePaymentIntent(long amount, string receiptEmail, string customerId, string customerName, string[] orderProducts, string paymentMethodId, string ConnectedAccountId, string restaurantSeoFriendlyName, long restaurantApplicationFeeAmount, bool isTestBusiness = false)
        {
            return CreatePaymentIntentAsync(amount, receiptEmail, customerId, customerName, orderProducts, paymentMethodId, ConnectedAccountId, restaurantSeoFriendlyName, restaurantApplicationFeeAmount, isTestBusiness).GetAwaiter().GetResult();
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(long amount, string receiptEmail, string customerId, string customerName, string[] orderProducts, string paymentMethod, string ConnectedAccountId, string restaurantSeoFriendlyName, long restaurantApplicationFeeAmount, bool isTestBusiness = false)
        {
            try
            {
                if (isTestBusiness)
                    StripeConfiguration.ApiKey = _Config.TestSecretKey;

                // Set your secret key. Remember to switch to your live secret key in production!
                // See your keys here: https://dashboard.stripe.com/account/apikeys
                //StripeConfiguration.ApiKey = _Config.SecretKey;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                var products = string.Join(',', orderProducts);

                var service = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    PaymentMethod = paymentMethod,
                    Customer = customerId,
                    //ReceiptEmail = receiptEmail,
                    Amount = amount,
                    Currency = "USD",
                    ConfirmationMethod = "manual",
                    CaptureMethod = "manual",
                    StatementDescriptorSuffix = "-" + restaurantSeoFriendlyName,
                    Metadata = metadata,
                    Description = customerName + '-' + products.Substring(0, products.Length > 255 ? 255 : products.Length),
                    ApplicationFeeAmount = restaurantApplicationFeeAmount,
                    TransferData = new PaymentIntentTransferDataOptions
                    {
                        Destination = ConnectedAccountId, //"acct_1Gdd7RFIp97ed87r",
                    },
                };
                var intent = await service.CreateAsync(options);

                return intent;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(long amount, long applicationFeeAmount, string receiptEmail, string customerId, string description, string paymentMethod, string connectedAccountId, string appplicationName, bool isTestMode = false)
        {
            try
            {
                if (isTestMode)
                    StripeConfiguration.ApiKey = _Config.TestSecretKey;

                // Set your secret key. Remember to switch to your live secret key in production!
                // See your keys here: https://dashboard.stripe.com/account/apikeys
                //StripeConfiguration.ApiKey = _Config.SecretKey;
                Dictionary<string, string> metadata = new Dictionary<string, string>();

                var service = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    PaymentMethod = paymentMethod,
                    Customer = customerId,
                    Amount = amount,
                    ReceiptEmail = receiptEmail,
                    //ApplicationFeeAmount = applicationFeeAmount,
                    Currency = "USD",
                    ConfirmationMethod = "manual",
                    CaptureMethod = "manual",
                    StatementDescriptorSuffix = "-" + appplicationName,
                    Metadata = metadata,
                    Description = description,
                    //TransferData = new PaymentIntentTransferDataOptions
                    //{
                    //    Destination = connectedAccountId,
                    //},
                };
                return await service.CreateAsync(options);
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestMode);
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentIntentId"></param>
        /// <returns></returns>
        public PaymentIntent ConfirmPaymentIntent(string paymentIntentId, bool isTestBusiness = false)
        {
            try
            {
                if (isTestBusiness)
                    StripeConfiguration.ApiKey = _Config.TestSecretKey;

                var service = new PaymentIntentService();
                var confirmOptions = new PaymentIntentConfirmOptions
                {
                };
                var paymentIntent = service.Confirm(paymentIntentId, confirmOptions);

                return paymentIntent;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }
        }

        /// <summary>
        /// Update payment intent data
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="paymentIntentId"></param>
        /// <returns></returns>
        public PaymentIntent UpdatePaymentIntent(long amount, long applicationFeeAmount, string receiptEmail, string paymentIntentId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new PaymentIntentUpdateOptions
                {
                    ReceiptEmail = receiptEmail,
                    Amount = amount,
                   // ApplicationFeeAmount = applicationFeeAmount
                };
                var service = new PaymentIntentService();
                var paymentIntent = service.Update(paymentIntentId, options);

                return paymentIntent;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }
        }

        /// <summary>
        /// Cancel Payment Intent
        /// </summary>
        /// <param name="paymentIntentId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public PaymentIntent CancelPaymentIntent(string paymentIntentId, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                // To create a PaymentIntent, see our guide at: https://stripe.com/docs/payments/payment-intents/creating-payment-intents#creating-for-automatic
                var service = new PaymentIntentService();
                var paymentIntent = service.Cancel(paymentIntentId);

                string status = paymentIntent.Status;
                return paymentIntent;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Make payment for pre authrize amount based generated intentId
        /// </summary>
        /// <param name="intentId"></param>
        /// <param name="amount">amount can be less or equal to pre approved ammount</param>
        /// <returns></returns>
        public PaymentIntent CapturePaymentIntent(string intentId, long amount, long applicationFeeAmount, bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                // Set your secret key. Remember to switch to your live secret key in production!
                // See your keys here: https://dashboard.stripe.com/account/apikeys
                //StripeConfiguration.ApiKey = _Config.SecretKey;

                var service = new PaymentIntentService();
                var options = new PaymentIntentCaptureOptions
                {
                    AmountToCapture = amount,
                   // ApplicationFeeAmount = applicationFeeAmount
                };
                var intent = service.Capture(intentId, options);

                //return intent.Id;
                return intent;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get list of all avialbale intents.
        /// </summary>
        /// <returns></returns>
        public List<PaymentIntent> GetListOfPaymentIntents(bool isTestBusiness = false)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new PaymentIntentListOptions
                {
                    Limit = 5,
                };
                var service = new PaymentIntentService();
                var paymentIntents = service.List(options);

                return paymentIntents.Data;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Make payment without customer registeration/ Card saving / pre approval of amount on Stripe
        /// </summary>
        /// <param name="stripeInfo"></param>
        /// <returns></returns>
        public PaymentStatus DirectPayment(StripeInfoModel stripeInfo, bool isTestBusiness = false)
        {
            try
            {
                if (isTestBusiness)
                    StripeConfiguration.ApiKey = _Config.TestSecretKey;

                PaymentStatus stripePaymentStatus = new PaymentStatus();
                stripePaymentStatus.Status = "failed";

                if (stripeInfo != null)
                {
                    Stripe.CreditCardOptions card = new Stripe.CreditCardOptions();
                    //card.Name = tParams.CardOwnerFirstName + " " + tParams.CardOwnerLastName;
                    card.Name = stripeInfo.CardHolderName;
                    card.Number = stripeInfo.CardNumber;
                    card.ExpYear = stripeInfo.ExpirationYear;
                    card.ExpMonth = stripeInfo.ExpirationMonth;
                    card.Cvc = Convert.ToString(stripeInfo.CVVNumber);

                    //Assign Card to Token Object and create Token  
                    Stripe.TokenCreateOptions token = new Stripe.TokenCreateOptions();
                    token.Card = card;
                    Stripe.TokenService serviceToken = new Stripe.TokenService();
                    Stripe.Token newToken = serviceToken.Create(token);

                    //Create Customer Object and Register it on Stripe
                    Stripe.CustomerCreateOptions myCustomer = new Stripe.CustomerCreateOptions();
                    myCustomer.Email = stripeInfo.CardHolderEmail;
                    myCustomer.Source = newToken.Id;
                    var customerService = new Stripe.CustomerService();
                    Stripe.Customer stripeCustomer = customerService.Create(myCustomer);

                    var transferData = new ChargeTransferDataOptions
                    {
                        Destination = stripeInfo.RestaurantConnectedAccountId,
                    };

                    //Create Charge Object with details of Charge  
                    var options = new Stripe.ChargeCreateOptions
                    {
                        Amount = Convert.ToInt32(stripeInfo.Amount),
                        Currency = "USD",
                        ReceiptEmail = stripeInfo.CardHolderEmail,
                        Customer = stripeCustomer.Id,
                        Description = stripeInfo.PaymentDescription, //Optional
                        ApplicationFeeAmount = stripeInfo.RestaurantApplicationFeeAmount,
                        StatementDescriptorSuffix = "-" + stripeInfo.RestaurantSeoFriendlyName,
                        TransferData = transferData,
                        Capture = false
                    };

                    //and Create Method of this object is doing the payment execution.  
                    var service = new Stripe.ChargeService();
                    Stripe.Charge charge = service.Create(options); // This will do the Payment 

                    if (!string.IsNullOrEmpty(charge.Status))
                    {
                        string paymentStatus = charge.Status;
                        stripePaymentStatus.Status = paymentStatus;
                        stripePaymentStatus.StripeCustomerId = charge.CustomerId;
                        stripePaymentStatus.StripeInvoiceId = charge.InvoiceId;
                        stripePaymentStatus.StripePaymentId = charge.Id;
                        stripePaymentStatus.CardId = charge.PaymentMethodId;
                        stripePaymentStatus.CardType = charge.PaymentMethodDetails.Card.Brand;

                        //string paymentChargeId = charge.Id;
                        //string paymentChargeInvoiceId = charge.InvoiceId;
                        //string paymentBillingTansId = charge.BalanceTransactionId;
                        //string paymentMethodId = charge.PaymentMethodId;
                        //string paymentMethodDetails = charge.PaymentMethodDetails.Type;
                        //string paymentRecieptNumber = charge.ReceiptNumber;
                        //string paymentRecieptUrl = charge.ReceiptUrl;
                        //string paymentCustomerId = charge.CustomerId;
                        //float paymentAmount = charge.Amount;
                        //string paymentDescription = charge.Description;
                        //float paymentAmountRefunded = charge.AmountRefunded;

                    }


                }

                return stripePaymentStatus;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }




        }

        #endregion

        #region [Refund]

        /// <summary>
        /// Create refund for intent
        /// </summary>
        /// <param name="paymentIntentId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Refund CreateRefundForIntent(CreateRefundRequest createRefundRequest)
        {
            if (createRefundRequest.IsTestRestaurant)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            try
            {
                var refunds = new RefundService();
                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = createRefundRequest.PaymentIntentId,
                    Reason = RefundReasons.RequestedByCustomer,
                    RefundApplicationFee = createRefundRequest.RefundApplicationFee,
                    ReverseTransfer = createRefundRequest.ReverseTransfer,
                    Amount = createRefundRequest.Amount,
                    Metadata = new Dictionary<string, string>
                  {
                    { "order_id", createRefundRequest.OrderId },
                  }
                };
                var refund = refunds.Create(refundOptions);

                return refund;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, createRefundRequest.IsTestRestaurant);
            }

        }

        ///// <summary>
        ///// Create Partial  refund for intent
        ///// </summary>
        ///// <param name="paymentIntentId"></param>
        ///// <param name="isTestRestaurant"></param>
        ///// <param name="amount"></param>
        ///// <returns></returns>
        //public Refund CreatePartialRefundForIntent(string paymentIntentId, bool isTestRestaurant, long amount)
        //{
        //    if (isTestRestaurant)
        //        StripeConfiguration.ApiKey = _Config.TestSecretKey;

        //    try
        //    {
        //        var refunds = new RefundService();
        //        var refundOptions = new RefundCreateOptions
        //        {
        //            PaymentIntent = paymentIntentId,
        //            Amount = amount
        //        };
        //        var refund = refunds.Create(refundOptions);

        //        return refund;
        //    }
        //    catch (StripeException ex)
        //    {

        //        throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestRestaurant);
        //    }

        //}

        /// <summary>
        /// Create a refund for charge Id
        /// </summary>
        /// <param name="chargeId"></param>
        /// <returns></returns>
        public Refund CreateRefundForCharge(string chargeId, long amount, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new RefundCreateOptions
                {
                    Charge = chargeId,
                    Amount = amount
                };
                var service = new RefundService();
                var refund = service.Create(options);

                //return refund.Id;
                return refund;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get details of refund for generated refund id 
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>,
        public Refund GetRefundDetails(string refundId, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var service = new RefundService();

                var refund = service.Get(refundId);

                return refund;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Update refund details
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public Refund UpdateRefund(string refundId, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new RefundUpdateOptions
                {
                    Metadata = new Dictionary<string, string>
                  {
                    { "order_id", "6735" },
                  },
                };
                var service = new RefundService();
                var refund = service.Update(refundId, options);

                return refund;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get list of all refunds.
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public List<Refund> GetRefundList(bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new RefundListOptions { Limit = 5 };
                var service = new RefundService();
                var refunds = service.List(options);

                return refunds.Data;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }
        #endregion

        #region [Dispute]
        /// <summary>
        /// Get details of dispute based on dispute Id
        /// </summary>
        /// <param name="disputeId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Dispute GetDisputeDetails(string disputeId, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var service = new DisputeService();
                var dispute = service.Get(disputeId);

                return dispute;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get list of all disputes 
        /// </summary>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public StripeList<Dispute> GetDisputeList(bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new DisputeListOptions
                {
                    Limit = 3,
                };
                var service = new DisputeService();
                StripeList<Dispute> disputes = service.List(
                  options
                );

                return disputes;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        /// <summary>
        /// Get list of dispute based on payment intent Id
        /// </summary>
        /// <param name="paymentIntentId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public StripeList<Dispute> GetDisputeListForPaymentIntent(string paymentIntentId, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new DisputeListOptions
                {
                    PaymentIntent = paymentIntentId,
                    Limit = 3,
                };
                var service = new DisputeService();
                StripeList<Dispute> disputes = service.List(
                  options
                );

                return disputes;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }

        //public Dispute CreateDispute( string transactionId, bool isTestRestaurant)
        //{
        //    if (isTestRestaurant)
        //        StripeConfiguration.ApiKey = _Config.TestSecretKey;

        //    try
        //    {
        //        var options = new Stripe.Issuing.DisputeCreateOptions
        //        {                    
        //            DisputedTransaction = transactionId,                   
        //        };

        //        var service = new DisputeService();
        //        var dispute = service.Create(options);
        //        return dispute;
        //    }
        //    catch (StripeException ex)
        //    {

        //        throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestRestaurant);
        //    }

        //}
        /// <summary>
        /// Update details of dispute based on dispute Id
        /// </summary>
        /// <param name="disputeId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Dispute UpdateDispute(DisputeUpdateRequest disputeUpdateRequest)
        {
            if (disputeUpdateRequest.IsTestRestaurant)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var options = new DisputeUpdateOptions
                {
                    Metadata = new Dictionary<string, string>
                      {
                        { "order_id", disputeUpdateRequest.OrderId },
                      },
                };
                var service = new DisputeService();
                var dispute = service.Update(disputeUpdateRequest.DisputeId, options);

                return dispute;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, disputeUpdateRequest.IsTestRestaurant);
            }

        }

        /// <summary>
        /// Close the dispute based on dispte Id
        /// </summary>
        /// <param name="disputeId"></param>
        /// <param name="isTestRestaurant"></param>
        /// <returns></returns>
        public Dispute CloseDispute(string disputeId, bool isTestBusiness)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            try
            {
                var service = new DisputeService();
                var dispute = service.Close(disputeId);

                return dispute;
            }
            catch (StripeException ex)
            {

                throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
            }

        }
        #endregion

        #region [CardScanner]


        /// <summary>
        /// Create reader location for a business
        /// </summary>
        /// <param name="isTestBusiness"></param>
        /// <param name="connectedAccountId"></param>
        /// <param name="locationName"></param>
        /// <param name="addressLine1"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="postalCode"></param>
        /// <returns></returns>
        public Location CreateLocation(bool isTestBusiness, string connectedAccountId ,string locationName, string addressLine1, string city, string state, string postalCode)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var options = new LocationCreateOptions
            {
                DisplayName = locationName,
                Address = new AddressOptions
                {
                    Line1 = addressLine1,
                    City = city,
                    State = state,
                    Country = "US",
                    PostalCode = postalCode,
                },
            };

            var requestOptions = new RequestOptions();          
            var service = new LocationService();
            var location = service.Create(options, requestOptions);

            return location;

        }

        public Location GetLocation(bool isTestBusiness, string locationId)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            var options = new LocationGetOptions();
            var requestOptions = new RequestOptions();
            var service = new LocationService();
            var location = service.Get(locationId,options, requestOptions);

            return location;
        }


        /// <summary>
        /// Update reader location for a business
        /// </summary>
        /// <param name="isTestBusiness"></param>
        /// <param name="locationId"></param>
        /// <param name="locationName"></param>
        /// <param name="addressLine1"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="postalCode"></param>
        /// <returns></returns>
        public Location UpdateLocation(bool isTestBusiness, string locationId, string locationName, string addressLine1, string city, string state, string postalCode)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var options = new LocationUpdateOptions
            {
                DisplayName = locationName,
                Address = new AddressOptions
                {
                    Line1 = addressLine1,
                    City = city,
                    State = state,
                    Country = "US",
                    PostalCode = postalCode,
                },
            };

            var requestOptions = new RequestOptions();
            var service = new LocationService();
            var location = service.Update(locationId,options);

            return location;

        }

        /// <summary>
        /// Register card reader for a business
        /// </summary>
        /// <param name="isTestRestaurant"></param>
        /// <param name="readerRegistrationCode"></param>
        /// <param name="readerName"></param>
        /// <returns></returns>
        public Reader CreateReader(bool isTestBusiness, string connectedAccountId, string readerRegistrationCode, string readerName, string LocationId)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var options = new ReaderCreateOptions
            {
                RegistrationCode = readerRegistrationCode,
                Label = readerName,
                Location = LocationId,

            };

            var requestOptions = new RequestOptions(); 
            var service = new ReaderService();
            var reader = service.Create(options, requestOptions);
            
            return reader;
        }


        /// <summary>
        /// Update selected reader
        /// </summary>
        /// <param name="isTestBusiness"></param>
        /// <param name="readerId"></param>
        /// <returns></returns>
        public Reader UpdateReader(bool isTestBusiness, string readerId, string readerName)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var options = new ReaderUpdateOptions
            {
                Label = readerName,
            };
            var service = new ReaderService();
           var response= service.Update(readerId, options);

            return response;
        }
        /// <summary>
        /// Delete selected reader
        /// </summary>
        /// <param name="isTestBusiness"></param>
        /// <param name="readerId"></param>
        /// <returns></returns>
        public Reader DeleteReader(bool isTestBusiness, string readerId)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var service = new ReaderService();
            var response = service.Delete(readerId);

            return response;
        }


        /// <summary>
        /// Get list of connected card reader
        /// </summary>
        /// <param name="isTestBusiness"></param>
        /// <param name="connectedAccountId"></param>
        /// <returns></returns>
        public StripeList<Reader> GetReaderList(bool isTestBusiness, string connectedAccountId, string locationId)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var options = new ReaderListOptions {
                Limit = 5,
                Location= locationId
            };
            var service = new ReaderService();
            StripeList<Reader> readers = service.List(
              options
            );
           
            return readers;
        }

        /// <summary>
        /// Generate token for card reader connection 
        /// </summary>
        /// <param name="isTestBusiness"></param>
        /// <param name="connectedAccountId"></param>
        /// <returns></returns>
        public ConnectionToken GenerateConnectionToken(bool isTestBusiness, string connectedAccountId,string locationId)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var options = new ConnectionTokenCreateOptions { 
            Location= locationId
            };
            var requestOptions = new RequestOptions();
            var service = new ConnectionTokenService();
            var connectionToken = service.Create(options);
            return connectionToken;

        }


        /// <summary>
        /// Create payment intent using card scanner
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public PaymentIntent CreatePaymentIntentUsingCardScanner(long amount, bool isTestBusiness, string connectedAccountId,long digitalFee = 0)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;

            var service = new PaymentIntentService();
            // For Terminal payments, the 'payment_method_types' parameter must include
            // 'card_present' and the 'capture_method' must be set to 'manual'
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card_present" },
                CaptureMethod = "manual",
                ApplicationFeeAmount = digitalFee,//Will lookup take order digital fee later.
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = connectedAccountId, //"acct_1Gdd7RFIp97ed87r",
                },                
            };

            var requestOptions = new RequestOptions();
            var intent = service.Create(options, requestOptions);

            return intent;

        }

        /// <summary>
        /// Capture payment using card scanner
        /// </summary>
        /// <param name="paymentIntentId"></param>
        /// <returns></returns>
        public PaymentIntent CapturePaymentIntentUsingCardScanner(string paymentIntentId, bool isTestBusiness, string connectedAccountId,long digitalFee = 0)
        {
            if (isTestBusiness)
                StripeConfiguration.ApiKey = _Config.TestSecretKey;
            var options = new PaymentIntentCaptureOptions
            {
                ApplicationFeeAmount = digitalFee,
            };
            var service = new PaymentIntentService();

            var requestOptions = new RequestOptions();           
            var intent = service.Capture(paymentIntentId, null, requestOptions);
            return intent;
        }

        #endregion

    }
}