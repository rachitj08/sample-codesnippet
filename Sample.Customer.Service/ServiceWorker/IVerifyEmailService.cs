using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.Service.ServiceWorker
{
   
    public interface IVerifyEmailService
    {
       
        Task<ResponseResult<SuccessMessageModel>> ConfirmEmail(VerifyEmailVM emailVerificationVM,long loogedInAccountId,long loggedInUserId);
    }
}
