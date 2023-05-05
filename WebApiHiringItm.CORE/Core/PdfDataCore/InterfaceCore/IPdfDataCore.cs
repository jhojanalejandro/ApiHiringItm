using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.PdfDto;

namespace WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore
{
    public interface IPdfDataCore
    {
        Task<ExecutionReportDto> GetExecutionReport(Guid contractId, Guid ContractorId);
        Task<ChargeAccountDto> GetChargeAccount(Guid contractId, Guid ContractorId);

    }
}
