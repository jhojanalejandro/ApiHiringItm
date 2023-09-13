using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.PdfDto;

namespace WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore
{
    public interface IPdfDataCore
    {
        Task<ExecutionReportDto> GetExecutionReport(Guid contractId, Guid ContractorId);
        Task<ChargeAccountDto> GetChargeAccount(Guid contractId, Guid ContractorId);
        Task<MacroMinuteDto?> GetminuteMacroContract(Guid contractId);
        Task<List<MinutaDto>> GetDataBill(ContractContractorsDto contractors);
        Task<List<MinuteExtensionDto>> GetminuteExtension(ContractContractorsDto contractors);
        Task<PreviusStudyContractorsDto> GetPreviusStudy(ContractContractorsDto contractors);
        Task<CommiteeRequestDtoContractorsDto> GetCommitteeRequest(ContractContractorsDto contractors);
    }
}
