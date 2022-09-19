using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core
{
    public class ContractorCore: IContractorCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;

        public ContractorCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ContractorDto>> GetAll()
        {
            var result = _context.Contractor.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<ContractorDto>>(result);
            return await Task.FromResult(map);
            
        }

        public async Task<List<ContractorDto>> GetByIdFolder(int id)
        {
            var contractor = _context.Contractor.Where(x => x.IdFolder == id).ToList();
            var map = _mapper.Map<List<ContractorDto>>(contractor);
            return await Task.FromResult(map);
        }
        public async Task<ContractorDto> GetById(int id)
        {
            var result = _context.Contractor.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<ContractorDto>(result);
            return await Task.FromResult(map);
        }
  
        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.Contractor.Where(x => x.Id == id).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.Contractor.Remove(resultData);
                    await _context.SaveChangesAsync();

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
            return false;
        }

        public async Task<bool> Create(ContractorDto model)
        {
            var getData = _context.Contractor.Where(x => x.Id == model.Id).FirstOrDefault();
            if (getData == null)
            {
                var map = _mapper.Map<Contractor>(model);
                _context.Contractor.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.Contractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;

        }
    }
}
