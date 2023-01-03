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

        public async Task<bool> Create(List<ContractorPaymentsDto> model)
        {
            List<ContractorPayments> paymentListAdd = new List<ContractorPayments>();
            List<ContractorPayments> paymentListUpdate = new List<ContractorPayments>();

            var map = _mapper.Map<List<ContractorPayments>>(model);

            try
            {
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
                        paymentListAdd.Add(map[i]);
                    }
                }
                if (paymentListUpdate.Count > 0)
                    _context.ContractorPayments.UpdateRange(paymentListUpdate);
                if (paymentListAdd.Count > 0)
                    _context.ContractorPayments.AddRange(paymentListAdd);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex )
            {
                throw new Exception("Error", ex);

            }

        }
    }
}
