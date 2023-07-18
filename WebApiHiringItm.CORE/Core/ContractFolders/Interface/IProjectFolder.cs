using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contrato;
using WebApiHiringItm.MODEL.Dto.ContratoDto;

namespace WebApiHiringItm.CORE.Core.ProjectFolders.Interface
{
    public interface IProjectFolder
    {
        Task<List<ContractListDto>> GetAllHistory();
        Task<ContractFolderDto> GetById(Guid id);
        Task<bool> Delete(Guid id);
        Task<bool> SaveContract(RProjectForlderDto model);
        Task<bool> UpdateCost(ProjectFolderCostsDto model);
        Task<List<DetalleContratoDto>> GetDetailByIdList(Guid ContractId);
        Task<DetalleContratoDto> GetDetailByIdContract(Guid ContractId);
        Task<DetalleContratoDto?> GetDetailByIdLastDate(Guid ContractId);
        Task<List<ContractListDto>> GetAllInProgess(string typeModule);
        Task<List<ContractListDto>> GetAllActivate();
        Task<bool> UpdateStateContract(Guid id);
        Task<List<ContractFolderDto>> GetAllProjectsRegistered();
        Task<bool> AssignmentUser(List<AssignmentUserDto> modelAssignment);
        Task<bool> SaveTermFileContract(TermContractDto modelTermContract);
    }
}
