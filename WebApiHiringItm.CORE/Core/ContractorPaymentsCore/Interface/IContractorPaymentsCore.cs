using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.ContractorPaymentsCore.Interface
{
    public interface IContractorPaymentsCore
    {
        Task<List<ContractorPaymentsDto>> GetAll();
        Task<ContractorPaymentsDto> GetById(int id);
        Task<int> Create(ContractorPaymentsDto model);
        Task<bool> Delete(int id);
    }
}
