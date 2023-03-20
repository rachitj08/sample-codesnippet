using Stripe;

namespace PaymentService.Error
{
    /// <summary>
    /// Class for handling stripe errors
    /// </summary>
    public static class HandleStripeError
    {
        /// <summary>
        /// Handle Error method for handling different type of errors
        /// </summary>
        /// <param name="stripeException">stripe generated error</param>
        /// <param name="IsTestRestaurant">bool value to indicate the of restaurant(live/test)</param>
        /// <returns></returns>
        public static Stripe.StripeException HandleError(Stripe.StripeException stripeException, bool IsTestRestaurant)
        {
            //Console.WriteLine("Code: " + stripeException.StripeError.Code);
            //Console.WriteLine("Message: " + stripeException.StripeError.Message);
            string ErrorMessage;
            switch (stripeException.StripeError.ErrorType)
            {
                case "card_error": //Card errors are the most common type of error you should expect to handle. They result when the user enters a card that can't be charged for some reason.
                    if (stripeException.StripeError.Code == "card_declined")
                    {
                        ErrorMessage = GetMessageForCardError(stripeException);
                    }
                    else if (stripeException.StripeError.Code == "card_decline_rate_limit_exceeded")
                    {
                        ErrorMessage = "This card has been declined too many times. Please try this card again after 24 hours.";
                    }
                    else
                    {
                        ErrorMessage = stripeException.StripeError.Message;
                    }
                    break;
                case "invalid_request_error": //Invalid request errors arise when your request has invalid parameters.
                    if (stripeException.StripeError.Code == "resource_missing")
                    {
                        if (IsTestRestaurant)
                            ErrorMessage = "You are a live customer and can't place order for a test restaurant.";
                        else
                            ErrorMessage = "You are a test customer and can't place order for a live restaurant.";
                    }
                    else
                    {
                        //i.e. stripeException.StripeError.Code == "amount_too_small"
                        ErrorMessage = stripeException.StripeError.Message;
                    }
                    break;
                case "api_connection_error": //Failure to connect to Stripe's API. This message might come up because your DNS is not working.
                    ErrorMessage = stripeException.StripeError.Message;
                    break;
                case "api_error": //API errors cover any other type of problem (e.g., a temporary problem with Stripe's servers), and are extremely uncommon.
                    ErrorMessage = stripeException.StripeError.Message;
                    break;
                case "authentication_error": //No API key provided (Failure to properly authenticate yourself in the request.)
                    ErrorMessage = stripeException.StripeError.Message;
                    break;
                case "rate_limit_error": //Too many requests hit the API too quickly.
                    ErrorMessage = stripeException.StripeError.Message;
                    break;
                case "validation_error":
                    ErrorMessage = stripeException.StripeError.Message;
                    break;
                default:
                    ErrorMessage = stripeException.StripeError.Message;
                    break;
            }

            return new StripeException(stripeException.HttpStatusCode, stripeException.StripeError, ErrorMessage);
        }



        /// <summary>
        /// Method to handle stripe card errors
        /// </summary>
        /// <param name="stripeException"></param>
        /// <returns></returns>
        private static string GetMessageForCardError(StripeException stripeException)

