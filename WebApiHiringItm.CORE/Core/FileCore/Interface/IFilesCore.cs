using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.Usuario;

namespace WebApiHiringItm.CORE.Core.FileCore.Interface
{
    public interface IFilesCore
    {
        Task<List<FilesDto>> GetFileContractorByFolder(Guid contractorId, string folderId, Guid contractId);
        Task<List<FilesDto>> GetAllByContract(Guid contractorId, Guid contractId);

        Task<FilesDto> GetById(string id);
        Task<bool> Delete(string id);
        Task<bool> Create(FilesDto model);
        Task<List<GetFilesPaymentDto>> GetAllByDate(Guid contractId, string type, string date);
        Task<List<FilesDto>> GetAllFileByIdContract(Guid id);
        Task<bool> CreateDetail(DetailFileDto model);
        Task<bool> Addbill(FilesDto model);
    }
}
