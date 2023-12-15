using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;
using Aspose.Cells;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using WebApiHiringItm.CORE.Core.ImportExcelCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.MODEL.Dto;
using NPOI.SS.Formula.Functions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Properties;
using Microsoft.AspNetCore.Hosting;
using WebApiHiringItm.MODEL;

namespace WebApiHiringItm.CORE.Core.ImportExcelCore
{
    public class ImportExcelCore : IImportExcelCore
    {

        #region BUILDER
        private const string TIPOASIGNACIONELEMENTO = "Elemento";
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly MailSettings _mailSettings;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImportExcelCore(HiringContext context, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<MailSettings> mailSettings, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _mailSettings = mailSettings.Value;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<string> ImportarExcel(FileRequest model)
        {
            List<DetailContractor> listDetail = new List<DetailContractor>();
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
            Guid RollIdId = _context.Roll.Where(w => w.Code.Equals(RollEnum.Contratista.Description())).Select(s => s.Id).FirstOrDefault();
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
            DataColumn newColumn3 = new DataColumn("Fecha Creacion", typeof(DateTime));
            newColumn3.DefaultValue = DateTime.Now;
            dataTable.Columns.Add(newColumn3);
            DataColumn newColumn4 = new DataColumn("Fecha Actualizacion", typeof(DateTime));
            newColumn4.DefaultValue = DateTime.Now;
            dataTable.Columns.Add(newColumn4);
            DataColumn newColumnRollId = new DataColumn("Roll Id", typeof(Guid));
            newColumnRollId.DefaultValue = RollIdId;
            dataTable.Columns.Add(newColumnRollId);
            DataColumn newColumnEnableEdit = new DataColumn("EnableEdit", typeof(bool));
            newColumnEnableEdit.DefaultValue = true;
            dataTable.Columns.Add(newColumnEnableEdit);
            DataColumn newColumnableChangePassword = new DataColumn("EnableChangePassword", typeof(bool));
            newColumnableChangePassword.DefaultValue = true;
            dataTable.Columns.Add(newColumnableChangePassword);

            DataColumn newColumnEps = new DataColumn("Eps", typeof(Guid));
            newColumnEps.DefaultValue = null;
            dataTable.Columns.Add(newColumnEps);
            DataColumn newColumnArl = new DataColumn("Arl", typeof(Guid));
            newColumnArl.DefaultValue = null;
            dataTable.Columns.Add(newColumnArl); 
            DataColumn newColumnAfp = new DataColumn("Afp", typeof(Guid));
            newColumnAfp.DefaultValue = null;
            dataTable.Columns.Add(newColumnAfp);
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

                        }
                        else if (dataTable.Rows.Count == 0)
                        {
                            return "No se agrego la Información por que es repetida";
                        }

                        var valor = dataTable.Rows[j]["Identificacion"];
                        Guid idValor = (Guid)dataTable.Rows[j]["Id"];
                        dataTable.Rows[j]["Id"] = Guid.NewGuid();
                        if (valor != null)
                        {
                            DetailContractor DetailContractor = new DetailContractor();
                            DetailContractor.ContractId = model.ContractId;

                            DetailContractor.ContractorId = (Guid)dataTable.Rows[j]["Id"];
                            var resultado = _context.Contractor.FirstOrDefault(x => x.Identificacion.Equals(valor));
                            if (resultado != null)
                            {
                                DetailContractor.ContractorId = resultado.Id;
                                dataTable.Rows.Remove(dataTable.Rows[j]);
                                j--;
                            }
                            listDetail.Add(DetailContractor);
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

  
        static async Task HacerPeticionApi()
        {
            // URL de la API a la que quieres hacer la petición
            string apiUrl = "https://webapi.optoride.com/api/Passengers/RegisterPassenger";

            // Crear una instancia de HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Realizar la petición GET y obtener la respuesta
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Verificar si la petición fue exitosa (código de estado 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta como una cadena
                        string contenido = await response.Content.ReadAsStringAsync();

                        // Puedes procesar el contenido de la respuesta según tus necesidades
                        Console.WriteLine("Respuesta de la API: " + contenido);
                    }
                    else
                    {
                        Console.WriteLine("La petición no fue exitosa. Código de estado: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al hacer la petición: " + ex.Message);
                }
            }
        }

        public async Task<IGenericResponse<string>> ImportCdp(FileRequest model)
        {
            var getHiring = _context.DetailContractor
                    .Include(i => i.HiringData)    
                    .Where(w => w.ContractId.Equals(model.ContractId)).ToList();

            //string path = Path.Combine(@"D:\Trabajo\PROYECTOS\ITMHIRINGPROJECT\PruebaExcel\", model.Excel.FileName);
            string rutaSubida = Path.Combine(_hostingEnvironment.ContentRootPath, "Subidas");
            string rutaArchivo = Path.Combine(rutaSubida, model.Excel.FileName);
            if (!Directory.Exists(rutaSubida))
            {
                Directory.CreateDirectory(rutaSubida);
            }

            //Save the uploaded Excel file.
            string fileName = Path.GetFileName(model.Excel.FileName);
            string filePath = Path.Combine(rutaSubida, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                model.Excel.CopyTo(stream);
            }
            FileStream test = new FileStream(filePath, FileMode.Open);

            Workbook workbook = new Workbook(test);

            Worksheet worksheet = workbook.Worksheets[0];

            DataTable dataTable = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.LastCell.Column + 1, true);
            dataTable.TableName = "HiringData";
            List<HiringData> hiringDataList = new();

            dataTable.Columns[0].ColumnName = ToCamelCase(Regex.Replace(Regex.Replace(dataTable.Columns[0].ColumnName.Trim().Replace("(dd/mm/aaaa)", "").ToLowerInvariant(), @"\s", "_").ToLowerInvariant().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
            var columna = dataTable.Columns[0].ColumnName;
            var listaHring = _context.Contractor.ToList();

            if (columna.Equals("Identificadorbasededatos"))
            {
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    int posicion = j;
                    if (dataTable.Rows.Count == 1)
                    {
                        posicion = 0;

                    }
                    string idValor = null;
                    var valor = dataTable.Rows[j]["Identificadorbasededatos"];
                    if (valor != DBNull.Value)
                    {
                        
                        idValor = (string)valor;
                        var resultado = _context.HiringData.FirstOrDefault(x => x.ContractorId.Equals(Guid.Parse(idValor)));
                        if (resultado != null)
                        {
                            TypeCode tipoDatoCdp = Type.GetTypeCode(dataTable.Rows[j]["Número Contrato"].GetType());
                            TypeCode tipoDatoContract = Type.GetTypeCode(dataTable.Rows[j]["Número Contrato"].GetType());
                            if (tipoDatoCdp == TypeCode.String)
                            {
                                if ((string)dataTable.Rows[j]["CDP"] == null)
                                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.ERRORUPLOADEXCEL);
                            }

                            if (tipoDatoContract == TypeCode.String)
                            {
                                if ((string)dataTable.Rows[j]["Número Contrato"] == null)
                                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.ERRORUPLOADEXCEL);
                            }
                            var cdp = dataTable.Rows[j]["CDP"];
                            var contrato = dataTable.Rows[j]["Número Contrato"];
                            resultado.Cdp = Convert.ToString(cdp);
                            resultado.Contrato = Convert.ToString(contrato);
                            hiringDataList.Add(resultado);

                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
            test.Close();
            _context.HiringData.UpdateRange(hiringDataList);
            await _context.SaveChangesAsync();
            // Borra todos los archivos dentro de la carpeta
            foreach (string archivo in Directory.GetFiles(rutaSubida))
            {
                File.Delete(archivo);
            }
            Directory.Delete(rutaSubida);
            return ApiResponseHelper.CreateResponse<string>(Resource.EXCELIMPORTSUCCESS);
        }

        public async Task<IGenericResponse<string>> ImportElement(FileRequest model)
        {

            string path = Path.Combine(@"D:\Trabajo\PROYECTOS\ITMHIRINGPROJECT\PruebaExcelElemento\", model.Excel.FileName);
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
            dataTable.TableName = "ElementComponent";
            List<ElementComponent> elementDataList = new();
            dataTable.Columns[0].ColumnName = ToCamelCase(Regex.Replace(Regex.Replace(dataTable.Columns[0].ColumnName.Trim().Replace("(dd/mm/aaaa)", "").ToLowerInvariant(), @"\s", "_").ToLowerInvariant().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
            var columna = dataTable.Columns[0].ColumnName;
            var listaHring = _context.Contractor.ToList();

            if (columna.Equals("IdentificadorInterno"))
            {
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    int posicion = j;
                    if (dataTable.Rows.Count == 1)
                    {
                        posicion = 0;

                    }

                    string idValor = null;
                    var valor = dataTable.Rows[j]["IdentificadorInterno"];
                    if (valor != DBNull.Value)
                    {
                        idValor = (string)valor;
                        var resultado = _context.ElementComponent.FirstOrDefault(x => x.Id.Equals(Guid.Parse(idValor)));
                        if (resultado != null)
                        {
                            if (dataTable.Rows[j]["Perfil Academico Requerido"] != null && dataTable.Rows[j]["Perfil Experiencia Requerido"] != null)
                            {
                                var perfilAcademico = dataTable.Rows[j]["Perfil Academico Requerido"];
                                var perfilExperiencia = dataTable.Rows[j]["Perfil Experiencia Requerido"];

                                resultado.PerfilRequeridoAcademico = Convert.ToString(perfilAcademico);
                                resultado.PerfilRequeridoExperiencia = Convert.ToString(perfilExperiencia);

                                elementDataList.Add(resultado);
                            }

                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }

            test.Close();
            _context.ElementComponent.UpdateRange(elementDataList);
            _context.SaveChanges();
            return ApiResponseHelper.CreateResponse<string>(Resource.EXCELIMPORTSUCCESS);

        }

        public static string ToCamelCase(string str)
        {
            var words = str.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);

            words = words
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();

            return string.Join(string.Empty, words);
        }
        #endregion

        #region PRIVATE METHODS
        private async Task<bool> UpdateDetailContract(List<DetailContractor> listDetail)
        {
            try
            {
                var GetstatusContractor = _context.StatusContractor.FirstOrDefault(dt => dt.Code.Equals(StatusContractorEnum.ENREVISIÓN.Description()));
                for (int i = 0; i < listDetail.Count; i++)
                {

                    var resultDetail = _context.DetailContractor.FirstOrDefault(dt => dt.ContractorId == listDetail[i].ContractorId && dt.ContractId == listDetail[i].ContractId);
                    if (resultDetail != null)
                    {
                        listDetail.Remove(listDetail[i]);
                    }
                    else
                    {
                        Guid idD = Guid.NewGuid();
                        listDetail[i].Id = idD;
                        listDetail[i].Consecutive = 1;
                        listDetail[i].StatusContractor = GetstatusContractor.Id;
                    }
                }
                _context.DetailContractor.AddRange(listDetail);
                var result = await _context.SaveChangesAsync();
                if (result != 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }

        }

        #endregion
    }
}
