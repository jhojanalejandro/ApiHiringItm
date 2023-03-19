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
using WebApiHiringItm.MODEL.Dto.ContratoDto;

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
        public async Task<MemoryStream> ExportToExcelViabilidad(ControllerBase controller, Guid idContrato)
        {
            // Get the user list 
            var data = _context.DetailProjectContractor
                .Include(x => x.Contractor)
                .Include(x => x.Element)
                .Include(x => x.HiringData)
                .Include(x => x.Componente)
                .Include(x => x.Contract)
                .Where(x => x.ContractId == idContrato);

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
                var dataList = data.Select(w => new DetailProjectContractorDto()
                {
                    Convenio = w.Contractor.Convenio,
                    CompanyName = w.Contract.CompanyName,
                    NombreComponente = w.Componente.NombreComponente,
                    Cpc = w.Element.Cpc,
                    Nombre = w.Contractor.Nombre + " " + w.Contractor.Apellido,
                    Identificacion = w.Contractor.Identificacion,
                    DescriptionProject = w.Contract.DescriptionProject,
                    ObjetoConvenio = w.Contractor.ObjetoConvenio,
                    ValorTotal = w.Element.ValorTotal,
                    NombreElemento = w.Element.NombreElemento,
                    FuenteRubro = w.HiringData.FuenteRubro,
                    NombreRubro = w.HiringData.NombreRubro,
                    Rubro = w.Contract.Rubro,
                })
                .AsNoTracking()
                .ToList();
                foreach (var user in dataList)
                {
                    if (user.Cpc != null && user.Convenio != null && user.NombreComponente != null && user.NombreElemento != null && user.ValorTotal != null)
                    {
                        worksheet.Cells[row, 1].Value = user.Convenio;
                        worksheet.Cells[row, 2].Value = user.CompanyName;
                        worksheet.Cells[row, 3].Value = user.NombreComponente;
                        worksheet.Cells[row, 4].Value = "";
                        worksheet.Cells[row, 5].Value = "";
                        worksheet.Cells[row, 6].Value = user.Cpc;
                        worksheet.Cells[row, 7].Value = user.Nombre;
                        worksheet.Cells[row, 8].Value = user.Identificacion;
                        worksheet.Cells[row, 9].Value = "";
                        worksheet.Cells[row, 10].Value = user.DescriptionProject;
                        worksheet.Cells[row, 11].Value = user.ObjetoConvenio;
                        worksheet.Cells[row, 12].Value = user.ValorTotal;
                        worksheet.Cells[row, 13].Value = user.NombreComponente;
                        worksheet.Cells[row, 14].Value = user.NombreElemento;
                        worksheet.Cells[row, 15].Value = "";
                        worksheet.Cells[row, 16].Value = "";
                        row++;
                    }
                }
                worksheet.Columns.AutoFit();

                xlPackage.Workbook.Properties.Title = "Lista de contratistas";
                xlPackage.Workbook.Properties.Author = "ITM";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Lista de contratistas";

                xlPackage.Save();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> ExportContratacionDap(ControllerBase controller, Guid idContrato)
        {
            var data = _context.DetailProjectContractor
                .Include(x => x.Contractor)
                .Include(x => x.Element)
                .Include(x => x.HiringData)
                .Include(x => x.Componente)
                .Include(x => x.Contract)
                .Where(x => x.ContractId == idContrato);
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
                var dataList = data.Select(w => new DetailProjectContractorDto()
                {
                    Convenio = w.Contractor.Convenio,
                    CompanyName = w.Contract.CompanyName,
                    NombreComponente = w.Componente.NombreComponente,
                    Cpc = w.Element.Cpc,
                    Nombre = w.Contractor.Nombre + " " + w.Contractor.Apellido,
                    Identificacion = w.Contractor.Identificacion,
                    DescriptionProject = w.Contract.DescriptionProject,
                    ObjetoConvenio = w.Contractor.ObjetoConvenio,
                    ValorTotal = w.Element.ValorTotal,
                    NombreElemento = w.Element.NombreElemento,
                    FuenteRubro = w.HiringData.FuenteRubro,
                    NombreRubro = w.HiringData.NombreRubro,
                    Rubro = w.Contract.Rubro,
                    Cdp = w.HiringData.Cdp,
                })
                .AsNoTracking()
                .ToList();
                foreach (var user in dataList)
                {
                    
                    if (user.ValorTotal != null && user.Cpc != null && user.Cdp != null)
                    {
                        worksheet.Cells[row, 1].Value = user.Convenio;
                        worksheet.Cells[row, 2].Value = user.Nombre;
                        worksheet.Cells[row, 3].Value = user.Cpc;
                        worksheet.Cells[row, 4].Value = user.Cdp;
                        worksheet.Cells[row, 5].Value = user.ValorTotal;
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

        public async Task<MemoryStream> ExportCdp(ControllerBase controller, Guid idContrato)
        {
            // Get the user list 
            var data = _context.DetailProjectContractor.Where(x => x.ContractId == idContrato)
                .Include(x => x.Contractor)
                .Include(x => x.Element)
                .Include(x => x.HiringData)
                .Include(x => x.Componente)
                .Include(x => x.Contract);

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
                int nro = 0;
                var dataList = data.Select(w => new DetailProjectContractorDto()
                {
                    Convenio = w.Contractor.Convenio,
                    CompanyName = w.Contract.CompanyName,
                    NombreComponente = w.Componente.NombreComponente,
                    Cpc = w.Element.Cpc,
                    Nombre = w.Contractor.Nombre + " " + w.Contractor.Apellido,
                    Identificacion = w.Contractor.Identificacion,
                    DescriptionProject = w.Contract.DescriptionProject,
                    ObjetoConvenio = w.Contractor.ObjetoConvenio,
                    ValorTotal = w.Element.ValorTotal,
                    NombreElemento = w.Element.NombreElemento,
                    FuenteRubro = w.HiringData.FuenteRubro,
                    NombreRubro = w.HiringData.NombreRubro,
                    Rubro = w.Contract.Rubro,
                })
                .AsNoTracking()
                .ToList();
                foreach (var user in dataList)
                {
                    if (user.Rubro != null && user.FuenteRubro != null && user.Cpc != null && user.ValorTotal != null)
                    {
                        nro++;
                        worksheet.Cells[row, 3].Value = nro;
                        worksheet.Cells[row, 2].Value = user.Rubro;
                        worksheet.Cells[row, 1].Value = user.FuenteRubro;
                        worksheet.Cells[row, 4].Value = user.DescriptionProject;
                        worksheet.Cells[row, 5].Value = user.Convenio;
                        worksheet.Cells[row, 6].Value = user.Cpc;
                        worksheet.Cells[row, 7].Value = user.ValorTotal;
                        row++;
                    }
                }
                { }
                worksheet.Columns.AutoFit();
                xlPackage.Workbook.Properties.Title = "Solicitud CDP - DAP";
                xlPackage.Workbook.Properties.Author = "ITM";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud CDP - DAP";
                xlPackage.Save();
            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> ExportSolicitudPaa(ControllerBase controller, Guid idContrato)
        {
            // Get the user list 
            var data = _context.DetailProjectContractor.Where(x => x.ContractId == idContrato)
                .Include(x => x.Contractor)
                .Include(x => x.Element)
                .Include(x => x.HiringData)
                .Include(x => x.Componente)
                .Include(x => x.Contract);

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
                //worksheet.Cells["A1"].Value = "Con el fin de proceder a completar las columnas: Código UNSPSC, Duración del contrato (intervalo: días, meses, años), Modalidad de selección, Fuente de los recursos, ¿Se requieren vigencias futuras?, Estado de solicitud de vigencias futuras; vea la " + " Hoja de soporte " + " para saber cuáles son los códigos que aplican a cada columna.";
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

                var dataList = data.Select(w => new DetailProjectContractorDto()
                {
                    Convenio = w.Contractor.Convenio,
                    CompanyName = w.Contract.CompanyName,
                    NombreComponente = w.Componente.NombreComponente,
                    Cpc = w.Element.Cpc,
                    Nombre = w.Contractor.Nombre + " " + w.Contractor.Apellido,
                    Identificacion = w.Contractor.Identificacion,
                    DescriptionProject = w.Contract.DescriptionProject,
                    ObjetoConvenio = w.Contractor.ObjetoConvenio,
                    ValorTotal = w.Element.ValorTotal,
                    NombreElemento = w.Element.NombreElemento,
                    FuenteRubro = w.HiringData.FuenteRubro,
                    NombreRubro = w.HiringData.NombreRubro,
                    Rubro = w.Contract.Rubro,
                })
                .AsNoTracking()
                .ToList();

                row = 2;
                int nro = 0;
                foreach (var user in dataList)
                {
                    if (user.Rubro != null && user.ObjetoConvenio != null && user.FuenteRubro != null && user.Cpc != null && user.ValorTotal != null)
                    {
                        nro++;
                        worksheet.Cells[row, 1].Value = nro;
                        worksheet.Cells[row, 2].Value = user.Rubro;
                        worksheet.Cells[row, 3].Value = user.FuenteRubro;
                        worksheet.Cells[row, 4].Value = user.ObjetoConvenio;
                        worksheet.Cells[row, 5].Value = user.Convenio;
                        worksheet.Cells[row, 6].Value = user.Cpc;
                        worksheet.Cells[row, 7].Value = user.ValorTotal;
                        row++;
                    }
                 
                }
                if (nro > 0)
                {
                    worksheet.Columns.AutoFit();
                    xlPackage.Workbook.Properties.Title = "Solicitud CDP - DAP";
                    xlPackage.Workbook.Properties.Author = "ITM";
                    xlPackage.Workbook.Properties.Created = DateTime.Now;
                    xlPackage.Workbook.Properties.Subject = "Solicitud CDP - DAP";
                    xlPackage.Save();
                }
                else
                {
                    return null;
                }

            }
            stream.Position = 0;
            return await Task.FromResult(stream);
        }
        #endregion
    }
}
