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
using WebApiHiringItm.CORE.Core.ExcelCore.interfaces;
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
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.ExcelCore
{
    public class UploadExcelCore : IUploadExcelCore
    {
        #region Dependency
        private readonly Hiring_V1Context _context;
        private readonly MailSettings _mailSettings;
        static readonly byte[] keys = Encoding.UTF8.GetBytes("401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1");
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public UploadExcelCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        public async Task<string> ImportarExcel(FileRequest obj)
        {
            string path = Path.Combine(@"D:\Trabajo\PROYECTOS\ITMHIRINGPROJECT\PruebaExcel\", obj.Excel.FileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Save the uploaded Excel file.
            string fileName = Path.GetFileName(obj.Excel.FileName);
            string filePath = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                obj.Excel.CopyTo(stream);
            }
            FileStream test = new FileStream(filePath, FileMode.Open);

            Workbook workbook = new Workbook(test);

            Worksheet worksheet = workbook.Worksheets[0];

            DataTable dataTable = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.LastCell.Column + 1, true);
            dataTable.TableName = "ExcelInfo";
            dataTable.Columns.Add("IdUsuario", typeof(Int32));
            dataTable.Columns.Add("IdContrato", typeof(Int32));
            dataTable.Columns.Add("FechaCreacion", typeof(DateTime));
            dataTable.Columns.Add("FechaActualizacion", typeof(DateTime));
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                switch (dataTable.Columns[i].ColumnName)
                {
                    case "IdUsuario":
                        dataTable.Columns[i].DefaultValue = obj.IdUser;
                        break;
                    case "IdContrato":
                        dataTable.Columns[i].DefaultValue = obj.IdContrato;
                        break;
                    case "FechaCreacion":
                        dataTable.Columns[i].DefaultValue = DateTime.Now;
                        break;
                    case "FechaActualizacion":
                        dataTable.Columns[i].DefaultValue = DateTime.Now;
                        break;
                }
                dataTable.Columns[i].ColumnName = ToCamelCase(Regex.Replace(Regex.Replace(dataTable.Columns[i].ColumnName.Trim().Replace("(dd/mm/aaaa)", "").ToLowerInvariant(), @"\s", "_").ToLowerInvariant().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
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
        private async Task<string> SearchMails(int idAgreement)
        {
            var result = _context.ExcelInfo.Where(x => x.IdContrato.Equals(idAgreement)).ToList();
            foreach (var item in result)
            {
                createPassword(item.Correo);

            }

            return "";
        }
        #endregion

        #region METODS PRIVATE

        private async Task<string> createPassword(string mail)
        {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            if (finalString != null)
            {
                var message = new MailRequestContractor();
                message.Body = "Cordial saludo señ@r contratista acontinuacion le adjuntaremos correo, contraseña y un link de ingreso para que ingresa  para que adjunte los diferentes documetnos mecesarios" ;
                message.ToEmail = mail;
                message.Password = finalString;
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
                var userupdate = _context.Files.Where(x => x.IdContractor.Equals(model.IdContractor) && x.IdFolder.Equals(model.IdFolder)).FirstOrDefault();
                var map = _mapper.Map(model, userupdate);
                _context.Contractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            return false;
        }
        private async Task<bool> Update(AddPasswordContractorDto model)
        {
            if (model.Id != 0)

            {
                var userupdate = _context.Contractor.Where(x => x.Id.Equals(model.Id) && x.DocumentoDeIdentidificacion.Equals(model.Documentodeidentificacion)).FirstOrDefault();
                var map = _mapper.Map(model, userupdate);
                _context.Contractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            return false;
        }
        #endregion
    }
}
