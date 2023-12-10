using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;

namespace WebApiHiringItm.CORE.Helpers.GenericResponse
{
    public static class ApiResponseHelper
    {
        public static IGenericResponse<T> CreateResponse<T>(T data, bool success = true, string message = "")
        {
            return new GenericResponse<T>
            {
                Success = success,
                Message = message,
                Data = data           
            };
        }

        public static IGenericResponse<T> CreateErrorResponse<T>(string message)
        {
            return new GenericResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T)
            };
        }
    }
}
