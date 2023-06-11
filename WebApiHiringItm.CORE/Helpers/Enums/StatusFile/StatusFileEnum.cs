using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.StatusFile
{
    public enum StatusFileEnum
    {
        [Display(Description = "RE")]
        REMITIDO = 0,
        [Display(Description = "AP")]
        APROBADO = 1,
        [Display(Description = "SU")]
        SUBSANADO = 2,
        [Display(Description = "PR")]
        ENPROCESO = 3,

    }
}
