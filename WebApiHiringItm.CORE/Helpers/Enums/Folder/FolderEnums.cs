using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApiHiringItm.CORE.Helpers.Enums.Folder
{
    public enum FolderEnums
    {
        [Display(Description = "Cargar a gmas")]
        SUBIRGMAS = 0,
        [Display(Description = "Carpeta pagos")]
        CARPETAPAGOS = 1,
        [Display(Description = "Archivos adicionales")]
        ARCHIVOSADICIONALES = 2,
    }
}
