using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.User.Interface
{
    public interface IUserCore
    {
        Task<List<TeamDto>> GetTeam();
        Task<UserTDto> GetById(Guid id);
        UserT GetByIdd(Guid id);
        Task<bool> Delete(Guid id);
        Task<bool> ValidateT(string authToken);
        Task<bool> UpdatePassword(UserUpdatePasswordDto model);
        Task<bool> GetUserForgetPassword(RetrievePassword model);
        Task<IGenericResponse<string>> UpdateTeamRoll(UserTDto model);
        IGenericResponse<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<IGenericResponse<string>> SignUp(UserTDto model);
    }
}
