using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.FileDto;

namespace WebApiHiringItm.CORE.Core.FileCore.Interface
{
    public interface IFilesCore
    {
        Task<List<FilesDto>> GetFileContractorByFolder(int contractorId, int folderId, int contractId);
        Task<List<FilesDto>> GetAllByContract(int contractorId, int contractId);

        Task<FilesDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(FilesDto model);
        Task<List<GetFilesPaymentDto>> GetAllByDate(int contractId, string type, string date);
        Task<List<FilesDto>> GetAllFileByIdContract(int id);
        Task<bool> CreateDetail(DetailFileDto model);
        Task<List<GetFilesPaymentDto>> GetAllByType(GetFilesPaymentDto model);
    }
}
