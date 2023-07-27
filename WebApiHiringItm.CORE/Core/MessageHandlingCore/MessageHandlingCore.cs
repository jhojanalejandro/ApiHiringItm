using AutoMapper;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;
using Outlook = Microsoft.Office.Interop.Outlook;
using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Security;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using NPOI.SS.Formula.Functions;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using System.Diagnostics.Contracts;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using Microsoft.Office.Interop.Outlook;

namespace WebApiHiringItm.CORE.Core.MessageHandlingCore
{
    public class MessageHandlingCore : IMessageHandlingCore
    {

        private const string NOASIGNADA = "NoAsignada";
        private readonly HiringContext _context;
        private readonly MailSettings _mailSettings;
        public MessageHandlingCore(HiringContext context, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mailSettings = mailSettings.Value;
        }
        public async Task<IGenericResponse<string>> SendContractorCount(SendMessageAccountDto contractors)
        {

            if (string.IsNullOrEmpty(contractors.ContractId) || !contractors.ContractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.GUIDNOTVALID);

            var getStatusId = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.INVITADO.Description())).Select(s => s.Id).FirstOrDefault();
            var getCredencialUser = _context.UserFile
                .Where(x => x.UserId.Equals(Guid.Parse(contractors.UserId)))
                .Select(s => new  MailRequestContractor
                {
                    FromEmail = s.User.UserEmail,
                    Password = s.User.PasswordMail,
                    ImageMessage = s.FileData
                }
                )
                .AsNoTracking()
                .FirstOrDefault();

