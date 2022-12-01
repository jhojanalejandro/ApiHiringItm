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
        Task<List<UserFirmDto>> GetAll();
        Task<UserFirmDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(UserFirmDto model);
    }
}
