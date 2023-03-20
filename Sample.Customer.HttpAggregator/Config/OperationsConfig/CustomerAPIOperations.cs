
using System;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.Config.OperationsConfig
{
    /// <summary>
    /// User Management Operations
    /// </summary>
    public static class CustomerAPIOperations
    {
        /// <summary>
        /// Root path for User Login
        /// </summary>
        /// <returns></returns>
        public static string Authenticate() => $"/api/authentication/authenticate";

        /// <summary>
        /// Root path for User Login
        /// </summary>
        /// <returns></returns>
        public static string AuthenticateExternal() => $"/api/authentication/authenticateExternalUser";

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public static string Logout() => $"/api/authentication/logout";

        /// <summary>
        /// Token Refresh
        /// </summary>
        /// <returns></returns>
        public static string TokenRefresh() => $"/api/authentication/refresh";

        /// <summary>
        ///  Verify token for User
        /// </summary>
        /// <returns></returns>
        public static string VerifyToken() => $"/api/authentication/verify";


        /// <summary>
        /// Root path for Register User
        /// </summary>
        /// <returns></returns>
        public static string RegisterUser() => $"/api/users/registeruser";

        /// <summary>
        /// Root path for Get User Modules By UserId
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <returns></returns>
        public static string GetUserModulesByUserId(long userId) => $"/api/users/getusermodulesbyuserid?userId=" + userId;

        /// <summary>
        /// Root path for Get User Group By UserId
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <returns></returns>
        public static string GetUserGroupsByUserId(long userId) => $"/api/users/getusergroupsbyuserid?userId=" + userId;

        /// <summary>
        /// Root path for Get module right by UserId
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <param name="moduleId">The ModuleId</param>
        /// <returns></returns>
        public static string CheckModulePermissionByUserId(long userId, long moduleId) => $"/api/users/checkmodulepermissionbyuserid?userId=" + userId + "&moduleId=" + moduleId;

        /// <summary>
        /// Root path for Get User Permissions By UserId
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <returns></returns>
        public static string GetUserPermissionsByUserId(long userId) => $"/api/users/getuserpermissionsbyuserid?userId=" + userId;

        /// <summary>
        /// Root path for Get MfaTypes By UserId
        /// </summary>
        /// <param name="userId">The UserId</param>
        /// <returns></returns>
        public static string GetMfaTypesByUserId(long userId) => $"/api/users/getmfatypesbyuserid?userId=" + userId;

        /// <summary>
        /// BuildUserNavigationByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string BuildUserNavigationByUserId(long userId) => $"/api/users/" + userId + "/buildUserNavigation";


        /// <summary>
        /// Change Password of User
        /// </summary>
        /// <returns></returns>
        public static string ChangeUserPassword() => $"/api/users/changePassword";
        /// <summary>
        /// Change Password By MobileNo
        /// </summary>
        /// <returns></returns>
        public static string ChangePasswordByMobileNo() => $"/api/users/changepasswordbymobileno";

        /// <summary>
        /// Forgot Password of User
        /// </summary>
        /// <returns></returns>
        public static string ForgotUserPassword() => $"/api/users/forgotPassword";

        /// <summary>
        /// Get User Modules
        /// </summary>
        /// <returns></returns>
        public static string GetUserModules(string ordering, int offset, int pageSize, int pageNumber, bool all) => $"/api/users/getUserModules/?ordering=" + ordering + "&offset=" + offset + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&all=" + all;

        /// <summary>
        /// Set Password of User
        /// </summary>
        /// <returns></returns>
        public static string SetPassword() => $"/api/users/setPassword";

        /// <summary>
        /// Root path for Get Password Policy of account
        /// </summary>
        /// <param name="accountId">The accountId</param>
        /// <returns></returns>
        public static string GetPasswordPolicyByAccountId(long accountId) => $"/api/passwordPolicies/?accountId=" + accountId;


        /// <summary>
        /// Root path for creating password policy
        /// </summary>
        /// <returns></returns>
        public static string CreatePasswordPolicy() => $"/api/passwordPolicies";

        /// <summary>
        /// Root path for updating password policy
        /// </summary>
        /// <returns></returns>
        public static string UpdatePasswordPolicy() => $"/api/passwordPolicies/";

        /// <summary>
        /// Root path to get user list
        /// </summary>
        /// <returns></returns>
        public static string GetAllUsers(string ordering, int offset, int pageSize, int pageNumber, bool all) => $"/api/users/?ordering=" + ordering + "&offset=" + offset + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&all=" + all;


        /// <summary>
        /// Root path for Create User
        /// </summary>
        /// <returns></returns>
        public static string CreateUser() => $"/api/users/";

        /// <summary>
        /// To update existing User
        /// </summary>
        /// <returns></returns>
        public static string UpdateUser(long userId) => $"/api/users/" + userId;

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <returns></returns>
        public static string GetUserById(long userId) => $"/api/users/" + userId;
        /// <summary>
        /// GetUserDetails
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetUserDetails(long userId) => $"/api/users/{userId}/GetUserDetails";
        /// <summary>
        /// GetUsersByAccountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static string GetUsersByAccountId(long accountId) => $"/api/users/{accountId}/GetUsersByAccountId";
        /// <summary>
        /// GetGroupByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetGroupByUserId(long userId) => $"/api/groups/{userId}/GetGroupByUserId";

        /// <summary>
        /// To update existing User partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialUser(long userId) => $"/api/users/" + userId;

        /// <summary>
        /// To Delete USer
        /// </summary>
        /// <param name="userId">The userId to delete </param>
        /// <returns></returns>
        public static string DeleteUser(long userId) => $"/api/users/" + userId;

        #region Groups Api's
        /// <summary>
        /// Group Service to get group list
        /// </summary>
        /// <returns></returns>
        public static string GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var rtnURL = $"/api/groups?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            return rtnURL;
        }

        /// <summary>
        /// Get Group By Id
        /// </summary>
        /// <returns></returns>
        public static string GetGroupById(long groupId) => $"/api/groups/" + groupId;

        /// <summary>
        /// Root path for Create Group
        /// </summary>
        /// <returns></returns>
        public static string CreateGroup() => $"/api/groups/";

        /// <summary>
        /// To update existing Group
        /// </summary>
        /// <returns></returns>
        public static string UpdateGroup(long groupId) => $"/api/groups/" + groupId;

        /// <summary>
        /// To update existing Group partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialGroup(long groupId) => $"/api/groups/" + groupId;

        /// <summary>
        /// To Delete Group
        /// </summary>
        /// <param name="groupId">The groupId to delete </param>
        /// <returns></returns>
        public static string DeleteGroup(long groupId) => $"/api/groups/" + groupId;
        #endregion


        /// <summary>
        /// To update existing User
        /// </summary>
        /// <returns></returns>
        public static string VerifyUserMobileNumber(long userId) => $"/api/Users/VerifyUserMobileNumber/" + userId;

        /// <summary>
        /// To update existing User
        /// </summary>
        /// <returns></returns>
        public static string VerifyUserEmailAddress(long userId) => $"/api/Users/VerifyUserEmailAddress/" + userId;


        /// <summary>
        /// Send Mobile OTP
        /// </summary>
        /// <returns></returns>
        public static string SendMobileOTP() => $"/api/Users/SendMobileOTP/";
        /// <summary>
        /// Forget Password By SMS
        /// </summary>
        /// <returns></returns>
        public static string ForgetPasswordBySMS() => $"/api/Users/ForgetPasswordBySMS/";

        /// <summary>
        /// Send Mobile OTP
        /// </summary>
        /// <returns></returns>
        public static string VerifyOTP() => $"/api/Users/VerifyOTP/";

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <returns></returns>
        public static string GetUserProfile() => $"/api/Users/GetUserProfile/";

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <returns></returns>
        public static string UpdateUserProfile() => $"/api/Users/UpdateUserProfile/";

        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <returns></returns>
        public static string UpdateUserProfileImage() => $"/api/Users/UpdateUserProfileImage/";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUserVehicleCategoryFeatures() => $"/api/UserVehicleCategoryFeatures/GetUserVehicleCategoryFeatures";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SaveUserVehicleCategoryFeatures() => $"/api/UserVehicleCategoryFeatures/saveUserVehicleCategoryFeatures";
        
        /// <summary>
        /// To Get the Category And Features
        /// </summary>
        /// <returns></returns>
        public static string GetVehicleCategoryAndFeature() => $"/api/VehicleFeatures/getvehiclecategoryandfeatures/";

        /// <summary>
        /// Send Verification
        /// </summary>
        /// <returns></returns>
        public static string SendEmailVerification() => $"/api/users/SendVerificationCode";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string VerifyEmail() => $"/api/VerifyEmail/Verify";
        /// <summary>
        /// Insert Email Parser FileData
        /// </summary>
        /// <returns></returns>
        public static string InsertEmailParserFileData() => $"/api/EmailParserCallback/insertfiledata/";
        /// <summary>
        /// Update Email Parsert Callback Result
        /// </summary>
        /// <returns></returns>
        public static string UpdateEmailParsertCallbackResult() => $"/api/EmailParserCallback/UpdateCallbackResult/";

        /// <summary>
        /// Get Email Parser Details
        /// </summary>
        /// <returns></returns>
        public static string GetEmailParserDetails(Guid messageId) => $"/api/EmailParserCallback/getdetailsfrommessageid/{messageId}/";


        /// <summary>
        /// Get Email Parser Details
        /// </summary>
        /// <returns></returns>
        public static string GetAllPendingEmailParserDetails() => $"/api/EmailParserCallback/getallpendingemaildetails";

        /// <summary>
        /// Get Email Parser Details
        /// </summary>
        /// <returns></returns>
        public static string MarkedEmailParseDetailDone(long emailParseDetailId) => $"/api/EmailParserCallback/markedemailparsedetaildone/{emailParseDetailId}/";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetVehicleInfo() => $"/api/VehicleInfo/GetVehicleInfo";

        
        #region " Authentication Config Keys Values "
        /// <summary>
        /// To Authentication Config Keys Values
        /// </summary>
        /// <param name="accountId">The accountId to get Authentication Config Keys Values </param>
        /// <returns></returns>
        public static string GetAuthConfigKeyValue(long accountId) => $"/api/authenticationConfigKeysValues/getAuthConfigValuesByAccountId?accountId=" + accountId;
        #endregion


        #region Reservation Api's

        /// <summary>
        /// GetAllReservation
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public static string GetAllReservation(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var rtnURL = $"/api/Reservation/GetAllReservations?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            return rtnURL;
        }

        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public static string GetReservationById(long reservationid) => $"/api/Reservation/GetReservationById/" + reservationid;
        /// <summary>
        /// GenerateReservationInvoiceById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public static string GenerateReservationInvoiceById(long reservationid) => $"/api/Reservation/GenerateReservationInvoiceById/" + reservationid;

        /// <summary>
        /// GenerateReservationById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public static string GenerateReservationById(long reservationid) => $"/api/Reservation/GenerateReservationById/" + reservationid;


        ///// <summary>
        ///// CreateReservation
        ///// </summary>
        ///// <returns></returns>
        //public static string CreateReservation() => $"/api/Reservation/CreateFlightAndParkingReservation/";

        /// <summary>
        /// CreateReservation
        /// </summary>
        /// <returns></returns>
        public static string CalcuatePriceAndAddressDataByAirportID() => $"/api/Reservation/CalcuatePriceAndAddressDataByAirportID/";
        /// <summary>
        /// CreateReservationNew
        /// </summary>
        /// <returns></returns>
        public static string CreateReservationNew() => $"/api/Reservation/createreservationnew/";

        /// <summary>
        /// UpdateReservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public static string UpdateReservation(long reservationid) => $"/api/Reservation/UpdateFlightAndParkingReservation/" + reservationid;

        /// <summary>
        /// Get Email Itinerary Reservation For Review
        /// </summary>
        /// <returns></returns>
        public static string GetEmailItineraryReservationForReview() => $"/api/Reservation/getEmailItineraryReservationForReview/";

        /// <summary>
        /// Confirm Email Itinerary Reservation
        /// </summary>
        /// <param name="flightReservationId"></param>
        /// <returns></returns>
        public static string ConfirmEmailItineraryReservation(long flightReservationId) => $"/api/Reservation/confirmEmailItineraryReservation/" + flightReservationId;


        /// <summary>
        /// DeleteReservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public static string DeleteReservation(long reservationid) => $"/api/Reservation/DeleteReservation/" + reservationid;

        /// <summary>
        /// Get All Trips
        /// </summary>
        /// <returns></returns>
        public static string GetAllOngoingUpcomingTrips() => $"/api/reservation/getallongoingupcomingtrips/";

        /// <summary>
        /// Get all trips
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetAllTrips(string flag) => $"/api/reservation/getalltrips/" + flag;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetAllUpcomingTrip() => $"/api/OngoingUpcomingTrip/getupcomingtrips/";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public static string CancelReservation( long reservationid) => $"/api/Reservation/cancelreservation/"+reservationid;

        #endregion


        /// <summary>
        /// Get All Airport Address
        /// </summary>
        /// <returns></returns>
        public static string GetAllAirportAddress() => $"/api/AirportAddress/getairportaddress/";
        

        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="activityCodeId"></param>
        /// <returns></returns>
        public static string GetRActivityCodeById(long activityCodeId) => $"/api/ActivityCode/GetActivityCodeById/" + activityCodeId;
        /// <summary>
        /// Get All Activity
        /// </summary>
        /// <returns></returns>
        public static string GetAllActivity(long parkingProviderLocationId) => $"/api/ActivityCode/GetAllActivity?parkingProviderLocationId=" + parkingProviderLocationId;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SaveTripPaxAndBags() => $"/api/TripPaxAndBags/SaveTripPaxAndBags/";

        /// <summary>
        /// Get Current Activity Code
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public static string GetCurrentActivityCode(long reservationId) => $"/api/Reservation/GetCurrentActivityCode/?reservationId=" + reservationId;
        
        /// <summary>
        /// UpdateCreateReservationActivtyCode
        /// </summary>
        /// <returns></returns>
        public static string UpdateCreateReservationActivtyCode() => $"/api/Reservation/updatecreatereservationactivtycode";

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        public static string ShuttleETA() => $"/api/Reservation/ShuttleETA/";

        /// <summary>
        /// Get All Cars
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public static string GetAllCars(string flag, long parkingProviderLocationId, int searchType) => $"/api/Vehicles/GetAllCars/" + flag + "/" + parkingProviderLocationId + "/" + searchType;

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        public static string GetReservationVehicleDetailsByVinNumber(long reservationId, string vinNumber) => $"/api/Reservation/GetReservationVehicleDetailsByVinNumber/" + reservationId + "/" + vinNumber;
        /// <summary>
        /// Get Car Count
        /// </summary>
        /// <param name="parkingReservationDate"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        public static string GetCarCount(string parkingReservationDate, long parkingProviderLocationId) => $"/api/Vehicles/GetCarCount/" + parkingReservationDate + "/"+ parkingProviderLocationId;

        /// <summary>
        /// Get Day Wise Count
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        public static string GetDayWiseCount(long parkingProviderLocationId) => $"/api/Vehicles/GetDayWiseCount/" + parkingProviderLocationId;





        /// <summary>
        /// GetVehicleDetailByTagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public static string GetVehicleDetailByTagId(string tagId) => $"/api/Vehicles/GetVehicleDetailByTagId/" + tagId;

        /// <summary>
        /// CreateReservationVehicle
        /// </summary>
        /// <returns></returns>
        public static string CreateReservationVehicle() => $"/api/Vehicles/CreateReservationVehicle/";

        /// <summary>
        /// CreateVehicle
        /// </summary>
        /// <returns></returns>
        public static string CreateVehicle() => $"/api/Vehicles/CreateVehicle/";
        /// <summary>
        /// UpdateParkingReservation
        /// </summary>
        /// <returns></returns>
        public static string UpdateParkingReservation() => $"/api/Reservation/updateparkingreservation/";
        /// <summary>
        /// Create payment Method
        /// </summary>
        /// <returns></returns>
        public static string CreatepaymentMethodNew() => $"/api/PaymentMethod/CreateCustomerPayment/";
        /// <summary>
        /// Check Valid Payment
        /// </summary>
        /// <returns></returns>
        public static string FetchValidPaymentMethod() => $"/api/PaymentMethod/FetchValidPaymentMethod/";
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public static string GetJourneyCompletedlist() => $"/api/reservation/GetJourneyCompletedlist" ;
        /// <summary>
        /// Get outgoing day wise count
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        public static string Getoutgoingdaywisecount(long parkingProviderLocationId) => $"/api/reservation/getoutgoingdaywisecount?parkingProviderLocationId=" + parkingProviderLocationId;
        /// <summary>
        /// Get Analytic Detail
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        public static string GetAnalyticDetail(long parkingProviderLocationId) => $"/api/reservation/getanalyticdetail?parkingProviderLocationId=" + parkingProviderLocationId;
        /// <summary>
        /// GetShuttleBoardedList
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        public static string GetShuttleBoardedList(long terminalId) => $"/api/reservation/getshuttleboardedlist/"+ terminalId;
        /// <summary>
        /// Get Airline List
        /// </summary>
        /// <returns></returns>
        public static string GetAirlineList() => $"/api/airline/getairlinelist/";
        /// <summary>
        /// De-BoardFromShuttle
        /// </summary>
        /// <returns></returns>
        public static string DeBoardFromShuttle() => $"/api/reservation/DeBoardFromShuttle/";
        /// <summary>
        /// GetReservationsByParkingLocationId
        /// </summary>
        /// <returns></returns>
        public static string GetReservationsByParkingLocationId() => $"/api/reservation/getreservationsbyparkinglocationid/";
        /// <summary>
        /// GetReservationsByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetReservationsByUserId(long userId) => $"/api/reservation/getreservationsbyuserid/" + userId;

        #region Parking Provider
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetParkingProvider() => $"/api/qrgenerator/getparkingprovider/";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetActivityCode() => $"/api/qrgenerator/getactivitycode/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public static string GetParkingProviderLocation(long providerId) => $"/api/qrgenerator/getparkingproviderlocation/{providerId}";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        public static string GetSubLocation(long parkingProviderlocationId) => $"/api/qrgenerator/getsublocation/{parkingProviderlocationId}";
        /// <summary>
        /// GetSubLocationType
        /// </summary>
        /// <returns></returns>
        public static string GetSubLocationType() => $"/api/qrgenerator/getsublocationtype/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subLocationId"></param>
        /// <returns></returns>
        public static string GetParkingSpotId(long subLocationId) => $"/api/qrgenerator/getparkingspotid/{subLocationId}";
        /// <summary>
        /// Upload and SaveQR
        /// </summary>
        /// <returns></returns>
        public static string UploadandSaveQR() => $"/api/qrgenerator/uploadandsaveqr/";
        /// <summary>
        /// GetQRList
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        public static string GetQRList(long parkingProviderlocationId) => $"/api/qrgenerator/getqrlist/{parkingProviderlocationId}";
        #endregion
    }
}
