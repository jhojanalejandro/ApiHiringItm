using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.Contractors
{
    public class ContractorPaymentCore : IContractorPaymentsCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;


        public ContractorPaymentCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ContractorPaymentsDto>> GetAll()
        {
            var result = _context.ContractorPayments.ToList();
            var map = _mapper.Map<List<ContractorPaymentsDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<ContractorPaymentsDto> GetById(string id)
        {
            var result = _context.ContractorPayments.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
            var map = _mapper.Map<ContractorPaymentsDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(string id)
        {
            var getData = _context.ContractorPayments.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
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

        public async Task<bool> Create(List<ContractorPaymentsDto> model)
        {
            List<ContractorPayments> paymentListAdd = new List<ContractorPayments>();
            List<ContractorPayments> paymentListUpdate = new List<ContractorPayments>();

            var map = _mapper.Map<List<ContractorPayments>>(model);

            for (var i = 0; i < map.Count; i++)
            {
                var getData = _context.ContractorPayments.Where(x => x.FromDate == map[i].FromDate && x.ToDate == map[i].ToDate && x.ContractorId == map[i].ContractorId).FirstOrDefault();
                if (getData != null)
                {
                    var mapData = _mapper.Map(model[i], getData);
                    paymentListUpdate.Add(getData);
                    map.Remove(map[i]);
                    i--;
                }
                else
                {
                    map[i].Id = Guid.NewGuid();
                    paymentListAdd.Add(map[i]);
                }
            }
            if (paymentListUpdate.Count > 0)
                _context.ContractorPayments.UpdateRange(paymentListUpdate);
            if (paymentListAdd.Count > 0)
                _context.ContractorPayments.AddRange(paymentListAdd);
            var result = await _context.SaveChangesAsync();
            return result != 0 ? true : false;

        }
    }
}
