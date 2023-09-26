using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces
{
    public interface IExportToExcelCore
    {
        Task<MemoryStream> ExportContratacionDap(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> ExportCdp(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> ExportSolicitudPaa(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> ExportElement(ControllerBase controller, Guid ContractId);
        Task<MemoryStream> GenerateSatisfactionReport(Guid contractId, string base64);
        Task<MemoryStream> GenerateReport(RequestReportContract reportContract);
        Task<IGenericResponse<MemoryStream>> ExportToExcelCdp(Guid ContractId);
    }
}
