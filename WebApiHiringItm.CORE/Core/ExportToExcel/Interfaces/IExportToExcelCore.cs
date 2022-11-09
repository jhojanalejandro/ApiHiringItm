using Microsoft.AspNetCore.Mvc;

namespace WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces
{
    public interface IExportToExcelCore
    {
        Task<MemoryStream> ExportToExcelViabilidad(ControllerBase controller);
        Task<MemoryStream> ExportContratacionDap(ControllerBase controller);
        Task<MemoryStream> ExportCdp(ControllerBase controller);
        Task<MemoryStream> ExportSolicitudPpa(ControllerBase controller);
    }
}
