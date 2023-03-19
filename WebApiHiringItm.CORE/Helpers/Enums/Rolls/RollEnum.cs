using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.Rolls
{
    public enum RollEnum
    {
        [Display(Description = "ADM")]
        Admin = 0,
        [Display(Description = "CTL")]
        Contractual = 1,
        [Display(Description = "NMA")]
        Nomina = 2,
        [Display(Description = "PLNC")]
        Planeacion = 3,
        [Display(Description = "JURD")]
        Juridico = 4,
        [Display(Description = "CMT")]
        Comite = 5,
        [Display(Description = "SPV")]
        Supervisor = 6,
        [Display(Description = "CTA")]
        Contratista = 7,
        [Display(Description = "DTV")]
        Desactivada = 8,

    }
}
