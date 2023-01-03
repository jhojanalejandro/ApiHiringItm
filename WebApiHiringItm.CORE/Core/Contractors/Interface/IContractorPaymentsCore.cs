using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Contratista;

namespace WebApiHiringItm.CORE.Core.Contractors.Interface
{
    public interface IContractorPaymentsCore
    {
        Task<List<ContractorPaymentsDto>> GetAll();
        Task<ContractorPaymentsDto> GetById(int id);
        Task<bool> Create(List<ContractorPaymentsDto> model);
        Task<bool> Delete(int id);
    }
}
