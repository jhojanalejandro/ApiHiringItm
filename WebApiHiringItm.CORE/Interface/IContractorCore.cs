using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Interface
{
    public interface IContractorCore
    {
        Task<List<ContractorDto>> GetAll();
        Task<ContractorDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(ContractorDto model);
    }
}
