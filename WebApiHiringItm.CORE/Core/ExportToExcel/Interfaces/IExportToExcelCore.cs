using Microsoft.AspNetCore.Mvc;

namespace WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces
{
    public interface IExportToExcelCore
    {
        Task<MemoryStream> ExportToExcelViabilidad(ControllerBase controller, int idContrato);
        Task<MemoryStream> ExportContratacionDap(ControllerBase controller, int idContrato);
        Task<MemoryStream> ExportCdp(ControllerBase controller, int idContrato);
        Task<MemoryStream> ExportSolicitudPaa(ControllerBase controller, int idContrato);
    }
}
