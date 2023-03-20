using PaymentService.Card;
using PaymentService.Customer;
using PaymentService.DisputeModel;
using PaymentService.RefundModel;
using Stripe;
using Stripe.Terminal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService
{
    public interface IPaymentServices
    {
        //ConnectedAccount

        Account CreateConnectedAccount(string email, bool isTestBusiness);
        Account GetConnectedAccountDetails(string connectedAccountId, bool isTestBusiness = false);
        AccountLink GetConnectedAccountPendingInformation(string connectedAccountId, string baseUrl, bool isTestBusiness = false);
        StripeList<Account> GetConnectedAccountList(bool isTestBusiness);
        OAuthToken AuthorizeConnectedAccount(string code, bool isTestBusiness = false);
        OAuthDeauthorize DeAuthorizeConnectedAccount(string connectedAccountId, bool isTestBusiness = false);
        Account UpdateConnectedAccount(string connectedAccountId, string clientIPAddress, bool isTestBusiness = false);
        Account UpdateConnectedAccountEmail(string connectedAccountId, string email, bool isTestBusiness = false);
        LoginLink CreateConnectedAccountLoginLink(string connectedAccountId, bool isTestBusiness);
        AccountLink CreateConnectedAccountLink(string connectedAccountId, string baseUrl, string currentUrl,string seoName, bool isTestBusiness);
        Account RejectConnectedAccount(string connectedAccountId, string reason, bool isTestBusiness);

        //Customer
        Stripe.Customer CreateCustomer(CustomerRequest customerRequest, bool isTestBusiness = false);
        Stripe.Customer GetCustomer(string customerId, bool isTestBusiness = false);
        Stripe.Customer UpdateCustomer(string customerId, CustomerRequest customerRequest, bool isTestBusiness = false);
        Stripe.Customer SetCustomersDefaultPaymentMethod(string customerId, string paymentMethodId, bool isTestBusiness = false);
        bool DeleteCustomer(string customerId, bool isTestBusiness = false);


        //Payment Method
        PaymentMethod CreatePaymentMethod(CardRequest cardDetailsModel, bool isTestBusiness = false);        
        StripeList<PaymentMethod> GetPaymentMethodList(string customerId, bool isTestBusiness = false);
        PaymentMethod GetPaymentMethod(string paymentMethodId, bool isTestBusiness = false);
        PaymentMethod UpdatePaymentMethod(CardRequest cardDetailsModel, string paymentMethodId, bool isTestBusiness = false);
        PaymentMethod AttachPaymentMethod(string customerId, string paymentMethodId, bool isTestBusiness = false);
        PaymentMethod DetachPaymentMethod(string paymentMethodId, bool isTestBusiness = false);
        

        //Payment Intent
        PaymentIntent GetPaymentIntent(string paymentIntentId, bool isTestBusiness = false);
        List<PaymentIntent> GetListOfPaymentIntents(bool isTestBusiness = false);
        PaymentIntent CreatePaymentIntent(long amount, string receiptEmail, string customerId, string customerName, string[] orderProducts, string paymentMethodId, string ConnectedAccountId, string restaurantSeoFriendlyName,long restaurantApplicationFeeAmount, bool isTestBusiness = false);
        Task<PaymentIntent> CreatePaymentIntentAsync(long amount, string receiptEmail, string customerId, string customerName, string[] orderProducts, string paymentMethod, string ConnectedAccountId, string restaurantSeoFriendlyName, long restaurantApplicationFeeAmount, bool isTestBusiness = false);
        Task<PaymentIntent> CreatePaymentIntentAsync(long amount, long applicationFeeAmount, string receiptEmail, string customerId, string description, string paymentMethod, string connectedAccountId, string appplicationName, bool isTestMode = false);
        PaymentIntent ConfirmPaymentIntent(string paymentIntentId, bool isTestBusiness = false);               
        PaymentIntent CapturePaymentIntent(string intentId, long amount,long applicationFeeAmount, bool isTestBusiness = false);
        PaymentIntent UpdatePaymentIntent(long amount, long applicationFeeAmount, string receiptEmail, string paymentIntentId, bool isTestBusiness = false);
        PaymentIntent CancelPaymentIntent(string paymentIntentId, bool isTestBusiness = false);
        PaymentStatus DirectPayment(StripeInfoModel stripeInfo, bool isTestBusiness = false);

        //Refund
        Refund CreateRefundForIntent(CreateRefundRequest createRefundRequest);
        Refund CreateRefundForCharge(string chargeId, long amount, bool isTestBusiness);
        Refund GetRefundDetails(string refundId, bool isTestBusiness);
        Refund UpdateRefund(string refundId, bool isTestBusiness);
        List<Refund> GetRefundList(bool isTestBusiness);

        //Dispute
        Dispute GetDisputeDetails(string disputeId, bool isTestBusiness);
        StripeList<Dispute> GetDisputeList(bool isTestBusiness);
        StripeList<Dispute> GetDisputeListForPaymentIntent(string paymentIntentId, bool isTestBusiness);
        Dispute UpdateDispute(DisputeUpdateRequest disputeUpdateRequest);
        Dispute CloseDispute(string disputeId, bool isTestBusiness);

        //Card
        string SaveCard(CardRequest cardDetailsModel, string customerId, bool isTestBusiness = false);
        Stripe.Card GetCardDetails(string customerId, string cardTokenId, bool isTestBusiness = false);
        List<Stripe.Card> GetListofCards(string customerId, bool isTestBusiness = false);
        Stripe.Card UpdateCardDetails(CardRequest cardDetailsModel, string customerId, string cardTokenId, bool isTestBusiness = false);
        bool DeleteCard(string customerId, string cardTokenId, bool isTestBusiness = false);
        CardValidationResponse ValidateCardDetails(CardRequest cardDetailsModel, bool isTestBusiness = false);

        // Card Reader
        Reader CreateReader(bool isTestBusiness, string connectedAccountId, string readerRegistrationCode, string readerName, string LocationId);
        StripeList<Reader> GetReaderList(bool isTestBusiness, string connectedAccountId, string locationId);
        Reader DeleteReader(bool isTestBusiness, string readerId);
        Reader UpdateReader(bool isTestBusiness, string readerId, string readerName);
        ConnectionToken GenerateConnectionToken(bool isTestBusiness, string connectedAccountId, string locationId);
        Location CreateLocation(bool isTestBusiness, string connectedAccountId, string locationName, string addressLine1, string city, string state, string postalCode);
        Location GetLocation(bool isTestBusiness, string locationId);
        Location UpdateLocation(bool isTestBusiness, string locationId, string locationName, string addressLine1, string city, string state, string postalCode);
        PaymentIntent CreatePaymentIntentUsingCardScanner(long amount, bool isTestBusiness, string connectedAccountId, long digitalFee = 0);
        PaymentIntent CapturePaymentIntentUsingCardScanner(string paymentIntentId, bool isTestBusiness, string connectedAccountId, long digitalFee = 0);
    }
}