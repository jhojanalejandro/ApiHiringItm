using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.FolderType
{
    public enum FolderTypeEnum
    {
        [Display(Description = "Contratista")]
        CONTRATISTA = 0,

        [Display(Description = "Contrato")]
        CONTRATO = 1,

    }
}
