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
        public ExportToExcelCore(Hiring_V1Context context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<MemoryStream> ExportToExcelViabilidad(ControllerBase controller)
        {
            // Get the user list 
            var data = _context.Contractor.ToList();

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

                //Create Headers and format them
                //worksheet.Cells["A1"].Value = "Contratos";
                //using (var r = worksheet.Cells["A1:Y1"])
                //{
                //    r.Merge = true;
                //    r.Style.Font.Color.SetColor(Color.White);
                //    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                //}

                worksheet.Cells["A1"].Value = "No";
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

                row = 2;
                foreach (var user in data)
                {
                    worksheet.Cells[row, 1].Value = user.No;
                    worksheet.Cells[row, 2].Value = user.Convenio;
                    worksheet.Cells[row, 3].Value = user.Entidad;
                    worksheet.Cells[row, 4].Value = user.Componente;
                    worksheet.Cells[row, 5].Value = "";
                    worksheet.Cells[row, 6].Value = "";
                    worksheet.Cells[row, 7].Value = "";
                    worksheet.Cells[row, 8].Value = user.NombreCompleto;
                    worksheet.Cells[row, 9].Value = user.DocumentoDeIdentificacion;
                    worksheet.Cells[row, 10].Value = "";
                    worksheet.Cells[row, 11].Value = user.ObjetoConvenio;
                    worksheet.Cells[row, 12].Value = "";
                    worksheet.Cells[row, 13].Value = user.Componente;
                    worksheet.Cells[row, 14].Value = user.Id;
                    worksheet.Cells[row, 15].Value = user.TalentoHumano;
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

        public async Task<MemoryStream> ExportToExcelPPA(ControllerBase controller)
        {
            // Get the user list 
            var data = _context.Contractor.ToList();
            var hiringData = _context.HiringData.ToList();
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

                //Create Headers and format them
                //worksheet.Cells["A1"].Value = "Contratos";
                //using (var r = worksheet.Cells["A1:Y1"])
                //{
                //    r.Merge = true;
                //    r.Style.Font.Color.SetColor(Color.White);
                //    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                //}

                worksheet.Cells["A1"].Value = "No";
                worksheet.Cells["B1"].Value = "Convenio";
                worksheet.Cells["C1"].Value = "Entidad";
                worksheet.Cells["D1"].Value = "Componente";
                worksheet.Cells["E1"].Value = "Rubro Presupuestal";
                worksheet.Cells["F1"].Value = "Nombre del Rubro";
                worksheet.Cells["G1"].Value = "CPC";
                worksheet.Cells["H1"].Value = "Nombre Completo";
                worksheet.Cells["I1"].Value = "Documento de Identificación";
                worksheet.Cells["A1:Q1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:Q1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                worksheet.Cells["A1:Q1"].Style.Font.Bold = true;
                worksheet.Cells["A1:Q1"].Style.Border.DiagonalDown = true;

                row = 2;
                foreach (var user in data)
                {
                    foreach (var item in hiringData)
                    {
                        worksheet.Cells[row, 1].Value = user.No;
                        worksheet.Cells[row, 2].Value = user.Convenio;
                        worksheet.Cells[row, 3].Value = user.Entidad;
                        worksheet.Cells[row, 4].Value = user.Componente;
                        worksheet.Cells[row, 5].Value = "";
                        worksheet.Cells[row, 6].Value = "";
                        worksheet.Cells[row, 7].Value = "";
                        worksheet.Cells[row, 8].Value = user.NombreCompleto;
                        worksheet.Cells[row, 9].Value = user.DocumentoDeIdentificacion;
                    }                    

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
        #endregion
    }
}
