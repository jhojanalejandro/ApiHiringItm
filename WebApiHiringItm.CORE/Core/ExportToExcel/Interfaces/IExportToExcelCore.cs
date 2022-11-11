using Microsoft.AspNetCore.Mvc;

namespace WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces
{
    public interface IExportToExcelCore
    {
        Task<MemoryStream> ExportToExcelViabilidad(ControllerBase controller, int idContrato);
        Task<MemoryStream> ExportContratacionDap(ControllerBase controller);
        Task<MemoryStream> ExportCdp(ControllerBase controller, int idContrato);
        Task<MemoryStream> ExportSolicitudPpa(ControllerBase controller, int idContrato);
    }
}
