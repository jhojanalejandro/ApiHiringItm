using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contratista;

namespace WebApiHiringItm.CORE.Core.HiringDataCore.Interface
{
    public interface IHiringDataCore
    {
        Task<List<HiringDataDto>> GetAll();
        Task<bool> Delete(Guid id);
        Task<IGenericResponse<string>> SaveHiringData(List<HiringDataDto> model);
        Task<IGenericResponse<HiringDataDto>> GetByIdHinringData(string contractorId, string contractId);
        Task<ContractorDateDto?> GetDateContractById(string contractorId, string contractId);
    }
}
