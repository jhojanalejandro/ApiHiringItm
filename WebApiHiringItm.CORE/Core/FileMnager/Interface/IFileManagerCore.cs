using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.FileManagerDo;

namespace WebApiHiringItm.CORE.Core.FileMnager.Interface
{
    public interface IFileManagerCore
    {
        Task<FileManagerDto> GetFolderFilesContract(Guid id);
        Task<FolderManagerContractDto> GetAllContract();
        Task<List<FilesDto>?> GetFileContract(Guid id);
    }
}
