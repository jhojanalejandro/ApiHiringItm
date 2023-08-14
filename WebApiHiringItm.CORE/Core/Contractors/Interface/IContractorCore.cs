using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
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
        
        Task<bool> UpdateAsignment(AsignElementOrCompoenteDto model);
        Task<List<ContractsContarctorDto>> getContractsByContractor(string contractorId);
        Task<FilesDto?> GetDocumentPdf(Guid contractId, Guid contractorId);
        Task<bool> AddNewness(NewnessContractorDto model);
        Task<List<HistoryContractorDto>> GetHistoryContractor();
        Task<IGenericResponse<List<ContractorByContractDto>>> GetContractorByContract(string contractId);
        ValidateFileDto ValidateDocumentUpload(Guid contractId, Guid contractorId);
        Task<IGenericResponse<string>> SavePersonalInformation(PersonalInformation model);
    }
}
