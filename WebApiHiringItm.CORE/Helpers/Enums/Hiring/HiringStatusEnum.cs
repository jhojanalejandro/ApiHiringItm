using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.Hiring
{
    public enum HiringStatusEnum
    {
        [Display(Description = "CONTRATANDO")]
        CONTRATANDO = 0,

        [Display(Description = "EN ESPERA")]
        ENESPERA = 1,

        [Display(Description = "EN PROCESO")]
        ENPROCESO = 2,
    }
}
