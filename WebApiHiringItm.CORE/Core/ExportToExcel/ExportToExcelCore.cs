using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.ExportDataDto;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;
using Color = System.Drawing.Color;

namespace WebApiHiringItm.CORE.Core.ExportToExcel
{
    public class ExportToExcelCore : IExportToExcelCore
    {
        #region Dependency
        private readonly HiringContext _context;
        private readonly IContractorCore _contractorCore;
        #endregion

        #region Constructor
        public ExportToExcelCore(HiringContext context, IContractorCore contractorCore)
        {
            _context = context;
            _contractorCore = contractorCore;
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
                    ObjetoElemento = w.DetailContractor.Element.ObjetoElemento,
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
                    if (user.InitialDate != null && user.FinalDate != null && user.NombreElemento != null && user.ValorTotal != null && user.ObjetoElemento != null && user.NombreElemento != null)
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
                        worksheet.Cells[row, 13].Value = user.ObjetoElemento;
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
                var data = _context.EconomicdataContractor.Where(x => x.DetailContractor.ContractId.Equals(ContractId));

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
                    Cedula = w.DetailContractor.Contractor.Identificacion,
                    ValorTotal = w.TotalValue
                })
                .AsNoTracking()
                .ToList();
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                var date = DateTime.Now.ToString("yyyyMMdd");

