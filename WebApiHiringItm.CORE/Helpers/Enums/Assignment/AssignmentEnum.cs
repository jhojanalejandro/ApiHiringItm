using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.Enums.Assignment
{
    public enum AssignmentEnum
    {
        [Display(Description = "SPVC")]
        SUPERVISORCONTRATO = 0,
        [Display(Description = "JRDCC")]
        JIRIDICOCONTRATO = 1,
        [Display(Description = "RPSC")]
        RESPONSABLECONTRATO = 2,
    }
}
