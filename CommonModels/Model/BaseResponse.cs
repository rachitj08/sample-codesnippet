namespace Common.Model
{
   public class BaseResponse
    {
        /// <summary>
        /// status response code 
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        ///  Response Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  Error Message
        /// </summary>
        public ErrorResponseResult Error { get; set; }
    }
}
