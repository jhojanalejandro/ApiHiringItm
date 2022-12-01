using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.Contractors.Interface
{
    public interface IContractorCore
    {
        Task<List<ContractorDto>> GetAll();
        Task<ContractorDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(ContractorDto model);
        Task<bool> SendContractorCount(int idAgreement, int[] idContractors);
        Task<string> ImportarExcel(FileRequest obj);
        Task<List<ContractorDto>> GetByIdFolder(int id);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<bool> UpdateAsignment(AsignElementOrCompoenteDto model);
    }
}
