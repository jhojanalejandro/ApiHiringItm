using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.Contractors.Interface
{
    public interface IContractorCore
    {
        Task<List<ContractorDto>> GetAll();
        Task<List<MinutaDto>> GetDataBill(ContractContractorsDto contractors);
        Task<bool> Create(ContractorDto model);
        Task<List<ContractorByContractDto>> GetByIdFolder(Guid id);
        Task<bool> UpdateAsignment(AsignElementOrCompoenteDto model);
        Task<List<ContractorPaymentsDto>> GetPaymentsContractorList(Guid contractId, Guid contractorId);
        Task<List<ContractsContarctorDto>> getContractsByContractor(string contractorId);
        Task<FilesDto?> GetDocumentPdf(Guid contractId, Guid contractorId);
        Task<bool> AddNewness(NewnessContractorDto model);
        Task<List<HistoryContractorDto>> GetHistoryContractor();
    }
}
