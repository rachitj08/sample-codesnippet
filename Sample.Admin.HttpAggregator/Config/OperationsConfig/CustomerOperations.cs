using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.Config.OperationsConfig
{
    /// <summary>
    /// 
    /// </summary>
    public static class CustomerOperations
    {
        /// <summary>
        /// Root path for Setup Default Tenet User Configuration
        /// </summary>
        /// <returns></returns>
        public static string SetupDefaultTenetUserConfiguration() => $"/api/users/RootUserSetup";
    }
}
