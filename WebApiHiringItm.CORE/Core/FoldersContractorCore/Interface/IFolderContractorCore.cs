using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface
{
    public interface IFolderContractorCore
    {
        Task<List<FolderContractorDto>> GetAllById(int idContractor);
        Task<FolderContractorDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(FolderContractorDto model);
    }
}
