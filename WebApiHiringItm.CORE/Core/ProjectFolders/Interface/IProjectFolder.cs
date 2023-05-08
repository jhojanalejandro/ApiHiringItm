using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.ContratoDto;

namespace WebApiHiringItm.CORE.Core.ProjectFolders.Interface
{
    public interface IProjectFolder
    {
        Task<List<ProjectFolderDto>> GetAll();
        Task<ProjectFolderDto> GetById(Guid id);
        Task<bool> Delete(Guid id);
        Task<bool> Create(RProjectForlderDto model);
        Task<bool> UpdateCost(ProjectFolderCostsDto model);
        Task<List<DetalleContratoDto>> GetDetailByIdList(Guid idContrato);
        Task<DetalleContratoDto> GetDetailById(Guid idContrato);
        Task<DetalleContratoDto?> GetDetailByIdLastDate(Guid idContrato);
        Task<List<ProjectFolderDto>> GetAllInProgess(string typeModule);
        Task<List<ProjectFolderDto>> GetAllActivate();
        Task<bool> UpdateState(Guid id);
        Task<List<ProjectFolderDto>> GetAllProjectsRegistered();
    }
}
