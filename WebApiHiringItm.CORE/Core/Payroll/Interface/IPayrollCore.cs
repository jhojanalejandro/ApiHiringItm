using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.Payroll.Interface
{
    public interface IPayrollCore
    {
        Task<List<PayrollDto>> GetAll();
        Task<PayrollDto> GetById(int id);
        Task<int> Create(PayrollDto model);
        Task<bool> Delete(int id);
    }
}
