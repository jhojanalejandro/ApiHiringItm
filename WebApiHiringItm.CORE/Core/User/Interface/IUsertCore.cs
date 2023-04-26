using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.User.Interface
{
    public interface IUserCore
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<List<UserTDto>> GetAll();
        Task<List<UserTDto>> GetAllAdmins();
        Task<UserTDto> GetById(Guid id);
        UserT GetByIdd(Guid id);
        Task<string> SignUp(UserTDto model);
        Task<bool> Delete(Guid id);
        Task<bool> Update(UserTDto model);
        Task<bool> ValidateT(string authToken);
        Task<bool> UpdatePassword(UserUpdatePasswordDto model);
        Task<bool> GetUserForgetPassword(RetrievePassword model);
    }
}
