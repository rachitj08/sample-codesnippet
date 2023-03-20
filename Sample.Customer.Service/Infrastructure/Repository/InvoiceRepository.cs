using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public async Task<int> CreateInvoice(Invoice model)
        {
            return await CreateAndSaveAsync(model);
        }


        public async Task<int> CreateInvoice(InvoiceVM model, long accountId, long userId)
        {
            string query = @$"call customer.sp_insert_invoicedetails({accountId}::bigint, {model.InvoiceType}::integer,
                            '{model.InvoiceDate}':: timestamp with time zone,'{model.InvoiceNo}'::character varying,
                             {model.ParkingReservationId}::bigint,{model.TotalAmount}::money, {userId}::bigint,
                            '{JsonConvert.SerializeObject(model.InvoiceDetails)}'::character varying,'{model.InvoicePath}'::character varying)";

            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                await connection.ExecuteAsync(query);
                return 1;
            }
        }

        
       
    }
}
