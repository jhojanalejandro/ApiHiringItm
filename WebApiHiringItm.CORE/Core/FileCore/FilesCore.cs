using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Atp;
using System.Diagnostics;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.FolderType;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
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
            var getFolderContract = _context.Folder.Any(wf => wf.Id.Equals(Guid.Parse(folderId)) && wf.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()));

            var result = _context.DetailFile
                .Include(i => i.File)
                .Include(i => i.StatusFile)
                .Include(i => i.File)
                    .ThenInclude(i => i.Folder)
                .Where(x => x.ContractorId.Equals(contractorId) && x.File.ContractId.Equals(contractId) && x.File.FolderId.Equals(Guid.Parse(folderId)) || (x.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.SOLICITUDCOMITE.Description()) && getFolderContract && x.ContractorId.Equals(contractorId))).OrderByDescending(o => o.RegisterDate);
            return await result
                .OrderByDescending(o => o.RegisterDate)
                .GroupBy(f => new { f.FileId,f.File.FilesName,
                    f.File.FileType, f.File.DescriptionFile,
                    f.File.DocumentTypeNavigation.Code,
                    f.File.DocumentTypeNavigation.DocumentTypeDescription,
                    f.File.Folder.FolderName
                     })
                .Select(f => new FileContractDto
            {
                    Id = f.Key.FileId,
                    FilesName = f.Key.FilesName,
                    FileType = f.Key.FileType,
                    DescriptionFile = f.Key.DescriptionFile,
                    RegisterDate = f.First().RegisterDate,
                    DocumentTypes = f.Key.DocumentTypeDescription,
                    Passed = f.OrderByDescending(o => o.RegisterDate).First().Passed,
                    StatusFile = f.OrderByDescending(o => o.RegisterDate).First().StatusFileId,
                    FolderName = f.Key.FolderName,
                    DocumentTypeCode = f.Key.Code,
                    CategoryCode = f.First().StatusFile.Category,
                    ContractId = contractId,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<FileContractDto>> GetFileContractByFolder(string folderId, string contractId)
        {
            var result = _context.DetailFile
                .Include(i => i.File)
                .Where(x => x.ContractorId == null && x.File.ContractId.Equals(Guid.Parse(contractId)) && x.File.FolderId.Equals(Guid.Parse(folderId))).ToList();
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
                .Where(x => x.ContractorId.Equals(contractorId) && x.File.ContractId.Equals(contractId)).OrderByDescending(o => o.RegisterDate);
            return result
                .GroupBy(g => g.ContractorId)
                .Select(f => new FileDetailDto
            {
                Id = f.First().File.Id,
                Filedata = f.First().File.Filedata,
                FilesName = f.First().File.FilesName,
                FileType = f.First().File.FileType,
                DescriptionFile = f.First().File.DescriptionFile,
                RegisterDate = f.First().RegisterDate,
                DocumentTypes = f.First().File.DocumentTypeNavigation.DocumentTypeDescription.ToUpper(),
                DocumentTypesCode = f.First().File.DocumentTypeNavigation.Code.ToUpper(),
                Passed = f.First().Passed

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
            if (type == null || type == "null" || type == "")
            {
                type = Guid.NewGuid().ToString();
            }
            var result = _context.DetailFile.Where(x => x.File.ContractId.Equals(contractId) && x.File.MonthPayment.Equals(date) && x.File.DocumentType.Equals(Guid.Parse(type)));
            return await result.Select(f => new GetFilesPaymentDto
            {
                     Id = f.FileId,
                     FilesName = f.File.FilesName,
                     FileType = f.File.FileType,
                     DescriptionFile = f.File.DescriptionFile,
                     RegisterDate = f.RegisterDate,
                     DocumentTypes = f.File.DocumentTypeNavigation.DocumentTypeDescription,
                     Passed = f.Passed,
                     StatusFile = f.StatusFileId.ToString(),
                     FolderName = f.File.Folder.FolderName,
                     DocumentTypeCode = f.File.DocumentTypeNavigation.Code,
                     CategoryCode = f.StatusFile.Category,
                     ContractId = contractId
                 })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FileDetailDto?> GetByIdFile(Guid FileId)
        {
            var result = _context.DetailFile
                .Include(i => i.File)
                .Include(i => i.User)
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile)
                .Where(x => x.FileId.Equals(FileId)).OrderByDescending(o => o.RegisterDate);
            return result.Select(f => new FileDetailDto
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
                StatusPayment = f.StatusFile.StatusFileDescription,
                Observation = f.Observation,
                UserContractual = f.User.UserName,
                Reason = f.ReasonRejection,

            })
            .AsNoTracking()
            .FirstOrDefault();
        }

        public async Task<bool> DeleteFile(string fileId)
        {
            try
            {
                var deleteDetail = await DeleteDetail(fileId);
                if (deleteDetail)
                {
                    var resultData = _context.Files.FirstOrDefault(x => x.Id.Equals(Guid.Parse(fileId)));
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

        public async Task<IGenericResponse<string>>AddFileContractor(FilesDto modelFileDto)
        {
            Guid folderId = Guid.Empty;
            bool documentExist = false;
            var getDataFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(x => x.File.DocumentTypeNavigation.Id.Equals(modelFileDto.DocumentType) && x.ContractorId.Equals(modelFileDto.ContractorId) && x.File.ContractId.Equals(modelFileDto.ContractId));
            var getFolderId = _context.FolderType.FirstOrDefault(x => x.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()));
            var getStatusContractor = _context.StatusContractor.FirstOrDefault(x => x.Code.Equals(StatusContractorEnum.ENREVISIÓN.Description())).Id;
            var getDetailContractor = _context.DetailContractor.FirstOrDefault(w => w.ContractorId.Equals(modelFileDto.ContractorId) && w.ContractId.Equals(modelFileDto.ContractId) && !w.StatusContractor.Equals(getStatusContractor));
            var getUser = _context.AssigmentContract.FirstOrDefault(x => x.ContractId.Equals(modelFileDto.ContractId) && x.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.CONTRACTUALCONTRATO.Description()));

            if (getDetailContractor != null)
            {
                getDetailContractor.StatusContractor = getStatusContractor;
                _context.DetailContractor.Update(getDetailContractor);
            }
            var getFolder = _context.Folder.FirstOrDefault(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()) && x.ContractorId.Equals(modelFileDto.ContractorId));
            if (getFolder == null)
            {
                Folder folderPago = new Folder();
                folderPago.Id = Guid.NewGuid();
                folderPago.FolderType = getFolderId.Id;
                folderPago.FolderName = CONTRATO;
                folderPago.DescriptionProject = modelFileDto.DescriptionFile;
                folderPago.ContractorId = modelFileDto.ContractorId;
                folderPago.ContractId = modelFileDto.ContractId;
                folderPago.RegisterDate = DateTime.Now;
                folderPago.ModifyDate = DateTime.Now;
                folderPago.Consutive = 1;
                _context.Folder.Add(folderPago);
                folderId = folderPago.Id;
            }
            else
            {
                folderId = getFolder.Id;
            }
            var map = _mapper.Map<Files>(modelFileDto);
            map.Id = Guid.NewGuid();
            map.FolderId = folderId;
            _context.Files.Add(map);
            DetailFileDto detailFileDto = new();
            detailFileDto.Passed = false;
            detailFileDto.RegisterDate = modelFileDto.RegisterDate;
            detailFileDto.FileId = map.Id;
            detailFileDto.ContractorId = modelFileDto.ContractorId;
            if (modelFileDto.UserId != Guid.Empty && modelFileDto.UserId != null)
            {
                detailFileDto.UserId = modelFileDto.UserId;
            }
            else
            {
                detailFileDto.UserId = getUser.UserId;
            }
            if (getDataFile != null && modelFileDto.Anexo == false)
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
            var resultDetail = await CreateDetail(detailFileDto, true, documentExist, modelFileDto.Anexo.Value);
            if (resultDetail)
            {
                return ApiResponseHelper.CreateResponse<string>(null, true,Resource.REGISTERSUCCESSFULL);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ERRORDETAILFILE);

            }

        }

        public async Task<IGenericResponse<string>> AddFileContract(FileContractDto model)
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
                await CreateDetail(detailFileDto, false, false, true);
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.Files.Update(map);
                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);

            }
        }

        public async Task<IGenericResponse<string>> AddbillContractor(List<FilesDto> model)
        {
            if (model[0].UserId == Guid.Empty)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ATTACHMENTEMPTY);

            var getStatus= _context.StatusFile.Where(x => x.Code.Equals(StatusFileEnum.GENERADO.Description())).Select(s => s.Id).FirstOrDefault();

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
                    detailFileDto.ContractorId = item.ContractorId;
                    detailFilelList.Add(detailFileDto);

                }
            }
            var mapDetailFiles = _mapper.Map<List<DetailFile>>(detailFilelList);
            _context.DetailFile.AddRange(mapDetailFiles);
            await _context.SaveChangesAsync(); 
            return ApiResponseHelper.CreateResponse<string>(null,true,Resource.DOCUMENTGENERATE);
        }

        public async Task<IGenericResponse<string>> SaveCommitteeContractor(FilesDto modelFile)
        {
            try
            {
                if (modelFile.UserId == Guid.Empty)
                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.ATTACHMENTEMPTY);

                var getStatus = _context.StatusFile.Where(x => x.Code.Equals(StatusFileEnum.GENERADO.Description())).Select(s => s.Id).FirstOrDefault();

                SaveFileResponse updateFilexist = await SaveFileCommiteeContractor(modelFile);
                List<DetailFile> detailFileListDto = new();
                foreach (var item in modelFile.Contractors)
                {
                    DetailFile detailFileDto = new();
                    detailFileDto.Id = Guid.NewGuid();
                    detailFileDto.Passed = true;
                    detailFileDto.RegisterDate = updateFilexist.RegisterDate;
                    detailFileDto.FileId = updateFilexist.FileId;
                    detailFileDto.UserId = updateFilexist.UserId;
                    detailFileDto.StatusFileId = getStatus;
                    detailFileDto.ContractorId = item;
                    var mapDetailFiles = _mapper.Map<DetailFile>(detailFileDto);
                    detailFileListDto.Add(mapDetailFiles);
                }

                _context.DetailFile.AddRange(detailFileListDto);
                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.DOCUMENTGENERATE);
            }catch(Exception ex)
            {
                throw new Exception("error " + ex.Message, ex);
            }

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
            return ApiResponseHelper.CreateResponse<string>(null,true,Resource.MAILSENDSUCCESS);

        }
        
        public async Task<bool> CreateDetail(DetailFileDto model, bool contractor,bool documentExist, bool anexo)
        {

            if (!contractor) {
                var getDetailFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).Where(w => w.FileId.Equals(model.FileId)).FirstOrDefault();
                if (getDetailFile != null)
                {
                    model.Id = getDetailFile.Id;
                    var mapDetail = _mapper.Map(model,getDetailFile);
                    _context.DetailFile.Update(mapDetail);
                }
                else
                {
                    var getStatusFileList = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.APROBADO.Description())).Select(s => s.Id).FirstOrDefault();
                    var map = _mapper.Map<DetailFile>(model);
                    map.Id = Guid.NewGuid();
                    map.StatusFileId = getStatusFileList;
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
                    else if (anexo)
                    {
                        var getStatusFileList = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.APROBADO.Description())).Select(s => s.Id).FirstOrDefault();
                        model.StatusFileId = getStatusFileList;
                    }
                    else
                    {
                        var getStatusFileList = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.ENPROCESO.Description())).Select(s => s.Id).FirstOrDefault();
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
            return res > 0 ? true : false;

        }

        public async Task<bool> CreateDetailList(List<DetailFileDto> modelDetailFile)
        {
            try
            {
                List<DetailFileDto> listAddFile = new();
                List<DetailFileDto> listUpdateFile = new();
                var getDetailFileList = _context.DetailFile.OrderByDescending(o => o.RegisterDate).Where(w => w.FileId.Equals(modelDetailFile[0].FileId)).ToList();
                var getStatusFileId = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.APROBADO.Description())).Select(s => s.Id).FirstOrDefault();

                foreach (var item in modelDetailFile)
                {
                    var getDetailFile = getDetailFileList.Find(f => f.ContractorId.Equals(item.ContractorId));
                    if (getDetailFile != null)
                    {
                        item.Id = getDetailFile.Id;
                        item.StatusFileId = getStatusFileId;
                        listUpdateFile.Add(item);
                    }
                    else
                    {
                        item.Id = Guid.NewGuid();
                        item.StatusFileId = getStatusFileId;
                        listAddFile.Add(item);

                    }

                }
                if (listUpdateFile.Count > 0)
                {
                    var mapDetailUpdate = _mapper.Map<List<DetailFile>>(listUpdateFile);
                    _context.DetailFile.UpdateRange(mapDetailUpdate);


                }
                else if (listAddFile.Count > 0)
                {
                    var mapDetailAdd = _mapper.Map<List<DetailFile>>(listAddFile);
                    _context.DetailFile.AddRange(mapDetailAdd);
                }

                var res = await _context.SaveChangesAsync();
                return res > 0 ? true : false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
       

        }
        
        public async Task<IGenericResponse<string>> CreateDetailCommittee(DetailFileDto model)
        {
            var getDetailFileList = _context.DetailFile.OrderByDescending(o => o.RegisterDate).Where(w => w.FileId.Equals(model.FileId)).ToList();
            foreach (var detail in getDetailFileList)
            {
                detail.StatusFileId = model.StatusFileId;
            }
            _context.DetailFile.UpdateRange(getDetailFileList);
            await _context.SaveChangesAsync();
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.UPDATESUCCESSFULL);

        }

        public async Task<IGenericResponse<string>> CreateDetailObservation(ObservationFileRequest modelDetailFile)
        {
            try
            {
                var termTypeId = _context.TermType.Where(x => x.Code.Equals("DCCT")).Select(s => s.Id).FirstOrDefault();
                var detailContractor = _context.DetailContractor.Where(x => x.ContractorId.Equals(modelDetailFile.ContractorId)).Select(s => s.Id).FirstOrDefault();
                List<DetailFile> detailFile = new();
                TermContractDto termContractDto = new TermContractDto();
                termContractDto.ContractId = modelDetailFile.ContractId;
                termContractDto.ContractorId = modelDetailFile.ContractorId.ToString();
                termContractDto.TermDate = modelDetailFile.TermDate;
                termContractDto.DetailContractor = detailContractor;
                termContractDto.TermType = termTypeId;

                var responseTerm = await _projectFolder.SaveTermFileContract(termContractDto);

                if (responseTerm.Success)
                {
                    SendMessageObservationDto sendMessageObservationtDto = new();
                    sendMessageObservationtDto.ContractId = modelDetailFile.ContractId;
                    sendMessageObservationtDto.ContractorId = modelDetailFile.ContractorId.Value;
                    sendMessageObservationtDto.UserId = modelDetailFile.UserId.ToString();
                    foreach (var item in modelDetailFile.Files)
                    {
                        sendMessageObservationtDto.Documents.Add(item.DocumentTypes);
                    }
                    sendMessageObservationtDto.Observation += modelDetailFile.Observation;
                    sendMessageObservationtDto.TermDate = modelDetailFile.TermDate;
                    var resulMessage = await _messageHandlingCore.SendContractorObservation(sendMessageObservationtDto);
                    if (resulMessage.Success)
                    {
                        foreach (var itemFile in modelDetailFile.Files)
                        {
                            modelDetailFile.Id = Guid.NewGuid();
                            modelDetailFile.FileId = itemFile.Id;
                            var mapDetailFile = _mapper.Map<DetailFile>(modelDetailFile);
                            detailFile.Add(mapDetailFile);
                        }
                        _context.DetailFile.AddRange(detailFile);

                    }

                }
                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.UPDATESUCCESSFULL);
            }
            catch(Exception ex)
            {
                throw new Exception("error",ex);
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

        public async Task<List<FileContractDto>> GetFileDonwloadContractual(ContractContractorsDto contractContractors)
        {
            var result = _context.DetailFile
                .Include(i => i.File)
                .Include(i => i.StatusFile)
                .Include(i => i.File)
                    .ThenInclude(i => i.Folder)
                .Where(x => contractContractors.contractors.Contains(x.ContractorId.ToString()) && x.File.ContractId.Equals(Guid.Parse(contractContractors.contractId)) && x.File.DocumentTypeNavigation.Code.Equals(contractContractors.TypeMinute)).OrderByDescending(o => o.RegisterDate);
            return await result
                .GroupBy(f => new {
                    f.FileId,
                    f.File.Filedata,
                    f.File.FilesName,
                    f.File.FileType,
                    f.File.DescriptionFile,
                    f.File.DocumentTypeNavigation.Code,
                    f.File.DocumentTypeNavigation.DocumentTypeDescription,
                    f.File.Folder.FolderName
                })
                .Select(f => new FileContractDto
                {
                    Id = f.Key.FileId,
                    FilesName = f.Key.FilesName,
                    FileType = f.Key.FileType,
                    DescriptionFile = f.Key.DescriptionFile,
                    RegisterDate = f.First().RegisterDate,
                    DocumentTypes = f.Key.DocumentTypeDescription,
                    Passed = f.First().Passed,
                    StatusFile = f.First().StatusFileId,
                    FolderName = f.Key.FolderName,
                    DocumentTypeCode = f.Key.Code,
                    Filedata = f.Key.Filedata
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IGenericResponse<string>> AddFilePayrollContractor(FilesDto modelFileDto, int consecutive)
        {
            Guid folderId = Guid.Empty;
            bool documentExist = false;
            var getDataFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(x => x.File.DocumentTypeNavigation.Id.Equals(modelFileDto.DocumentType) && x.ContractorId.Equals(modelFileDto.ContractorId) && x.File.ContractId.Equals(modelFileDto.ContractId));
            var getFolderId = _context.FolderType.FirstOrDefault(x => x.Code.Equals(FolderTypeCodeEnum.PAGOSCODE.Description()));
            var getUser = _context.AssigmentContract.FirstOrDefault(x => x.ContractId.Equals(modelFileDto.ContractId) && x.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.CONTRACTUALCONTRATO.Description()));

            var getFolder = _context.Folder.FirstOrDefault(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.PAGOSCODE.Description()) && x.ContractorId.Equals(modelFileDto.ContractorId));
            if (getFolder == null)
            {
                Folder folderPago = new Folder();
                folderPago.Id = Guid.NewGuid();
                folderPago.FolderType = getFolderId.Id;
                folderPago.FolderName = PAGO;
                folderPago.DescriptionProject = "CARPETA SEGURIDAD";
                folderPago.ContractorId = modelFileDto.ContractorId;
                folderPago.ContractId = modelFileDto.ContractId;
                folderPago.RegisterDate = DateTime.Now;
                folderPago.ModifyDate = DateTime.Now;
                folderPago.Consutive = consecutive;
                _context.Folder.Add(folderPago);
                folderId = folderPago.Id;
            }
            else
            {
                folderId = getFolder.Id;
            }
            var map = _mapper.Map<Files>(modelFileDto);
            map.Id = Guid.NewGuid();
            map.FolderId = folderId;
            _context.Files.Add(map);
            DetailFileDto detailFileDto = new();
            detailFileDto.Passed = false;
            detailFileDto.RegisterDate = modelFileDto.RegisterDate;
            detailFileDto.FileId = map.Id;
            detailFileDto.ContractorId = modelFileDto.ContractorId;

            if (getDataFile != null)
            {
                documentExist = true;
            }

            var resultDetail = await CreateDetailPayroll(detailFileDto);
            if (resultDetail)
            {
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ERRORDETAILFILE);

            }

        }

        private async Task<bool> CreateDetailPayroll(DetailFileDto model)
        {
            var getDetailFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).Where(w => w.FileId.Equals(model.FileId)).FirstOrDefault();
            if (getDetailFile != null)
            {
                model.Id = getDetailFile.Id;
                var mapDetail = _mapper.Map(model, getDetailFile);
                _context.DetailFile.Update(mapDetail);
            }
            else
            {
                var getStatusFileList = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.APROBADO.Description())).Select(s => s.Id).FirstOrDefault();
                var map = _mapper.Map<DetailFile>(model);
                map.Id = Guid.NewGuid();
                map.StatusFileId = getStatusFileList;
                _context.DetailFile.Add(map);
            }

            var res = await _context.SaveChangesAsync();
            return res > 0 ? true : false;

        }

        public async Task<List<FileContractDto>> GetFileContractorByFolderToDownload(Guid contractorId, string folderId, Guid contractId)
        {
            var getFolderContract = _context.Folder.Any(wf => wf.Id.Equals(Guid.Parse(folderId)) && wf.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()));

            var result = _context.DetailFile
                .Include(i => i.File)
                .Include(i => i.StatusFile)
                .Include(i => i.File)
                    .ThenInclude(i => i.Folder)
                .Where(x => x.ContractorId.Equals(contractorId) && x.File.ContractId.Equals(contractId) && x.File.FolderId.Equals(Guid.Parse(folderId)) || (!x.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.SOLICITUDCOMITE.Description()) && !x.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()) && getFolderContract && x.ContractorId.Equals(contractorId))).OrderByDescending(o => o.RegisterDate);
            return await result
                .OrderByDescending(o => o.RegisterDate)
                .GroupBy(f => new {
                    f.FileId,
                    f.File.FilesName,
                    f.File.FileType,
                    f.File.DescriptionFile,
                    f.File.DocumentTypeNavigation.Code,
                    f.File.DocumentTypeNavigation.DocumentTypeDescription,
                    f.File.Folder.FolderName
                })
                .Select(f => new FileContractDto
                {
                    Id = f.Key.FileId,
                    FilesName = f.Key.FilesName,
                    FileType = f.Key.FileType,
                    DescriptionFile = f.Key.DescriptionFile,
                    RegisterDate = f.First().RegisterDate,
                    DocumentTypes = f.Key.DocumentTypeDescription,
                    Passed = f.OrderByDescending(o => o.RegisterDate).First().Passed,
                    StatusFile = f.OrderByDescending(o => o.RegisterDate).First().StatusFileId,
                    FolderName = f.Key.FolderName,
                    DocumentTypeCode = f.Key.Code,
                    CategoryCode = f.First().StatusFile.Category,
                    ContractId = contractId,
                })
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<IGenericResponse<string>> AddFileShareContractor(FilesDto modelFileDto)
        {
            var getDataFile = _context.DetailFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(x => x.File.DocumentTypeNavigation.Id.Equals(modelFileDto.DocumentType) && x.ContractorId.Equals(modelFileDto.ContractorId) && x.File.ContractId.Equals(modelFileDto.ContractId));

            List<DetailFileDto> detailFileList = new();
            List<Files> FilesList = new();
            var map = _mapper.Map<Files>(modelFileDto);
            map.Id = Guid.NewGuid();
            foreach (var item in modelFileDto.Contractors)
            {
                DetailFileDto detailFileDto = new();
                detailFileDto.Passed = false;
                detailFileDto.RegisterDate = modelFileDto.RegisterDate;
                detailFileDto.FileId = map.Id;
                detailFileDto.ContractorId = item;
                detailFileDto.ContractId = modelFileDto.ContractId.ToString();
                detailFileDto.UserId = modelFileDto.UserId;
                detailFileList.Add(detailFileDto);
                FilesList.Add(map);

            }
            _context.Files.AddRange(FilesList);

            var resultDetail = await CreateDetailList(detailFileList);
            if (resultDetail)
            {
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.ERRORDETAILFILE);
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
            try
            {
                var getFolderList = _context.Folder.Where(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()) && x.ContractId.Equals(modelFiles[0].ContractId)).ToList();

                var getFileList = _context.DetailFile
                    .Include(i => i.File)
                        .ThenInclude(i => i.DocumentTypeNavigation)
                    .Where(x => x.File.DocumentTypeNavigation.Id.Equals(modelFiles[0].DocumentType)).ToList();
                List<Files> SaveFiles = new();
                List<SaveFileResponse> saveFileResponses = new();
                List<Files> updateFiles = new();

                foreach (var item in modelFiles)
                {
                    var getFolder = getFolderList.OrderByDescending(O => O.RegisterDate).Where(x => x.ContractId.Equals(item.ContractId) && x.ContractorId.Equals(item.ContractorId)).FirstOrDefault();
                    if (getFolder == null)
                    {
                        return null;
                    }
                    var getFile = getFileList.Find(x => x.ContractorId.Equals(item.ContractorId) && x.File.DocumentTypeNavigation.Id.Equals(item.DocumentType));
                    SaveFileResponse saveFileResponse = new SaveFileResponse();
                    bool FileExist = false;

                    item.FolderId = getFolder.Id;

                    if (getFile != null)
                    {
                        FileExist = true;
                        item.Id = getFile.File.Id;
                        var updateFile = _mapper.Map(item, getFile.File);
                        saveFileResponse.RegisterDate = item.RegisterDate;
                        saveFileResponse.UserId = item.UserId.Value;
                        saveFileResponse.FileId = updateFile.Id;
                        saveFileResponse.ContractorId = item.ContractorId;
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
                        saveFileResponse.ContractorId = item.ContractorId;
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
            catch(Exception ex)
            {
                return null;
            }


        }

        private async Task<SaveFileResponse> SaveFileCommiteeContractor(FilesDto modelFiles)
        {
            var getFolderList = _context.Folder.Where(x => x.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()) && x.ContractId.Equals(modelFiles.ContractId)).ToList();

            var getDetailFile = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Where(w => w.File.DocumentTypeNavigation.Id.Equals(modelFiles.DocumentType) && modelFiles.Contractors.Contains(w.ContractorId)).FirstOrDefault();
            var contractorIdsToCheck = modelFiles.Contractors; // Lista de ContractorId a verificar
            var getDetailFileContarctors = _context.DetailFile
                .Include(i => i.File)
                .ThenInclude(i => i.DocumentTypeNavigation)
                .Where(w => w.File.DocumentTypeNavigation.Id.Equals(modelFiles.DocumentType) && contractorIdsToCheck.Contains(w.ContractorId))
                .ToList(); // Ejecuta la consulta y obtiene los resultados en una lista

            //var missingContractors = contractorIdsToCheck.Except(getDetailFileContarctors.Select(r => r.ContractorId)).ToList();
            //if (missingContractors.Count > 0 )
            //{

            //}

            SaveFileResponse saveFileResponses = new();
            DetailContractor updateDetail = new();
            var getFolder = getFolderList.OrderByDescending(o => o.RegisterDate).Where(x => x.ContractId.Equals(modelFiles.ContractId) && modelFiles.Contractors.Contains(x.ContractorId.Value)).ToList();

            SaveFileResponse saveFileResponse = new SaveFileResponse();

            if (getDetailFile != null)
            {
                modelFiles.Id = getDetailFile.File.Id;
                var updateFileMap = _mapper.Map(modelFiles, getDetailFile.File);
                saveFileResponse.FileId = updateFileMap.Id;
                saveFileResponse.RegisterDate = DateTime.Now;
                saveFileResponse.UserId = modelFiles.UserId.Value;
                saveFileResponse.FileExist = true;
                _context.Files.Update(updateFileMap);
            }
            else
            {
                var mapModelFile = _mapper.Map<Files>(modelFiles);
                mapModelFile.Id = Guid.NewGuid();
                saveFileResponse.RegisterDate = DateTime.Now;
                saveFileResponse.UserId = modelFiles.UserId.Value;
                saveFileResponse.FileId = mapModelFile.Id;
                saveFileResponse.FileExist = false;
                _context.Files.Add(mapModelFile);

            }
            return saveFileResponse;

        }

        private async Task<SaveFileResponse> SaveFileContract(FilesDto modelFiles)
        {

            List<Files> filesSave = new();

            var getFolder = _context.Folder.OrderByDescending(O => O.RegisterDate).Where(x => x.ContractId.Equals(modelFiles.ContractId) && x.ContractorId.Equals(modelFiles.ContractId)).FirstOrDefault();

            var getFile = _context.DetailFile.FirstOrDefault(x => x.ContractorId.Equals(modelFiles.ContractorId) && x.File.DocumentTypeNavigation.Id.Equals(modelFiles.DocumentType));
            SaveFileResponse saveFileResponse = new SaveFileResponse();
            bool FileExist = false;
            Guid FileId = Guid.Empty;
            modelFiles.FolderId = getFolder.Id;

            if (getFile != null)
            {
                FileExist = true;
                modelFiles.Id = getFile.Id;

                var updateFile = _mapper.Map(modelFiles, getFile.File);
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