                foreach (var user in dataList)
                {
                    if (user.Consecutivo != null && user.NombreElemento != null && user.NumeroConvenio != null && user.Cpc != null && user.ValorTotal > 0)
                    {
                        nro++;
                        string valorTotal = user.ValorTotal.ToString("N0");

                        worksheet.Cells[row, 1].Value = 108;
                        worksheet.Cells[row, 2].Value = nro;
                        worksheet.Cells[row, 3].Value = 49;
                        worksheet.Cells[row, 4].Value = nro;
                        worksheet.Cells[row, 5].Value = date;
                        worksheet.Cells[row, 6].Value = 1;
                        worksheet.Cells[row, 7].Value = user.NumeroConvenio + "/" + year + user.NombreElemento + "(" + user.Consecutivo + ")";
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

        public async Task<MemoryStream> ExportSolicitudPaa(ControllerBase controller, Guid contractId)
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
                    .Where(x => x.DetailContractor.ContractId.Equals(contractId));
                var usermanager = _context.UserT.Where(w => w.Roll.Code.Equals(RollEnum.JEFEUNIDADESTRATEGICA.Description())).FirstOrDefault();
                var result = data.Select(w => new DetailProjectContractorDto()
                {
                    ContractorId = w.DetailContractor.ContractorId.ToString(),
                    Convenio = w.DetailContractor.Contract.ProjectName,
                    CompanyName = w.DetailContractor.Contract.CompanyName,
                    NombreComponente = w.DetailContractor.Component.NombreComponente,
                    Nombre = w.DetailContractor.Contractor.Nombres + " " + w.DetailContractor.Contractor.Apellidos,
                    Identificacion = w.DetailContractor.Contractor.Identificacion,
                    ObjetoElemento = w.DetailContractor.Element.ObjetoElemento,
                    ValorTotal = Math.Ceiling(w.TotalValue),
                    InitialDate = w.DetailContractor.HiringData.FechaRealDeInicio,
                    FinalDate = w.DetailContractor.HiringData.FechaFinalizacionConvenio,
                    User = w.DetailContractor.Contractor.User.UserName,
                    Email = w.DetailContractor.Contractor.User.UserEmail,
                    EmailManager = usermanager.UserEmail,
                    UserManager = usermanager.UserName,
                    ChargeManager = usermanager.Professionalposition,
                    IdentificationManager = usermanager.Identification,
                    UnitValue = Math.Ceiling(w.UnitValue),
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
                    if (user.ObjetoElemento != null && user.ValorTotal != null && user.InitialDate.HasValue && user.FinalDate.HasValue)
                    {
                        var durationContract = DateInDays(user.InitialDate.Value, user.FinalDate.Value);
                        string numeroFormateado = user.ValorTotal.ToString("N0");

                        nro++;
                        worksheet.Cells[row, 1].Value = 80111600;
                        worksheet.Cells[row, 2].Value = user.ObjetoElemento;
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
                        worksheet.Cells[row, 17].Value = user.EmailManager;
                        worksheet.Cells[row, 18].Value = "Convenios-funcionamiento";
                        worksheet.Cells[row, 19].Value = user.IdentificationManager;
                        worksheet.Cells[row, 20].Value = user.UserManager;
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

        public async Task<MemoryStream> ExportElement(ControllerBase controller, Guid contractId)
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
                    .Where(x => x.Component.ContractId.Equals(contractId));

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
            var getReportContract = await _contractorCore.GetContractorsByContract(reportContract.ContractId);
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Adquisiciones");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;
                //Create Headers and format them
                worksheet.Cells["A1"].Value = "REPORTE DEL ESTADO DE CONVENIO INTER ADMINISTRATIVO " + getDataContract.ProjectName + " CON NÚMERO " + getDataContract.NumberProject;
                using (var r = worksheet.Cells["A1:J2"])
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
                worksheet.Cells["D3"].Value = "Correo";
                worksheet.Cells["E3"].Value = "Estado de la contratación";
                worksheet.Cells["F3"].Value = "Estado Proceso juridico";
                worksheet.Cells["G3"].Value = "Estado Proceso Contractual";
                worksheet.Cells["H3"].Value = "Solicitud de comite";
                worksheet.Cells["I3"].Value = "Estudios previos";
                worksheet.Cells["J3"].Value = "Minuta contrato";

                worksheet.Cells["A3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:J3"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(70, 130, 180));
                worksheet.Cells["A3:J3"].Style.Font.Bold = true;
                worksheet.Cells["A3:J3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A3:J3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A3:J3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A3:J3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                var date = DateTime.Now.ToString("dd/MM/yyyy");
                row = 4;
                int nro = 0;

                foreach (var user in getReportContract.Data)
                {
                    nro++;
                    worksheet.Cells[row, 1].Value = nro;
                    worksheet.Cells[row, 2].Value = user.Identificacion;
                    worksheet.Cells[row, 3].Value = user.Nombre;
                    worksheet.Cells[row, 4].Value = user.Correo;
                    worksheet.Cells[row, 5].Value = user.StatusContractor;
                    worksheet.Cells[row, 6].Value = user.LegalProccess;
                    worksheet.Cells[row, 7].Value = user.HiringStatus;
                    worksheet.Cells[row, 8].Value = user.ComiteGenerated;
                    worksheet.Cells[row, 9].Value = user.PreviusStudy;
                    worksheet.Cells[row, 10].Value = user.MinuteGnenerated;
                    row++;
                    //if (user.StatusContractor == "INVITADO")
                    //{
                    //    worksheet.Cells[row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    worksheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    //}else if (user.StatusContractor == "EN REVISIÓN")
                    //{
                    //    worksheet.Cells[row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    worksheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(Color.Red);

                    //}else if (user.StatusContractor == "CONTRATANDO")
                    //{
                    //    worksheet.Cells[row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    worksheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                    //}

                }
                if (nro > 0)
                {
                    worksheet.Columns.AutoFit();
                    xlPackage.Workbook.Properties.Title = "REPORTE DE ESTADO";
                    xlPackage.Workbook.Properties.Author = "ITM";
                    xlPackage.Workbook.Properties.Created = DateTime.Now;
                    xlPackage.Workbook.Properties.Subject = "REPORTE DE ESTADO";
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


        public async Task<MemoryStream> GenerateSatisfactionReport(Guid contractId, string base64)
        {

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("JUSTICIA");
                worksheet.Cells["E1"].Value = "RECIBIDO A SATISFACCIÓN PARA CONTRATISTAS (CONTRATOS Y CONVENIOS INTERADMINISTRATIVOS)";
                using (var r = worksheet.Cells["E1:O3"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
                worksheet.Cells["P1"].Value = "Código";
                worksheet.Cells["P2"].Value = "Versión";
                worksheet.Cells["P3"].Value = "Fecha";
                worksheet.Cells["R1"].Value = "FG 016";
                worksheet.Cells["R2"].Value = "02";
                worksheet.Cells["R3"].Value = "13-09-2019";
                worksheet.Cells["A4"].Value = "Medellin," + DateTime.Now;
                worksheet.Cells["A6"].Value = "En mi calidad de supervisor de los contratos de prestación de servicios del  contrato N° 4600094802  DE 2022 cuyo objeto es \"CONTRATO INTERADMINISTRATIVO PARA EL FORTALECIMIENTO DE LOS PROCESOS DE ATENCIÓN EN LAS CASAS DE JUSTICIA Y COMISARÍAS DE FAMILIA DE LA CIUDAD..\", me permito certificar que las personas que relaciono a continuación, cumplieron con las obligaciones contraídas en el objeto contractual según los informes de productos entregados por los contratistas. Igualmente certifico que se ha verificado el pago de los aportes al sistema de seguridad social en salud, pensión y riesgos laborales (ARL) correspondientes al periodo de cobro.\r\n\r\nPor lo anterior considero pertinente desembolsar el pago a las siguientes personas.\r\n";

                worksheet.Cells["A14"].Value = "No. ";
                worksheet.Cells["B14"].Value = "No. Contrato";
                worksheet.Cells["C14"].Value = "vr Contrato";
                worksheet.Cells["D14"].Value = "No. Adición";
                worksheet.Cells["E14"].Value = "Documento de Identidad";
                worksheet.Cells["F14"].Value = "Apellidos";
                worksheet.Cells["G14"].Value = "Nombres";
                worksheet.Cells["H14"].Value = "Periodo inicial";
                worksheet.Cells["I14"].Value = "Periodo final";
                worksheet.Cells["J14"].Value = "vr a pagar del periodo";
                worksheet.Cells["K14"].Value = "vr Salud - Pensión - ARL";
                worksheet.Cells["L14"].Value = "ARL - Riesgos Lab.";
                worksheet.Cells["M14"].Value = "Aportes a Salud";
                worksheet.Cells["N14"].Value = "Aportes a Pensión";
                worksheet.Cells["O14"].Value = "(*) Novedad";
                worksheet.Cells["P14"].Value = "Días";
                worksheet.Cells["Q14"].Value = "Código área de negocios";
                worksheet.Cells["R14"].Value = "Nombre área de negocios ";
                worksheet.Cells["S14"].Value = "No.PLANILLA SS ";
                worksheet.Cells["T14"].Value = "NOVEDAD";

                worksheet.Cells["W14"].Value = "IBC";
                worksheet.Cells["X14"].Value = "PENSION";
                worksheet.Cells["Y14"].Value = "SALUD";
                worksheet.Cells["Z14"].Value = "NIVEL RIESGO";
                worksheet.Cells["AA14"].Value = "ARL";
                worksheet.Cells["AB14"].Value = "SUMATORIA";


                worksheet.Cells["A14:AB14"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A14:AB14"].Style.Fill.BackgroundColor.SetColor(Color.White);
                worksheet.Cells["A14:AB14"].Style.Font.Bold = true;
                worksheet.Cells["A14:AB14"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A14:AB14"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A14:AB14"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A14:AB14"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                using (var r = worksheet.Cells["A6:s12"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                using (var r = worksheet.Cells["A4:H4"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                using (var r = worksheet.Cells["P1:Q1"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                }

                using (var r = worksheet.Cells["P2:Q2"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //border.Bottom.Color.SetColor(Color.Green);  // Color rojo
                using (var r = worksheet.Cells["P3:Q3"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                using (var r2 = worksheet.Cells["R3:S3"])
                {
                    r2.Style.WrapText = true;
                    r2.Merge = true;
                    r2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                using (var r2 = worksheet.Cells["R1:S1"])
                {
                    r2.Style.WrapText = true;
                    r2.Merge = true;
                    r2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                using (var r2 = worksheet.Cells["R2:S2"])
                {
                    r2.Style.WrapText = true;
                    r2.Merge = true;
                    //r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                    r2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                using (var r = worksheet.Cells["A1:D3"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                const int startRow = 1;
                if (base64.StartsWith("data:image/png;base64,"))
                {
                    base64 = base64.Substring("data:image/png;base64,".Length);
                }

                byte[] imageBytes = Convert.FromBase64String(base64);

                // Guardar la imagen temporal en formato PNG
                string tempFilePath = Path.GetTempFileName();
                string imagePath = Path.ChangeExtension(tempFilePath, "png"); // Cambiar la extensión a .png
                File.WriteAllBytes(imagePath, imageBytes);

                FileInfo imageFile = new FileInfo(imagePath);

                // Agregar la imagen a la hoja de cálculo
                var row = 16;
                int nro = 0;
                //var data = _context.ElementComponent
                //    .Include(i => i.Component)
                //    .Include(i => i.Activity)
                //    .Where(x => x.Component.ContractId.Equals(contractId));

                //var dataList = data.Select(w => new ExportElementDto()
                //{
                //    Id = w.Id.ToString(),
                //    Consecutive = w.Consecutivo,
                //    ElementName = w.NombreElemento,
                //    ComponentName = w.Component.NombreComponente,
                //    ActivityName = w.Activity.NombreActividad
                //})
                //.AsNoTracking()
                //.ToList();
                var getReportContract = _context.ContractorPaymentSecurity
                    .Include(i => i.DetailContractorNavigation)
                    .Include(i => i.ContractorPaymentsNavigation).Where(x => x.DetailContractorNavigation.ContractId.Equals(contractId)).OrderByDescending(o => o.Consecutive)
                        .Select(w => new ReportExportDto()
                        {
                            ProjectName = w.DetailContractorNavigation.Contract.ProjectName,
                            ContractorName = w.DetailContractorNavigation.Contractor.Nombres,
                            ContractorLastName = w.DetailContractorNavigation.Contractor.Apellidos,
                            ContractorIdentification = w.DetailContractorNavigation.Contractor.Identificacion,
                            ContractorMail = w.DetailContractorNavigation.Contractor.Correo,
                            ContractNumber = w.DetailContractorNavigation.Contract.NumberProject,
                            ContractValue = w.ContractorPaymentsNavigation.EconomicdataContractorNavigation.TotalValue,

                            NoAddition = w.DetailContractorNavigation.ChangeContractContractor.OrderByDescending(o => o.Consecutive).Select(s => s.NoAddition).FirstOrDefault(),
                            InitialPeriod = w.ContractorPaymentsNavigation.FromDate,
                            FinalPeriod = w.ContractorPaymentsNavigation.ToDate,

                            TotalValuePeriodPayment = w.ContractorPaymentsNavigation.Paymentcant,
                            EpsValue = w.PaymentEps,
                            ArlValue = w.PaymentArl,
                            AfpValue = w.PaymentPension,

                            AreaCode = w.DetailContractorNavigation.Contract.AreaCode,
                                            
                            AreaName = w.DetailContractorNavigation.Contract.NumberProject+ ' ' + w.DetailContractorNavigation.Contract.ProjectName + ' ' + w.DetailContractorNavigation.Contract.RegisterDateContract.Value.Year ,
                            PayrollNumber = w.PayrollNumber
                            //UnitValue = Math.Ceiling(w.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(w.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.UnitValue.Value).FirstOrDefault()),
                        })
                        .AsNoTracking()
                        .ToList();
                foreach (var item in getReportContract)
                {
                    nro++;
                    worksheet.Cells[row, 1].Value = nro;
                    worksheet.Cells["A:I"].AutoFitColumns(); // Ajusta automáticamente el ancho de las columnas dentro del rango

                    worksheet.Cells[row, 2].Value = item.ContractNumber;
                    worksheet.Cells[row, 3].Value = item.ContractValue;
                    worksheet.Cells[row, 4].Value = item.NoAddition;
                    worksheet.Cells[row, 5].Value = item.ContractorIdentification;
                    worksheet.Cells[row, 6].Value = item.ContractorLastName;
                    worksheet.Cells[row, 7].Value = item.ContractorName;
                    worksheet.Cells[row, 8].Value = item.InitialPeriod;
                    worksheet.Cells[row, 9].Value = item.FinalPeriod;
                    worksheet.Cells[row, 10].Value = item.TotalValuePeriodPayment;
                    worksheet.Cells[row, 11].Value = item.ArlValue;
                    worksheet.Cells[row, 12].Value = item.EpsValue;
                    worksheet.Cells[row, 13].Value = item.AfpValue;
                    worksheet.Cells[row, 14].Value = "N/N";
                    worksheet.Cells[row, 15].Value = item.CantDays;
                    worksheet.Cells[row, 16].Value = item.AreaCode;
                    worksheet.Cells[row, 17].Value = item.AreaName;
                    worksheet.Cells[row, 18].Value = item.PayrollNumber;
                    worksheet.Cells[row, 19].Value = item.News;

                    row++;
                }

                //for (int i = 0; i < getReportContract.Count; i++)
                //{
                //    nro++;
                //    worksheet.Cells[row, 1].Value = nro;
                //    worksheet.Cells["A:I"].AutoFitColumns(); // Ajusta automáticamente el ancho de las columnas dentro del rango

                //    worksheet.Cells[row, 2].Value = "user.Identificacion";
                //    worksheet.Cells[row, 3].Value = "user.Nombre";
                //    worksheet.Cells[row, 4].Value = "user.Correo";
                //    worksheet.Cells[row, 5].Value = "user.StatusContractor";
                //    worksheet.Cells[row, 6].Value = "user.LegalProccess";
                //    worksheet.Cells[row, 7].Value = "user.HiringStatus";
                //    worksheet.Cells[row, 8].Value = "user.ComiteGenerated";
                //    worksheet.Cells[row, 9].Value = "user.PreviusStudy";
                //    worksheet.Cells[row, 10].Value = "user.MinuteGnenerated";
                //    worksheet.Cells[row, 11].Value = "user.MinuteGnenerated";
                //    worksheet.Cells[row, 12].Value = "user.MinuteGnenerated";
                //    worksheet.Cells[row, 13].Value = "user.MinuteGnenerated";
                //    worksheet.Cells[row, 14].Value = "user.MinuteGnenerated";
                //    worksheet.Cells[row, 15].Value = "user.MinuteGnenerated";
                //    worksheet.Cells[row, 16].Value = "user.MinuteGnenerated";
                //    row++;
                //}
                var excelImage = worksheet.Drawings.AddPicture("imageName", imageFile);
                excelImage.SetPosition(startRow, 0, 1, 0); // Row, Column, StartRow, StartColumn
                excelImage.SetSize(160, 40);

                xlPackage.Save(); // Guardar el paquete Excel
                stream.Seek(0, SeekOrigin.Begin); // Establecer la posición del MemoryStream al principio

                //foreach (var user in 5)
                //{
                //    nro++;
                //    worksheet.Cells[row, 1].Value = nro;
                //    worksheet.Cells["A:T"].AutoFitColumns(); // Ajusta automáticamente el ancho de las columnas dentro del rango

                //    worksheet.Cells[row, 2].Value = "user.Identificacion";
                //    worksheet.Cells[row, 3].Value = "user.Nombre";
                //    worksheet.Cells[row, 4].Value = "user.Correo";
                //    worksheet.Cells[row, 5].Value = "user.StatusContractor";
                //    worksheet.Cells[row, 6].Value = "user.LegalProccess";
                //    worksheet.Cells[row, 7].Value = "user.HiringStatus";
                //    worksheet.Cells[row, 8].Value = "user.ComiteGenerated";
                //    worksheet.Cells[row, 9].Value = "user.PreviusStudy";
                //    worksheet.Cells[row, 10].Value = "user.MinuteGnenerated";
                //    row++;
                //}

            }
            //stream.Position = 0;
            return await Task.FromResult(stream);
        }

        public async Task<MemoryStream> GenerateEconomicTable(Guid contractId, string base64)
        {
            var ComponentsList = _context.Component
                .Where(w => w.ContractId.Equals(contractId)).ToList();
            if (ComponentsList.Count <= 0)
            {
                return null;
            }
            var lisIdComponent = ComponentsList.Select(w => w.Id).ToList();
            var ActivitiesList = _context.Activity.Where(w => lisIdComponent.Contains(w.ComponentId)).ToList();
            var idLidActivity = ActivitiesList.Select(w => w.Id).ToList();
            var ElementsList = _context.ElementComponent.Where(w => idLidActivity.Contains(w.ActivityId.Value) || lisIdComponent.Contains(w.ComponentId.Value)).ToList();
            var getReportContract = _context.DetailContract.OrderByDescending(o => o.Consecutive).Where(x => x.ContractId.Equals(contractId))
                    .Select(w => new EconomicTableDto()
                    {
                        ProjecName = w.Contract.ProjectName,
                        TotalValue = w.Contract.ValorContrato,
                        SubTotal = w.Contract.ValorSubTotal,
                        OperatingExpenses = w.Contract.GastosOperativos,
                        CompanyName = w.Contract.CompanyName
                    })
                    .AsNoTracking()
                    .FirstOrDefault();
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                //int row = 5; // Fila
                //int col = 1; // Columna

                var worksheet = xlPackage.Workbook.Worksheets.Add("JUSTICIA");
                worksheet.Cells["C1"].Value = "INSTITUTO TECNOLÓGICO METROPOLITANO";
                worksheet.Cells["C3"].Value = getReportContract.CompanyName;
                worksheet.Cells["C4"].Value = getReportContract.ProjecName;


                using (var r = worksheet.Cells["C1:G2"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                using (var r = worksheet.Cells["C3:G3"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                using (var r = worksheet.Cells["C4:G4"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                worksheet.Cells["H1"].Value = "Código";
                worksheet.Cells["H2"].Value = "Versión";
                worksheet.Cells["H3"].Value = "Fecha";
                worksheet.Cells["I1"].Value = "FPS 002";
                worksheet.Cells["I2"].Value = "05";
                worksheet.Cells["I3"].Value = " ";
                worksheet.Cells["A5"].Value = "Descripción/ Componente";
                worksheet.Cells["D5"].Value = "Cant.";
                worksheet.Cells["E5"].Value = "Duración";
                worksheet.Cells["F5"].Value = "Vr. Unidad";
                worksheet.Cells["G5"].Value = "Vr. Total";
                worksheet.Cells["H5"].Value = "REQUIERE CONTRATARSE (Marque X)";
                worksheet.Cells["H7"].Value = "SI";
                worksheet.Cells["I7"].Value = "NO";

                using (var r = worksheet.Cells["H7"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
                using (var r = worksheet.Cells["I7"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
                #region Styles
                using (var r = worksheet.Cells["H3:H4"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                using (var r = worksheet.Cells["H5:I6"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
                using (var r = worksheet.Cells["I3:I4"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }


                using (var r = worksheet.Cells["F5:F7"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                using (var r = worksheet.Cells["G5:G7"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                using (var r = worksheet.Cells["D5:D7"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
                using (var r = worksheet.Cells["E5:E7"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }

                using (var r = worksheet.Cells["A5:C7"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;

                }

                using (var r = worksheet.Cells["H1"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                }

                using (var r = worksheet.Cells["H2"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                using (var r2 = worksheet.Cells["I1"])
                {
                    r2.Style.WrapText = true;
                    r2.Merge = true;
                    r2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                using (var r2 = worksheet.Cells["I2"])
                {
                    r2.Style.WrapText = true;
                    r2.Merge = true;
                    //r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 232, 230));
                    r2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                using (var r = worksheet.Cells["A1:B4"])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Distributed;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Distributed;

                }
                #endregion

                var number = 8;
                for (int i = 0; i < ComponentsList.Count; i++)
                {
                    var getActivities = ActivitiesList.Where(f => f.ComponentId.Equals(ComponentsList[i].Id)).Select(s => s.Id).ToList();
                    var elements = ElementsList.Where(w => getActivities.Contains(w.ComponentId.Value) || w.ComponentId.Equals(ComponentsList[i].Id)).Count();
                    var cellValueComponent = "A" + number.ToString();
                    var sumCel = elements == 0 ? number + getActivities.Count - 1 : number + elements - 1;
                    if (i == ComponentsList.Count - 1)
                    {
                        sumCel = sumCel + 3;
                    }
                    var cellValue2 = "A" + sumCel;
                    using (var r = worksheet.Cells[cellValueComponent + ":" + cellValue2])
                    {
                        r.Style.WrapText = true;
                        r.Merge = true;
                        //r.Style.Font.Color.SetColor(Color.Black);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    }

                    if (getActivities.Count > 0)
                    {

                        for (int j = 0; j < getActivities.Count; j++)
                        {
                            var elementAcyivties = ElementsList.Where(w => w.ActivityId.Equals(getActivities[j])).ToList();
                            var cellValueAct = "B" + number;

                            if (elementAcyivties.Count() == 1 || elementAcyivties.Count() == 0)
                            {
                                using (var r = worksheet.Cells[cellValueAct])
                                {
                                    r.Style.WrapText = true;
                                    r.Merge = true;
                                    //r.Style.Font.Color.SetColor(Color.Black);
                                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    //r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                }
                                number += 1;
                            }
                            else
                            {
                                var cellValueAct2 = "B" + (elementAcyivties.Count() > 1 ? number + elementAcyivties.Count() - 1 : number + elementAcyivties.Count());
                                using (var r = worksheet.Cells[cellValueAct + ":" + cellValueAct2])
                                {
                                    r.Style.WrapText = true;
                                    r.Merge = true;
                                    //r.Style.Font.Color.SetColor(Color.Black);
                                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    //r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                }
                                number = elementAcyivties.Count() + number;
                            }

                        }

                    }


                }

                var cellValueText = "B" + number;
                var cellValueText2 = "F" + number;
                using (var r = worksheet.Cells[cellValueText + ":" + cellValueText2])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    //r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }
                cellValueText = "G" + number;
                using (var r = worksheet.Cells[cellValueText])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    //r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                cellValueText = "G" + (number + 1);
                using (var r = worksheet.Cells[cellValueText])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    //r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }

                cellValueText = "G" + (number + 2);
                using (var r = worksheet.Cells[cellValueText])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    //r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }

                cellValueText = "H" + (number);
                cellValueText2 = "I" + (number + 2);
                using (var r = worksheet.Cells[cellValueText + ":" + cellValueText2])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    //r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }


                cellValueText = "B" + (number + 1);
                cellValueText2 = "F" + (number + 1);
                using (var r = worksheet.Cells[cellValueText + ":" + cellValueText2])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    //r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }

                cellValueText = "B" + (number + 2);
                cellValueText2 = "F" + (number + 2);
                using (var r = worksheet.Cells[cellValueText + ":" + cellValueText2])
                {
                    r.Style.WrapText = true;
                    r.Merge = true;
                    //r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }
                const int startRow = 1;
                if (base64.StartsWith("data:image/png;base64,"))
                {
                    base64 = base64.Substring("data:image/png;base64,".Length);
                }

                byte[] imageBytes = Convert.FromBase64String(base64);

                // Guardar la imagen temporal en formato PNG
                string tempFilePath = Path.GetTempFileName();
                string imagePath = Path.ChangeExtension(tempFilePath, "png"); // Cambiar la extensión a .png
                File.WriteAllBytes(imagePath, imageBytes);

                FileInfo imageFile = new FileInfo(imagePath);

                // Agregar la imagen a la hoja de cálculo
                var row = 8;
                int nro = 0;
                var data = _context.ElementComponent
                    .Include(i => i.Component)
                    .Include(i => i.Activity)
                    .Where(x => x.Component.ContractId.Equals(contractId));

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

                for (int i = 0; i < ComponentsList.Count; i++)
                {
                    nro++;
                    worksheet.Cells[row, 1].Value = ComponentsList[i].NombreComponente;
                    var filteractivity = ActivitiesList.Where(w => w.ComponentId.Equals(ComponentsList[i].Id)).ToList();
                    for (int j = 0; j < filteractivity.Count; j++)
                    {

                        worksheet.Cells[row, 2].Value = filteractivity[j].NombreActividad;
                        var filterElement = ElementsList.Where(w => w.ActivityId.Equals(filteractivity[j].Id)).ToList();
                        for (int k = 0; k < filterElement.Count; k++)
                        {
                            worksheet.Cells["A:I"].AutoFitColumns(); // Ajusta automáticamente el ancho de las columnas dentro del rango
                            worksheet.Cells[row, 3].Value = filterElement[k].NombreElemento;
                            worksheet.Cells[row, 4].Value = filterElement[k].CantidadContratistas;
                            worksheet.Cells[row, 5].Value = filterElement[k].CantidadDias;
                            worksheet.Cells[row, 6].Value = filterElement[k].ValorUnidad;
                            worksheet.Cells[row, 7].Value = filterElement[k].ValorTotal;
                            worksheet.Cells[row, 8].Value = "X";
                            row++;
                        }

                    }
                    if (ActivitiesList.Count == 0 && ElementsList.Count() > 0)
                    {

                        for (int k = 0; k < ElementsList.Count; k++)
                        {
                            var filterElement = ElementsList.Where(w => w.ComponentId.Equals(ComponentsList[i].Id)).ToList();

                            worksheet.Cells["A:I"].AutoFitColumns();
                            worksheet.Cells[row, 3].Value = filterElement[k].NombreElemento;
                            worksheet.Cells[row, 4].Value = filterElement[k].CantidadContratistas;
                            worksheet.Cells[row, 5].Value = filterElement[k].CantidadDias;
                            worksheet.Cells[row, 6].Value = filterElement[k].ValorUnidad;
                            worksheet.Cells[row, 7].Value = filterElement[k].ValorTotal;
                            worksheet.Cells[row, 8].Value = "X";
                            row++;
                        }
                    }

                }

                worksheet.Cells[row, 2].Value = "TOTAL APOYO TECNICO Y/O PROFESIONAL";
                worksheet.Cells[row, 7].Value = getReportContract.SubTotal;
                row++;
                worksheet.Cells[row, 2].Value = "GASTOS OPERATIVOS Y/O ADMIN 8%";
                worksheet.Cells[row, 7].Value = getReportContract.OperatingExpenses;
                row++;
                worksheet.Cells[row, 2].Value = "TOTAL";
                worksheet.Cells[row, 7].Value = getReportContract.TotalValue;
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 15;
                worksheet.Column(3).Width = 50;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 15;
                worksheet.Column(7).Width = 15;
                worksheet.Column(8).Width = 10;
                worksheet.Column(9).Width = 10;


                var excelImage = worksheet.Drawings.AddPicture("imageName", imageFile);
                excelImage.SetPosition(startRow, 1, 1, 1); // Row, Column, StartRow, StartColumn
                excelImage.SetSize(140, 40);

                xlPackage.Save(); // Guardar el paquete Excel
                stream.Seek(0, SeekOrigin.Begin); // Establecer la posición del MemoryStream al principio

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
                    unifyObligation = ObligacionesGenerales;
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
