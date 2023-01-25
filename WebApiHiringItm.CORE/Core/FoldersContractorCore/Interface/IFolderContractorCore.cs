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
        Task<List<FolderContractorDto>> GetAllById(Guid contractorId, Guid contractId);
        Task<FolderContractorDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(FolderContractorDto model);
    }
}
