using AutoMapper;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class ParkingHeadsService : IParkingHeadsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IParkingHeadsRepository _parkingHeadsRepoitory;
        private readonly IParkingHeadsRateRepository _parkingHeadsRateRepository;
        private readonly IParkingProvidersLocationsService _parkingProvidersLocationsService;

        public ParkingHeadsService(IUnitOfWork unitOfWork, IParkingHeadsRepository parkingHeadsRepoitory,
            IParkingProvidersLocationsService parkingProvidersLocationsService, IParkingHeadsRateRepository parkingHeadsRateRepository)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(parkingHeadsRepoitory), parkingHeadsRepoitory);
            Check.Argument.IsNotNull(nameof(parkingProvidersLocationsService), parkingProvidersLocationsService);
            Check.Argument.IsNotNull(nameof(parkingHeadsRateRepository), parkingHeadsRateRepository);

            this.unitOfWork = unitOfWork;
            _parkingHeadsRepoitory = parkingHeadsRepoitory;
            _parkingProvidersLocationsService = parkingProvidersLocationsService;
            _parkingHeadsRateRepository = parkingHeadsRateRepository;
        }

        public ResponseResultList<ParkingHeadsVM> GetAllParkingHeads()
        {
            ResponseResultList<ParkingHeadsVM> objResponseResultResevation = new ResponseResultList<ParkingHeadsVM>();
            List<ParkingHeadsVM> lstParkingHeadsVM = new List<ParkingHeadsVM>();
            List<ParkingHeads> result = _parkingHeadsRepoitory.GetAllParkingHeads();
            if (result != null && result.Count() > 0)
            {
                foreach (var items in result)
                {
                    ParkingHeadsVM ojParkingHeadsVM = new ParkingHeadsVM();
                    ojParkingHeadsVM.ParkingHeadId = items.ParkingHeadId;
                    ojParkingHeadsVM.IsActive = items.IsActive;
                    ojParkingHeadsVM.SeqNo = items.SeqNo;
                    ojParkingHeadsVM.HeadName = items.HeadName;
                    ojParkingHeadsVM.Type = items.Type;
                    ojParkingHeadsVM.Description = items.Description;
                    ojParkingHeadsVM.BasisOn = items.BasisOn;
                    ojParkingHeadsVM.IsGovtFees = items.IsGovtFees;
                    ojParkingHeadsVM.AccountId = items.AccountId;
                    ojParkingHeadsVM.CreatedOn = items.CreatedOn;
                    ojParkingHeadsVM.CreatedBy = items.CreatedBy;
                    ojParkingHeadsVM.UpdatedOn = items.UpdatedOn;
                    ojParkingHeadsVM.UpdatedBy = items.UpdatedBy;
                    lstParkingHeadsVM.Add(ojParkingHeadsVM);
                }
                objResponseResultResevation = new ResponseResultList<ParkingHeadsVM>
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Data = lstParkingHeadsVM
                };
            }
            else
            {
                objResponseResultResevation = new ResponseResultList<ParkingHeadsVM>
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound
                };
            }
            return objResponseResultResevation;
        }

        public ParkingHeadsVM CreateParkingHeads(ParkingHeadsVM objParkingHeads, long accountId)
        {
            throw new NotImplementedException();
        }

        public void DeleteParkingHeads(ParkingHeadsVM objParkingHeads)
        {
            throw new NotImplementedException();
        }


        public ParkingHeadsVM GetParkingHeadsById(long parkingHeadsId)
        {
            throw new NotImplementedException();
        }

        public ParkingHeadsVM UpdateParkingHeads(long parkingHeadid, long accountId, ParkingHeadsVM objParkingHeads)
        {
            throw new NotImplementedException();
        }
        
        public ResponseResult<List<InvoicePriceDetailVM>> GetParkingPriceDetail(long parkingProviderLocationId, DateTime reservationStartDateTime, DateTime reservationEndDateTime, long accountId,bool isCustom,long reservationId)
        {
            if (parkingProviderLocationId < 1 || reservationStartDateTime == null || reservationEndDateTime == null || accountId < 1)
            {
                return new ResponseResult<List<InvoicePriceDetailVM>>()
                {
                    Message = "Invalid request",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            List<ParkingHeadsRateDetailVM> parkingHeadsRate;
            if (isCustom && reservationId > 0)
            {
                parkingHeadsRate = _parkingHeadsRateRepository.GetParkingCustomHeadsRateList(reservationEndDateTime, parkingProviderLocationId, accountId, reservationId);
            }
            else
            {
                // Get Parking Rates
                parkingHeadsRate = _parkingHeadsRateRepository.GetParkingHeadsRateList(reservationEndDateTime, parkingProviderLocationId, accountId);
            }
            if (parkingHeadsRate == null || parkingHeadsRate.Count < 1)
            {
                return new ResponseResult<List<InvoicePriceDetailVM>>()
                {
                    Message = "Could not able to get Parking Heads Rate details",
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }

            //15-Sep-2022 11:30 PM - 17-Sep-2022 11:50 AM= 3Days; 15-Sep-2022 1:30 PM - 15-Sep-2022 11:50 PM = 1 Days;
            short noOfDays = Convert.ToInt16((reservationEndDateTime.Date - reservationStartDateTime.Date).Days);
            noOfDays += 1;

            List<InvoicePriceDetailVM> priceDetails = new List<InvoicePriceDetailVM>();
            parkingHeadsRate = parkingHeadsRate.OrderBy(x => x.BasisOn).ToList();

            foreach(var rateDetails in parkingHeadsRate)
            {
                decimal amount = 0;
                short qty = 0;
                decimal discountAmount = 0;
                switch (rateDetails.Type.ToLower())
                {
                    case "daily":
                        amount = rateDetails.Rate * noOfDays;
                        qty = noOfDays;
                        break;

                    case "flat":
                        amount = rateDetails.Rate;
                        qty = 1;
                        break;

                    case "percentage":
                        if (rateDetails.BasisOn != null && rateDetails.BasisOn > 0)
                        {
                            var basisOnPrice = priceDetails.Where(x => x.ParkingHeadId == rateDetails.BasisOn).FirstOrDefault();
                            if (basisOnPrice != null && basisOnPrice.Amount > 0)
                            {
                                amount = ((basisOnPrice.Amount - (basisOnPrice.DiscountAmount ?? 0)) * rateDetails.Rate) / 100;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        qty = 1;
                        break;

                    default:
                        continue;
                }
                
                string discountType = "";
                if (rateDetails.MaxDiscountDollars > 0)
                {
                    discountAmount = rateDetails.MaxDiscountDollars;
                    discountType = "Flat";
                }
                else if (rateDetails.MaxDiscountPercentage > 0)
                {
                    discountAmount = (Convert.ToDecimal(rateDetails.MaxDiscountPercentage) * amount) / 100;
                    discountType = "Percentage";
                }

                var priceDetail = new InvoicePriceDetailVM()
                {
                    Rate = rateDetails.Rate,
                    Qty = qty,
                    Amount = amount,
                    SeqNo = rateDetails.SeqNo,
                    Description = rateDetails.Description,
                    Type = rateDetails.Type,
                    DiscountAmount = discountAmount,
                    DiscountType = discountType,
                    ParkingHeadId = rateDetails.ParkingHeadId,
                    ParkingHeadRateId = rateDetails.ParkingHeadsRateId
                };
                priceDetails.Add(priceDetail);
            }

            return new ResponseResult<List<InvoicePriceDetailVM>>()
            {
                Message = ResponseCode.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = priceDetails
            };
        }
    }
}
