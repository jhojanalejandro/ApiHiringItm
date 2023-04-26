using OfficeOpenXml;
using System.ComponentModel;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;
using WebApiHiringItm.MODEL.Entities;
using Aspose.Cells;
using System.Data;
using Microsoft.AspNetCore.Http;
using WebApiHiringItm.CONTEXT.Context;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using WebApiHiringItm.MODEL.Models;
using WebApiHiringItm.CORE.Helpers;
using System.Net;
using MimeKit;
using MailKit.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AutoMapper;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using Microsoft.Extensions.Options;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.Contractors
{
    public class ContractorCore : IContractorCore 
    {
        private const string NOASIGNADA = "NoAsignada";
        private const bool HABILITADO = true;
        private const string TIPOASIGNACIONELEMENTO = "Elemento";
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly MailSettings _mailSettings;
        public ContractorCore(HiringContext context, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _mailSettings = mailSettings.Value;
        }
        public async Task<List<ContractorDto>> GetAll()
        {
            var result = _context.Contractor.Where(x => x.Id != null).ToList();
            var map = _mapper.Map<List<ContractorDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<ContractorPaymentsDto>> GetPaymentsContractorList(Guid contractId, Guid contractorId)
        {
            var result = _context.ContractorPayments
                        .Where(p => p.ContractorId == contractorId && p.ContractId == contractId).ToList();
            var map = _mapper.Map<List<ContractorPaymentsDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<ContractsContractorDto>> GetSeveralContractsByContractor(string contractorId)
        {
            var result = _context.DetailProjectContractor.Where(x => x.ContractorId.Equals(contractorId)).ToList();
            var map = _mapper.Map<List<ContractsContractorDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<List<ContractorDto>> GetByIdFolder(Guid id)
        {
            var contractor = _context.DetailProjectContractor.Where(x => x.ContractId == id)
                .Include(dt => dt.Contractor).Where(ct => ct.Contractor.Habilitado == HABILITADO)
                .Select(ct => new ContractorDto()
                {
                    Id = ct.Contractor.Id,  
                    Codigo = ct.Contractor.Codigo,
                    FechaInicio = ct.Contractor.FechaInicio,
                    FechaFin = ct.Contractor.FechaFin,
                    Nombre = ct.Contractor.Nombre,
                    Apellido = ct.Contractor.Apellido,
                    Identificacion = ct.Contractor.Identificacion,
                    LugarExpedicion = ct.Contractor.LugarExpedicion,
                    FechaNacimiento = ct.Contractor.FechaNacimiento,
                    Telefono = ct.Contractor.Telefono,
                    Celular = ct.Contractor.Celular,
                    Correo = ct.Contractor.Correo,
                    ComponenteId = ct.ComponenteId,
                    ElementId = ct.ElementId,
                    UserId = ct.Contractor.UserId
                })
                .ToList();
            var map = _mapper.Map<List<ContractorDto>>(contractor);
            return await Task.FromResult(map);
        }

        public async Task<ChargeAccountDto?> ChargeAccountGetById(Guid contractorId, Guid ContractId)
        {
            var result = _context.ContractorPayments
                .Include(co => co.Contractor)
                    .ThenInclude(x => x.DetailProjectContractor)
                        .ThenInclude(x => x.Contract)
                    .ThenInclude(x => x.DetailProjectContractor)
                        .ThenInclude(x => x.Element)
                 .Where(x => x.Contractor.Id == contractorId && x.Contract.Id == ContractId).OrderByDescending(x => x.FromDate);

            if (result != null)
            {
                return await result.Select(cb => new ChargeAccountDto
                {
                    Codigo = cb.Contractor.Codigo,
                    Convenio = cb.Contractor.Convenio,
                    Nombre = cb.Contractor.Nombre + " " + cb.Contractor.Apellido,
                    Identificacion = cb.Contractor.Identificacion,
                    Direccion = cb.Contractor.Direccion,
                    Departamento = cb.Contractor.Departamento,
                    Municipio = cb.Contractor.Municipio,
                    Barrio = cb.Contractor.Barrio,
                    Telefono = cb.Contractor.Telefono,
                    Celular = cb.Contractor.Celular,
                    Correo = cb.Contractor.Correo,
                    TipoAdministradora = cb.Contractor.TipoAdministradora,
                    Administradora = cb.Contractor.Administradora,
                    CuentaBancaria = cb.Contractor.CuentaBancaria,
                    TipoCuenta = cb.Contractor.TipoCuenta,
                    EntidadCuentaBancaria = cb.Contractor.EntidadCuentaBancaria,
                    ContractId = cb.Contract.Id,
                    From = cb.FromDate,
                    To = cb.ToDate,
                    Company = cb.Contract.CompanyName,
                    Paymentcant = cb.Paymentcant,
                    ContractNumber = cb.Contract.NumberProject,
                    LugarExpedicion = cb.Contractor.LugarExpedicion,
                    NombreElemento = cb.Contract.DetailProjectContractor.Select(ne => ne.Element.NombreElemento).FirstOrDefault()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            }
            return null;

        }

        public async Task<FilesDto?> GetDocumentPdf(Guid contractId, Guid contractorId)
        {
            var result = _context.Files
                 .Where(x => x.ContractId.Equals(contractId) && x.ContractorId.Equals(contractorId));

            return await result.Select(fl => new FilesDto
            {
                Id = fl.Id,
                FilesName = fl.FilesName,
                Filedata = fl.Filedata,
                TypeFile = fl.TypeFile,
                DescriptionFile = fl.DescriptionFile
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        }

        public async Task<bool> Delete(Guid id)
        {
            var resultData = _context.Contractor.Where(x => x.Id == id).FirstOrDefault();
            if (resultData != null)
            {
                resultData.Habilitado = false;
                var result = _context.Contractor.Update(resultData);
                await _context.SaveChangesAsync();

            }
            return true;
        }

        public async Task<bool> Create(ContractorDto model)
        {
            var getData = _context.Contractor.Where(x => x.Id == model.Id).FirstOrDefault();
            if (getData == null)
            {
                var map = _mapper.Map<Contractor>(model);
                _context.Contractor.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.Contractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;

        }




        public async Task<List<MinutaDto>> GetDataBill(ContractContractorsDto contractors)
        {
            try
            {
                var contractor = _context.DetailProjectContractor.Where(x => x.ContractId == contractors.contractId)
                .Include(dt => dt.Contractor).Where(ct => ct.Contractor.Habilitado == HABILITADO)
                .Include(hd => hd.HiringData)
                .Include(el => el.Element)
                .Include(el => el.Contract)
                .Where(w => contractors.contractors.Contains(w.Contractor.Id.ToString()));

                return await contractor.Select(ct => new MinutaDto
                {
                    ContractorId = ct.ContractorId,
                    FechaFinalizacionConvenio = ct.HiringData.FechaFinalizacionConvenio,
                    Contrato = ct.HiringData.Contrato,
                    Compromiso = ct.HiringData.Compromiso,
                    SupervisorItm = ct.HiringData.SupervisorItm,
                    CargoSupervisorItm = ct.HiringData.CargoSupervisorItm,
                    IdentificacionSupervisor = ct.HiringData.IdentificacionSupervisor,
                    FechaRealDeInicio = ct.HiringData.FechaRealDeInicio,
                    FechaDeComite = ct.HiringData.FechaDeComite,
                    Rubro = ct.Contract.Rubro,
                    NombreRubro = ct.Contract.NombreRubro,
                    FuenteRubro = ct.Contract.FuenteRubro,
                    Cdp = ct.HiringData.Cdp,
                    NumeroActa = ct.HiringData.NumeroActa,
                    NombreElemento = ct.Element.NombreElemento,
                    ObligacionesGenerales = ct.Element.ObligacionesGenerales,
                    ObligacionesEspecificas = ct.Element.ObligacionesEspecificas,
                    CantidadDias = ct.Element.CantidadDias,
                    ValorUnidad = ct.Element.ValorUnidad,
                    ValorTotal = ct.Element.ValorTotal,
                    Cpc = ct.Element.Cpc,
                    NombreCpc = ct.Element.NombreCpc,
                    Consecutivo = ct.Element.Consecutivo,
                    ObjetoElemento = ct.Element.ObjetoElemento,
                    TipoContratacion = ct.Contractor.TipoContratacion,
                    Codigo = ct.Contractor.Codigo,
                    Convenio = ct.Contractor.Convenio,
                    FechaInicio = ct.Contractor.FechaInicio,
                    FechaFin = ct.Contractor.FechaFin,
                    Nombre = ct.Contractor.Nombre + " " + ct.Contractor.Apellido,
                    Identificacion = ct.Contractor.Identificacion,
                    LugarExpedicion = ct.Contractor.LugarExpedicion,
                    FechaNacimiento = ct.Contractor.FechaNacimiento,
                    Direccion = ct.Contractor.Direccion,
                    Departamento = ct.Contractor.Departamento,
                    Municipio = ct.Contractor.Municipio,
                    Barrio = ct.Contractor.Barrio,
                    Telefono = ct.Contractor.Telefono,
                    Celular = ct.Contractor.Celular,
                    Correo = ct.Contractor.Correo,
                    TipoAdministradora = ct.Contractor.TipoAdministradora,
                    Administradora = ct.Contractor.Administradora,
                    CuentaBancaria = ct.Contractor.CuentaBancaria,
                    TipoCuenta = ct.Contractor.TipoCuenta,
                    EntidadCuentaBancaria = ct.Contractor.EntidadCuentaBancaria,
                    FechaCreacion = ct.Contractor.FechaCreacion,
                    FechaActualizacion = ct.Contractor.FechaActualizacion,
                    ObjetoConvenio = ct.Contractor.ObjetoConvenio,
                    CompanyName = ct.Contract.CompanyName,
                    DescriptionProject = ct.Contract.DescriptionProject,
                    NumberProject = ct.Contract.NumberProject,
                })
                  .AsNoTracking()
                  .ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }

        }


        public async Task<bool> AddNewness(NewnessContractorDto model)
        {
            var getData = _context.NewnessContractor.Where(x => x.Id.Equals(model.Id)).FirstOrDefault();
            var getContractor = _context.Contractor.Where(x => x.Id.Equals(Guid.Parse(model.ContractorId))).FirstOrDefault();
            if (getContractor != null)
            {
                getContractor.Habilitado = false;
                var result = _context.Contractor.Update(getContractor);
            }
            if (getData == null)
            {
                var map = _mapper.Map<NewnessContractor>(model);
                _context.NewnessContractor.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.NewnessContractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
        }
        #region PUBLIC METODS
        public async Task<string> ImportarExcel(FileRequest model)
        {
            List<DetailProjectContractor> listDetail = new List<DetailProjectContractor>();
            string path = Path.Combine(@"D:\Trabajo\PROYECTOS\ITMHIRINGPROJECT\PruebaExcel\", model.Excel.FileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Save the uploaded Excel file.
            string fileName = Path.GetFileName(model.Excel.FileName);
            string filePath = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                model.Excel.CopyTo(stream);
            }
            FileStream test = new FileStream(filePath, FileMode.Open);

            Workbook workbook = new Workbook(test);

            Worksheet worksheet = workbook.Worksheets[0];

            DataTable dataTable = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.LastCell.Column + 1, true);
            dataTable.TableName = "Contractor";
            DataColumn newColumn = new DataColumn("User Id", typeof(Guid));
            newColumn.DefaultValue = model.UserId;
            dataTable.Columns.Add(newColumn);
            DataColumn newColumnid = new DataColumn("Id", typeof(Guid));
            newColumnid.DefaultValue = Guid.NewGuid();
            dataTable.Columns.Add(newColumnid);
            DataColumn newColumnPassword = new DataColumn("Clave Usuario", typeof(string));
            newColumnPassword.DefaultValue = "NoAsignada";
            dataTable.Columns.Add(newColumnPassword);
            DataColumn newColumn2 = new DataColumn("Contract Id", typeof(Guid));
            newColumn2.DefaultValue = model.ContractId;
            dataTable.Columns.Add(newColumn2);
            DataColumn newColumn3 = new DataColumn("Fecha Creacion", typeof(DateTime));
            newColumn3.DefaultValue = DateTime.Now;
            dataTable.Columns.Add(newColumn3);
            DataColumn newColumn4 = new DataColumn("Fecha Actualizacion", typeof(DateTime));
            newColumn4.DefaultValue = DateTime.Now;
            dataTable.Columns.Add(newColumn4);
            //DataColumn componentId = new DataColumn("Componente Id", typeof(int));
            //componentId.DefaultValue = 0;
            //dataTable.Columns.Add(componentId);
            //DataColumn elementId = new DataColumn("Element Id", typeof(int));
            //elementId.DefaultValue = 0;
            //dataTable.Columns.Add(elementId);
            DataColumn objeto = new DataColumn("Objeto Convenio", typeof(string));
            objeto.DefaultValue = "vacio";
            dataTable.Columns.Add(objeto);
            DataColumn habiliatdo = new DataColumn("Habilitado", typeof(bool));
            habiliatdo.DefaultValue = true;
            dataTable.Columns.Add(habiliatdo);
            dataTable.Columns.Remove("Nombre Completo");

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                dataTable.Columns[i].ColumnName = ToCamelCase(Regex.Replace(Regex.Replace(dataTable.Columns[i].ColumnName.Trim().Replace("(dd/mm/aaaa)", "").ToLowerInvariant(), @"\s", "_").ToLowerInvariant().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
                var columna = dataTable.Columns[i].ColumnName;
                var listaHring = _context.Contractor.ToList();

                if (columna.Equals("Identificacion"))
                {
                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        int posicion = j;
                        if (dataTable.Rows.Count == 1)
                        {
                            posicion = 0;

                        }else if (dataTable.Rows.Count == 0)
                        {
                            return "No se agrego la Información por que es repetida";
                        }

                        var valor = dataTable.Rows[j]["Identificacion"];
                        Guid idValor = (Guid)dataTable.Rows[j]["Id"];
                        if (valor != null)
                        {
                            DetailProjectContractor detailProjectContractor = new DetailProjectContractor();
                            detailProjectContractor.ContractId = model.ContractId;
                            var resultado = _context.Contractor.FirstOrDefault(x => x.Identificacion.Equals(valor));
                            if (resultado != null)
                            {
                                detailProjectContractor.ContractorId = resultado.Id;
                                dataTable.Rows.Remove(dataTable.Rows[j]);
                                j--;
                            }
                            else
                            {
                                detailProjectContractor.ContractorId = Guid.NewGuid();
                                dataTable.Rows[j]["Id"] = detailProjectContractor.ContractorId;
                            }
                            listDetail.Add(detailProjectContractor);
                        }

                    }
                }

            }
            test.Close();
            var conn = _context.Database.GetConnectionString();
            //File.Delete(filePath);
            using (SqlConnection connection = new SqlConnection(conn))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    connection.Open();
                    foreach (DataColumn c in dataTable.Columns)
                        bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    bulkCopy.DestinationTableName = dataTable.TableName;
                    try
                    {
                        bulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                connection.Close();
            }
            List<Dictionary<string, object>> filas = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                filas.Add(row);
            }
            var result = JsonConvert.SerializeObject(filas);
            await UpdateDetailContract(listDetail);
            return await Task.FromResult(result);
        }
        public static string ToCamelCase(string str)
        {
            var words = str.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);

            words = words
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();

            return string.Join(string.Empty, words);
        }

        public async Task<List<ContractsContarctorDto>> getContractsByContractor(string contractorId)
        {
            return await _context.ProjectFolder
            .Include(i => i.Contractor)
            .Where(w => w.Contractor.Select(s => s.Id).FirstOrDefault().Equals(Guid.Parse(contractorId)))
            .Select(s => new ContractsContarctorDto()
            {
                Id = s.Id.ToString(),
                CompanyName = s.CompanyName
            })
            .AsNoTracking()
            .ToListAsync();


        }

        //public async Task<Contractor> SendMessageById(int idFolder)
        //{
        //    var result = _context.Contractor.Where(x => x.IdFolder == idFolder).ToList();
        //    foreach (var resultItem in result) { 

        //    }
        //    var map = _mapper.Map<Contractor>(result);
        //    return await Task.FromResult(map);
        //}
        public async Task<bool> SendContractorCount(SendMessageAccountDto ids)
        {
            if (ids.IdContratistas.Length > 0)
            {
                foreach (var idContractor in ids.IdContratistas)
                {

                    var  result = _context.Contractor.Where(x => x.ContractId.Equals(ids.IdContratistas) && x.Id == idContractor).FirstOrDefault();
                    result.ClaveUsuario = await createPassword(result.Correo);
                    _context.Contractor.Update(result);
                    var res = await _context.SaveChangesAsync();
                }
                return true;
            }
            else
            {
                var listContractor = _context.Contractor.Where(x => x.ClaveUsuario.Equals(NOASIGNADA)).ToList();
                foreach (var item in listContractor)
                {
                    item.ClaveUsuario = await createPassword(item.Correo);
                    _context.Contractor.Update(item);
                    var res = await _context.SaveChangesAsync();
                }
                return true;

            }
            return false;
        }

        public async Task<bool> UpdateAsignment(AsignElementOrCompoenteDto model)
        {
            if (model.Id != null)
            {
                try
                {
                    List<DetailProjectContractor> asignDataListUpdate = new List<DetailProjectContractor>();
                    for (int i = 0; i < model.IdContractor.Length; i++)
                    {
                        if (model.Type == TIPOASIGNACIONELEMENTO)
                        {
                            var contractorUpdate = _context.DetailProjectContractor.FirstOrDefault(d => d.ContractId == model.ContractId && d.ContractorId.Equals(model.IdContractor[i]));
                            if (contractorUpdate != null)
                            {
                                contractorUpdate.ElementId = model.Id;
                                asignDataListUpdate.Add(contractorUpdate);
                            }
                        }
                        else
                        {
                            var contractorUpdate = _context.DetailProjectContractor.FirstOrDefault(d => d.ContractId == model.ContractId && d.ContractorId.Equals(model.IdContractor[i]));
                            if (contractorUpdate != null)
                            {
                                contractorUpdate.ComponenteId = model.Id;
                                asignDataListUpdate.Add(contractorUpdate);
                            }
                        }
                    }
                    _context.DetailProjectContractor.UpdateRange(asignDataListUpdate);
                    var res = await _context.SaveChangesAsync();
                    return res != 0 ? true : false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error", ex);
                }
            }
            return false;
        }
        #endregion

        #region METODS PRIVATE

        private async Task<string> createPassword(string mail)
        {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            if (finalString != null)
            {
                var message = new MailRequestContractor();
                message.Body = "Cordial saludo señ@r contratista acontinuacion le adjuntaremos correo, contraseña y un link de ingreso para que ingresa  para que adjunte los diferentes documentos necesarios" + 
                    "tu Contraseña es:" + finalString + "Ingresa con tu correo";
                message.ToEmail = mail;
                message.Subject = "Clave dinamica";
                sendMessage(message);
            }

            return finalString;
        }
        private async Task<bool> sendMessage(MailRequestContractor mailRequest)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //email.From.Add(MailboxAddress.Parse("alejoyepes.1000@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            
            var builder = new BodyBuilder();
            //CreateTestMessage2();
            //message();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            var resp = smtp;
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            try
            {
                await smtp.SendAsync(email);
                return true;

            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
            return false;

        }

        private async Task<bool> Update(AddPasswordContractorDto model)
        {
            if (model.Id != null)

            {
                var userupdate = _context.Contractor.Where(x => x.Id.Equals(model.Id) && x.Identificacion.Equals(model.Documentodeidentificacion)).FirstOrDefault();
                var map = _mapper.Map(model, userupdate);
                _context.Contractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            return false;
        }
        private async Task<bool> UpdateDetailContract(List<DetailProjectContractor> listDetail)
        {
            try
            {
                for (int i = 0; i < listDetail.Count; i++)
                {
                    var resultDetail = _context.DetailProjectContractor.FirstOrDefault(dt => dt.ContractorId == listDetail[i].ContractorId && dt.ContractId == listDetail[i].ContractId);
                    if (resultDetail != null)
                    {
                        listDetail.Remove(listDetail[i]);
                    }
                    else
                    {
                        Guid idD = Guid.NewGuid();
                        listDetail[i].Id = idD;
                    }
                }
                _context.DetailProjectContractor.AddRange(listDetail);
                var result = await _context.SaveChangesAsync();
                if (result != 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw new Exception("Error", ex);

            }

        }

        #endregion
    }
}
