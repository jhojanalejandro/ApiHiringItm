using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.StatusContract
{
    public enum StatusContractEnum
    {
        [Display(Description = "INID")]
        INICIADO = 0,
        [Display(Description = "PCS")]
        ENPROCESO = 1,
        [Display(Description = "TMD")]
        TERMINADO = 2,
    }
}
