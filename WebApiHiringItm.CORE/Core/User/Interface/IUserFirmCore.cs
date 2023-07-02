using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Usuario;

namespace WebApiHiringItm.CORE.Core.User.Interface
{
    public interface IUserFirmCore
    {
        Task<List<UserFirmDto>> GetAllFirms();
        Task<UserFirmDto> GetByIdFirm(string id);
        Task<bool> DeleteFirm(string id);
        Task<bool> SaveUserFirm(UserFirmDto modelFirm);
        Task<List<RollDto>> GetAllRolls();
        Task<List<TypeUserFileDto>> GetAllTypeUserFile();

    }
}
