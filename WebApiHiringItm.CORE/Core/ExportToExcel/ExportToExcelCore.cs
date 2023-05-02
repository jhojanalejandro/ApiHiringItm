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
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.ExportToExcel
{
    public class ExportToExcelCore : IExportToExcelCore
    {
        #region Dependency
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ExportToExcelCore(HiringContext context, IMapper mapper)
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
                    FuenteRubro = w.Contract.FuenteRubro,
                    NombreRubro = w.Contract.NombreRubro,
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
                    FuenteRubro = w.Contract.FuenteRubro,
                    NombreRubro = w.Contract.NombreRubro,
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

                worksheet.Cells["A1"].Value = "CODIGO DE LA EMPRESA";
                worksheet.Cells["B1"].Value = "CONSECUTIVO";
                worksheet.Cells["C1"].Value = "CODIGO TIPO DE OPERACIÓN";
                worksheet.Cells["D1"].Value = "NÚMERO DEL DOCUMENTO";
                worksheet.Cells["E1"].Value = "FECHA SOLICITUD";
                worksheet.Cells["F1"].Value = "CODIGO DE LA SUCURSAL";
                worksheet.Cells["G1"].Value = "DESCRIPCIÓN DEL MOVIMIENTO";
                worksheet.Cells["H1"].Value = "CODIGO DEL TERCERO";
                worksheet.Cells["I1"].Value = "DOCUMENTO DEL SUPERVISOR";
                worksheet.Cells["J1"].Value = "CLASE DE DOCUMENTO";
                worksheet.Cells["K1"].Value = "TIPO DE DOCUMENTO SOPORTE";
                worksheet.Cells["L1"].Value = "NÚMERO DE DOCUMENTO SOPORTE";
                worksheet.Cells["M1"].Value = "NÚMERO DEL SIIF";
                worksheet.Cells["N1"].Value = "NÚMERO DEL MES ASOCIADO A LA FECHA";
                worksheet.Cells["O1"].Value = "NÚMERO DEL DÍA ASOCIADO A LA FECHA";
                worksheet.Cells["P1"].Value = "CODIGO DEL RUBRO";
                worksheet.Cells["Q1"].Value = "DESCRIPCIÓN DEL DETALLE DEL MOVIMIENTO ASOCIADO AL RUBRO";
                worksheet.Cells["R1"].Value = "CODIGO DEL CENTRO DE COSTOS";
                worksheet.Cells["S1"].Value = "CODIGO DEL PROYECTO";
                worksheet.Cells["T1"].Value = "CPC";
                worksheet.Cells["U1"].Value = "SIGNO";
                worksheet.Cells["V1"].Value = "VALOR ASOCIADO AL RUBRO";
                worksheet.Cells["W1"].Value = "CONSECUTIVO REGISTRO BASE";
                worksheet.Cells["X1"].Value = "CEDULA";
                worksheet.Cells["Y1"].Value = "NOMBRE";

                worksheet.Cells["A1:Y1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:Y1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                worksheet.Cells["A1:Y1"].Style.Font.Bold = true;
                worksheet.Cells["A1:Y1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:Y1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:Y1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:Y1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
               
                row = 2;
                int nro = 0;
                var dataList = data.Select(w => new SolicitudCdpDto()
                {
                    Consecutivo = w.Element.Consecutivo,
                    NombreElemento = w.Element.NombreElemento,
                    NombreComponente = w.Componente.NombreComponente,
                    NumeroConvenio = w.Contract.NumberProject,
                    CedulaSupervisor = w.HiringData.IdentificacionSupervisor,
                    NombreSupervisor = w.HiringData.SupervisorItm,
                    Rubro = w.Contract.Rubro,
                    Cpc = w.Element.Cpc,
                    Projecto = w.Contract.Project,
                    Nombre = w.Contractor.Nombre,
                    Cedula = w.Contractor.Identificacion
                })
                .AsNoTracking()
                .ToList();
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                var date = DateTime.Now.ToString("dd/MM/yyyy");

                foreach (var user in dataList)
                {
                    if (user.Consecutivo != null && user.NombreElemento != null && user.NumeroConvenio != null && user.Cpc != null)
                    {
                        nro++;
                        worksheet.Cells[row, 3].Value = 108;
                        worksheet.Cells[row, 2].Value = nro;
                        worksheet.Cells[row, 1].Value = 49;
                        worksheet.Cells[row, 4].Value = nro;
                        worksheet.Cells[row, 5].Value = date;
                        worksheet.Cells[row, 6].Value = 1;
                        worksheet.Cells[row, 7].Value = user.NumeroConvenio + "/" + year + user.NombreElemento + "("+user.Consecutivo+")";
                        worksheet.Cells[row, 8].Value = 800214750;
                        worksheet.Cells[row, 9].Value = user.CedulaSupervisor;
                        worksheet.Cells[row, 10].Value = "SCDP";
                        worksheet.Cells[row, 11].Value = 0;
                        worksheet.Cells[row, 12].Value = 0;
                        worksheet.Cells[row, 13].Value = 0;
                        worksheet.Cells[row, 14].Value = month;
                        worksheet.Cells[row, 15].Value = day;
                        worksheet.Cells[row, 16].Value = user.NumeroConvenio + "/" + year + user.NombreElemento + "(" + user.Consecutivo + ")";
                        worksheet.Cells[row, 17].Value = user.Rubro;
                        worksheet.Cells[row, 18].Value = "1016231102123";
                        worksheet.Cells[row, 19].Value = user.Projecto;
                        worksheet.Cells[row, 20].Value = user.Cpc;
                        worksheet.Cells[row, 21].Value = "S";
                        worksheet.Cells[row, 22].Value = user.Rubro;
                        worksheet.Cells[row, 23].Value = 0;
                        worksheet.Cells[row, 24].Value = user.Cedula;
                        worksheet.Cells[row, 25].Value = user.Nombre;

                        row++;
                    }
                }
                { }
                worksheet.Columns.AutoFit();
                xlPackage.Workbook.Properties.Title = "Solicitud CDP";
                xlPackage.Workbook.Properties.Author = "ITM";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Solicitud CDP";
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
                    FuenteRubro = w.Contract.FuenteRubro,
                    NombreRubro = w.Contract.NombreRubro,
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
