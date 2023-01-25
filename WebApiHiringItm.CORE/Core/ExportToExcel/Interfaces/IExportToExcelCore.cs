using Microsoft.AspNetCore.Mvc;

namespace WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces
{
    public interface IExportToExcelCore
    {
        Task<MemoryStream> ExportToExcelViabilidad(ControllerBase controller, Guid idContrato);
        Task<MemoryStream> ExportContratacionDap(ControllerBase controller, Guid idContrato);
        Task<MemoryStream> ExportCdp(ControllerBase controller, Guid idContrato);
        Task<MemoryStream> ExportSolicitudPaa(ControllerBase controller, Guid idContrato);
    }
}
