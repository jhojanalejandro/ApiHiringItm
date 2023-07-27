using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.StatusContractor
{
    public enum StatusContractorEnum
    {
        [Display(Description = "IVTD")]
        INVITADO = 0,
        [Display(Description = "RVS")]
        ENREVISIÓN = 1,
        [Display(Description = "INHTD")]
        INHABILITADO = 2,
        [Display(Description = "CTTD")]
        CONTRATADO = 3,
        [Display(Description = "CTTND")]
        CONTRATANDO = 4,
    }
}
