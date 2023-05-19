using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.ImportExcelCore.Interface
{
    public interface IImportExcelCore
    {
        Task<string> ImportarExcel(FileRequest obj);
        Task<string> ImportCdp(FileRequest model);
    }
}
