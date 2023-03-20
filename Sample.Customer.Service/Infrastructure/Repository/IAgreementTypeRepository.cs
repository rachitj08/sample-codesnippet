using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IAgreementTypeRepository
    {
        /// <summary>
        /// Get All Records
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<AgreementTypeVM>> GetAllRecords(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <returns></returns> 
        Task<AgreementType> GetRecordById(long id);

        /// <summary>
        /// To Create Record
        /// </summary>
        /// <param name="model">New Object</param>
        /// <returns></returns>
        Task<AgreementType> CreateRecord(AgreementType model);

        /// <summary>
        /// To Update Record
        /// </summary>
        /// <param name="id">Id to Record </param>
        /// <param name="model">Data model object</param>
        /// <returns></returns>
        Task<int> UpdateRecord(long id, AgreementType model);

        /// <summary>
        /// To Delete Record
        /// </summary>
        /// <param name="id">Id to Record </param>
        /// <returns></returns>
        Task<int> DeleteRecord(long id);
    }
}
