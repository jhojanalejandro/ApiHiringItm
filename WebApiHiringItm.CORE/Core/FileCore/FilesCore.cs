using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.FolderType;
using WebApiHiringItm.CORE.Helpers.Enums.StatusFile;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.MessageDto;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.FileCore
{
    public class FilesCore : IFilesCore
    {


        #region FIELD
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly string CONTRATO = "CONTRATO";
        private readonly string PAGO = "PAGOS";
        private readonly IProjectFolder _projectFolder;
        private readonly IMessageHandlingCore _messageHandlingCore;
        private readonly string MINUTAMODIFICADA = "La minuta se ha actualizado";


        #endregion
        #region BUILD
        public FilesCore(HiringContext context, IMapper mapper, IProjectFolder projectFolder, IMessageHandlingCore messageHandlingCore)
        {
            _context = context;
            _mapper = mapper;
            _projectFolder = projectFolder;
            _messageHandlingCore = messageHandlingCore;
        }

        #endregion

        #region PUBLIC METHODS
        public async Task<List<FileContractDto>> GetFileContractorByFolder(Guid contractorId, string folderId, Guid contractId)
        {
            var result = _context.DetailFile
                .Include(i => i.File)
                .Include(i => i.StatusFile)
                .Include(i => i.File)
                    .ThenInclude(i => i.Folder)
                .Where(x => x.File.ContractorId.Equals(contractorId) && x.File.ContractId.Equals(contractId) && x.File.FolderId.Equals(Guid.Parse(folderId))).OrderByDescending(o => o.RegisterDate);
            return await result
                .GroupBy(f => new { f.FileId, f.File.Filedata,f.File.FilesName,
                    f.File.FileType, f.File.DescriptionFile,
                    f.File.DocumentTypeNavigation.Code,
                    f.File.DocumentTypeNavigation.DocumentTypeDescription,
                    f.File.Folder.FolderName
                     })
                .Select(f => new FileContractDto
                {
                    Id = f.Key.FileId,
                    Filedata = f.Key.Filedata,
                    FilesName = f.Key.FilesName,
                    FileType = f.Key.FileType,
                    DescriptionFile = f.Key.DescriptionFile,
                    RegisterDate = f.First().RegisterDate,
                    DocumentTypes = f.Key.DocumentTypeDescription,
                    Passed = f.First().Passed,
                    StatusFile = f.First().StatusFileId,
                    FolderName = f.Key.FolderName,
                    DocumentTypeCode = f.Key.Code
                })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<FileContractDto>> GetFileContractByFolder(string folderId, string contractId)
        {
            var result = _context.Files
                .Where(x => !x.ContractorId.HasValue && x.ContractId.Equals(Guid.Parse(contractId)) && x.FolderId.Equals(Guid.Parse(folderId))).ToList();
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
            bool documentExist = false;
            var getDataFile = _context.Files.FirstOrDefault(x => x.DocumentTypeNavigation.Id.Equals(model.DocumentType));
            var getFolderId = _context.FolderType.FirstOrDefault(x => x.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()));

            var getFolder = _context.Folder.FirstOrDefault(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()) && x.ContractorId.Equals(model.ContractorId));
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
            if (getDataFile != null)
            {
                documentExist = true;
            }
            //if (getDataFile == null)
            //{

            //}
            //else
            //{
            //    //model.Id = getDataFile.Id;
            //    //var map = _mapper.Map(model, getDataFile);
            //    //_context.Files.Update(map);

            //}
            var res = await _context.SaveChangesAsync();
            var resultDetail = await CreateDetail(detailFileDto, true,false);
            return resultDetail ? true : false;
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
                    var resultDetail = await CreateDetail(detailFileDto,false,false);
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

        public async Task<IGenericResponse<string>> AddbillContractor(List<FilesDto> model)
        {
            if (model[0].UserId == Guid.Empty)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ATTACHMENTEMPTY);

            var getStatus= _context.StatusFile.Where(x => x.Code.Equals(StatusFileEnum.APROBADO.Description())).Select(s => s.Id).FirstOrDefault();

            List<DetailFileDto> detailFilelList = new();
            List<SaveFileResponse> updateFilexist = await SaveFileContractor(model);
            foreach (var item in updateFilexist)
            {
                if (!item.FileExist)
                {
                    DetailFileDto detailFileDto = new();
                    detailFileDto.Id = Guid.NewGuid();
                    detailFileDto.Passed = true;
                    detailFileDto.RegisterDate = item.RegisterDate;
                    detailFileDto.FileId = item.FileId;
                    detailFileDto.UserId = item.UserId;
                    detailFileDto.StatusFileId = getStatus;
                    detailFilelList.Add(detailFileDto);

                }
            }
            var mapDetailFiles = _mapper.Map<List<DetailFile>>(detailFilelList);
            _context.DetailFile.AddRange(mapDetailFiles);
            await _context.SaveChangesAsync(); 
            return ApiResponseHelper.CreateResponse<string>(Resource.MAILSENDSUCCESS);


        }


        public async Task<IGenericResponse<string>> AddMinuteGenerateContract(FilesDto model)
        {
            if (model.UserId == Guid.Empty)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ATTACHMENTEMPTY);

            var getStatus = _context.StatusFile.Where(x => x.Code.Equals(StatusFileEnum.APROBADO.Description())).Select(s => s.Id).FirstOrDefault();

            List<DetailFileDto> detailFilelList = new();
            SaveFileResponse updateFilexist = await SaveFileContract(model);
            if (!updateFilexist.FileExist)
            {
                DetailFileDto detailFileDto = new();
                detailFileDto.Id = Guid.NewGuid();
                detailFileDto.Passed = true;
                detailFileDto.RegisterDate = updateFilexist.RegisterDate;
                detailFileDto.FileId = updateFilexist.FileId;
                detailFileDto.UserId = model.UserId;
                detailFileDto.StatusFileId = getStatus;
                detailFilelList.Add(detailFileDto);

            }
            var mapDetailFiles = _mapper.Map<List<DetailFile>>(detailFilelList);
            _context.DetailFile.AddRange(mapDetailFiles);
            return ApiResponseHelper.CreateResponse<string>(Resource.MAILSENDSUCCESS);

        }
        public async Task<bool> CreateDetail(DetailFileDto model, bool contractor,bool documentExist)
        {

            if (!contractor) {
                var getStatusFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).Where(w => w.StatusFileId.Equals(model.StatusFileId) && w.FileId.Equals(model.FileId)).FirstOrDefault();
                if (getStatusFile == null)
                {
                    var map = _mapper.Map<DetailFile>(model);
                    map.Id = Guid.NewGuid();
                    _context.DetailFile.Add(map);
                }

            }
            else
            {
                if (model.Passed)
                {
                    var map = _mapper.Map<DetailFile>(model);
                    map.Id = Guid.NewGuid();
                    map.StatusFileId = model.StatusFileId;
                    _context.DetailFile.Add(map);
                }
                else
                {
                    if (documentExist)
                    {
                        var getStatusFileList = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.SUBSANADO.Description())).Select(s => s.Id).FirstOrDefault();
                        model.StatusFileId = getStatusFileList;

                    }
                    var map = _mapper.Map<DetailFile>(model);
                    map.Id = Guid.NewGuid();
                    map.StatusFileId = model.StatusFileId;
                    _context.DetailFile.Add(map);
                    //model.Id = detailFile.Id;
                    //model.Reason += detailFile.Reason;
                    //model.StatusFileId = detailFile.StatusFileId;
                    //var map = _mapper.Map(model, detailFile);
                    //_context.DetailFile.Update(map);
                }
            }

            var res = await _context.SaveChangesAsync();
            return res != 0 ? true : false;

        }

        public async Task<bool> CreateDetailObservation(DetailFileDto model)
        {
            foreach (var item in model.Files)
            {
                var detailFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(x => x.FileId.Equals(model.FileId));

                if (detailFile != null)
                {
                    var termTypeId = _context.TermType.Where(x => x.Code.Equals("DCCT")).Select(s => s.Id).FirstOrDefault();

                    TermContractDto termContractDto = new TermContractDto();
                    termContractDto.ContractId = model.ContractId;
                    termContractDto.ContractId = model.ContractorId;
                    termContractDto.StartDate = model.StarDate;
                    termContractDto.TermType = termTypeId;

                    var responseTerm = await _projectFolder.SaveTermFileContract(termContractDto);

                    if (responseTerm)
                    {
                        SendMessageObservationDto sendMessageObservationtDto = new();
                        sendMessageObservationtDto.ContractId = model.ContractorId;
                        sendMessageObservationtDto.ContractorsId[0] = Guid.Parse(model.ContractorId);
                        sendMessageObservationtDto.UserId = model.UserId.ToString();
                        sendMessageObservationtDto.Documentos += item.DocumentTypes + "," ;
                        sendMessageObservationtDto.Observation += model.Observation;

                        var resulMessage = await _messageHandlingCore.SendContractorObservation(sendMessageObservationtDto);
                        if (resulMessage.Success)
                        {
                            model.Id = detailFile.Id;
                            model.FileId = item.Id;
                            var map = _mapper.Map(model, detailFile);
                            _context.DetailFile.Update(map);
                        }


                    }

                }

            }
            var res = await _context.SaveChangesAsync();

            return res != 0 ? true : false;

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


        #endregion
        #region PRIVATE METHODS
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
        private async Task<List<SaveFileResponse>> SaveFileContractor(List<FilesDto> modelFiles)
        {
            var getFolderList = _context.Folder.Where(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()) && x.ContractId.Equals(modelFiles[0].ContractId)).ToList();

            var getFileList = _context.Files
                .Include(i => i.DocumentTypeNavigation)
                .Where(x => x.DocumentTypeNavigation.Id.Equals(modelFiles[0].DocumentType)).ToList();
            List<Files> SaveFiles = new();
            List<SaveFileResponse> saveFileResponses = new();
            List<Files> updateFiles = new();

            foreach (var item in modelFiles)
            {
                var getFolder = getFolderList.OrderByDescending(O => O.RegisterDate).Where(x => x.ContractId.Equals(item.ContractId) && x.ContractorId.Equals(item.ContractorId)).FirstOrDefault();

                var getFile = getFileList.Find(x => x.ContractorId.Equals(modelFiles[0].ContractorId) && x.DocumentTypeNavigation.Id.Equals(modelFiles[0].DocumentType));
                SaveFileResponse saveFileResponse = new SaveFileResponse();
                bool FileExist = false;
                item.FolderId = getFolder.Id;

                if (getFile != null)
                {
                    FileExist = true;
                    item.Id = getFile.Id;
                    var updateFile = _mapper.Map(item, getFile);
                    saveFileResponse.RegisterDate = item.RegisterDate;
                    saveFileResponse.UserId = item.UserId.Value;
                    saveFileResponse.FileId = updateFile.Id;
                    updateFiles.Add(updateFile);
                }
                else
                {
                    FileExist = false;
                    var mapModel = _mapper.Map<Files>(item);
                    mapModel.Id = Guid.NewGuid();
                    saveFileResponse.RegisterDate = item.RegisterDate;
                    saveFileResponse.UserId = item.UserId.Value;
                    saveFileResponse.FileId = mapModel.Id;
                    saveFileResponse.FileExist = FileExist;
                    SaveFiles.Add(mapModel);

                }
                saveFileResponses.Add(saveFileResponse);

            }
            if (SaveFiles.Count > 0)
            {
                _context.Files.AddRange(SaveFiles);
            }
            if (updateFiles.Count > 0)
            {
                _context.Files.UpdateRange(updateFiles);
            }
            await _context.SaveChangesAsync();
            return saveFileResponses;

        }
        private async Task<SaveFileResponse> SaveFileContract(FilesDto modelFiles)
        {

            List<Files> filesSave = new();

            var getFolder = _context.Folder.OrderByDescending(O => O.RegisterDate).Where(x => x.ContractId.Equals(modelFiles.ContractId) && x.ContractorId.Equals(modelFiles.ContractId)).FirstOrDefault();

            var getFile = _context.Files.FirstOrDefault(x => x.ContractorId.Equals(modelFiles.ContractorId) && x.DocumentTypeNavigation.Id.Equals(modelFiles.DocumentType));
            SaveFileResponse saveFileResponse = new SaveFileResponse();
            bool FileExist = false;
            Guid FileId = Guid.Empty;
            modelFiles.FolderId = getFolder.Id;

            if (getFile != null)
            {
                FileExist = true;
                modelFiles.Id = getFile.Id;

                var updateFile = _mapper.Map(modelFiles, getFile);
                FileId = updateFile.Id;
                saveFileResponse.RegisterDate = modelFiles.RegisterDate;
                saveFileResponse.UserId = modelFiles.UserId.Value;
                saveFileResponse.FileId = FileId;
                _context.Files.Add(updateFile);
            }
            else
            {
                FileExist = false;
                var mapModel = _mapper.Map<Files>(modelFiles);
                FileId = mapModel.Id;
                mapModel.Id = Guid.NewGuid();
                _context.Files.Add(mapModel);
                filesSave.Add(mapModel);

            }
            saveFileResponse.FileExist = FileExist;
            saveFileResponse.FileId = FileId;
            await _context.SaveChangesAsync();
            return saveFileResponse;

        }


        #endregion

    }
}
