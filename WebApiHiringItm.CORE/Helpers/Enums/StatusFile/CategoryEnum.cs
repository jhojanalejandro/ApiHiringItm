using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.StatusFile
{
    public enum CategoryEnum
    {
        [Display(Description = "CGD")]
        CARGADO = 0,
        [Display(Description = "CRAD")]
        CREADO = 1,

    }
}
