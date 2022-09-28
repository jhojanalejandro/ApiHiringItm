using Microsoft.AspNetCore.Http;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.ExcelCore.interfaces
{
    public interface IUploadExcelCore
    {
        Task<string> ImportarExcel(FileRequest file);
    }
}
