using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model
{
    public class ErrorResponseResult
    {   
        public string Message { get; set; }
        public Dictionary<string, string[]> Detail { get; set; }
    }
}
