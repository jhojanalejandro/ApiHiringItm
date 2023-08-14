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
        [Display(Description = "RTD")]
        REMITIDO = 0,
        [Display(Description = "APD")]
        APROBADO = 1,
        [Display(Description = "SBND")]
        SUBSANADO = 2,
        [Display(Description = "PRCS")]
        ENPROCESO = 3,

    }
}
