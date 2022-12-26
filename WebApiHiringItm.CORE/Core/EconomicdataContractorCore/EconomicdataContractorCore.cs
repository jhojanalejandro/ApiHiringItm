using AutoMapper;
using System;
using System.Collections.Generic;
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
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;


        public EconomicdataContractorCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<EconomicdataContractorDto>> GetAll()
        {
            var result = _context.EconomicdataContractor.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<EconomicdataContractorDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<EconomicdataContractorDto> GetById(int id)
        {
            var result = _context.EconomicdataContractor.Where(x => x.ContractorId == id).FirstOrDefault();
            var map = _mapper.Map<EconomicdataContractorDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            var getData = _context.EconomicdataContractor.Where(x => x.Id == id).FirstOrDefault();
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

        public async Task<int> Create(EconomicdataContractorDto model)
        {
            var getData = _context.EconomicdataContractor.FirstOrDefault(x => x.ContractorId == model.ContractorId);
            if (getData == null)
            {
                var map = _mapper.Map<EconomicdataContractor>(model);
                var res = _context.EconomicdataContractor.Add(map);
                await _context.SaveChangesAsync();
                return map.Id != 0 ? map.Id : 0;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                var res = _context.EconomicdataContractor.Update(map);
                await _context.SaveChangesAsync();
                if (res.State != 0)
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}
