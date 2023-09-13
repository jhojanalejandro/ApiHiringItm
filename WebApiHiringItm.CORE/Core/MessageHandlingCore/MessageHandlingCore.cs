using AutoMapper;
using MailKit.Security;
using Microsoft.Extensions.Options;
using System.Net;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
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
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using WebApiHiringItm.MODEL.Dto.MessageDto;

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

                var attachmentMessage = _context.UserFile.Where(x => x.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.ARCHIVOSMENSAJE.Description())).ToList().Count > 0 
                ? _context.UserFile.Where(x => x.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.ARCHIVOSMENSAJE.Description()))
                .Select(s => new FileAttach
                {
                    FileData = s.FileData,
                    FileName = s.FileNameC,
                    FileType = s.FileType,
                })
                .ToList() : null;

            if (attachmentMessage == null)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ATTACHMENTEMPTY);

            var getCredencialUser = _context.UserFile
                .Where(x => x.UserId.Equals(Guid.Parse(contractors.UserId)))
                .Select(s => new MailRequestContractor
                {
                    FromEmail = s.User.UserEmail,
                    Password = s.User.PasswordMail,
                    ImageMessage = s.FileData,
                    FileMessageAttach = attachmentMessage
                }
                )
                .AsNoTracking()
                .FirstOrDefault();

            if (getCredencialUser == null)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.IMAGEUSERMESSAGE);
            var getTermDateList = _context.TermContract
                .Include(i => i.DetailContractorNavigation)
                .Where(x => x.DetailContractorNavigation.ContractId.Equals(Guid.Parse(contractors.ContractId))).ToList();
            if (getTermDateList.Count == 0)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.TERMDATENOTFOUND);
            if (contractors.ContractorsId.Length > 0)
            {
                if (getTermDateList.Count() < contractors.ContractorsId.Count())
                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.TERMDATENOTFOUND);

                var resultDetailList = _context.DetailContractor
                    .Include(i => i.StatusContractorNavigation)
                    .Where(x =>  x.ContractId.Equals(contractors.ContractId)).ToList();
                foreach (Guid idContractor in contractors.ContractorsId)
                {
                    var getContractor = _context.Contractor.Where(x => x.Id.Equals(idContractor)).FirstOrDefault();
                    var getTermDate = getTermDateList.Find(x => x.DetailContractorNavigation.ContractorId.Equals(idContractor));

                    var resultDetail = resultDetailList.Find(x => x.ContractorId.Equals(idContractor));
                    if (getContractor != null)
                    {
                        getCredencialUser.ToEmail = getContractor.Correo;
                        getCredencialUser.TermDate = getTermDate.TermDate;
                        getContractor.ClaveUsuario = await createPassword(getCredencialUser);
                        getCredencialUser.Body = "Para ingresar utilice la contraseña Asignada  " +
                        " CONTRASEÑA ASIGNADA ES:    " + getContractor.ClaveUsuario + "   en el siguiente link http://localhost:4200/sign-in";
                        getCredencialUser.Subject = "PARTICIPACIÓN PROCESO DE CONTRATACIÓN";

                        await sendMessageInvitation(getCredencialUser);
                        //resultDetail.StatusContractor = getStatusId;
                        _context.Contractor.Update(getContractor);
                        _context.DetailContractor.Update(resultDetail);
                        await _context.SaveChangesAsync();
                    }

                }

                return ApiResponseHelper.CreateResponse<string>(Resource.MAILSENDSUCCESS);

            }
            else
            {
                //w.ClaveUsuario.Equals(NOASIGNADA) &&
                var contractorList = _context.Contractor.Where(w =>  w.DetailContractor.Select(s => s.ContractId).Contains(Guid.Parse(contractors.ContractId))).ToList();
                List<DetailContractor> listDetailContractor = new();
                if (getTermDateList.Count() < contractorList.Count())
                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.TERMDATENOTFOUND);
                var resultDetailList = _context.DetailContractor
                         .Where(x => x.ContractId.Equals(Guid.Parse(contractors.ContractId))).ToList();
                foreach (var item in contractorList)
                {
                    var resultDetail = resultDetailList.Find(f => f.ContractorId.Equals(item.Id));
                    var getTermDate = getTermDateList.Find(x => x.DetailContractorNavigation.ContractorId.Equals(item.Id));

                    getCredencialUser.ToEmail = item.Correo;
                    getCredencialUser.TermDate = getTermDate.TermDate;

                    item.ClaveUsuario = await createPassword(getCredencialUser);
                    getCredencialUser.Body = "Para ingresar utilice la contraseña Asignada  " +
                    " CONTRASEÑA ASIGNADA ES:    " + item.ClaveUsuario + "   en el siguiente link http://localhost:4200/sign-in";
                    getCredencialUser.Subject = "PARTICIPACIÓN PROCESO DE CONTRATACIÓN";

                    await sendMessageInvitation(getCredencialUser);
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

        public async Task<IGenericResponse<string>> SendContractorObservation(SendMessageObservationDto messageObservation)
        {
            var getStatusId = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.INVITADO.Description())).Select(s => s.Id).FirstOrDefault();
            var getContractor = _context.Contractor.Where(x => x.Id.Equals(messageObservation.ContractorId)).FirstOrDefault();

            var getCredencialUser = _context.UserFile
            .Where(x => x.UserId.Equals(Guid.Parse(messageObservation.UserId)))
            .Select(s => new MailRequestContractor
            {
                FromEmail = s.User.UserEmail,
                Password = s.User.PasswordMail,
                ImageMessage = s.FileData
            }
            )
            .AsNoTracking()
            .FirstOrDefault();
            getCredencialUser.ToEmail = getContractor.Correo;
            getCredencialUser.TermDate = messageObservation.TermDate;
            getCredencialUser.Documents = messageObservation.Documentos;

            await sendMessageObservation(getCredencialUser);
            throw new NotImplementedException();
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

            return finalString;
        }

        private async Task<bool> sendMessageInvitation(MailRequestContractor mailRequest)
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
            string mes = mailRequest.TermDate.ToString("MMMM");
            int anio = mailRequest.TermDate.Year;
            int diaMes = mailRequest.TermDate.Day;
            string diaDeLaSemana = mailRequest.TermDate.ToString("dddd");
            string cuerpoHTML = "<html> <body>" +
                                "<p>Nos permitimos informar que se está realizando el proceso de contratación para la prestación del servicio en el marco del contrato interadministrativo " + mailRequest.ContractNumber + " suscrito con el ITM, por tanto y para poder realizar el proceso contractual es necesario que por favor nos hagan llegar los documentos relacionados y así poder verificar el cumplimiento de los requisitos.</p>" +
                                "<p>Con plazo para enviar documentos hasta el "+ diaDeLaSemana.ToUpper() +" "+ diaMes + " DE " + mes.ToUpper()+ " DE " + anio + "</p>" +
                                "<p>Tener en cuenta las siguientes consideraciones:</p>" +
                                "<p>Deberán entregar 4 archivos de PDF de la siguiente forma:</p>" +
                                "<ol>" +
                                    "<li>1.       En un archivo de PDF nombrado con su nombre completo, en mayúscula sostenida y deberá incluir el escaneo de los siguientes documentos y en estricto orden como se relacionan:</li>" +
                                    "<ol type=" + "a" + ">" +
                                        "<li>Formato de hoja de vida debidamente firmada</li>" +
                                        "<li>Formato de bienes y rentas debidamente firmada y fechada (Por favor indicar en la primera hoja en los cuadros marcar para tomar posesión)</li>" +
                                        "<li>Copia de la cedula</li>" +
                                        "<li>Copia del RUT</li>" +

                                    "</ol>" +
                                    "<li>2.  Un segundo archivo de PDF nombrado con su nombre completo, en mayúscula sostenida seguido de la frase “Documentos de Contratación”  y deberá incluir el escaneo de los siguientes documentos y en estricto orden como se relacionan:</li>" +
                                        "<ol type=" + "a" + ">" +
                                            "<ul>" +
                                                "<li>Carta de ARL debidamente firmada y fechada</li>" +
                                                "<li>Formato de consignación de pagos firmada y fechada</li>" +
                                                "<li>Copia de libreta militar: En caso de no tener Libreta Militar, se acepta la certificación emitida por la página del ejército. Esto con base en el Concepto C ‒ 089 de 2021 de Colombia compra eficiente:</li>" +
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
                                            "</ul>" +
                                        "</ol>" +
                                    "<li>Un tercer archivo de PDF nombrado “EXAMEN PREOCUPACIONAL¨, deberá incluir el escaneo únicamente de: (VIGENTES, FECHA DE EXPEDICION MENOR A 3 AÑOS)</li>" +
                                        "<ol type=" + "a" + ">" +
                                            "<li> Exámenes pre - ocupacionales:</li>" +
                                            "<ul>" +
                                                "<li>Ficha médica ocupacional con énfasis osteomuscular y optometría.</li>" +
                                                "<li>Perfil lipídico(Colesterol total, colesterol Hdl, colesterol ldl y triglicéridos) y Glicemia en Ayunas. SOLO PARA PERSONAS MAYORES DE 40 AÑOS y que su objeto contractual tenga relación con trabajos en alturas. El concepto ocupacional debe estar indicado de manera clara la lectura de todos los exámenes solicitado</li>" +
                                                "<li>Los exámenes deberán estar avalados por un médico ocupacional, firmado y con el número de la licencia médica</li>" +
                                            "</ul>" +
                                        "</ol>" +

                                    "<li> Un cuarto archivo de PDF nombrado REGISTRO SECOP deberá incluir el escaneo únicamente de:</li>" +
                                        "<ol type=" + "a" + ">" +
                                            "<li> El pantallazo del registro exitoso como proveedor en Secop II </li>" +
                                        "</ol>" +
                             "</ol>" +
                             "<p> " + mailRequest.Body + "</p>" +
                             "<p> NOTA: La solicitud y recepción de los documentos antes relacionados no obligan al ITM a su contratación final.</p>" +
                             "<img src=\"cid:imagen1\" />" +
                        "</body> </html>";

            // Crea un nuevo correo electrónico
            MailMessage correo = new MailMessage(remitente, destinatario, asunto, cuerpoHTML);
            correo.IsBodyHtml = true;
            if (mailRequest.FileMessageAttach.Count > 0)
            {
                AlternateView contenidoHTML = AlternateView.CreateAlternateViewFromString(cuerpoHTML, null, MediaTypeNames.Text.Html);

                for (int i = 0; i < mailRequest.FileMessageAttach.Count; i++)
                {
                    string contentType = "application/" + mailRequest.FileMessageAttach[i].FileType; // Cambia esto según el tipo de archivo adjunto

                    byte[] attachBytes = Convert.FromBase64String(mailRequest.FileMessageAttach[i].FileData);
                    LinkedResource imagenAdicional = new LinkedResource(new MemoryStream(attachBytes), contentType);
                    imagenAdicional.ContentId = "pdf" + (i + 2);

                    System.Net.Mime.ContentType archivoAdjuntoContentType = new System.Net.Mime.ContentType(contentType);
                    archivoAdjuntoContentType.Name = mailRequest.FileMessageAttach[i].FileName + "." + mailRequest.FileMessageAttach[i].FileType; // Cambia "nombre_del_archivo.pdf" por el nombre deseado

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

        private async Task<bool> sendMessageObservation(MailRequestContractor mailRequest)
        {
            string remitente = mailRequest.FromEmail;
            SecureString contraseña = new SecureString();
            foreach (char c in mailRequest.Password)
            {
                contraseña.AppendChar(c);
            }
            string destinatario = mailRequest.ToEmail;
            string asunto = "SUBSANACIÓN DOCUMENTOS";

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
            string mes = mailRequest.TermDate.ToString("MMMM");
            int anio = mailRequest.TermDate.Year;
            int diaMes = mailRequest.TermDate.Day;
            string diaDeLaSemana = mailRequest.TermDate.ToString("dddd");
            string cuerpoHTML = "<html> <body>" +
                                    "<p>para la prestación del servicio en el marco del contrato interadministrativo " + mailRequest.ContractNumber + " suscrito con el ITM, se han encontrado errores en los documentos adjuntados, es necesario que por favor los actualice .</p>" +
                                    "<p>Con plazo para enviar documentos hasta el " + diaDeLaSemana + diaMes + " DE " + mes + " DE " + anio + "</p>" +
                                    "<p>Documentos que se deben modificar :</p>" +
                                    "<p>" + mailRequest.Documents + "</p>" +
                                    "<p>Tener en cuenta las siguientes observaciones:</p>" +
                                    "<p>"+ mailRequest.Body + "</p>" +

                                    "<img src=\"cid:imagen1\" />" +
                                "</body> </html>";

            // Crea un nuevo correo electrónico
            MailMessage correo = new MailMessage(remitente, destinatario, asunto, cuerpoHTML);
            correo.IsBodyHtml = true;

            // Envía el correo
            using (clienteSmtp)
            {
                clienteSmtp.Send(correo);
            }
            return true;
        }
    }
}