        {

            string ErrorMessage;

            switch (stripeException.StripeError.DeclineCode)

            {

                case "authentication_required":
                    ErrorMessage = "Card Declined: Authentication Required.";

                    break;

                case "approve_with_id":

                    ErrorMessage = "The payment cannot be authorized.";

                    break;

                case "call_issuer":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "card_not_supported":

                    ErrorMessage = "The card does not support this type of purchase.";

                    break;

                case "card_velocity_exceeded":

                    ErrorMessage = "The card has exceeded the balance or credit limit available.";

                    break;

                case "currency_not_supported":

                    ErrorMessage = "The customer has exceeded the balance or credit limit available on their card.";

                    break;

                case "do_not_honor":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "do_not_try_again":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "duplicate_transaction":

                    ErrorMessage = "An identical transaction was submitted very recently.";

                    break;

                case "expired_card":

                    ErrorMessage = "The card has expired. Please contact card issuer for more information.";

                    break;

                case "fraudulent":

                    ErrorMessage = "The payment has been declined as Stripe suspects it is fraudulent.";

                    break;

                case "generic_decline":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "incorrect_number":

                    ErrorMessage = "The card number is incorrect.";

                    break;

                case "incorrect_cvc":

                    ErrorMessage = "The CVC number is incorrect.";

                    break;

                case "incorrect_pin":

                    ErrorMessage = "The PIN entered is incorrect.";

                    break;

                case "incorrect_zip":

                    ErrorMessage = "The ZIP/postal code is incorrect.";

                    break;

                case "insufficient_funds":

                    ErrorMessage = "The card has insufficient funds to complete the purchase.";

                    break;

                case "invalid_account":

                    ErrorMessage = "The card, or account the card is connected to, is invalid.";

                    break;

                case "invalid_amount":

                    ErrorMessage = "The payment amount is invalid, or exceeds the amount that is allowed.";

                    break;

                case "invalid_cvc":

                    ErrorMessage = "The CVC number is incorrect.";

                    break;

                case "invalid_expiry_year":

                    ErrorMessage = "The expiration year invalid.";

                    break;

                case "invalid_number":

                    ErrorMessage = "The card number is incorrect.";

                    break;

                case "invalid_pin":

                    ErrorMessage = "The PIN entered is incorrect.";

                    break;

                case "issuer_not_available":

                    ErrorMessage = "The card issuer could not be reached, so the payment could not be authorized.";

                    break;

                case "lost_card":

                    ErrorMessage = "The payment has been declined because the card is reported lost.";

                    break;

                case "merchant_blacklist":

                    ErrorMessage = "The payment has been declined because it matches a value on the Stripe user's block list.";

                    break;

                case "new_account_information_available":

                    ErrorMessage = "The card, or account the card is connected to, is invalid.";

                    break;

                case "no_action_taken":

                    ErrorMessage = "The card has been declined for an unknown reason.";

                    break;

                case "not_permitted":

                    ErrorMessage = "The payment is not permitted.";

                    break;

                case "offline_pin_required":

                    ErrorMessage = "The card has been declined as it requires a PIN.";

                    break;

                case "online_or_offline_pin_required":

                    ErrorMessage = "The card has been declined as it requires a PIN.";

                    break;

                case "pickup_card":

                    ErrorMessage = "Card Declined: It is possible it has been reported lost or stolen.";

                    break;

                case "pin_try_exceeded":

                    ErrorMessage = "The allowable number of PIN tries has been exceeded.";

                    break;

                case "processing_error":

                    ErrorMessage = "An error occurred while processing the card.";

                    break;

                case "reenter_transaction":

                    ErrorMessage = "Could Not Process: Unknown Reason. Please contact card issuer for more information.";

                    break;

                case "restricted_card":

                    ErrorMessage = "Card Declined: It is possible it has been reported lost or stolen.";

                    break;

                case "revocation_of_all_authorizations":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "revocation_of_authorization":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "security_violation":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "service_not_allowed":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "stolen_card":

                    ErrorMessage = "The payment has been declined because the card is reported stolen.";

                    break;

                case "stop_payment_order":

                    ErrorMessage = "The card has been declined for an unknown reason. Please contact card issuer for more information.";

                    break;

                case "testmode_decline":

                    ErrorMessage = "A Stripe test card number was used.";

                    break;

                case "transaction_not_allowed":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "try_again_later":

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;

                case "withdrawal_count_limit_exceeded":

                    ErrorMessage = "The customer has exceeded the balance or credit limit available on their card.";

                    break;

                default:

                    ErrorMessage = "Card Declined: Unknown reason. Please contact card issuer for more information.";

                    break;



            }



            return ErrorMessage;

        }

    }

}
