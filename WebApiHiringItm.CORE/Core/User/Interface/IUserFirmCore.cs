using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Usuario;

namespace WebApiHiringItm.CORE.Core.User.Interface
{
    public interface IUserFirmCore
    {
        Task<List<UserFirmDto>> GetAllFirms();
        Task<UserFirmDto> GetByIdFirm(int id);
        Task<bool> DeleteFirm(int id);
        Task<bool> CreateFirm(UserFirmDto model);

    }
}
