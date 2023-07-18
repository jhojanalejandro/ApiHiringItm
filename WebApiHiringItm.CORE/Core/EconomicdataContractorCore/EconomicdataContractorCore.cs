using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.EconomicdataContractorCore.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

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

        public async Task<List<EconomicdataContractorDto>> GetById(Guid?[] id)
        {
            List<EconomicdataContractorDto> economicDataContractorList = new List<EconomicdataContractorDto>();
            var result = _context.DetailContractor
                .Where(x => id.Contains(x.ContractorId))
                .Select(s => new EconomicdataContractorDto
                {
                    Id = s.EconomicdataNavigation.Id,
                    ContractorId = s.ContractorId,
                    ContractId = s.ContractorId,
                    TotalPaidMonth = s.EconomicdataNavigation.TotalPaIdMonth,
                    TotalValue = s.EconomicdataNavigation.TotalValue,
                    CashPayment = s.EconomicdataNavigation.CashPayment,
                    Debt = s.EconomicdataNavigation.Debt,
                    ModifyDate = s.EconomicdataNavigation.ModifyDate,
                    RegisterDate = s.EconomicdataNavigation.RegisterDate,
                    UnitValue = s.EconomicdataNavigation.UnitValue,
                    Freed = s.EconomicdataNavigation.Freed,
                    Missing = s.EconomicdataNavigation.Missing,
                })
                .AsNoTracking()
                .FirstOrDefault();
            if (result != null)
            {
                var map = _mapper.Map<EconomicdataContractorDto>(result);
                map.Id = Guid.NewGuid();
                economicDataContractorList.Add(map);
            }
            return await Task.FromResult(economicDataContractorList);
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

        public async Task<bool> AddEconomicData(List<EconomicdataContractorDto> model)
        {
            List<EconomicdataContractor> economicDataListAdd = new List<EconomicdataContractor>();
            List<EconomicdataContractor> economicDataListUpdate = new List<EconomicdataContractor>();
            var map = _mapper.Map<List<EconomicdataContractor>>(model);

            try
            {
                for (var i = 0; i < map.Count; i++)
                {
                    var getData = _context
                        .DetailContractor
                        .Include(i => i.EconomicdataNavigation)
                        .OrderByDescending(o => o.Consecutive)
                        .FirstOrDefault(x => x.ContractorId.Equals(model[i].ContractorId) && x.ContractId.Equals(model[i].ContractId));
                    if (getData.Economicdata != null)
                    {
                        model[i].Id = getData.Id;
                        var mapData = _mapper.Map(model[i], getData);
                        economicDataListUpdate.Add(mapData.EconomicdataNavigation);
                        map.Remove(map[i]);
                        i--;
                    }
                    else
                    {
                        map[i].Id = Guid.NewGuid();
                        economicDataListAdd.Add(map[i]);
                        getData.Economicdata = map[i].Id;
                    }
                }
                if (economicDataListUpdate.Count > 0)
                    _context.EconomicdataContractor.UpdateRange(economicDataListUpdate);
                if (economicDataListAdd.Count > 0)
                    _context.EconomicdataContractor.AddRange(economicDataListAdd);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }
        }

    }
}
