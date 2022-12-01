using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.ContractorPaymentsCore.Interface;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.ContractorPaymentsCore
{
    public class ContractorPaymentCore : IContractorPaymentsCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;


        public ContractorPaymentCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<ContractorPaymentsDto>> GetAll()
        {
            var result = _context.ContractorPayments.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<ContractorPaymentsDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<ContractorPaymentsDto> GetById(int id)
        {
            var result = _context.ContractorPayments.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<ContractorPaymentsDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            var getData = _context.ContractorPayments.Where(x => x.Id == id).FirstOrDefault();
            if (getData != null)
            {

                var result = _context.ContractorPayments.Remove(getData);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> Create(ContractorPaymentsDto model)
        {
            var getData = _context.ContractorPayments.Where(x => x.Id == model.Id).FirstOrDefault();
            if (getData == null)
            {
                var map = _mapper.Map<ContractorPayments>(model);
                var res = _context.ContractorPayments.Add(map);
                await _context.SaveChangesAsync();
                return map.Id != 0 ? map.Id : 0;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                var res = _context.ContractorPayments.Update(map);
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
