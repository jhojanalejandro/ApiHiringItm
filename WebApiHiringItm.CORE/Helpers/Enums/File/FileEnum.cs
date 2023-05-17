using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApiHiringItm.CORE.Helpers.Enums.File
{
    public enum FileEnum
    {
        [Display(Description = "Planilla")]
        PLANILLA = 0,
        [Display(Description = "Informe De Ejecucion")]
        INFORME = 1,
        [Display(Description = "Cuenta De Cobro")]
        CUENTADECOBRO = 2,
        [Display(Description = "Contrato")]
        CONTRATO = 3,
        [Display(Description = "Minuta")]
        MINUTA = 4,
        [Display(Description = "Otros")]
        OTROS = 5,
    }
}
