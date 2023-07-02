using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.StatusFile
{
    public enum TypeUserFileEnum
    {
        [Display(Description = "FMA")]
        FIRMA = 0,
        [Display(Description = "IMGC")]
        IMAGENCORREOS = 1,
    }
}
