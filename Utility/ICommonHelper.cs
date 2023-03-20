using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public interface ICommonHelper
    {
        Dictionary<string, string> GetTenantTheme(string cssValue);

        string EncryptString(string input, string key);

        string DecryptString(string input, string key);
        string DecryptStringForPayment(string encryptedValue, string key);
    }
}
