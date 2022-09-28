using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Interface
{
    public interface IUserCore
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<List<UserTDto>> GetAll();
        Task<UserTDto> GetById(int id);
        UserT GetByIdd(int id);
        Task<int> Create(UserTDto model);
        Task<bool> Delete(int id);
        Task<bool> Update(UserTDto model);
        Task<bool> ValidateT(string authToken);
        Task<bool> UpdatePassword(UserUpdatePasswordDto model);
        Task<bool> GetUserForgetPassword(RetrievePassword model);
    }
}
