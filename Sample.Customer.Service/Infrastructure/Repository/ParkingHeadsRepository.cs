using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// ParkingHeadsRepoitory
    /// </summary>
    public class ParkingHeadsRepository : RepositoryBase<ParkingHeads>, IParkingHeadsRepository
    {
        public ParkingHeadsRepository(CloudAcceleratorContext context) : base(context)
        {

        }
        /// <summary>
        /// CreateParkingHeads
        /// </summary>
        /// <param name="objParkingHeads"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public ParkingHeads CreateParkingHeads(ParkingHeads objParkingHeads, long accountId)
        {
            return base.Create(objParkingHeads);
        }

        /// <summary>
        /// UpdateParkingHeads
        /// </summary>
        /// <param name="ParkingHeadsid"></param>
        /// <param name="accountId"></param>
        /// <param name="objParkingHeads"></param>
        /// <returns></returns>
        public ParkingHeads UpdateParkingHeads(long parkingHeadId, long accountId, ParkingHeads objParkingHeads)
        {
            return base.Update(objParkingHeads);
        }

        /// <summary>
        /// GetAllParkingHeadss
        /// </summary>
        /// <returns></returns>
        public List<ParkingHeads> GetAllParkingHeads()
        {
            return base.GetAll().ToList();
            //var lstParkingHeadsData = base.context.ParkingHeads.Include(x => x.ParkingHeadsRate).ToList();
            //return lstParkingHeadsData;
        }

        /// <summary>
        /// Get All Parking Headss
        /// </summary>
        /// <returns></returns>
        public async Task<List<ParkingHeads>> GetParkingHeadsList(long accountId)
        {
            return await GetAll(x=> x.AccountId == accountId && x.IsActive);
        }

        /// <summary>
        /// GetParkingHeadsById
        /// </summary>
        /// <param name="ParkingHeadsId"></param>
        /// <returns></returns>
        public ParkingHeads GetParkingHeadsById(long parkingHeadId)
        {
            return base.GetById(parkingHeadId);
        }

        /// <summary>
        /// DeleteParkingHeads
        /// </summary>
        /// <param name="objParkingHeads"></param>
        /// <returns></returns>
        public void DeleteParkingHeads(ParkingHeads objParkingHeads)
        {
             base.Delete(objParkingHeads);
        }
    }
}
