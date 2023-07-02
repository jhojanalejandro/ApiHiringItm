using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.File
{
    public enum DocumentTypeEnum
    {
        [Display(Description = "MCNT")]
        MINUTACONTRATISTACODE = 0,

        [Display(Description = "RSC")]
        REGISTROSECOPCODE = 1,

        [Display(Description = "MMCR")]
        MINUTAMACRO = 2,

        [Display(Description = "EXPNL")]
        EXAMENESPREOCUPACIONALESCODE = 3,

        [Display(Description = "HV")]
        HOJADEVIDACODE = 4,

        [Display(Description = "Otros")]
        OTROS = 5,

        [Display(Description = "MNTA")]
        MINUTA = 6,
    }

}
