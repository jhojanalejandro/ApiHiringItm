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

namespace WebApiHiringItm.CORE.Core.ExcelCore
{
    public class UploadExcelCore : IUploadExcelCore
    {
        #region Dependency
        private readonly Hiring_V1Context _context;
        #endregion

        #region Constructor
        public UploadExcelCore(Hiring_V1Context context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<string> ImportarExcel(FileRequest obj)
        {
            string path = Path.Combine(@"C:\Users\Maicol\source\repos\Excel\Excel.API\Excel\", obj.Excel.FileName);
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
        #endregion


    }
}
