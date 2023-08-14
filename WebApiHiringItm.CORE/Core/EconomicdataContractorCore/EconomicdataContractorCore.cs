using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.EconomicdataContractorCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.EconomicdataContractorCore
{
    public class EconomicdataContractorCore : IEconomicdataContractorCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;


        public EconomicdataContractorCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<EconomicdataContractorDto>> GetAll()
        {
            var result = _context.EconomicdataContractor.ToList();
            var map = _mapper.Map<List<EconomicdataContractorDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<EconomicdataContractorDto>> GetEconiomicDataById(EconomicDataRequest economicData)
        {

                var getEconomicData = _context.EconomicdataContractor
                .Include(i => i.DetailContractor)
                    .ThenInclude(i => i.HiringData)
                .Where(x => economicData.Contractors.Contains(x.DetailContractor.ContractorId) && x.DetailContractor.ContractId.Equals(Guid.Parse(economicData.ContractId)));

                return await getEconomicData.Select(s => new EconomicdataContractorDto()
                {
                    Id = s.Id,
                    ContractorId = s.DetailContractor.ContractorId,
                    ContractId = s.DetailContractor.ContractorId,
                    TotalPaidMonth = s.TotalPaIdMonth,
                    TotalValue = Math.Ceiling(s.TotalValue.Value),
                    CashPayment = s.CashPayment,
                    Debt = s.Debt,
                    ModifyDate = s.ModifyDate,
                    RegisterDate = s.RegisterDate,
                    UnitValue = s.UnitValue,
                    Freed = s.Freed,
                    Missing = s.Missing,
                    PeriodFrom = s.DetailContractor.ContractorPayments.OrderByDescending(o => o.ToDate).Select(s => s.ToDate).FirstOrDefault(),
                })
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<bool> Delete(string id)
        {
            var getData = _context.EconomicdataContractor.FirstOrDefault(x => x.Id.Equals(Guid.Parse(id)));
            if (getData != null)
            {
                var result = _context.EconomicdataContractor.Remove(getData);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IGenericResponse<string>> AddEconomicData(List<EconomicdataContractorDto> model)
        {
            List<EconomicdataContractor> economicDataListAdd = new List<EconomicdataContractor>();
            List<EconomicdataContractor> economicDataListUpdate = new List<EconomicdataContractor>();

            for (var i = 0; i < model.Count; i++)
            {
                var getDetailContractor = _context.DetailContractor.Where(w => w.ContractId.Equals(model[i].ContractId) && w.ContractorId.Equals(model[i].ContractorId)).OrderByDescending(o => o.Consecutive).FirstOrDefault();
                var mapEconomicData = _mapper.Map<EconomicdataContractor>(model[i]);

                var getData = _context
                    .EconomicdataContractor
                    .Include(i => i.DetailContractor)
                    .OrderByDescending(o => o.Consecutive)
                    .FirstOrDefault(x => x.DetailContractor.ContractorId.Equals(model[i].ContractorId) && x.DetailContractor.ContractId.Equals(model[i].ContractId));

                if (getData != null && model[i].TotalValue != null && model[i].TotalValue > 0)
                {
     
                    model[i].Id = getData.Id;
                    model[i].DetailContractorId = getData.DetailContractorId;
                    var mapData = _mapper.Map(model[i], getData);
                    economicDataListUpdate.Add(mapData);
                }
                else
                {
                    mapEconomicData.Id = Guid.NewGuid();
                    mapEconomicData.DetailContractorId = getDetailContractor.Id;
                    mapEconomicData.Consecutive = 1;
                    economicDataListAdd.Add(mapEconomicData);
                }
            }
            if (economicDataListUpdate.Count > 0)
                _context.EconomicdataContractor.UpdateRange(economicDataListUpdate);
            if (economicDataListAdd.Count > 0)
                _context.EconomicdataContractor.AddRange(economicDataListAdd);
            await _context.SaveChangesAsync();
            if (economicDataListUpdate.Count > 0)
            {
                return ApiResponseHelper.CreateResponse<string>(null,true,Resource.UPDATESUCCESSFULL);
            }
            else
            {
                return ApiResponseHelper.CreateResponse<string>(Resource.REGISTERSUCCESSFULL);
            }
        }

    }
}
