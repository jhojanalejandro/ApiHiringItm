using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces;
using System.Data.Entity;

namespace WebApiHiringItm.CORE.Core.ExportToExcel
{
    public class ExportToExcelCore : IExportToExcelCore
    {
        #region Dependency
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ExportToExcelCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<MemoryStream> ExportToExcelViabilidad(ControllerBase controller, int idContrato)
        {
            // Get the user list 
            var data = _context.Contractor
                .Where(x => x.ContractId == idContrato).ToList();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("DAP");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 1;
                var row = startRow;

                worksheet.Cells["A1"].Value = "Convenio";
                worksheet.Cells["B1"].Value = "Entidad";
                worksheet.Cells["C1"].Value = "Componente";
                worksheet.Cells["D1"].Value = "Rubro Presupuestal";
                worksheet.Cells["E1"].Value = "Nombre del Rubro";
                worksheet.Cells["F1"].Value = "CPC";
                worksheet.Cells["G1"].Value = "Nombre Completo";
                worksheet.Cells["H1"].Value = "Documento de Identificación";
                worksheet.Cells["I1"].Value = "Actividad";
                worksheet.Cells["J1"].Value = "Objeto";
                worksheet.Cells["K1"].Value = "Valor";
                worksheet.Cells["L1"].Value = "Componente";
                worksheet.Cells["M1"].Value = "Consecutivo";
                worksheet.Cells["N1"].Value = "Talento Humano";
                worksheet.Cells["O1"].Value = "Fuente Rubro";
                worksheet.Cells["P1"].Value = "Posición";
                worksheet.Cells["A1:P1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:P1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                worksheet.Cells["A1:P1"].Style.Font.Bold = true;
                worksheet.Cells["A1:P1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:P1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:P1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:P1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                row = 2;
                foreach (var user in data)
                {
                    if (user.ComponenteId != 0)
                    {

                        var hiring = _context.HiringData.Where(x => x.ContractorId == user.Id).FirstOrDefault();
                        var component = _context.Componente.Where(x => x.Id == user.ComponenteId).FirstOrDefault();
                        var elemento = _context.ElementosComponente.Where(x => x.Id == user.ElementId).FirstOrDefault();
                        var convenio = _context.ElementosComponente.Where(x => x.Id == user.ElementId).FirstOrDefault();

                        worksheet.Cells[row, 1].Value = user.Convenio;
                        worksheet.Cells[row, 2].Value = "";
                        worksheet.Cells[row, 3].Value = component.NombreComponente;
                        worksheet.Cells[row, 4].Value = hiring.Rubro;
                        worksheet.Cells[row, 5].Value = hiring.NombreRubro;
                        worksheet.Cells[row, 6].Value = elemento.Cpc;
                        worksheet.Cells[row, 7].Value = user.Nombre + " " + user.Apellido;
                        worksheet.Cells[row, 8].Value = user.Identificacion;
                        worksheet.Cells[row, 9].Value = "";
                        worksheet.Cells[row, 10].Value = "";
                        worksheet.Cells[row, 11].Value = user.ObjetoConvenio;
                        worksheet.Cells[row, 12].Value = elemento.ValorTotal;
                        worksheet.Cells[row, 13].Value = user.ComponenteId;
                        worksheet.Cells[row, 14].Value = user.Id;
                        worksheet.Cells[row, 15].Value = user.ElementId;
                        worksheet.Cells[row, 16].Value = "";
                        row++;
                    }
                }
                worksheet.Columns.AutoFit();

                xlPackage.Workbook.Properties.Title = "Lista de contratistas";
                xlPackage.Workbook.Properties.Author = "jhojan";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Lista de contratistas";

                xlPackage.Save();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> ExportContratacionDap(ControllerBase controller)
        {
            var data = _context.Contractor.ToList();
            var hiringData = _context.HiringData.ToList();
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Hoja1");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 1;
                var row = startRow;
                worksheet.Cells["A1"].Value = "CONVENIO";
                worksheet.Cells["B1"].Value = "NOMBRE COMPLETO";
                worksheet.Cells["C1"].Value = "CPC";
                worksheet.Cells["D1"].Value = "CDP";
                worksheet.Cells["E1"].Value = "VALOR CONTRATO";
                worksheet.Cells["F1"].Value = "ACTA COMITÉ";
                worksheet.Cells["A1:F1"].AutoFilter = true;
                worksheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                worksheet.Cells["A1:F1"].Style.Font.Bold = true;
                worksheet.Cells["A1:F1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:F1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:F1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:F1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                row = 2;
                foreach (var user in data)
                {
                    if (user.ComponenteId != 0)
                    {
                        var hiring = _context.HiringData.Where(x => x.ContractorId == user.Id).FirstOrDefault();
                        var component = _context.Componente.Where(x => x.Id == user.ComponenteId).FirstOrDefault();
                        var elemento = _context.ElementosComponente.Where(x => x.Id == user.ElementId).FirstOrDefault();
                        var convenio = _context.ElementosComponente.Where(x => x.Id == user.ElementId).FirstOrDefault();
                        var project = _context.ProjectFolder.Where(x => x.Id == user.ContractId).FirstOrDefault();

                        //worksheet.Cells[row, 1].Value = user.Convenio;
                        worksheet.Cells[row, 2].Value = user.Nombre;
                        worksheet.Cells[row, 3].Value = elemento.Cpc;
                        worksheet.Cells[row, 4].Value = hiring.Cdp;
                        worksheet.Cells[row, 5].Value = elemento.ValorTotal;
                        worksheet.Cells[row, 6].Value = "";

                        row++;
                    }
                }
                worksheet.Columns.AutoFit();
                xlPackage.Workbook.Properties.Title = "Solicitud contratación DAP";
                xlPackage.Workbook.Properties.Author = "Maicol Yepes";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud contratación DAP";
                xlPackage.Save();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> ExportCdp(ControllerBase controller, int idContrato)
        {
            // Get the user list 
            var data = _context.Contractor.Where(x => x.ContractId == idContrato).ToList();
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Hoja1");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 1;
                var row = startRow;

                worksheet.Cells["A1"].Value = "N°";
                worksheet.Cells["B1"].Value = "Rubro";
                worksheet.Cells["C1"].Value = "Fuente de recursos";
                worksheet.Cells["D1"].Value = "Objeto";
                worksheet.Cells["E1"].Value = "Proyecto o Convenio";
                worksheet.Cells["F1"].Value = "CPC";
                worksheet.Cells["G1"].Value = "Valor";
                worksheet.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                worksheet.Cells["A1:G1"].Style.Font.Bold = true;
                worksheet.Cells["A1:G1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:G1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:G1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:G1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
               
                row = 2;
                foreach (var user in data)
                {
                    if (user.ComponenteId != 0)
                    {

                        var hiring = _context.HiringData.Where(x => x.ContractorId == user.Id).FirstOrDefault();
                        var component = _context.Componente.Where(x => x.Id == user.ComponenteId).FirstOrDefault();
                        var elemento = _context.ElementosComponente.Where(x => x.Id == user.ElementId).FirstOrDefault();
                        var convenio = _context.ElementosComponente.Where(x => x.Id == user.ElementId).FirstOrDefault();
                        var project = _context.ProjectFolder.Where(x => x.Id == user.ContractId).FirstOrDefault();

                        worksheet.Cells[row, 1].Value = elemento.Consecutivo;
                        worksheet.Cells[row, 2].Value = hiring.Rubro;
                        //worksheet.Cells[row, 3].Value = user.Nro;
                        worksheet.Cells[row, 4].Value = user.ObjetoConvenio;
                        worksheet.Cells[row, 5].Value = user.Convenio;
                        worksheet.Cells[row, 6].Value = elemento.Cpc;
                        worksheet.Cells[row, 7].Value = elemento.ValorTotal;
                    }
                    row++;
                }
                worksheet.Columns.AutoFit();
                xlPackage.Workbook.Properties.Title = "Solicitud CDP - DAP";
                xlPackage.Workbook.Properties.Author = "Maicol Yepes";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud CDP - DAP";
                xlPackage.Save();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> ExportSolicitudPpa(ControllerBase controller, int idContrato)
        {
            // Get the user list 
            var data = _context.Contractor.Where(x => x.ContractId == idContrato).ToList();
            var hiringData = _context.HiringData.ToList();
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Adquisiciones");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;
                //Create Headers and format them
                worksheet.Cells["A1"].Value = "Con el fin de proceder a completar las columnas: Código UNSPSC, Duración del contrato (intervalo: días, meses, años), Modalidad de selección, Fuente de los recursos, ¿Se requieren vigencias futuras?, Estado de solicitud de vigencias futuras; vea la " + " Hoja de soporte " + " para saber cuáles son los códigos que aplican a cada columna.";
                using (var r = worksheet.Cells["A1:Q3"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
                worksheet.Cells["A4"].Value = "N°";
                worksheet.Cells["B4"].Value = "Rubro";
                worksheet.Cells["C4"].Value = "Fuente de recursos";
                worksheet.Cells["D4"].Value = "Objeto";
                worksheet.Cells["E4"].Value = "Proyecto o Convenio";
                worksheet.Cells["F4"].Value = "CPC";
                worksheet.Cells["G4"].Value = "Valor";               
                row = 5;
                foreach (var user in data)
                {
                    worksheet.Cells[row, 1].Value = user.Id;
                    worksheet.Cells[row, 2].Value = "";
                    worksheet.Cells[row, 3].Value = "";
                    worksheet.Cells[row, 4].Value = user.ObjetoConvenio;
                    worksheet.Cells[row, 5].Value = user.Convenio;
                    worksheet.Cells[row, 6].Value = "";
                    worksheet.Cells[row, 7].Value = "";
                    row++;
                }
                worksheet.Columns.AutoFit();
                xlPackage.Workbook.Properties.Title = "Solicitud CDP - DAP";
                xlPackage.Workbook.Properties.Author = "Maicol Yepes";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud CDP - DAP";
                xlPackage.Save();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }
        #endregion
    }
}
