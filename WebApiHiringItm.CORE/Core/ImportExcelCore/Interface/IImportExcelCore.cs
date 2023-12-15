using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.ImportExcelCore.Interface
{
    public interface IImportExcelCore
    {
        Task<string> ImportarExcel(FileRequest obj);
        Task<IGenericResponse<string>> ImportElement(FileRequest model);
        Task<IGenericResponse<string>> ImportCdp(FileRequest model);
    }
}
