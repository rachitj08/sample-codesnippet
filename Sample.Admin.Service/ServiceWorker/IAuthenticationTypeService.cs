﻿using Sample.Admin.Service.Infrastructure.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IAuthenticationTypeService
    {
        /// <summary>
        /// Get All Authentication Types
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AuthenticationTypes>> GetAllAuthenticationTypes();
    }
}
