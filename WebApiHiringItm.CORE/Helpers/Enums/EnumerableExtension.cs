using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiHiringItm.CORE.Helpers.Enums
{
    public static class EnumerableExtension
    {
        public static string Description(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false); dynamic displayAttribute = null; if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }
            return displayAttribute?.Description ?? "";
        }
    }
}
