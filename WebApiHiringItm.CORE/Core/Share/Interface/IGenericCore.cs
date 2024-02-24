using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto.Share;

namespace WebApiHiringItm.CORE.Core.Share.Interface
{
    public interface IGenericCore
    {
        Task<IGenericResponse<string>> UpdateSessionPanel(SessionPanelDto sessionPanel);
    }
}
