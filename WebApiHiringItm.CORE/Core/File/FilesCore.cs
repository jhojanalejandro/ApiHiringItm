using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.File.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.File
{
    public class FilesCore : IFilesCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;

        public FilesCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FilesDto>> GetAll()
        {
            var result = _context.Files.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<FilesDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FilesDto> GetById(int id)
        {
            var result = _context.Files.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<FilesDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.Files.Where(x => x.Id == id).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.Files.Remove(resultData);
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

        public async Task<bool> Create(FilesDto model)
        {
            var getData = _context.Files.Where(x => x.Id == model.Id).FirstOrDefault();
            //Convert pdf in Binary formate
            //int lenght = model.FilesName.ContentLength;
            //byte[] data = new byte[lenght];
            //model.FilesName.InputStream.Read(data, 0, lenght);
            if (getData == null)
            {
                var map = _mapper.Map<Files>(model);
                _context.Files.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.Files.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;

        }


        //Basado en lo publicado por otros

        //Buscar el archivo




        public string CodificarArchivo(string sNombreArchivo)
        {
            string sBase64 = "";
            // Declaramos fs para tener acceso al archivo residente en la maquina cliente.
            FileStream fs = new FileStream(sNombreArchivo, FileMode.Open);
            // Declaramos un Leector Binario para accesar a los datos del archivo pasarlos a un arreglo de bytes
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = new byte[(int)fs.Length];
            try
            {
                br.Read(bytes, 0, bytes.Length);
                // base64 es la cadena en donde se guarda el arreglo de bytes ya convertido
                sBase64 = Convert.ToBase64String(bytes);
                return sBase64;



            }

            catch (Exception ex)
            {
                throw new Exception("Ocurri un error al cargar el archivo Error", ex);
            }
            // Se cierran los archivos para liberar memoria.
            finally
            {
                fs.Close();
                fs = null;
                br = null;
                bytes = null;
            }
        }



        public string DecodificarArchivo(string sBase64)
        {
            // Declaramos fs para tener crear un nuevo archivo temporal en la maquina cliente.
            // y memStream para almacenar en memoria la cadena recibida.
            string sImagenTemporal = @"c:PRUEBA.pdf";  //Nombre del archivo y su extencion
            FileStream fs = new FileStream(sImagenTemporal, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(sBase64);
                bw.Write(bytes);
                return sImagenTemporal;
            }
            catch (FormatException ex)
            {
                throw new Exception("Ocurri un error al cargar el archivo Error", ex);
                return sImagenTemporal = @"c:PRUEBA.MP3";
            }
            finally
            {
                fs.Close();
                bytes = null;
                bw = null;
                sBase64 = null;
            }
        }



    }
}
