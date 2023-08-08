using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.CORE.Helpers.GenericValidation
{
    public static class GenericValidations
    {

        public static bool IsGuid(this string value) => Guid.TryParse(value, out _);


    }
}
