using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApiHiringItm.CORE.Helpers.Enums.File
{
    public enum TypeUserFileEnum
    {
        [Display(Description = "FMA")]
        FIRMA = 0,
        [Display(Description = "IMGC")]
        IMAGENCORREOS = 1,
        [Display(Description = "ACMJ")]
        ARCHIVOSMENSAJE = 2,
    }
}
