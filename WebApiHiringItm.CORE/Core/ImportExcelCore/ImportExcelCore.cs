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
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.MODEL.Dto;
using NPOI.SS.Formula.Functions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public ImportExcelCore(HiringContext context, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _mailSettings = mailSettings.Value;
        }
        #endregion

        #region PUBLIC METHODS
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
                            DetailProjectContractor detailProjectContractor = new DetailProjectContractor();
                            detailProjectContractor.ContractId = model.ContractId;
                            detailProjectContractor.ContractorId = (Guid)dataTable.Rows[j]["Id"];
                            var resultado = _context.Contractor.FirstOrDefault(x => x.Identificacion.Equals(valor));
                            if (resultado != null)
                            {
                                detailProjectContractor.ContractorId = resultado.Id;
                                dataTable.Rows.Remove(dataTable.Rows[j]);
                                j--;
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

        public async Task<string> ImportCdp(FileRequest model)
        {
            var getHiring = _context.DetailProjectContractor
                    .Include(i => i.HiringData)    
                    .Where(w => w.ContractId.Equals(model.ContractId)).ToList();

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
            dataTable.TableName = "HiringData";
            List<HiringData> hiringDataList = new();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                dataTable.Columns[i].ColumnName = ToCamelCase(Regex.Replace(Regex.Replace(dataTable.Columns[i].ColumnName.Trim().Replace("(dd/mm/aaaa)", "").ToLowerInvariant(), @"\s", "_").ToLowerInvariant().Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
                var columna = dataTable.Columns[i].ColumnName;
                var listaHring = _context.Contractor.ToList();

                if (columna.Equals("Cedula"))
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

                        string idValor = (string)dataTable.Rows[j]["IdentificadorBaseDedatos"];
                        if (idValor != null)
                        {
                            var resultado = _context.HiringData.FirstOrDefault(x => x.ContractorId.Equals(Guid.Parse(idValor)));
                            if (resultado != null)
                            {
                                if ((double)dataTable.Rows[j]["CDP"] != null && (double)dataTable.Rows[j]["Número Contrato"] != null)
                                {
                                    var cdp = dataTable.Rows[j]["CDP"];
                                    var contrato = dataTable.Rows[j]["Número Contrato"];
                                    resultado.Cdp = Convert.ToString(cdp);
                                    resultado.Contrato = Convert.ToString(contrato);
                                    hiringDataList.Add(resultado);
                                }

                            }
                        }

                    }
                }
            }

            test.Close();
            _context.HiringData.UpdateRange(hiringDataList);
            _context.SaveChanges();
            return "Registro exitoso";
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
        private async Task<bool> UpdateDetailContract(List<DetailProjectContractor> listDetail)
        {
            try
            {
                var GetstatusContractor = _context.StatusContractor.FirstOrDefault(dt => dt.Code.Equals(StatusContractorEnum.ENREVISIÓN.Description()));
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
                        listDetail[i].StatusContractor = GetstatusContractor.Id;
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
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }

        }

        #endregion
    }
}
