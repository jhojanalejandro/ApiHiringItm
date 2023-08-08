using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.FolderType
{
    public enum MinuteTypeEnum
    {
        [Display(Description = "ADC")]
        CONTRATO = 0,
        [Display(Description = "MC")]
        PAGOSCODE = 1,
        [Display(Description = "APC")]
        APC = 1,
        [Display(Description = "AA")]
        AA = 1,
        [Display(Description = "AAM")]
        AAM = 1,
        [Display(Description = "AM")]
        AM = 1,
        [Display(Description = "ADMC")]
        ADMC = 1,
    }
}
