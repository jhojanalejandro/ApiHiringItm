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
        Task<List<UserFileDto>> GetAllFirms();
        Task<UserFileDto> GetByIdFirm(string id);
        Task<bool> DeleteFirm(string id);
        Task<bool> SaveUserDocument(UserFileDto modelFirm);
        Task<List<RollDto>> GetAllRolls();
        Task<List<TypeUserFileDto>> GetAllTypeUserFile();

    }
}
