using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Contrato;
using WebApiHiringItm.MODEL.Dto.ContratoDto;

namespace WebApiHiringItm.CORE.Core.ProjectFolders.Interface
{
    public interface IProjectFolder
    {
        Task<List<ContractListDto>> GetAllHistory();
        Task<ContractFolderDto> GetById(Guid id);
        Task<bool> Delete(Guid id);
        Task<bool> Create(RProjectForlderDto model);
        Task<bool> UpdateCost(ProjectFolderCostsDto model);
        Task<List<DetalleContratoDto>> GetDetailByIdList(Guid ContractId);
        Task<DetalleContratoDto> GetDetailById(Guid ContractId);
        Task<DetalleContratoDto?> GetDetailByIdLastDate(Guid ContractId);
        Task<List<ContractListDto>> GetAllInProgess(string typeModule);
        Task<List<ContractListDto>> GetAllActivate();
        Task<bool> UpdateState(Guid id);
        Task<List<ContractFolderDto>> GetAllProjectsRegistered();
    }
}
