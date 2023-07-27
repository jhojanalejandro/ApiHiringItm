using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.FolderType;
using WebApiHiringItm.CORE.Helpers.Enums.StatusFile;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.FileCore
{
    public class FilesCore : IFilesCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly string CONTRATO = "CONTRATO";
        private readonly string PAGO = "PAGOS";
        public FilesCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region PUBLIC METHODS
        public async Task<List<FileContractDto>> GetFileContractorByFolder(Guid contractorId, string folderId, Guid contractId)
        {
            var result = _context.DetailFile
                .Include(i => i.File)
                .Include(i => i.StatusFile)
                .Where(x => x.File.ContractorId.Equals(contractorId) && x.File.ContractId.Equals(contractId) && x.File.FolderId.Equals(Guid.Parse(folderId))).OrderByDescending(o => o.RegisterDate);
            return await result
                .GroupBy(f => new { f.FileId, f.File.Filedata,f.File.FilesName,
                    f.File.FileType, f.File.DescriptionFile,f.RegisterDate, 
                    f.File.DocumentTypeNavigation.DocumentTypeDescription,
                    f.Passed, f.StatusFileId })
                .Select(f => new FileContractDto
            {
                Id = f.Key.FileId,
                Filedata = f.Key.Filedata,
                FilesName = f.Key.FilesName,
                FileType = f.Key.FileType,
                DescriptionFile = f.Key.DescriptionFile,
                RegisterDate = f.Key.RegisterDate,
                DocumentTypes = f.Key.DocumentTypeDescription,
                Passed = f.Key.Passed,
                StatusFile = f.Key.StatusFileId
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<FileContractDto>> GetFileContractByFolder(string folderId, Guid contractId)
        {
            var result = _context.Files
                .Where(x => !x.ContractorId.HasValue && x.ContractId.Equals(contractId) && x.FolderId.Equals(Guid.Parse(folderId))).ToList();
            if (result != null)
            {
                var map = _mapper.Map<List<FileContractDto>>(result);
                return await Task.FromResult(map);

            }
            else
            {
                return null;

            }
        }

        public async Task<List<FileDetailDto>> GetAllByContract(Guid contractorId, Guid contractId)
        {
            var result = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Where(x => x.File.ContractorId.Equals(contractorId) && x.File.ContractId.Equals(contractId));
            return result.Select(f => new FileDetailDto
            {
                Id = f.File.Id,
                Filedata = f.File.Filedata,
                FilesName = f.File.FilesName,
                FileType = f.File.FileType,
                DescriptionFile = f.File.DescriptionFile,
                RegisterDate = f.RegisterDate,
                DocumentTypes = f.File.DocumentTypeNavigation.DocumentTypeDescription.ToUpper(),
                DocumentTypesCode = f.File.DocumentTypeNavigation.Code.ToUpper(),
                Passed = f.Passed

            })
                .AsNoTracking()
                .ToList();

        }

        public async Task<List<FilesDto>> GetAllFileByIdContract(Guid id)
        {
            var result = _context.Files.Where(x => x.ContractId.Equals(id) && x.DocumentTypeNavigation.Code.Equals(FolderTypeCodeEnum.PAGOSCODE.Description())).ToList();
            var map = _mapper.Map<List<FilesDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<GetFilesPaymentDto>> GetAllByDate(Guid contractId, string type, string date)
        {
            var result = _context.Files.Where(x => x.ContractId.Equals(contractId) && x.MonthPayment.Equals(date) && x.DocumentTypeNavigation.Code.Equals(type)).ToList();
            var map = _mapper.Map<List<GetFilesPaymentDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FileDetailDto?> GetByIdFile(Guid id)
        {
            var result = _context.DetailFile
                .Include(i => i.File)
                .Include(i => i.User)
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile)
                .Where(x => x.FileId.Equals(id)).OrderByDescending(o => o.RegisterDate);
            return await result.Select(f => new FileDetailDto
            {
                Id = f.File.Id,
                FilesName = f.File.FilesName,
                Filedata = f.File.Filedata,
                FileType = f.File.FileType,
                DescriptionFile = f.File.DescriptionFile,
                AsistencialUser = f.User.UserName,
                RegisterDate = f.RegisterDate,
                MonthPayment = f.File.MonthPayment,
                Passed = f.Passed,
                DocumentTypes = f.File.DocumentTypeNavigation.DocumentTypeDescription,
                StatusPayment = f.StatusFile.StatusFileDescription
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var deleteDetail = await DeleteDetail(id);
                if (deleteDetail)
                {
                    var resultData = _context.Files.FirstOrDefault(x => x.Id.Equals(Guid.Parse(id)));
                    if (resultData != null)
                    {
                        _context.Files.Remove(resultData);
                        var result = await _context.SaveChangesAsync();
                         return result != 0 ? true : false;
                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        public async Task<bool> AddFileContractor(FilesDto model)
        {
            Guid? folderId;
            var getData = _context.Files.FirstOrDefault(x => x.Id.Equals(model.Id));
            var getFolderId = _context.FolderType.FirstOrDefault(x => x.Code.Equals(FolderTypeCodeEnum.PAGOSCODE.Description()));

            if (getData == null)
            {
                var getFolder = _context.Folder.FirstOrDefault(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.PAGOSCODE.Description()) && x.ContractorId == model.ContractorId);
                if (getFolder == null)
                {
                    Folder folderPago = new Folder();
                    folderPago.Id = Guid.NewGuid();
                    folderPago.FolderType = getFolderId.Id;
                    folderPago.FolderName = CONTRATO;
                    folderPago.DescriptionProject = model.DescriptionFile;
                    folderPago.ContractorId = model.ContractorId;
                    folderPago.ContractId = model.ContractId;
                    folderPago.RegisterDate = DateTime.Now;
                    folderPago.ModifyDate = DateTime.Now;
                    _context.Folder.Add(folderPago);
                    folderId = folderPago.Id;
                }
                else
                {
                    folderId = getFolder.Id;
                }
                var map = _mapper.Map<Files>(model);
                map.Id = Guid.NewGuid();
                map.FolderId = folderId;
                _context.Files.Add(map);
                DetailFileDto detailFileDto = new();
                detailFileDto.Passed = false;
                detailFileDto.RegisterDate = model.RegisterDate;
                detailFileDto.FileId = map.Id;
                if (model.UserId != Guid.Empty)
                {
                    detailFileDto.UserId = model.UserId;
                }
                var res = await _context.SaveChangesAsync();
                if (res != 0)
                {
                    var resultDetail = await CreateDetail(detailFileDto);
                    return resultDetail ? true : false;
                }
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

        public async Task<bool> AddFileContract(FileContractDto model)
        {
            var getData = _context.Files.FirstOrDefault(x => x.Id.Equals(model.Id));

            if (getData == null)    
            {
                var map = _mapper.Map<Files>(model);
                map.Id = Guid.NewGuid();
                _context.Files.Add(map);
                DetailFileDto detailFileDto = new();
                detailFileDto.Passed = false;
                detailFileDto.RegisterDate = model.RegisterDate;
                detailFileDto.FileId = map.Id;
                if (model.UserId != Guid.Empty)
                {
                    detailFileDto.UserId = model.UserId;
                }
                var res = await _context.SaveChangesAsync();
                if (res != 0)
                {
                    var resultDetail = await CreateDetail(detailFileDto);
                    return resultDetail ? true : false;
                }
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
            var getData = _context.Files.Where(x => x.ContractorId.Equals(model.ContractorId) && x.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description())).FirstOrDefault();
            var getFolder = _context.Folder.Where(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.PAGOSCODE.Description()) && x.ContractId.Equals(model.ContractId)).FirstOrDefault();

            if (getData == null)
            {
                if (getFolder == null)
                {
                    Folder carpetaMinuta = new Folder();
                    carpetaMinuta.Id = Guid.NewGuid();
                    //carpetaMinuta.FolderName = FolderEnums.CONTRATOS.Description();
                    carpetaMinuta.DescriptionProject = "archivos del contrato";
                    carpetaMinuta.ContractorId = model.ContractorId;
                    carpetaMinuta.RegisterDate = DateTime.Now;
                    carpetaMinuta.ContractId = model.ContractId;
                    carpetaMinuta.ModifyDate = DateTime.Now;
                    carpetaMinuta.UserId = model.UserId.Value;
                    model.FolderId = carpetaMinuta.Id;
                    _context.Folder.Add(carpetaMinuta);
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
                    Folder carpetaMinuta = new Folder();
                    carpetaMinuta.Id = Guid.NewGuid();
                    //carpetaMinuta.FolderName = FolderEnums.CONTRATOS.Description();
                    carpetaMinuta.DescriptionProject = "archivos del contrato";
                    carpetaMinuta.ContractorId = getData.ContractorId;
                    carpetaMinuta.RegisterDate = DateTime.Now;
                    carpetaMinuta.ModifyDate = DateTime.Now;
                    model.FolderId = carpetaMinuta.Id;
                    _context.Folder.Add(carpetaMinuta);

                }

            }
            var mapModel = _mapper.Map<Files>(model);
            mapModel.Id = Guid.NewGuid();
            _context.Files.Add(mapModel);
            var res = await _context.SaveChangesAsync();
            if (res != 0)
            {
                DetailFileDto detailFileDto = new();
                detailFileDto.Passed = false;
                detailFileDto.RegisterDate = model.RegisterDate;
                detailFileDto.FileId = mapModel.Id;
                if (model.UserId != Guid.Empty)
                {
                    detailFileDto.UserId = model.UserId;
                }
                var resultDetail = await CreateDetail(detailFileDto);
                return resultDetail ? true : false;
            }
            return false;
        }

        public async Task<bool> CreateDetail(DetailFileDto model)
        {
            DetailFile detailFile = null;
            if (model.Passed )
            {
                detailFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(x => x.FileId.Equals(model.FileId));
            }
            if (detailFile == null)
            {
                Guid? getStatusFile = _context.StatusFile.FirstOrDefault(x => x.Code.Equals(StatusFileEnum.ENPROCESO.Description())).Id;
                var map = _mapper.Map<DetailFile>(model);
                map.Id = Guid.NewGuid();
                map.StatusFileId = getStatusFile;
                _context.DetailFile.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            else
            {
                model.Id = detailFile.Id;
                model.Reason += detailFile.Reason;
                var map = _mapper.Map(model, detailFile);
                _context.DetailFile.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }

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

        private async Task<bool> DeleteDetail(string id)
        {
            try
            {
                var resultData = _context.DetailFile.Where(x => x.FileId.Equals(Guid.Parse(id))).ToList();
                if (resultData.Count > 0)
                {
                    _context.DetailFile.RemoveRange(resultData);
                    var result = await _context.SaveChangesAsync();
                    if (result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
        #endregion

    }
}