                var attachmentMessage = _context.UserFile.Where(x => x.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.ARCHIVOSMENSAJE.Description())).ToList().Count > 0 
                ? _context.UserFile.Where(x => x.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.ARCHIVOSMENSAJE.Description()))
                .Select(s => new FileAttach
                {
                    FileData = s.FileData,
                    FileName = s.FileNameC,
                    FileType = s.FileType
                }).ToList() : null;

            if (attachmentMessage == null)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ATTACHMENTEMPTY);
            getCredencialUser.ImageMessageAttach = attachmentMessage;

            if (contractors.ContractorsId.Length > 0)
            {

                foreach (Guid idContractor in contractors.ContractorsId)
                {
                    var result = _context.Contractor.Where(x => x.Id.Equals(idContractor)).FirstOrDefault();
                    var resultDetail = _context.DetailContractor.Where(x => x.ContractorId.Equals(idContractor) && x.ContractId.Equals(contractors.ContractId)).FirstOrDefault();
                    if (result != null)
                    {
                        getCredencialUser.ToEmail = result.Correo;

                        result.ClaveUsuario = await createPassword(getCredencialUser);
                        //resultDetail.StatusContractor = getStatusId;
                        _context.Contractor.Update(result);
                        _context.DetailContractor.Update(resultDetail);
                        await _context.SaveChangesAsync();
                    }

                }

                return ApiResponseHelper.CreateResponse<string>(Resource.MAILSENDSUCCESS);

            }
            else
            {
                //w.ClaveUsuario.Equals(NOASIGNADA) &&
                var listContractor = _context.Contractor.Where(w =>  w.DetailContractor.Select(s => s.ContractId).Contains(Guid.Parse(contractors.ContractId))).ToList();
                List<DetailContractor> listDetailContractor = new();
                var resultDetailList = _context.DetailContractor
                         .Where(x => x.ContractId.Equals(Guid.Parse(contractors.ContractId))).ToList();
                foreach (var item in listContractor)
                {
                    var resultDetail = resultDetailList.Find(f => f.ContractorId.Equals(item.Id));
                    getCredencialUser.ToEmail = item.Correo;
                    item.ClaveUsuario = await createPassword(getCredencialUser);
                    resultDetail.StatusContractor = getStatusId;
                    _context.Contractor.Update(item);
                    _context.DetailContractor.Update(resultDetail);

                    //  listDetailContractor.Add(resultDetail);
                }
                //_context.DetailContractor.UpdateRange(listDetailContractor);
                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(Resource.MAILSENDSUCCESS);
            }
        }


        private async Task<string> createPassword(MailRequestContractor message)
        {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-*";

            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            if (finalString != null)
            {
                message.Body = "Para ingresar utilice la contraseña Asignada  " +
                    " CONTRASEÑA ASIGNADA ES:    " + finalString + "   en el siguiente link http://localhost:4200/sign-in";
                message.Subject = "PARTICIPACIÓN PROCESO DE CONTRATACIÓN";

                await sendMessage(message);
            }

            return finalString;
        }

        private async Task<bool> sendMessage(MailRequestContractor mailRequest)
        {
            string remitente = mailRequest.FromEmail;
            SecureString contraseña = new SecureString();
            foreach (char c in mailRequest.Password)
            {
                contraseña.AppendChar(c);
            }
            string destinatario = mailRequest.ToEmail;
            string asunto = mailRequest.Subject;

            // Crea una instancia de SmtpClient
            SmtpClient clienteSmtp = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
            clienteSmtp.Credentials = new NetworkCredential(remitente, contraseña);
            clienteSmtp.EnableSsl = true;

            // Crea una instancia de LinkedResource con la imagen en base64
            byte[] imageBytes = Convert.FromBase64String(mailRequest.ImageMessage);

            LinkedResource userImage = new LinkedResource(new MemoryStream(imageBytes), "image/jpg");
            userImage.ContentId = "imagen1";

            // Crea el objeto AlternateView para el contenido HTML
            //string cuerpoHTML = "<html><body><h1>"+ asunto + "</h1><img src=\"cid:imagen1\" /> <p>"+ mailRequest.Body + "<p/></body></html>";

            string cuerpoHTML = "<html> <body>" +
                                "<p>Nos permitimos informar que se está realizando el proceso de contratación para la prestación del servicio en el marco del contrato interadministrativo " + mailRequest.ContractNumber + " suscrito con el ITM, por tanto y para poder realizar el proceso contractual es necesario que por favor nos hagan llegar los documentos relacionados y así poder verificar el cumplimiento de los requisitos.</p>" +
                                "<p>Con plazo para enviar documentos hasta el MARTES 04 DE JULIO DE 2023</p>" +
                                "<p>Tener en cuenta las siguientes consideraciones:</p>" +
                                "<p>Deberán entregar 3 archivos de PDF de la siguiente forma:</p>" +
                                "<ol>" +
                                    "<li>En un archivo de PDF nombrado con su nombre completo, en mayúscula sostenida y deberá incluir el escaneo de los siguientes documentos y en estricto orden como se relacionan:</li>" +
                                    "<ol type=" + "a" + ">" +
                                        "<li>Formato de hoja de vida debidamente firmada</li>" +
                                        "<li>Formato de bienes y rentas debidamente firmada y fechada (Por favor indicar en la primera hoja en los cuadros marcar para tomar posesión)</li>" +
                                        "<li>Formato de consignación de pagos firmada y fechada</li>" +
                                        "<li>Carta de ARL debidamente firmada y fechada</li>" +
                                        "<li>Copia de la cedula</li>" +
                                        "<li>Copia de libreta militar: En caso de no tener Libreta Militar, se acepta la certificación emitida por la página del ejército. Esto con base en el Concepto C ‒ 089 de 2021 de Colombia compra eficiente:</li>" +
                                        "<li>Copia del RUT</li>" +
                                        "<li>Certificación bancaria de cuenta de ahorros personal o cuenta de nómina personal, expedida por la entidad bancaria (Fecha de expedición del mes en que se está llevando a cabo el proceso contractual)</li>" +
                                        "<li>Fotocopia de los certificados de acreditación académica</li>" +
                                        "<li>Fotocopia de la tarjeta profesional, de requerirla la profesión. En las profesiones del área de la salud, adjuntar la resolución por medio de la cual se autoriza ejercerla</li>" +
                                        "<li>Copia de certificados laborales</li>" +
                                        "<li>Certificado de procuraduría</li>" +
                                        "<li>Certificado de contraloría</li>" +
                                        "<li>Certificado medidas correctivas</li>" +
                                        "<li>Certificado de antecedente judicial</li>" +
                                        "<li>Certificado de afiliación a salud</li>" +
                                        "<li>Certificado de afiliación a pensión</li>" +
                                        "<li>Certificado de delitos sexuales</li>" +
                                    "</ol>" +
                                    "<li>Un segundo archivo de PDF nombrado “EXAMEN PREOCUPACIONAL¨, deberá incluir el escaneo únicamente de: (VIGENTES, FECHA DE EXPEDICION MENOR A 3 AÑOS)</li>" +
                                 "<ol type=" + "a" + ">" +
                                     "<li> Exámenes pre - ocupacionales:</li>" +
                                     "<ul>" +
                                         "<li> Ficha médica ocupacional con énfasis osteomuscular y optometría.</li>" +
                                         "<li> Perfil lipídico(Colesterol total, colesterol Hdl, colesterol ldl y triglicéridos) y Glicemia en Ayunas. SOLO PARA PERSONAS MAYORES DE 40 AÑOS y que su objeto contractual tenga relación con trabajos en alturas. El concepto ocupacional debe estar indicado de manera clara la lectura de todos los exámenes solicitado</li>" +
                                         "<li> Los exámenes deberán estar avalados por un médico ocupacional, firmado y con el número de la licencia médica</li>" +
                                     "</ul>" +
                                 "</ol>" +

                                 "<li> Un tercer archivo de PDF nombrado REGISTRO SECOP deberá incluir el escaneo únicamente de:</li>" +
                                 "<ol type=" + "a" + ">" +
                                     "<li> El pantallazo del registro exitoso como proveedor en Secop II </li>" +
                                 "</ol>" +
                             "</ol>" +
                             "<p> " + mailRequest.Body + "</p>" +
                             "<p> NOTA: La solicitud y recepción de los documentos antes relacionados no obligan al ITM a su contratación final.</p>" +
                             "<img src=\"cid:imagen1\" style=\"width: 300px; height: auto;\"/>" +
                        "</body> </html>";

            // Crea un nuevo correo electrónico
            MailMessage correo = new MailMessage(remitente, destinatario, asunto, cuerpoHTML);
            correo.IsBodyHtml = true;
            if (mailRequest.ImageMessageAttach.Count > 0)
            {
                AlternateView contenidoHTML = AlternateView.CreateAlternateViewFromString(cuerpoHTML, null, MediaTypeNames.Text.Html);

                for (int i = 0; i < mailRequest.ImageMessageAttach.Count; i++)
                {
                    string contentType = "application/" + mailRequest.ImageMessageAttach[i].FileType; // Cambia esto según el tipo de archivo adjunto

                    byte[] attachBytes = Convert.FromBase64String(mailRequest.ImageMessageAttach[i].FileData);
                    LinkedResource imagenAdicional = new LinkedResource(new MemoryStream(attachBytes), contentType);
                    imagenAdicional.ContentId = "pdf" + (i + 2);

                    System.Net.Mime.ContentType archivoAdjuntoContentType = new System.Net.Mime.ContentType(contentType);
                    archivoAdjuntoContentType.Name = mailRequest.ImageMessageAttach[i].FileName + "." + mailRequest.ImageMessageAttach[i].FileType; // Cambia "nombre_del_archivo.pdf" por el nombre deseado

                    // Agrega el archivo adjunto a LinkedResource
                    imagenAdicional.ContentType = archivoAdjuntoContentType;
                    contenidoHTML.LinkedResources.Add(imagenAdicional);
                }
                //contenidoHTML.LinkedResources.Add(attachImage);
                correo.AlternateViews.Add(contenidoHTML);
            }


            // Envía el correo
            using (clienteSmtp)
            {
                clienteSmtp.Send(correo);
            }
            return true;
        }
    }
}
