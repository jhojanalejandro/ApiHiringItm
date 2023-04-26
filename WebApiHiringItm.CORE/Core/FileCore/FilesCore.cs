using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.Folder;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.FileCore
{
    public class FilesCore : IFilesCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;

        public FilesCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FilesDto>> GetFileContractorByFolder(Guid contractorId, string folderId, Guid contractId)
        {
            var result = _context.Files
                .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId.Equals(contractId) && x.FolderId.Equals(Guid.Parse(folderId))).ToList();
            if (result != null)
            {
                var map = _mapper.Map<List<FilesDto>>(result);
                return await Task.FromResult(map);

            }
            else
            {
                return new List<FilesDto>();

            }
        }

        public async Task<List<FilesDto>> GetAllByContract(Guid contractorId, Guid contractId)
        {
            var result = _context.Files
                .Where(x => x.ContractorId == contractorId && x.ContractId == contractId).ToList();
            if (result != null)
            {
                var map = _mapper.Map<List<FilesDto>>(result);
                //map.ForEach(e =>
                //{
                //    var detail = _context.ElementosComponente.Where(d => d.IdComponente == e.Id).ToList();
                //    e.DetalleFile = _mapper.Map<List<DetailFileDto>>(detail);
                //});
                return await Task.FromResult(map);

            }
            else
            {
                return new List<FilesDto>();

            }
        }

        public async Task<List<FilesDto>> GetAllFileByIdContract(Guid id)
        {
            var result = _context.Files.Where(x => x.ContractId.Equals(id) && x.TypeFilePayment.Equals(FileEnum.CONTRATO.Description())).ToList();
            var map = _mapper.Map<List<FilesDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<List<GetFilesPaymentDto>> GetAllByDate(Guid contractId, string type, string date)
        {
            var result = _context.Files.Where(x => x.ContractId.Equals(contractId) && x.MonthPayment.Equals(date) && x.TypeFilePayment.Equals(type)).ToList();
            var map = _mapper.Map<List<GetFilesPaymentDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FilesDto> GetById(int id)
        {
            var result = _context.Files.FirstOrDefault(x => x.Id == id);
            var resultFile = _context.DetalleFile.FirstOrDefault(df => df.FileId == id);
            var mapDf = _mapper.Map<DetailFileDto>(resultFile);
            var map = _mapper.Map<FilesDto>(result);
            map.DetailFile = mapDf;
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.Files.FirstOrDefault(x => x.Id == id);
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
            var getData = _context.Files.FirstOrDefault(x => x.Id == model.Id);
            if (getData == null)
            {
                if (model.TypeFilePayment.Equals(FileEnum.INFORME.Description()) || model.TypeFilePayment.Equals(FileEnum.CUENTADECOBRO.Description()) || model.TypeFilePayment.Equals(FileEnum.PLANILLA.Description()))
                {
                    var getFolder = _context.FolderContractor.FirstOrDefault(x => x.TypeFolder.Equals(FolderEnums.CARPETAPAGOS.Description()) && x.ContractorId == model.ContractorId);
                    if (getFolder == null)
                    {
                        FolderContractor folderPago = new FolderContractor();
                        folderPago.TypeFolder = FolderEnums.CARPETAPAGOS.Description();
                        folderPago.FolderName = FolderEnums.CARPETAPAGOS.Description();
                        folderPago.DescriptionProject = model.DescriptionFile;
                        folderPago.ContractorId = model.ContractorId;
                        folderPago.RegisterDate = DateTime.Now;
                        folderPago.ModifyDate = DateTime.Now;
                        _context.FolderContractor.Add(folderPago);
                    }
                }
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

        public async Task<bool> Addbill(FilesDto model)
        {
            var getData = _context.Files.Where(x => x.ContractorId.Equals(model.ContractorId) && x.TypeFilePayment.Equals(FileEnum.MINUTA.Description())).FirstOrDefault();
            var getFolder = _context.FolderContractor.Where(x => x.FolderName.Equals(FolderEnums.SUBIRGMAS.Description()) && x.ContractId.Equals(model.ContractorId)).FirstOrDefault();

            if (getData == null)
            {
                if (getFolder == null)
                {
                    FolderContractor carpetaMinuta = new FolderContractor();
                    carpetaMinuta.Id = Guid.NewGuid();
                    carpetaMinuta.FolderName = FolderEnums.SUBIRGMAS.Description();
                    carpetaMinuta.DescriptionProject = "Carpeta para cargar documentos a plataforma Gmas";
                    carpetaMinuta.ContractorId = model.ContractorId;
                    carpetaMinuta.RegisterDate = DateTime.Now;
                    carpetaMinuta.ContractId = model.ContractId;
                    carpetaMinuta.ModifyDate = DateTime.Now;
                    model.FolderId = carpetaMinuta.Id;
                    _context.FolderContractor.Add(carpetaMinuta);
                }
                else
                {
                    model.FolderId = getFolder.Id;

                }
            }
            else 
            {
                if (getFolder != null)
                {
                    model.FolderId = getFolder.Id;
                    model.Id = getData.Id;
                }
                else
                {
                    FolderContractor carpetaMinuta = new FolderContractor();
                    carpetaMinuta.Id = Guid.NewGuid();
                    carpetaMinuta.FolderName = FolderEnums.SUBIRGMAS.Description();
                    carpetaMinuta.DescriptionProject = "Carpeta para cargar documentos a plataforma Gmas";
                    carpetaMinuta.ContractorId = getData.ContractorId;
                    carpetaMinuta.RegisterDate = DateTime.Now;
                    carpetaMinuta.ModifyDate = DateTime.Now;
                    model.FolderId = carpetaMinuta.Id;
                    _context.FolderContractor.Add(carpetaMinuta);

                }

            }
            var mapModel = _mapper.Map<Files>(model);
            _context.Files.Add(mapModel);
            var res = await _context.SaveChangesAsync();
            return res != 0 ? true : false;
        }

        public async Task<bool> CreateDetail(DetailFileDto model)
        {
            var getData = _context.DetalleFile.FirstOrDefault(x => x.Id == model.Id);
            if (getData == null)
            {
                var map = _mapper.Map<DetalleFile>(model);
                _context.DetalleFile.Add(map);
                var fileData = _context.Files.FirstOrDefault(fl => fl.Id == model.FileId);
                if (fileData != null)
                    fileData.Passed = false;
                _context.Files.Update(fileData);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            else
            {
                model.Id = getData.Id;
                model.Reason += getData.Reason;
                var map = _mapper.Map(model, getData);
                _context.DetalleFile.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;

        }


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
