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
        [Display(Description = "DOCUMENTOS CONTRATO")]
        CONTRATOS = 0,
        [Display(Description = "CARPETA PAGOS")]
        CARPETAPAGOS = 1,
        [Display(Description = "Archivos adicionales")]
        ARCHIVOSADICIONALES = 2,
    }
}
