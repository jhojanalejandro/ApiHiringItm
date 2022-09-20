using Aspose.Cells;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Data.SqlClient;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesCore _file;

        public FilesController(IFilesCore file)
        {
            _file = file;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _file.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : (NoContent());
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _file.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : (NoContent());
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile files)
        {
            try
            {
                //Obtenemos todos los registros.
                string path = Path.Combine(@"C:\Users\Maicol\source\repos\Excel\Excel.API\Excel\", files.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Save the uploaded Excel file.
                string fileName = Path.GetFileName(files.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    files.CopyTo(stream);
                }
                FileStream test = new FileStream(filePath, FileMode.Open);

                Workbook workbook = new Workbook(test);

                Worksheet worksheet = workbook.Worksheets[0];
                var rows = worksheet.Cells.MaxRow;
                var columns = worksheet.Cells.MaxColumn;

                DataTable dataTable = worksheet.Cells.ExportDataTable(0, 0, rows, columns, true);
                dataTable.TableName = "";

                test.Close();

                System.IO.File.Delete(filePath);
                var builder = WebApplication.CreateBuilder();
                var conn = builder.Configuration.GetConnectionString("HiringDatabase");

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
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
                }
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(dataTable));
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(FilesDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _file.Create(model);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _file.Delete(id);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
