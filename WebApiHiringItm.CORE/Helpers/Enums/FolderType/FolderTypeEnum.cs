using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.FolderType
{
    public enum FolderTypeCodeEnum
    {
        [Display(Description = "CTT")]
        CONTRATO = 0,
        [Display(Description = "PG")]
        PAGOSCODE = 1,

    }
}
