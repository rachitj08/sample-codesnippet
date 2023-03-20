namespace Common.Model
{
    /// <summary>
    /// status messages for response 
    /// </summary>
    public static class ResponseMessage
    {
        //General
        public const string SomethingWentWrong = "Something Went Wrong";
        public const string RecordFetched = "Records fetched sucessfully";
        public const string RecordSaved = "Record saved sucessfully";
        public const string NoRecordFound = "No Record Found";
        public const string InternalServerError = "Internal server error.";
        public const string HttpResponseNull = "Get response null from Microservice";
        public const string ValidationFailed = "Validation Failed";
        public const string RecordDeleted = "Record Deleted sucessfully";
        public const string RecordAlreadyExist = "Record Already Exist";
        public const string VehicleDataNotExistWithTag = "Vehicle data not exist with this tag";

        //Login
        public const string Authenticated = "User authenticated sucessfully";
        public const string Unauthorized = "User not authorized";
        public const string SchemaTableNotExist = "Schema Table Not Exist";
        public const string SchemaReferenceTableNotExist = "Foreign key reference not exist for this table";
        public const string MasterConfigurationNotDone = "Forms master table configuration not done";
        public const string AccountServiceConfigurationNotAvaliable = "Account service configuration not avaliable";
        public const string AccountNotExist = "Account not exist";
        public const string RootUserConfiguredSuccessfully = "Root user configured successfully";
        public const string RootUserConfigurationAlreadyExists = "Root user configuration already exists";
        public const string SubscriptionCreated = "Subscription created successfully";

        //EmailVerification
        public const string EmailAlreadyVerified= "Email already verified";
        //QR Code Invalid
        public const string QRCodeInvalid= "QR Code Invalid";

        //Reservation Activity Code
        public const string ReservationActivityCreated = "Reservation Activity Created";
        public const string ReservationActivityNotCreated = "Reservation Activity Not Created";

        public const string ParkingStatus = "Parking Status Updated.";
        public const string PaymentMethodCreated = "Payment created successfully.";
        public const string PaymentMethodNotCreated = "Payment not created successfully.";
        public const string ValidPaymentMethod = "Payment method are Valid.";
        public const string InvalidPaymentMethod = "Payment method are not Valid.";
        public const string CustomerNotExist = "Customer does not exist.";
    }

   
    /// <summary>
    /// Response status code
    /// </summary>
    public static class ResponseCode
    {
        public const string SomethingWentWrong = "000";
        public const string HttpResponseNull = "009";
        // Login code 001 to 010
       
        public const string ValidLogin = "001";
        public const string Unauthorized = "002";
        public const string PasswordExpired = "003";
        public const string NoUser = "004";

        // General code 011 to ...
        public const string InternalServerError = "011";
        public const string RecordFetched = "012";
        public const string NoRecordFound = "013";
        public const string RecordSaved = "014";
        public const string ValidationFailed = "015";
        public const string RecordDeleted = "016";
        public const string SchemaTableNotExist = "017";
        public const string SchemaReferenceTableNotExist = "018";
        public const string MasterConfigurationNotDone = "019";
        public const string AccountServiceConfigurationNotAvaliable = "020";
        public const string AccountNotExist = "021";
        public const string RootUserConfiguredSuccessfully = "022";
        public const string RootUserConfigurationAlreadyExists = "023";
        public const string SubscriptionCreated = "024";
        public const string RecordAlreadyExist = "025";
        public const string QRCodeInvalid = "026";
        

        //Reservation Activity Code
        public const string ReservationActivityCreated = "027";
        public const string ReservationActivityNotCreated = "028";
        public const string VehicleDataNotExistWithTag = "029";

        public const string ParkingStatus = "030";
        public const string PaymentMethodCreated = "031";
        public const string PaymentMethodNotCreated = "032";
        public const string ValidPaymentMethod = "033";
        public const string InvalidPaymentMethod = "034";
        public const string CustomerNotExist = "035";
    }
}
