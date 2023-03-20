using System.Collections.Generic;

namespace Common.Model
{
    public class ResponseResult : BaseResponse
    {
        /// <summary>
        ///  Dynamic Result Data
        /// </summary>
        public dynamic Data { get; set; }
        // TO DO -- In future we will add response from base response
    }

    public class ResponseResult<T> : BaseResponse
    {
        /// <summary>
        ///  Result Data
        /// </summary>
        public T Data { get; set; }
        // TO DO -- In future we will add response from base response
    }

    public class ResponseResultList : ResponseResult
    {
        /// <summary>
        ///  Total Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///  URL for Next Page
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        ///  URL for Previous Page
        /// </summary>
        public string Previous { get; set; }
    }


    public class ResponseResultList<T> : ResponseResult<List<T>>
    {
        /// <summary>
        ///  Total Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///  URL for Next Page
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        ///  URL for Previous Page
        /// </summary>
        public string Previous { get; set; }
    }

}
