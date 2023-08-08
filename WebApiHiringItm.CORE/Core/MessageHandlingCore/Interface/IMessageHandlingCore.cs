using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto.MessageDto;

namespace WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface
{
    public interface IMessageHandlingCore
    {
        Task<IGenericResponse<string>> SendContractorCount(SendMessageAccountDto ids);
        Task<IGenericResponse<string>> SendContractorObservation(SendMessageObservationDto ids);

    }
}
