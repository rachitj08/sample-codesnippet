﻿namespace PaymentService.Error
{
    public enum StripeErrorEnum
    {
        card_error,
        invalid_request_error,
        api_connection_error,
        api_error,
        authentication_error,
        rate_limit_error,
        validation_error,
        card_declined,
        card_decline_rate_limit_exceeded,
        resource_missing,
        authentication_required,
        approve_with_id,
        call_issuer,
        card_not_supported,
        card_velocity_exceeded,
        currency_not_supported,
        do_not_honor,
        do_not_try_again,
        duplicate_transaction,
        expired_card,
        fraudulent,
        generic_decline,
        incorrect_number,
        incorrect_cvc,
        incorrect_pin,
        incorrect_zip,
        insufficient_funds,
        invalid_account,
        invalid_amount,
        invalid_cvc,
        invalid_expiry_year,
        invalid_number,
        invalid_pin,
        issuer_not_available,
        lost_card,
        merchant_blacklist,
        new_account_information_available,
        no_action_taken,
        not_permitted,
        offline_pin_required,
        online_or_offline_pin_required,
        pickup_card,
        pin_try_exceeded,
        processing_error,
        reenter_transaction,
        restricted_card,
        revocation_of_all_authorizations,
        revocation_of_authorization,
        security_violation,
        service_not_allowed,
        stolen_card,
        stop_payment_order,
        testmode_decline,
        transaction_not_allowed,
        try_again_later,
        withdrawal_count_limit_exceeded


    }
}
