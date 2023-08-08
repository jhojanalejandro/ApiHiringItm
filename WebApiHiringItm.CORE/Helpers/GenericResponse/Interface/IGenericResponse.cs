using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.GenericResponse.Interface
{
    public interface IGenericResponse<T>
    {
        bool Success { get; set; }
        string Message { get; set; }
        T Data { get; set; }
    }
}
