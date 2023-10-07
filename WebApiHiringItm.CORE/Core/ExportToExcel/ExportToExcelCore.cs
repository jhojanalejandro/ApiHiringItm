using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.ExportDataDto;
using WebApiHiringItm.MODEL.Models;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

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

        #region PUBLIC METHODS
        public async Task<IGenericResponse<MemoryStream>> ExportToExcelCdp(Guid ContractId)
        {

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("CDP");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 1;
                var row = startRow;
                worksheet.Cells["A1"].Value = "IdentificadorBaseDedatos";
                worksheet.Cells["B1"].Value = "Consecutivo";
                worksheet.Cells["C1"].Value = "Convenio";
                worksheet.Cells["D1"].Value = "Cedula";
                worksheet.Cells["E1"].Value = "Nombre Contratista";
                worksheet.Cells["F1"].Value = "Fecha nacimiento";
                worksheet.Cells["G1"].Value = "Telefono";
                worksheet.Cells["H1"].Value = "Direción";
                worksheet.Cells["I1"].Value = "Correo";
                worksheet.Cells["J1"].Value = "Eps";
                worksheet.Cells["K1"].Value = "Arl";
                worksheet.Cells["L1"].Value = "Afp";
                worksheet.Cells["M1"].Value = "Objeto";
                worksheet.Cells["N1"].Value = "Duración";
                worksheet.Cells["O1"].Value = "Valor Contrato";
                worksheet.Cells["P1"].Value = "Elemento";
                worksheet.Cells["Q1"].Value = "Obligaciones";
                worksheet.Cells["R1"].Value = "CDP";
                worksheet.Cells["S1"].Value = "Número Contrato";
                var celdasBloqueadas = worksheet.Cells["A2:Q2"];
                celdasBloqueadas.Style.Locked = true;
                worksheet.Protection.IsProtected = true;
                worksheet.Protection.SetPassword("ContractacionITM");

                var celdasEditables = worksheet.Cells["R2:S50"];
                celdasEditables.Style.Locked = false;
                worksheet.Cells["A1:V1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:V1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                worksheet.Cells["A1:V1"].Style.Font.Bold = true;
                worksheet.Cells["A1:V1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:V1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:V1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:V1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //worksheet.Protection.IsProtected = true;
                //worksheet.Protection.SetPassword("ContratacionITM");


                row = 2;
                var data = _context.EconomicdataContractor
                            .Where(x => x.DetailContractor.ContractId == ContractId);
                var dataList = data.Select(w => new DetailProjectContractorDto()
                {
                    ContractorId = w.DetailContractor.ContractorId.ToString(),
                    NombreComponente = w.DetailContractor.Component.NombreComponente,
                    Nombre = w.DetailContractor.Contractor.Nombres + " " + w.DetailContractor.Contractor.Apellidos,
                    Identificacion = w.DetailContractor.Contractor.Identificacion,
                    ObjetoConvenio = w.DetailContractor.Element.ObjetoElemento,
                    ValorTotal = Math.Ceiling(w.TotalValue),
                    NombreElemento = w.DetailContractor.Element.NombreElemento,
                    GeneralObligation = w.DetailContractor.Element.ObligacionesGenerales,
                    SpecificObligation = w.DetailContractor.Element.ObligacionesEspecificas,
                    InitialDate = w.DetailContractor.HiringData.FechaRealDeInicio,
                    FinalDate = w.DetailContractor.HiringData.FechaFinalizacionConvenio,
                    Convenio = w.DetailContractor.Contract.ProjectName,
                    ContractorAddress = w.DetailContractor.Contractor.Direccion,
                    ContractorMail = w.DetailContractor.Contractor.Correo,
                    Eps = w.DetailContractor.Contractor.EpsNavigation.EntityHealthDescription,
                    Arl = w.DetailContractor.Contractor.ArlNavigation.EntityHealthDescription,
                    Afp = w.DetailContractor.Contractor.AfpNavigation.EntityHealthDescription,
                    BirthDate = w.DetailContractor.Contractor.FechaNacimiento
                })
                .AsNoTracking()
                .ToList();
                int nro = 0;
                foreach (var user in dataList)
                {
                    if (user.InitialDate != null && user.FinalDate != null && user.NombreElemento != null && user.ValorTotal != null && user.ObjetoConvenio != null && user.NombreElemento != null)
                    {
                        nro++;
                        var durationContract = DateInDays(user.InitialDate.Value, user.FinalDate.Value);
                        var unifyObligation = SeparateObligation(user.GeneralObligation, user.SpecificObligation);
                        string totalValue = user.ValorTotal.ToString("N0");                 
                      
                        worksheet.Cells[row, 1].Value = user.ContractorId;
                        worksheet.Cells[row, 2].Value = nro;
                        worksheet.Cells[row, 3].Value = user.Convenio;
                        worksheet.Cells[row, 4].Value = user.Identificacion;
                        worksheet.Cells[row, 5].Value = user.Nombre;
                        worksheet.Cells[row, 6].Value = user.BirthDate;
                        worksheet.Cells[row, 7].Value = user.PhoneNumber;
                        worksheet.Cells[row, 8].Value = user.ContractorAddress;
                        worksheet.Cells[row, 9].Value = user.ContractorMail;
                        worksheet.Cells[row, 10].Value = user.Eps;
                        worksheet.Cells[row, 11].Value = user.Arl;
                        worksheet.Cells[row, 12].Value = user.Afp;
                        worksheet.Cells[row, 13].Value = user.ObjetoConvenio;
                        worksheet.Cells[row, 14].Value = durationContract;
                        worksheet.Cells[row, 15].Value = totalValue;
                        worksheet.Cells[row, 16].Value = user.NombreElemento;
                        worksheet.Cells[row, 17].Value = unifyObligation;
                        worksheet.Cells[row, 18].Value = "";
                        worksheet.Cells[row, 19].Value = "";
                        row++;
                    }
                }
                if (nro == 0)
                {
                    return ApiResponseHelper.CreateErrorResponse<MemoryStream>(Resource.EXCELEMPTY);

                }
                worksheet.Columns.AutoFit();

                xlPackage.Workbook.Properties.Title = "Lista de contratistas";
                xlPackage.Workbook.Properties.Author = "ITM";
                xlPackage.Workbook.Properties.Created = DateTime.Now;
                xlPackage.Workbook.Properties.Subject = "Lista de contratistas";

                xlPackage.Save();

            }
            stream.Position = 0;
            return ApiResponseHelper.CreateResponse(stream);
        }

        public async Task<MemoryStream> ExportContratacionDap(ControllerBase controller, Guid ContractId)
        {
            var data = _context.DetailContractor
                .Include(x => x.Contractor)
                .Include(x => x.Element)
                    .ThenInclude(t => t.Cpc)
                .Include(x => x.HiringData)
                .Include(x => x.Component)
                .Include(x => x.Contract)
                .Where(x => x.ContractId == ContractId);
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
                    Convenio = w.Contract.ProjectName,
                    CompanyName = w.Contract.CompanyName,
                    NombreComponente = w.Component.NombreComponente,
                    Cpc = w.Element.Cpc.CpcName,
                    Nombre = w.Contractor.Nombres + " " + w.Contractor.Apellidos,
                    Identificacion = w.Contractor.Identificacion,
                    ObjetoConvenio = w.Contract.ObjectContract,
                    ValorTotal = w.Element.ValorTotal,
                    NombreElemento = w.Element.NombreElemento,

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

        public async Task<MemoryStream> ExportCdp(ControllerBase controller, Guid ContractId)
        {

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
                worksheet.Cells["E1"].Value = "FECHA MOVIMIENTO";
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
                // Get the user list 
                var data = _context.EconomicdataContractor.Where(x => x.DetailContractor.ContractId == ContractId);

                var dataList = data.Select(w => new SolicitudCdpDto()
                {
                    Consecutivo = w.DetailContractor.Element.Consecutivo,
                    NombreElemento = w.DetailContractor.Element.NombreElemento,
                    NombreComponente = w.DetailContractor.Component.NombreComponente,
                    NumeroConvenio = w.DetailContractor.Contract.NumberProject,
                    CedulaSupervisor = w.DetailContractor.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Identification).FirstOrDefault(),
                    NombreSupervisor = w.DetailContractor.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.UserName).FirstOrDefault(),
                    Rubro = w.DetailContractor.Contract.RubroNavigation.RubroNumber,
                    Cpc = w.DetailContractor.Element.Cpc.CpcNumber,
                    Projecto = w.DetailContractor.Contract.Project,
                    Nombre = w.DetailContractor.Contractor.Nombres,
                    Cedula = w.DetailContractor.Contractor.Identificacion
                })
                .AsNoTracking()
                .ToList();
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                var date = DateTime.Now.ToString("yyyyMMdd");

                foreach (var user in dataList)
                {
                    if (user.Consecutivo != null && user.NombreElemento != null && user.NumeroConvenio != null && user.Cpc != null)
                    {
                        nro++;
                        string valorTotal = user.ValorTotal.ToString("N0");

                        worksheet.Cells[row, 1].Value = 108;
                        worksheet.Cells[row, 2].Value = nro;
                        worksheet.Cells[row, 3].Value = 49;
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
                        worksheet.Cells[row, 16].Value = user.Rubro;
                        worksheet.Cells[row, 17].Value = user.NumeroConvenio + "/" + year + user.NombreElemento + "(" + user.Consecutivo + ")";
                        worksheet.Cells[row, 18].Value = "1016231102123";
                        worksheet.Cells[row, 19].Value = user.Projecto;
                        worksheet.Cells[row, 20].Value = user.Cpc;
                        worksheet.Cells[row, 21].Value = "S";
                        worksheet.Cells[row, 22].Value = valorTotal;
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

        public async Task<MemoryStream> ExportSolicitudPaa(ControllerBase controller, Guid ContractId)
        {

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
                worksheet.Cells["A4"].Value = "Código UNSPSC";
                worksheet.Cells["B4"].Value = "Descripción";
                worksheet.Cells["C4"].Value = "Fecha estimada de inicio de proceso de selección";
                worksheet.Cells["D4"].Value = "Fecha estimada de presentación de ofertas";
                worksheet.Cells["E4"].Value = "Duración del contrato";
                worksheet.Cells["F4"].Value = "Duración del contrato";
                worksheet.Cells["G4"].Value = "Modalidad de selección ";
                worksheet.Cells["H4"].Value = "Fuente de los recursos";
                worksheet.Cells["I4"].Value = "Valor total estimado";
                worksheet.Cells["J4"].Value = "Valor estimado en la vigencia actual";
                worksheet.Cells["K4"].Value = "¿Se requieren vigencias futuras?";
                worksheet.Cells["L4"].Value = "Estado de solicitud de vigencias futuras";
                worksheet.Cells["M4"].Value = "Unidad de contratación";
                worksheet.Cells["N4"].Value = "Ubicación";
                worksheet.Cells["O4"].Value = "Nombre del responsable";
                worksheet.Cells["P4"].Value = "Teléfono del responsable ";
                worksheet.Cells["Q4"].Value = "Correo electrónico del responsable";
                worksheet.Cells["R4"].Value = "Proyecto de inversión";
                worksheet.Cells["S4"].Value = "DOCUMENTO";
                worksheet.Cells["T4"].Value = "NOMBRE";
                worksheet.Cells["U4"].Value = "HONORARIOS";
                worksheet.Cells["V4"].Value = "FECHA REQUERIDA DE INICIO PIMER SEMESTRE";
                worksheet.Cells["W4"].Value = "FECHA TERMINACIÓN PRIMER CONTRATO";
                worksheet.Cells["X4"].Value = "DURACION";
                worksheet.Cells["Y4"].Value = "TOTAL CONTRATO";
                worksheet.Cells["Z4"].Value = "¿Debe cumplir con invertir mínimo el 30% de los recursos del presupuesto destinados a comprar alimentos, cumpliendo con lo establecido en la Ley 2046 de 2020, reglamentada por el Decreto 248 de 2021?";
                worksheet.Cells["AA4"].Value = "¿El contrato incluye el suministro de bienes y servicios distintos a alimentos?";


                worksheet.Cells["A4:AA4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A4:AA4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                worksheet.Cells["A4:AA4"].Style.Font.Bold = true;
                worksheet.Cells["A4:AA4"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A4:AA4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A4:AA4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A4:AA4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                var data = _context.EconomicdataContractor
                    .Where(x => x.DetailContractor.ContractId.Equals(ContractId));

                var result =  data.Select(w => new DetailProjectContractorDto()
                {
                    ContractorId = w.DetailContractor.ContractorId.ToString(),
                    Convenio = w.DetailContractor.Contract.ProjectName,
                    CompanyName = w.DetailContractor.Contract.CompanyName,
                    NombreComponente = w.DetailContractor.Component.NombreComponente,
                    Nombre = w.DetailContractor.Contractor.Nombres + " " + w.DetailContractor.Contractor.Apellidos,
                    Identificacion = w.DetailContractor.Contractor.Identificacion,
                    ObjetoConvenio = w.DetailContractor.Element.ObjetoElemento,
                    ValorTotal = Math.Ceiling(w.TotalValue),
                    InitialDate = w.DetailContractor.HiringData.FechaRealDeInicio,
                    FinalDate = w.DetailContractor.HiringData.FechaFinalizacionConvenio,
                    User = w.DetailContractor.Contractor.User.UserName,
                    Email = w.DetailContractor.Contractor.User.UserEmail,
                    UnitValue = Math.Ceiling( w.UnitValue),
                })
                .AsNoTracking()
                .ToList();
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                var date = DateTime.Now.ToString("dd/MM/yyyy");
                row = 5;
                int nro = 0;
                foreach (var user in result)
                {
                    if ( user.ObjetoConvenio != null && user.ValorTotal != null && user.InitialDate.HasValue && user.FinalDate.HasValue)
                    {
                        var durationContract = DateInDays(user.InitialDate.Value, user.FinalDate.Value);
                        string numeroFormateado = user.ValorTotal.ToString("N0");

                        nro++;
                        worksheet.Cells[row, 1].Value = 80111600;
                        worksheet.Cells[row, 2].Value = user.ObjetoConvenio;
                        worksheet.Cells[row, 3].Value = month;
                        worksheet.Cells[row, 4].Value = month;
                        worksheet.Cells[row, 5].Value = 0;
                        worksheet.Cells[row, 6].Value = durationContract;
                        worksheet.Cells[row, 7].Value = "CCE-16";
                        worksheet.Cells[row, 8].Value = 0;
                        worksheet.Cells[row, 9].Value = numeroFormateado;
                        worksheet.Cells[row, 10].Value = numeroFormateado;
                        worksheet.Cells[row, 11].Value = 0;
                        worksheet.Cells[row, 12].Value = 0;
                        worksheet.Cells[row, 13].Value = "Unidad estratégica de negociosos";
                        worksheet.Cells[row, 14].Value = "CO-ANT-05001";
                        worksheet.Cells[row, 15].Value = user.User;
                        worksheet.Cells[row, 16].Value = 4405100;
                        worksheet.Cells[row, 17].Value = user.Email;
                        worksheet.Cells[row, 18].Value = "Convenios-funcionamiento";
                        worksheet.Cells[row, 19].Value = user.Identificacion;
                        worksheet.Cells[row, 20].Value = user.Nombre;
                        worksheet.Cells[row, 21].Value = user.UnitValue;
                        worksheet.Cells[row, 22].Value = user.InitialDate;
                        worksheet.Cells[row, 23].Value = user.FinalDate;
                        worksheet.Cells[row, 24].Value = durationContract;
                        worksheet.Cells[row, 25].Value = user.ValorTotal;
                        worksheet.Cells[row, 26].Value = 0;
                        worksheet.Cells[row, 27].Value = 0;
                        row++;
                    }
                 
                }
                if (nro > 0)
                {
                    worksheet.Columns.AutoFit();
                    xlPackage.Workbook.Properties.Title = "Solicitud PAA";
                    xlPackage.Workbook.Properties.Author = "ITM";
                    xlPackage.Workbook.Properties.Created = DateTime.Now;
                    xlPackage.Workbook.Properties.Subject = "Solicitud PAA";
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

        public async Task<MemoryStream> ExportElement(ControllerBase controller, Guid ContractId)
        {

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

                worksheet.Cells["A1"].Value = "Identificador Interno";
                worksheet.Cells["B1"].Value = "Consecutivo Registro";
                worksheet.Cells["C1"].Value = "Componente";
                worksheet.Cells["D1"].Value = "Actividad";
                worksheet.Cells["E1"].Value = "Consecutivo Elemento";
                worksheet.Cells["F1"].Value = "Nombre Elemento";
                worksheet.Cells["G1"].Value = "Perfil Academico Requerido";
                worksheet.Cells["H1"].Value = "Perfil Experiencia Requerido";
                var celdasBloqueadas = worksheet.Cells["A2:F2"];
                celdasBloqueadas.Style.Locked = true;
                worksheet.Protection.IsProtected = true;
                worksheet.Protection.SetPassword("ContractacionITM");

                var celdasEditablesG1 = worksheet.Cells["G2:G40"];
                celdasEditablesG1.Style.Locked = false;
                var celdasEditablesH1 = worksheet.Cells["H2:H40"];
                celdasEditablesH1.Style.Locked = false;
                //var celdasEditablesG2 = worksheet.Cells["G30"];
                //celdasEditablesG2.Style.Locked = false;
                worksheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                worksheet.Cells["A1:H1"].Style.Font.Bold = true;
                worksheet.Cells["A1:H1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:H1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:H1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1:H1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                var data = _context.ElementComponent
                    .Include(i => i.Component)
                    .Include(i => i.Activity)
                    .Where(x => x.Component.ContractId.Equals(ContractId));

                var dataList = data.Select(w => new ExportElementDto()
                {
                    Id = w.Id.ToString(),
                    Consecutive = w.Consecutivo,
                    ElementName = w.NombreElemento,
                    ComponentName = w.Component.NombreComponente,
                    ActivityName = w.Activity.NombreActividad
                })
                .AsNoTracking()
                .ToList();
                row = 2;
                int nro = 0;
                foreach (var user in dataList)
                {
                    if (user.ElementName != null && user.Consecutive != null && user.Id != null)
                    {
                        nro++;
                        worksheet.Cells[row, 1].Value = user.Id;
                        worksheet.Cells[row, 2].Value = nro;
                        worksheet.Cells[row, 3].Value = user.ComponentName;
                        worksheet.Cells[row, 4].Value = user.ActivityName;
                        worksheet.Cells[row, 5].Value = user.Consecutive;
                        worksheet.Cells[row, 6].Value = user.ElementName;
                        worksheet.Cells[row, 7].Value = "";
                        row++;
                    }

                }
                if (nro > 0)
                {
                    worksheet.Columns.AutoFit();
                    xlPackage.Workbook.Properties.Title = "EXPORTAR ELEMENTO";
                    xlPackage.Workbook.Properties.Author = "ITM";
                    xlPackage.Workbook.Properties.Created = DateTime.Now;
                    xlPackage.Workbook.Properties.Subject = "EXPORTAR ELEMENTO";
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

        public async Task<MemoryStream> GenerateReport(RequestReportContract reportContract)
        {
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var getDataContract = _context.ContractFolder.Where(w => w.Id.Equals(Guid.Parse(reportContract.ContractId))).FirstOrDefault();
            var getReportContract = _context.DetailContractor.Where(x => x.ContractId.Equals(Guid.Parse(reportContract.ContractId)))
            .Select(w => new ReportExportDto()
            {
                contractorNames = w.Contractor.Nombres,
                ContractorLastNames = w.Contractor.Apellidos,
                ContractorIdentification = w.Contractor.Identificacion,
                ContractorMail = w.Contractor.Correo,

                //JuridicProcess = w.Contractor.User.UserEmail,
                //UnitValue = Math.Ceiling(w.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(w.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.UnitValue.Value).FirstOrDefault()),
            })
            .AsNoTracking()
            .ToList();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Adquisiciones");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;
                //Create Headers and format them
                worksheet.Cells["A1"].Value = "REPORTE DEL ESTADO DE CONVENIO INTER ADMINISTRATIVO " + getDataContract.ProjectName + " CON NÚMERO "+ getDataContract.NumberProject;
                using (var r = worksheet.Cells["A1:k2"])
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
                worksheet.Cells["A3"].Value = "Consecutivo";
                worksheet.Cells["B3"].Value = "Cedula";
                worksheet.Cells["C3"].Value = "Nombres";
                worksheet.Cells["D3"].Value = "Apelllidos";
                worksheet.Cells["E3"].Value = "Correo";
                worksheet.Cells["F3"].Value = "Estado de la contratación";
                worksheet.Cells["G3"].Value = "Estado Proceso juridico";
                worksheet.Cells["H3"].Value = "Estado Proceso Contractual";
                worksheet.Cells["I3"].Value = "Solicitud de comite";
                worksheet.Cells["J3"].Value = "Estudios previos";
                worksheet.Cells["K3"].Value = "Minuta contrato";
 
                worksheet.Cells["A3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:K3"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                worksheet.Cells["A3:K3"].Style.Font.Bold = true;
                worksheet.Cells["A3:K3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A3:K3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A3:K3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A3:K3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                var date = DateTime.Now.ToString("dd/MM/yyyy");
                row = 5;
                int nro = 0;

                foreach (var user in getReportContract)
                {
                    //var durationContract = CalculateDateContract(user.InitialDate.Value, user.FinalDate.Value);
                    nro++;
                    worksheet.Cells[row, 1].Value = nro;
                    worksheet.Cells[row, 2].Value = user.ContractorIdentification;
                    worksheet.Cells[row, 3].Value = user.contractorNames;
                    worksheet.Cells[row, 4].Value = user.ContractorLastName;
                    worksheet.Cells[row, 5].Value = user.ContractorMail;
                    worksheet.Cells[row, 6].Value = "estado registro";
                    worksheet.Cells[row, 7].Value = "estado de juridico";
                    worksheet.Cells[row, 8].Value = "estados contractacion";
                    worksheet.Cells[row, 9].Value = "comite";
                    worksheet.Cells[row, 10].Value = "estudios previos";
                    worksheet.Cells[row, 11].Value = "minuta";
                    row++;

                }
                if (nro > 0)
                {
                    worksheet.Columns.AutoFit();
                    xlPackage.Workbook.Properties.Title = "Solicitud PAA";
                    xlPackage.Workbook.Properties.Author = "ITM";
                    xlPackage.Workbook.Properties.Created = DateTime.Now;
                    xlPackage.Workbook.Properties.Subject = "Solicitud PAA";
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


        public async Task<MemoryStream> GenerateSatisfactionReport(Guid contractId,string base64)
        {
            var getReportContract = _context.DetailContractor.Where(x => x.ContractId.Equals(contractId))
                    .Select(w => new ReportExportDto()
                    {
                        //ProjecName = w.Contract.ProjectName,
                        contractorNames = w.Contractor.Nombres,
                        ContractorLastNames = w.Contractor.Apellidos,
                        ContractorIdentification = w.Contractor.Identificacion,
                        ContractorMail = w.Contractor.Correo,
                        //phoneNumber = w.Contractor.Telefono,
                        //JuridicProcess = w.Contractor.User.UserEmail,
                        //UnitValue = Math.Ceiling(w.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(w.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.UnitValue.Value).FirstOrDefault()),
                    })
                    .AsNoTracking()
                    .ToList();
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

  
            using (var xlPackage = new ExcelPackage(stream))
            {

                var worksheet = xlPackage.Workbook.Worksheets.Add("JUSTICIA");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;
                //Create an Image object from the memory stream

                using (var workbook = new XLWorkbook())
                {
                    var worksheets = workbook.Worksheets.Add("MiHojaDeCalculo");

                    if (base64.StartsWith("data:image/png;base64,"))
                    {
                        base64 = base64.Substring("data:image/png;base64,".Length);
                    }
                   byte[] imageBytes = Convert.FromBase64String(base64);

                    // Agregar la imagen desde los bytes a la hoja de cálculo
                    var image = worksheets.AddPicture(new MemoryStream(imageBytes))
                        .MoveTo(worksheets.Cell("A1")) // Posición de la imagen en la celda A1
                        .WithSize(200, 200); // Tamaño de la imagen en píxeles



                    MemoryStream objImage = new MemoryStream(imageBytes);
                    Image image2 = Image.FromStream(objImage);
                    worksheets.Pictures.Add(stream);

                    workbook.SaveAs(stream);

                    // Establecer la posición del MemoryStream al principio
                    stream.Seek(0, SeekOrigin.Begin);
                }

                //Create Headers and format them
                //worksheet.Cells["A1"].Value = "RECIBIDO A SATISFACCIÓN PARA CONTRATISTAS                                                                                                                                                                          (CONTRATOS Y CONVENIOS INTERADMINISTRATIVOS)";
                //using (var r = worksheet.Cells["A1:Q3"])
                //{
                //    r.Style.WrapText = true;
                //    r.Merge = true;
                //    r.Style.Font.Color.SetColor(Color.Black);
                //    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                //    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //}
                //worksheet.Cells["A14"].Value = "No.";
                //worksheet.Cells["B14"].Value = "No. Contrato";
                //worksheet.Cells["C14"].Value = "vr Contrato";
                //worksheet.Cells["D14"].Value = "No. Adición";
                //worksheet.Cells["E14"].Value = "Documento de Identidad";
                //worksheet.Cells["F14"].Value = "Apellidos";
                //worksheet.Cells["G14"].Value = "Nombres";
                //worksheet.Cells["H14"].Value = "Rev CC";
                //worksheet.Cells["I14"].Value = "Periodo inicial";
                //worksheet.Cells["J14"].Value = "Periodo final";
                //worksheet.Cells["K14"].Value = " vr a pagar del periodo";
                //worksheet.Cells["L14"].Value = "vr Salud - Pensión - ARL";
                //worksheet.Cells["M14"].Value = "ARL - Riesgos Lab.";
                //worksheet.Cells["N14"].Value = "Aportes a Salud";
                //worksheet.Cells["O14"].Value = "Aportes a Pensión";
                //worksheet.Cells["P14"].Value = "(*) Novedad";
                //worksheet.Cells["Q14"].Value = "Días";
                //worksheet.Cells["R14"].Value = "Código área de negocios ";
                //worksheet.Cells["S14"].Value = "Nombre área de negocios ";
                //worksheet.Cells["T14"].Value = "No.PLANILLA SS PONER MES";
                //worksheet.Cells["U14"].Value = "NOVEDAD";
                //worksheet.Cells["V14"].Value = " ";
                //worksheet.Cells["W14"].Value = "IBC";
                //worksheet.Cells["X14"].Value = "PENSION";
                //worksheet.Cells["Y14"].Value = "SALUD";
                //worksheet.Cells["Z14"].Value = "NIVEL RIESGO";
                //worksheet.Cells["AA14"].Value = "ARL";
                //worksheet.Cells["AB14"].Value = "SUMATORIA";

                //worksheet.Cells["A4:AA4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells["A4:AA4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                //worksheet.Cells["A4:AA4"].Style.Font.Bold = true;
                //worksheet.Cells["A4:AA4"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //worksheet.Cells["A4:AA4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //worksheet.Cells["A4:AA4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //worksheet.Cells["A4:AA4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                //var year = DateTime.Now.Year;
                //var month = DateTime.Now.Month;
                //var day = DateTime.Now.Day;
                //var date = DateTime.Now.ToString("dd/MM/yyyy");
                //row = 5;
                //int nro = 0;
                //foreach (var user in getReportContract)
                //{
                //    //var durationContract = CalculateDateContract(user.InitialDate.Value, user.FinalDate.Value);
                //    nro++;
                //    worksheet.Cells[row, 1].Value = 80111600;
                //    //worksheet.Cells[row, 2].Value = user.ObjetoConvenio;
                //    worksheet.Cells[row, 3].Value = month;
                //    worksheet.Cells[row, 4].Value = month;
                //    worksheet.Cells[row, 5].Value = 0;
                //    //worksheet.Cells[row, 6].Value = durationContract;
                //    worksheet.Cells[row, 7].Value = "CCE-16";
                //    //worksheet.Cells[row, 8].Value = 0;
                //    //worksheet.Cells[row, 9].Value = user.ValorTotal;
                //    //worksheet.Cells[row, 10].Value = user.ValorTotal;
                //    worksheet.Cells[row, 11].Value = 0;
                //    worksheet.Cells[row, 12].Value = 0;
                //    worksheet.Cells[row, 13].Value = "Unidad estratégica de negociosos";
                //    worksheet.Cells[row, 14].Value = "CO-ANT-05001";
                //    //worksheet.Cells[row, 15].Value = user.User;
                //    //worksheet.Cells[row, 16].Value = 4405100;
                //    //worksheet.Cells[row, 17].Value = user.Email;
                //    worksheet.Cells[row, 18].Value = "Convenios-funcionamiento";
                //    //worksheet.Cells[row, 19].Value = user.Identificacion;
                //    //worksheet.Cells[row, 20].Value = user.Nombre;
                //    //worksheet.Cells[row, 21].Value = user.UnitValue;
                //    //worksheet.Cells[row, 22].Value = user.InitialDate;
                //    //worksheet.Cells[row, 23].Value = user.FinalDate;
                //    //worksheet.Cells[row, 24].Value = durationContract;
                //    //worksheet.Cells[row, 25].Value = user.ValorTotal;
                //    worksheet.Cells[row, 26].Value = 0;
                //    worksheet.Cells[row, 27].Value = 0;
                //    row++;

                //}
                //if (nro > 0)
                //{
                //    worksheet.Columns.AutoFit();
                //    xlPackage.Workbook.Properties.Title = "Solicitud PAA";
                //    xlPackage.Workbook.Properties.Author = "ITM";
                //    xlPackage.Workbook.Properties.Created = DateTime.Now;
                //    xlPackage.Workbook.Properties.Subject = "Solicitud PAA";
                //    xlPackage.Save();
                //}
                //else
                //{
                //    return null;
                //}

            }
            //stream.Position = 0;
            return await Task.FromResult(stream);
        }
        #endregion

        #region PRIVATE METHODS

        private string CalculateDateContract(DateTime initialDate, DateTime finalDate)
        {

            TimeSpan diferencia = finalDate - initialDate;

            int totalDias = (int)diferencia.TotalDays;
            int totalMeses = (int)(diferencia.TotalDays / 30.436875); // Promedio de días por mes
            int totalAnios = (int)(diferencia.TotalDays / 365.25); // Promedio de días por año

            int dias = totalDias % 30; // Días restantes después de los meses completos
            int meses = totalMeses % 12; // Meses restantes después de los años completos
            if (totalDias > 0 && totalMeses > 0 && totalAnios > 0)
            {
                return $"{totalAnios} años, {meses} meses, {dias} días";
            }
            else if (totalDias > 0 && totalMeses > 0)
            {
                return $"{meses} meses, {dias} días";
            }
            else
            {
                return $"{dias} días";
            }

        }

        private int DateInDays(DateTime initialDate, DateTime finalDate)
        {
            TimeSpan diferencia = finalDate - initialDate;
            // Muestra la diferencia en días
            return diferencia.Days;

        }

        private string SeparateObligation(string generalObligation, string specificObligation)
        {
            string unifyObligation = null;
            var generalObligationList = generalObligation.Split("->");
            var specificObligationList = specificObligation.Split("->");
            var ObligacionesGenerales = "OBLIGACIONES GENERALES: ";
            var ObligacionesEspecificas = "OBLIGACIONES ESPECIFICAS: ";
            var cont = 0;
            foreach (var item in generalObligationList)
            {
                cont++;
                if (cont == 1)
                {
                    unifyObligation =ObligacionesGenerales;
                }
                unifyObligation += item;
            }
            cont = 0;
            foreach (var item in specificObligationList)
            {
                cont++;
                if (cont == 1)
                {
                     unifyObligation += ObligacionesEspecificas;
                }
                unifyObligation += item;
            }
            return unifyObligation;
        }

        #endregion
    }
}
