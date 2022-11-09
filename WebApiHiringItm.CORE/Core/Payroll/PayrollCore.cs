using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Payroll.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.Payroll
{
    public class PayrollCore : IPayrollCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;


        public PayrollCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<PayrollDto>> GetAll()
        {
            var result = _context.PayRoll.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<PayrollDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<PayrollDto> GetById(int id)
        {
            var result = _context.PayRoll.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<PayrollDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            var getData = _context.PayRoll.Where(x => x.Id == id).FirstOrDefault();
            if (getData != null)
            {

                var result = _context.PayRoll.Remove(getData);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> Create(PayrollDto model)
        {
            var getData = _context.PayRoll.Where(x => x.Id == model.Id).FirstOrDefault();
            if (getData == null)
            {
                var map = _mapper.Map<PayRoll>(model);
                var res = _context.PayRoll.Add(map);
                await _context.SaveChangesAsync();
                return map.Id != 0 ? map.Id : 0;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                var res = _context.PayRoll.Update(map);
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
