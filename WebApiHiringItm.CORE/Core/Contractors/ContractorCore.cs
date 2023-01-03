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
using WebApiHiringItm.MODEL.Dto.CuentaCobroDto;
using WebApiHiringItm.MODEL.Dto.FileDto;

namespace WebApiHiringItm.CORE.Core.Contractors
{
    public class ContractorCore : IContractorCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly MailSettings _mailSettings;
        static readonly byte[] keys = Encoding.UTF8.GetBytes("401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1");
        public ContractorCore(Hiring_V1Context context, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _mailSettings = mailSettings.Value;
        }

        public async Task<List<ContractorDto>> GetAll()
        {
            var result = _context.Contractor.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<ContractorDto>>(result);
            return await Task.FromResult(map);

        }

        public async Task<List<ContractorDto>> GetByIdFolder(int id)
        {
            var contractor = _context.Contractor.Where(x => x.ContractId == id).ToList();
            var map = _mapper.Map<List<ContractorDto>>(contractor);
            return await Task.FromResult(map);
        }
        public async Task<CuentaCobroDto> GetById(int id)
        {
            var result = _context.Contractor.Include(co => co.ContractorPayments).Include(x => x.Contract)
                .Where(x => x.Id == id).FirstOrDefault();
            var payment =  result.ContractorPayments.OrderByDescending(X => X.FromDate).FirstOrDefault();

            var map = _mapper.Map<CuentaCobroDto>(result);
            map.UnitValue = payment.Paymentcant;
            map.From = payment.FromDate;
            map.To = payment.ToDate;
            map.Company = result.Contract.CompanyName;
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.Contractor.Where(x => x.Id == id).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.Contractor.Remove(resultData);
                    await _context.SaveChangesAsync();

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
            return false;
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
        #region PUBLIC METODS
        public async Task<string> ImportarExcel(FileRequest model)
        {
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
            DataColumn newColumn = new DataColumn("User Id", typeof(int));
            newColumn.DefaultValue = model.UserId;
            dataTable.Columns.Add(newColumn);
            DataColumn newColumnid = new DataColumn("Id", typeof(int));
            dataTable.Columns.Add(newColumnid);
            DataColumn newColumnPassword = new DataColumn("Clave Usuario", typeof(string));
            newColumnPassword.DefaultValue = "NoAsignada";
            dataTable.Columns.Add(newColumnPassword);
            DataColumn newColumn2 = new DataColumn("Contract Id", typeof(int));
            newColumn2.DefaultValue = model.ContractId;
            dataTable.Columns.Add(newColumn2);
            DataColumn newColumn3 = new DataColumn("Fecha Creacion", typeof(DateTime));
            newColumn3.DefaultValue = DateTime.Now;
            dataTable.Columns.Add(newColumn3);
            DataColumn newColumn4 = new DataColumn("Fecha Actualizacion", typeof(DateTime));
            newColumn4.DefaultValue = DateTime.Now;
            dataTable.Columns.Add(newColumn4);
            DataColumn componentId = new DataColumn("Componente Id", typeof(int));
            componentId.DefaultValue = 0;
            dataTable.Columns.Add(componentId);
            DataColumn elementId = new DataColumn("Element Id", typeof(int));
            elementId.DefaultValue = 0;
            dataTable.Columns.Add(elementId);
            DataColumn objeto = new DataColumn("Objeto Convenio", typeof(string));
            objeto.DefaultValue = "vacio";
            dataTable.Columns.Add(objeto);
            dataTable.Columns.Remove("Nombre Completo");
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {

                dataTable.Columns[i].ColumnName = ToCamelCase(Regex.Replace(Regex.Replace(dataTable.Columns[i].ColumnName.Trim().Replace("(dd/mm/aaaa)", "").ToLowerInvariant(), @"\s", "_").ToLowerInvariant().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
                var columna = dataTable.Columns[i].ColumnName;
                var listaHring = _context.Contractor.ToList();

                if (columna.Equals("Identificacion") && listaHring.Count > 0)
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
                        var resultado = _context.Contractor.FirstOrDefault(x => x.Identificacion.Equals(valor));
                        if (resultado != null)
                            {
                            dataTable.Rows.Remove(dataTable.Rows[j]);
                            j--;
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
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var getUser = _context.Contractor.Where(x => x.Correo == model.Username && x.ClaveUsuario.Equals(model.Password)).FirstOrDefault();

            if (getUser == null)
            {
                return null;
            }
            var token = generateJwtToken(getUser);

            return new AuthenticateResponse(getUser, token);
        }
        public string generateJwtToken(Contractor user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
                var listContractor = _context.Contractor.ToList();
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
        private async Task<bool> Add(FilesDto model)
        {
            if (model.Id != 0)

            {
                var userupdate = _context.Files.Where(x => x.ContractorId.Equals(model.ContractorId) && x.ContractId.Equals(model.ContractId)).FirstOrDefault();
                var map = _mapper.Map(model, userupdate);
                _context.Files.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            return false;
        }
        private async Task<bool> Update(AddPasswordContractorDto model)
        {
            if (model.Id != 0)

            {
                var userupdate = _context.Contractor.Where(x => x.Id.Equals(model.Id) && x.Identificacion.Equals(model.Documentodeidentificacion)).FirstOrDefault();
                var map = _mapper.Map(model, userupdate);
                _context.Contractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            return false;
        }

        public async Task<bool> UpdateAsignment(AsignElementOrCompoenteDto model)
            {
            if (model.Id != 0)

            {
                try
                {
                    if (model.Type == "Elemento")
                    {
                        var contractorUpdate = _context.Contractor.Where(x => x.Id.Equals(model.IdContractor)).FirstOrDefault();
                        contractorUpdate.ElementId = model.Id;
                        _context.Contractor.Update(contractorUpdate);
                        var res = await _context.SaveChangesAsync();
                        return res != 0 ? true : false;
                    }
                    else
                    {
                        var contractorUpdate = _context.Contractor.Where(x => x.Id.Equals(model.IdContractor)).FirstOrDefault();
                        contractorUpdate.ComponenteId = model.Id;
                        _context.Contractor.Update(contractorUpdate);
                        var res = await _context.SaveChangesAsync();
                        return res != 0 ? true : false;
                    }
                }
                catch (Exception ex)
                {
                        throw new Exception("Error", ex);
                }
             


            }
            return false;
        }


        #endregion
    }
}
