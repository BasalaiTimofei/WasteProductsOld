namespace WasteProducts.Logic.Constants.Donations
{
    /// <summary>
    /// Represents PayPal IPN variables.
    /// </summary>
    public static class IPN
    {
        /// <summary>
        /// Represents Transaction information variables from a IPN request.
        /// </summary>
        public static class Transaction
        {
            /// <summary>
            /// Email address or account ID of the payment recipient(that is, the merchant).
            /// Equivalent to the values of receiver_email(if payment is sent to primary account)
            /// and business set in the Website Payment HTML.
            /// Note: The value of this variable is normalized to lowercase characters.
            /// </summary>
            public const string BUSINESS = "business";

            /// <summary>
            /// Character set.
            /// </summary>
            public const string CHARSET = "charset";

            /// <summary>
            /// Custom value as passed by you, the merchant.
            /// These are pass-through variables that are never presented to your customer.
            /// Length: 255 characters.
            /// </summary>
            public const string CUSTOM = "custom";

            /// <summary>
            /// Internal; only for use by MTS.
            /// </summary>
            public const string IPN_TRACK_ID = "ipn_track_id";

            /// <summary>
            /// Message's version number.
            /// </summary>
            public const string NOTIFY_VERSION = "notify_version";

            /// <summary>
            /// In the case of a refund, reversal, or canceled reversal,
            /// this variable contains the "txn_id" of the original transaction,
            /// while "txn_id" contains a new ID for the new transaction.
            /// Length: 19 characters.
            /// </summary>
            public const string PARENT_TXN_ID = "parent_txn_id";

            /// <summary>
            /// Unique ID generated during guest checkout (payment by credit card without logging in).
            /// </summary>
            public const string RECEIPT_ID = "receipt_id";

            /// <summary>
            /// Primary email address of the payment recipient (that is, the merchant).
            /// If the payment is sent to a non-primary email address on your PayPal account,
            /// the receiver_email is still your primary email.
            /// Note: The value of this variable is normalized to lowercase characters.
            /// Length: 127 characters.
            /// </summary>
            public const string RECEIVER_EMAIL = "receiver_email";

            /// <summary>
            /// Unique account ID of the payment recipient (i.e., the merchant).
            /// This is the same as the recipient's referral ID. 
            /// Length: 13 characters.
            /// </summary>
            public const string RECEIVER_ID = "receiver_id";

            /// <summary>
            /// Whether this IPN message was resent (equals true);
            /// otherwise, this is the original message.
            /// </summary>
            public const string RESEND = "resend";

            /// <summary>
            /// ISO 3166 country code associated with the country of residence.
            /// Length: 2 characters.
            /// </summary>
            public const string RESIDENCE_COUNTRY = "residence_country";

            /// <summary>
            /// Whether the message is a test message. Value is:
            /// 1 — the message is directed to the Sandbox.
            /// </summary>
            public const string TEST_IPN = "test_ipn";

            /// <summary>
            /// The merchant's original transaction identification number for the payment from the buyer,
            /// against which the case was registered.
            /// </summary>
            public const string TXN_ID = "txn_id";

            /// <summary>
            /// The kind of transaction for which the IPN message was sent.
            /// </summary>
            public const string TXN_TYPE = "txn_type";

            /// <summary>
            /// Encrypted string used to validate the authenticity of the transaction.
            /// </summary>
            public const string VERIFY_SIGN = "verify_sign";
        }

        /// <summary>
        /// Represents Buyer information variables from a IPN request.
        /// </summary>
        public static class Buyer
        {
            /// <summary>
            /// Country of customer's address.
            /// Length: 64 characters.
            /// </summary>
            public const string ADDRESS_COUNTRY = "address_country";

            /// <summary>
            /// City of customer's address.
            /// Length: 40 characters.
            /// </summary>
            public const string ADDRESS_CITY = "address_city";

            /// <summary>
            /// ISO 3166 country code associated with customer's address.
            /// Length: 2 characters.
            /// </summary>
            public const string ADDRESS_COUNTRY_CODE = "address_country_code";

            /// <summary>
            /// Name used with address(included when the customer provides a Gift Address).
            /// Length: 128 characters.
            /// </summary>
            public const string ADDRESS_NAME = "address_name";

            /// <summary>
            /// State of customer's address.
            /// Length: 40 characters.
            /// </summary>
            public const string ADDRESS_STATE = "address_state";

            /// <summary>
            /// Whether the customer provided a confirmed address. Value is:
            /// "confirmed" — Customer provided a confirmed address;
            /// "unconfirmed" — Customer provided an unconfirmed address.
            /// </summary>
            public const string ADDRESS_STATUS = "address_status";

            /// <summary>
            /// Customer's street address.
            /// Length: 200 characters.
            /// </summary>
            public const string ADDRESS_STREET = "address_street";

            /// <summary>
            /// Zip code of customer's address.
            /// Length: 20 characters.
            /// </summary>
            public const string ADDRESS_ZIP = "address_zip";

            /// <summary>
            /// Customer's telephone number.
            /// Length: 20 characters.
            /// </summary>
            public const string CONTACT_PHONE = "contact_phone";

            /// <summary>
            /// Customer's first name.
            /// Length: 64 characters.
            /// </summary>
            public const string FIRST_NAME = "first_name";

            /// <summary>
            /// Customer's last name.
            /// Length: 64 characters.
            /// </summary>
            public const string LAST_NAME = "last_name";

            /// <summary>
            /// Customer's company name, if customer is a business.
            /// Length: 127 characters.
            /// </summary>
            public const string PAYER_BUSINESS_NAME = "payer_business_name";

            /// <summary>
            /// Customer's primary email address. Use this email to provide any credits.
            /// Length: 127 characters.
            /// </summary>
            public const string PAYER_EMAIL = "payer_email";

            /// <summary>
            /// Unique customer ID.
            /// Length: 13 characters.
            /// </summary>
            public const string PAYER_ID = "payer_id";

            /// <summary>
            /// Represents the status of the buyer's address.
            /// </summary>
            public static class AddressStatus
            {
                /// <summary>
                /// Customer provided a confirmed address.
                /// </summary>
                public const string CONFIRMED = "confirmed";
            }
        }

        /// <summary>
        /// Represents Payment information variables from a IPN request.
        /// </summary>
        public static class Payment
        {
            /// <summary>
            /// Authorization amount.
            /// </summary>
            public const string AUTH_AMOUNT = "auth_amount";

            /// <summary>
            /// Authorization expiration date and time, in the following format: HH:MM:SS DD Mmm YY, YYYY PST.
            /// Length: 28 characters.
            /// </summary>
            public const string AUTH_EXP = "auth_exp";

            /// <summary>
            /// Authorization identification number.
            /// Length: 19 characters.
            /// </summary>
            public const string AUTH_ID = "auth_id";

            /// <summary>
            /// Status of authorization.
            /// </summary>
            public const string AUTH_STATUS = "auth_status";

            /// <summary>
            /// The total discount to be applied to a shopping cart in the currency of "mc_currency".
            /// </summary>
            public const string DISCOUNT = "discount";

            /// <summary>
            /// The time an eCheck was processed; for example,
            /// when the status changes to Success or Completed.
            /// The format is as follows:
            /// hh:mm:ss MM DD, YYYY ZONE, e.g. 04:55:30 May 26, 2011 PDT.
            /// </summary>
            public const string ECHECK_TIME_PROCESSED = "echeck_time_processed";

            /// <summary>
            /// Exchange rate used if a currency conversion occurred.
            /// </summary>
            public const string EXCHANGE_RATE = "exchange_rate";

            /// <summary>
            /// Pass-through variable you can use to identify your Invoice Number for this purchase.
            /// If omitted, no variable is passed back.
            /// Length: 127 characters.
            /// </summary>
            public const string INVOICE = "invoice";

            /// <summary>
            /// * For payment IPN notifications, this is the currency of the payment.
            /// * For non-payment subscription IPN notifications
            /// (i.e., txn_type= signup, cancel, failed, eot, or modify),
            /// this is the currency of the subscription.
            /// * For payment subscription IPN notifications,
            /// it is the currency of the payment(i.e., txn_type = subscr_payment).
            /// </summary>
            public const string MC_CURRENCY = "mc_currency";

            /// <summary>
            /// Transaction fee associated with the payment.
            /// "mc_gross" minus "mc_fee" equals the amount deposited into the "receiver_email" account.
            /// Equivalent to "payment_fee" for USD payments.
            /// If this amount is negative, it signifies a refund or reversal,
            /// and either of those payment statuses can be for the full or partial amount of the original transaction fee.
            /// </summary>
            public const string MC_FEE = "mc_fee";

            /// <summary>
            /// Full amount of the customer's payment,
            /// before transaction fee is subtracted.
            /// Equivalent to payment_gross for USD payments.
            /// If this amount is negative, it signifies a refund or reversal,
            /// and either of those payment statuses can be for the full or partial amount of the original transaction.
            /// </summary>
            public const string MC_GROSS = "mc_gross";

            /// <summary>
            /// Total handling amount associated with the transaction.
            /// </summary>
            public const string MC_HANDLING = "mc_handling";

            /// <summary>
            /// Total shipping amount associated with the transaction.
            /// </summary>
            public const string MC_SHIPPING = "mc_shipping";

            /// <summary>
            /// Memo as entered by your customer in PayPal Website Payments note field.
            /// Length: 255 characters.
            /// </summary>
            public const string MEMO = "memo";

            /// <summary>
            /// If this is a PayPal Shopping Cart transaction, number of items in cart.
            /// </summary>
            public const string NUM_CART_ITEMS = "num_cart_items";

            /// <summary>
            /// Whether the customer has a verified PayPal account.
            /// verified — Customer has a verified PayPal account.
            /// unverified — Customer has an unverified PayPal account.
            /// </summary>
            public const string PAYER_STATUS = "payer_status";

            /// <summary>
            /// Time/Date stamp generated by PayPal, in the following format: HH:MM:SS Mmm DD, YYYY PDT.
            /// Length: 28 characters.
            /// </summary>
            public const string PAYMENT_DATE = "payment_date";

            /// <summary>
            /// USD transaction fee associated with the payment.
            /// "payment_gross" minus "payment_fee" equals the amount deposited into the receiver email account.
            /// Is empty for non-USD payments.
            /// If this amount is negative, it signifies a refund or reversal,
            /// and either of those payment statuses can be for the full or partial amount of the original transaction fee.
            /// Note: This is a deprecated field.Use mc_fee instead.
            /// </summary>
            public const string PAYMENT_FEE = "payment_fee";

            /// <summary>
            /// Full USD amount of the customer's payment, before transaction fee is subtracted.
            /// Will be empty for non-USD payments.
            /// This is a deprecated field replaced by mc_gross.
            /// If this amount is negative, it signifies a refund or reversal,
            /// and either of those payment statuses can be for the full or partial amount of the original transaction.
            /// </summary>
            public const string PAYMENT_GROSS = "payment_gross";

            /// <summary>
            /// The status of the payment:
            /// 
            /// Canceled_Reversal: A reversal has been canceled.For example,
            /// you won a dispute with the customer,
            /// and the funds for the transaction that was reversed have been returned to you.
            /// 
            /// Completed: The payment has been completed,
            /// and the funds have been added successfully to your account balance.
            /// 
            /// Created: A German ELV payment is made using Express Checkout.
            /// 
            /// Denied: The payment was denied.
            /// This happens only if the payment was previously pending because of
            /// one of the reasons listed for the "pending_reason" variable or the "Fraud_Management_Filters_x" variable.
            /// 
            /// Expired: This authorization has expired and cannot be captured.
            /// 
            /// Failed: The payment has failed.
            /// This happens only if the payment was made from your customer's bank account.
            /// 
            /// Pending: The payment is pending. See "pending_reason" for more information.
            /// 
            /// Refunded: You refunded the payment.
            /// 
            /// Reversed: A payment was reversed due to a chargeback or other type of reversal.
            /// The funds have been removed from your account balance and returned to the buyer.
            /// The reason for the reversal is specified in the "ReasonCode" element.
            /// 
            /// Processed: A payment has been accepted.
            /// 
            /// Voided: This authorization has been voided.
            /// </summary>
            public const string PAYMENT_STATUS = "payment_status";

            /// <summary>
            /// echeck: This payment was funded with an eCheck.
            /// instant: This payment was funded with PayPal balance, credit card, or Instant Transfer.
            /// </summary>
            public const string PAYMENT_TYPE = "payment_type";

            /// <summary>
            /// This variable is set only if "payment_status" is "Pending".
            /// 
            /// address: Your Payment Receiving Preferences are set so that
            /// if a customer does not include a confirmed shipping address,
            /// you must manually accept or deny the payment.To change your preference,
            /// go to the Preferences section of your Profile.
            /// 
            /// authorization: You set the payment action to Authorization and have not yet captured funds.
            /// 
            /// delayed_disbursement: The transaction has been approved and is currently awaiting funding from the bank.
            /// This typically takes less than 48 hrs.
            /// 
            /// echeck: The payment is pending because it was made by an eCheck that has not yet cleared.
            /// 
            /// intl: The payment is pending because you hold a non-U.S. account and do not have a withdrawal mechanism.
            /// You must manually accept or deny this payment from your Account Overview.
            /// 
            /// multi_currency: You do not have a balance in the currency sent,
            /// and you do not have your profiles's Payment Receiving Preferences option set to
            /// automatically convert and accept this payment.
            /// As a result, you must manually accept or deny this payment.
            /// 
            /// order: You set the payment action to Order and have not yet captured funds.
            /// 
            /// paymentreview: The payment is pending while it is reviewed by PayPal for risk.
            /// 
            /// regulatory_review: The payment is pending because PayPal is reviewing it for compliance with government regulations.
            /// PayPal will complete this review within 72 hours.
            /// When the review is complete,
            /// you will receive a second IPN message whose payment_status/reason code variables indicate the result.
            /// 
            /// unilateral: The payment is pending because it was made to an email address that is not yet registered or confirmed.
            /// 
            /// upgrade: The payment is pending because it was made via credit card and
            /// you must upgrade your account to Business or Premier status before you can receive the funds.
            /// upgrade can also mean that you have reached the monthly limit for transactions on your account.
            /// 
            /// verify: The payment is pending because you are not yet verified.
            /// You must verify your account before you can accept this payment.
            /// 
            /// other: The payment is pending for a reason other than those listed above.
            /// For more information, contact PayPal Customer Service.
            /// </summary>
            public const string PENDING_REASON = "pending_reason";

            /// <summary>
            /// The status of the seller's protection eligibility.
            /// Possible values: Eligible, Ineligible, Partially Eligible - INR Only,
            /// Partially Eligible - Unauth Only,
            /// PartiallyEligible, None,
            /// Active Fraud Control - Unauth Premium Eligible.
            /// </summary>
            public const string PROTECTION_ELIGIBILITY = "protection_eligibility";

            /// <summary>
            /// Quantity as entered by your customer or as passed by you, the merchant.
            /// If this is a shopping cart transaction,
            /// PayPal appends the number of the item (e.g. quantity1, quantity2).
            /// </summary>
            public const string QUANTITY = "quantity";

            /// <summary>
            /// This variable is set if "payment_status" is "Reversed", "Refunded", "Canceled_Reversal", or "Denied".
            /// 
            /// adjustment_reversal: Reversal of an adjustment.
            /// 
            /// admin_fraud_reversal: The transaction has been reversed due to fraud detected by PayPal administrators.
            /// 
            /// admin_reversal: The transaction has been reversed by PayPal administrators.
            /// 
            /// buyer-complaint: The transaction has been reversed due to a complaint from your customer.
            /// 
            /// chargeback: The transaction has been reversed due to a chargeback by your customer.
            /// 
            /// chargeback_reimbursement: Reimbursement for a chargeback.
            /// 
            /// chargeback_settlement: Settlement of a chargeback.
            /// 
            /// guarantee: The transaction has been reversed because your customer exercised a money-back guarantee.
            /// 
            /// other: Unspecified reason.
            /// 
            /// refund: The transaction has been reversed because you gave the customer a refund.
            /// 
            /// regulatory_block: PayPal blocked the transaction due to a violation of a government regulation.
            /// In this case, payment_status is Denied.
            /// 
            /// regulatory_reject: PayPal rejected the transaction due to a violation of a government regulation
            /// and returned the funds to the buyer.In this case, payment_status is Denied.
            /// 
            /// regulatory_review_exceeding_sla: PayPal did not complete the review for compliance with
            /// government regulations within 72 hours, as required.
            /// Consequently, PayPal auto-reversed the transaction and returned the funds to the buyer.
            /// In this case, payment_status is Denied.Note that "sla" stand for "service level agreement".
            /// 
            /// unauthorized_claim: The transaction has been reversed because it was not authorized by the buyer.
            /// 
            /// unauthorized_spoof: The transaction has been reversed due to a customer dispute
            /// in which an unauthorized spoof is suspected.
            /// 
            /// Note: Additional codes may be returned.
            /// </summary>
            public const string REASON_CODE = "reason_code";

            /// <summary>
            /// Remaining amount that can be captured with Authorization and Capture.
            /// </summary>
            public const string REMAINING_SETTLE = "remaining_settle";

            /// <summary>
            /// Amount that is deposited into the account's primary balance after a currency conversion from
            /// automatic conversion (through your Payment Receiving Preferences) or
            /// manual conversion (through manually accepting a payment).
            /// </summary>
            public const string SETTLE_AMOUNT = "settle_amount";

            /// <summary>
            /// Currency of settle_amount.
            /// </summary>
            public const string SETTLE_CURRENCY = "settle_currency";

            /// <summary>
            /// Shipping charges associated with this transaction.
            /// Format: unsigned, no currency symbol, two decimal places.
            /// </summary>
            public const string SHIPPING = "shipping";

            /// <summary>
            /// The name of a shipping method from the Shipping Calculations section of the merchant's account profile.
            /// The buyer selected the named shipping method for this transaction.
            /// </summary>
            public const string SHIPPING_METHOD = "shipping_method";

            /// <summary>
            /// Amount of tax charged on payment. PayPal appends the number of the item (e.g., item_name1, item_name2).
            /// The taxx variable is included only if there was a specific tax amount applied to a particular shopping cart item.
            /// Because total tax may apply to other items in the cart, the sum of taxx might not total to tax.
            /// </summary>
            public const string TAX = "tax";

            /// <summary>
            /// Authorization and Capture transaction entity.
            /// </summary>
            public const string TRANSACTION_ENTITY = "transaction_entity";

            /// <summary>
            /// Represents the status of the payment.
            /// </summary>
            public static class Status
            {
                /// <summary>
                /// The payment has been completed,
                /// and the funds have been added successfully to your account balance.
                /// </summary>
                public const string COMPLETED = "Completed";

                /// <summary>
                /// A payment has been accepted.
                /// </summary>
                public const string PROCESSED = "Processed";
            }

            /// <summary>
            /// Represents the status of the payer.
            /// </summary>
            public static class PayerStatus
            {
                /// <summary>
                /// Customer has a verified PayPal account.
                /// </summary>
                public const string VERIFIED = "verified";
            }
        }
    }
}