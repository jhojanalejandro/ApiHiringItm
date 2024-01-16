using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto.Contratista;

namespace WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface
{
    public interface IFolderContractorCore
    {
        Task<List<FolderDto>> GetAllFolderById(Guid contractorId, Guid contractId);
        Task<FolderDto> GetById(string id);
        Task<bool> SaveFolderContract(FolderDto model);
        Task<IGenericResponse<string>> Delete(string folderId);

    }
}
