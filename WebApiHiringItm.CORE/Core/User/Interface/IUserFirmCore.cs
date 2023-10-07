using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Usuario;

namespace WebApiHiringItm.CORE.Core.User.Interface
{
    public interface IUserFirmCore
    {
        Task<List<UserFileDto>> GetAllFirms();
        Task<UserFileDto> GetByIdFirm(string id);
        Task<bool> DeleteFirm(string id);
        Task<List<RollDto>> GetAllRolls();
        Task<List<TypeUserFileDto>> GetAllTypeUserFile();
        Task<IGenericResponse<string>> SaveUserDocument(UserFileDto modelFirm);
        Task<IGenericResponse<string>> SaveAttachFile(List<UserFileDto> modelAnexo);
    }
}
