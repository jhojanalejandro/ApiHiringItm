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
        [Display(Description = "MNT")]
        MINUTACODE = 0,
        [Display(Description = "RSC")]
        REGISTROSECOPCODE = 1,
        [Display(Description = "EXPNL")]
        EXAMENESPREOCUPACIONALESCODE = 3,
        [Display(Description = "HV")]
        HOJADEVIDACODE = 4,
        [Display(Description = "PNLL")]
        PLANILLA = 5,
        [Display(Description = "CNTCB")]
        CUENTACOBRO = 5,
        [Display(Description = "INFEJ")]
        INFORMEEJECUCIÓN = 6,
        [Display(Description = "ETPR")]
        ESTUDIOSPREVIOS = 7,
        [Display(Description = "SLCMT")]
        SOLICITUDCOMITE = 8,
        [Display(Description = "ANX")]
        ANEXO = 9,
    }

}
