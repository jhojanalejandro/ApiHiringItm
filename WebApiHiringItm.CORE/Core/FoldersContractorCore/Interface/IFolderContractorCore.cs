using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Contratista;

namespace WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface
{
    public interface IFolderContractorCore
    {
        Task<List<FolderDto>> GetAllById(Guid contractorId, Guid contractId);
        Task<FolderDto> GetById(string id);
        Task<bool> Delete(string id);
        Task<bool> Create(FolderDto model);
    }
}
