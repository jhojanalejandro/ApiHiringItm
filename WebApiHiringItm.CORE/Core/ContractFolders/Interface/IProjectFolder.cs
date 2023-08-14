using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
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
        Task<bool> UpdateCost(ProjectFolderCostsDto model);
        Task<List<DetailContractDto>> GetDetailByIdList(Guid ContractId);
        Task<DetailContractDto> GetDetailByIdContract(Guid ContractId);
        Task<DetailContractDto?> GetDetailByIdLastDate(Guid ContractId);
        Task<List<ContractListDto>> GetAllInProgess(string typeModule);
        Task<List<ContractListDto>> GetAllActivate();
        Task<List<ContractFolderDto>> GetAllProjectsRegistered();
        Task<bool> AssignmentUser(List<AssignmentUserDto> modelAssignment);
        Task<bool> SaveTermFileContract(TermContractDto modelTermContract);
        Task<IGenericResponse<string>> SaveContract(RProjectForlderDto model);
        Task<IGenericResponse<string>> UpdateStateContract(string contractId);
    }
}
