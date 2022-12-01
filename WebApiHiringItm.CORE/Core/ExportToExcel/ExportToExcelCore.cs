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
            var data = _context.Contractor.Where(x => x.ContractId == idContrato).ToList();

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

                worksheet.Cells["B1"].Value = "Convenio";
                worksheet.Cells["C1"].Value = "Entidad";
                worksheet.Cells["D1"].Value = "Componente";
                worksheet.Cells["E1"].Value = "Rubro Presupuestal";
                worksheet.Cells["F1"].Value = "Nombre del Rubro";
                worksheet.Cells["G1"].Value = "CPC";
                worksheet.Cells["H1"].Value = "Nombre Completo";
                worksheet.Cells["I1"].Value = "Documento de Identificación";
                worksheet.Cells["J1"].Value = "Actividad";
                worksheet.Cells["K1"].Value = "Objeto";
                worksheet.Cells["L1"].Value = "Valor";
                worksheet.Cells["M1"].Value = "Componente";
                worksheet.Cells["N1"].Value = "Consecutivo";
                worksheet.Cells["O1"].Value = "Talento Humano";
                worksheet.Cells["P1"].Value = "Fuente Rubro";
                worksheet.Cells["Q1"].Value = "Posición";
                worksheet.Cells["A1:Q1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:Q1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                worksheet.Cells["A1:Q1"].Style.Font.Bold = true;
                worksheet.Cells["A1:Q1"].Style.Border.DiagonalDown = true;
                worksheet.Columns.AutoFit();
                row = 2;
                foreach (var user in data)
                {
                    //worksheet.Cells[row, 2].Value = user.Convenio;
                    //worksheet.Cells[row, 3].Value = user.Entidad;
                    //worksheet.Cells[row, 4].Value = user.Componente;
                    worksheet.Cells[row, 5].Value = "";
                    worksheet.Cells[row, 6].Value = "";
                    worksheet.Cells[row, 7].Value = "";
                    worksheet.Cells[row, 8].Value = user.Nombre;
                    worksheet.Cells[row, 8].Value = user.Apellido;
                    worksheet.Cells[row, 9].Value = user.Identificacion;
                    worksheet.Cells[row, 10].Value = "";
                    worksheet.Cells[row, 11].Value = user.ObjetoConvenio;
                    worksheet.Cells[row, 12].Value = "";
                    worksheet.Cells[row, 13].Value = user.ComponenteId;
                    worksheet.Cells[row, 14].Value = user.Id;
                    worksheet.Cells[row, 15].Value = user.ElementId;
                    worksheet.Cells[row, 16].Value = "";
                    worksheet.Cells[row, 17].Value = "";

                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Lista de contratistas";
                xlPackage.Workbook.Properties.Author = "Maicol Yepes";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Lista de contratistas";

                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> ExportContratacionDap(ControllerBase controller)
        {
            // Get the user list 
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
                worksheet.Cells["A1:A1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                worksheet.Cells["B1:B1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 204, 153));
                worksheet.Cells["E1:E1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 204, 153));
                worksheet.Cells["C1:D1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 153, 51));
                worksheet.Cells["F1:F1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 153, 51));
                worksheet.Cells["A1:F1"].Style.Font.Bold = true;
                worksheet.Cells["A1:F1"].Style.Border.DiagonalDown = true;
                worksheet.Columns.AutoFit();
                row = 2;
                foreach (var user in data)
                {
                    foreach (var item in hiringData)
                    {
                        //worksheet.Cells[row, 1].Value = user.Convenio;
                        worksheet.Cells[row, 2].Value = user.Nombre;
                        worksheet.Cells[row, 3].Value = "";
                        worksheet.Cells[row, 4].Value = "";
                        worksheet.Cells[row, 5].Value = "";
                        worksheet.Cells[row, 6].Value = "";
                    }

                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Solicitud contratación DAP";
                xlPackage.Workbook.Properties.Author = "Maicol Yepes";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud contratación DAP";

                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> ExportCdp(ControllerBase controller, int idContrato)
        {
            // Get the user list 
            var data = _context.Contractor.Where(x => x.ContractId == idContrato).ToList();
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

                worksheet.Cells["A1"].Value = "N°";
                worksheet.Cells["B1"].Value = "Rubro";
                worksheet.Cells["C1"].Value = "Fuente de recursos";
                worksheet.Cells["D1"].Value = "Objeto";
                worksheet.Cells["E1"].Value = "Proyecto o Convenio";
                worksheet.Cells["F1"].Value = "CPC";
                worksheet.Cells["G1"].Value = "Valor";
                worksheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:F1"].Style.Font.Bold = true;
                worksheet.Cells["A1:F1"].Style.Border.DiagonalDown = true;
                worksheet.Columns.AutoFit();
                row = 2;
                foreach (var user in data)
                {
                    foreach (var item in hiringData)
                    {
                        //worksheet.Cells[row, 1].Value = user.Nro;
                        worksheet.Cells[row, 2].Value = "";
                        worksheet.Cells[row, 3].Value = "";
                        worksheet.Cells[row, 4].Value = user.ObjetoConvenio;
                        //worksheet.Cells[row, 5].Value = user.Convenio;
                        worksheet.Cells[row, 6].Value = "";
                        worksheet.Cells[row, 7].Value = "";
                    }

                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Solicitud CDP - DAP";
                xlPackage.Workbook.Properties.Author = "Maicol Yepes";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud CDP - DAP";

                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
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
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));
                }

                worksheet.Cells["A4"].Value = "N°";
                worksheet.Cells["B4"].Value = "Rubro";
                worksheet.Cells["C4"].Value = "Fuente de recursos";
                worksheet.Cells["D4"].Value = "Objeto";
                worksheet.Cells["E4"].Value = "Proyecto o Convenio";
                worksheet.Cells["F4"].Value = "CPC";
                worksheet.Cells["G4"].Value = "Valor";
                worksheet.Columns.AutoFit();
                row = 5;
                foreach (var user in data)
                {
                    //worksheet.Cells[row, 1].Value = user.Nro;
                    worksheet.Cells[row, 2].Value = "";
                    worksheet.Cells[row, 3].Value = "";
                    worksheet.Cells[row, 4].Value = user.ObjetoConvenio;
                    //worksheet.Cells[row, 5].Value = user.Convenio;
                    worksheet.Cells[row, 6].Value = "";
                    worksheet.Cells[row, 7].Value = "";
                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Solicitud CDP - DAP";
                xlPackage.Workbook.Properties.Author = "Maicol Yepes";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud CDP - DAP";

                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }
        #endregion
    }
}
