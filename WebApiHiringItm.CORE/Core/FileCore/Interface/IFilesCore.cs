using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.FileCore.Interface
{
    public interface IFilesCore
    {
        Task<List<FilesDto>> GetAllById(GetFileDto model);
        Task<FilesDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(FilesDto model);
        Task<List<GetFilesPaymentDto>> GetAllByDate(GetFilesPaymentDto model);
    }
}
