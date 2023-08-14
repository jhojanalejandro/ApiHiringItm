using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces
{
    public interface IExportToExcelCore
    {
        Task<MemoryStream> ExportContratacionDap(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> ExportCdp(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> ExportSolicitudPaa(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> ExportToExcelCdp(Guid ContractId);
        Task<MemoryStream> ExportElement(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> GenerateReport(ControllerBase controller, RequestReportContract reportContract);
    }
}
