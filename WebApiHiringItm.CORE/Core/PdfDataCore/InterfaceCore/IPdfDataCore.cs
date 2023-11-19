using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.PdfDto;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;

namespace WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore
{
    public interface IPdfDataCore
    {
        Task<ExecutionReportDto> GetExecutionReport(Guid contractId, Guid ContractorId);
        Task<MacroMinuteDto?> GetminuteMacroContract(Guid contractId);
        Task<ResponsePdfDataDto<PreviusStudyDto>> GetPreviusStudy(ContractContractorsDto contractors);
        Task<ResponsePdfDataDto<CommiteeRequestDto>> GetCommitteeRequest(ContractContractorsDto contractors);
        Task<ResponsePdfDataDto<MinutaDto>> GetDataBill(ContractContractorsDto contractors);
        Task<ResponsePdfDataDto<MinuteModifyDataDto>> GetminuteModifyData(ContractContractorsDto contractors);
    }
}
