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
        Task<ProjectFolderDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(RProjectForlderDto model);
        Task<bool> UpdateCost(ProjectFolderCostsDto model);
        Task<List<DetalleContratoDto>> GetDetailById(int idContrato);
        Task<DetalleContratoDto?> GetDetailByIdLastDate(int idContrato);
        Task<List<ProjectFolderDto>> GetAllInProgess(string typeModule);
        Task<List<ProjectFolderDto>> GetAllActivate();
        Task<bool> UpdateState(int id);
    }
}
