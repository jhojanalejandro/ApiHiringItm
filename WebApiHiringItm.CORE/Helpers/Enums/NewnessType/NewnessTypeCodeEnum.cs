using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.Hiring
{
    public enum NewnessTypeCodeEnum
    {
        [Display(Description = "DEDD")]
        DESPEDIDO = 0,

        [Display(Description = "RNCA")]
        RENUNCIA = 1,

        [Display(Description = "CANCE")]
        CANCELADO = 2,

        [Display(Description = "NCONT")]
        NOCONTRATADO = 3,

        [Display(Description = "RCONT")]
        RECONTRATAR = 4,

    }
}
