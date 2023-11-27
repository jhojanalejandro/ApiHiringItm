using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.Hiring;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.CORE.Helpers.Enums.StatusFile;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.Contractors
{
    public class ContractorCore : IContractorCore
    {
        private const string TIPOASIGNACIONELEMENTO = "Elemento";
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        public ContractorCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region PUBLIC METODS
        public async Task<List<ContractorDto>> GetAll()
        {
            var result = _context.Contractor.Where(x => x.Id != null).ToList();
            var map = _mapper.Map<List<ContractorDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<IGenericResponse<List<ContractsContractorDto>>> GetSeveralContractsByContractor(string contractorId)
        {
            if (string.IsNullOrEmpty(contractorId) || !contractorId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractsContractorDto>>(Resource.GUIDNOTVALID);

            var result = _context.DetailContractor.Where(x => x.ContractorId.Equals(contractorId)).ToList();
            var map = _mapper.Map<List<ContractsContractorDto>>(result);
            if (map != null && map.Count > 0)
            {
                return ApiResponseHelper.CreateResponse(map);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<List<ContractsContractorDto>>(Resource.INFORMATIONEMPTY);
            }
        }

        public async Task<IGenericResponse<List<ContractorByContractDto>>> GetContractorsByContract(string contractId)
        {
            if (string.IsNullOrEmpty(contractId) || !contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorByContractDto>>(Resource.GUIDNOTVALID);

            var getStatusFileProcess = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.ENPROCESO.Description())).FirstOrDefault()?.StatusFileDescription;

            var getStatusFiles = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile);
            IQueryable<DetailContractor> contractor = _context.DetailContractor.Where(x => x.ContractId.Equals(Guid.Parse(contractId)));

            var resultByContract = await contractor.Select(ct => new ContractorByContractDto
            {
                Id = ct.Contractor.Id,
                Nombre = ct.Contractor.Nombres + " " + ct.Contractor.Apellidos,
                Identificacion = ct.Contractor.Identificacion,
                FechaNacimiento = ct.Contractor.FechaNacimiento,
                Telefono = ct.Contractor.Telefono,
                Celular = ct.Contractor.Celular,
                Correo = ct.Contractor.Correo,
                Direccion = ct.Contractor.Direccion,
                StatusContractor = ct.StatusContractorNavigation.StatusContractorDescription,
                ElementId = ct.ElementId.ToString().ToLower(),
                ComponentId = ct.ComponentId.ToString().ToLower(),
                ActivityId = ct.ActivityId.ToString().ToLower(),
                LegalProccess = getStatusFiles.Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId)
                && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && ((w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description())
                || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description())))).ToList().Count >= 4
                ? HiringStatusEnum.APROBADO.Description()
                : getStatusFiles.OrderByDescending(o => o.RegisterDate).Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId) && !w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && ((w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description())))).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() == null
                    ? getStatusFileProcess
                    : getStatusFiles.OrderByDescending(o => o.RegisterDate).Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId) && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))).ToList().Count < 4
                        ? HiringStatusEnum.ENESPERA.Description()
                        : getStatusFiles.OrderByDescending(o => o.RegisterDate).Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId) && !w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && ((w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description())))).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault(),
                HiringStatus = ct.HiringData != null ? ct.StatusContractorNavigation.Code.Equals(StatusContractorEnum.CONTRATADO.Description()) ? HiringStatusEnum.CONTRATADO.Description() : HiringStatusEnum.CONTRATANDO.Description() : HiringStatusEnum.ENESPERA.Description(),
                AssignmentUser = ct.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.CONTRACTUALCONTRATO.Description())).Select(s => s.User.Id).ToList(),
                MinuteGnenerated = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s).FirstOrDefault() != null ? ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() : HiringStatusEnum.PENDIENTE.Description(),
                ComiteGenerated = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.SOLICITUDCOMITE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s).FirstOrDefault() != null ? ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.SOLICITUDCOMITE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() : HiringStatusEnum.PENDIENTE.Description(),
                PreviusStudy = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.ESTUDIOSPREVIOS.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s).FirstOrDefault() != null ? ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.ESTUDIOSPREVIOS.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() : HiringStatusEnum.PENDIENTE.Description(),
                Gender = ct.Contractor.Genero,
                ContractValue = ct.EconomicdataContractor.OrderByDescending(o => o.Consecutive).Select(s => s.TotalValue).FirstOrDefault(),
                Nacionality = ct.Contractor.Nacionalidad,
                ExpeditionPlace = ct.Contractor.LugarExpedicion,
                InitialContractDate = ct.HiringData.FechaRealDeInicio,
                FinalContractDate = ct.HiringData.FechaFinalizacionConvenio,
                Cdp = ct.HiringData.Cdp,
                BankEntity = ct.Contractor.EntidadCuentaBancariaNavigation.BankName,
                Level = ct.HiringData.Nivel,
                Contract = ct.HiringData.Contrato,
                Eps = ct.Contractor.Eps.ToString().ToLower(),
                Afp = ct.Contractor.Afp.ToString().ToLower(),
                Arl = ct.Contractor.Arl.ToString().ToLower(),
            })
             .AsNoTracking()
             .ToListAsync();

            if (resultByContract != null)
            {
                return ApiResponseHelper.CreateResponse(resultByContract);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<List<ContractorByContractDto>>(Resource.INFORMATIONEMPTY);
            }
        }

        public async Task<IGenericResponse<ContractorByContractDto>> GetContractorByContract(string contractId, string contractorId)
        {
            if (string.IsNullOrEmpty(contractId) || !contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<ContractorByContractDto>(Resource.GUIDNOTVALID);

            var getStatusFileProcess = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.ENPROCESO.Description())).FirstOrDefault()?.StatusFileDescription;

            var getStatusFiles = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile);
            IQueryable<DetailContractor> contractor = _context.DetailContractor.Where(x => x.ContractId.Equals(Guid.Parse(contractId)))
                 .Include(dt => dt.Contractor)
                 .Include(dt => dt.HiringData)
                 .Include(i => i.Contract)
                     .ThenInclude(i => i.StatusContract);

            var resultByContract = await contractor.Select(ct => new ContractorByContractDto
            {
                Id = ct.Contractor.Id,
                Nombre = ct.Contractor.Nombres + " " + ct.Contractor.Apellidos,
                Identificacion = ct.Contractor.Identificacion,
                FechaNacimiento = ct.Contractor.FechaNacimiento,
                Telefono = ct.Contractor.Telefono,
                Celular = ct.Contractor.Celular,
                Correo = ct.Contractor.Correo,
                Direccion = ct.Contractor.Direccion,
                StatusContractor = ct.StatusContractorNavigation.StatusContractorDescription,
                ElementId = ct.ElementId.ToString().ToLower(),
                ComponentId = ct.ComponentId.ToString().ToLower(),
                ActivityId = ct.ActivityId.ToString().ToLower(),
                LegalProccess = getStatusFiles.Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId)
                && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && ((w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description())
                || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description())))).ToList().Count >= 4
                ? HiringStatusEnum.APROBADO.Description()
                : getStatusFiles.OrderByDescending(o => o.RegisterDate).Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId) && !w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && ((w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description())))).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() == null
                    ? getStatusFileProcess
                    : getStatusFiles.OrderByDescending(o => o.RegisterDate).Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId) && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))).ToList().Count < 4
                        ? HiringStatusEnum.ENESPERA.Description()
                        : getStatusFiles.OrderByDescending(o => o.RegisterDate).Where(w => w.File.ContractId.Equals(ct.ContractId) && w.ContractorId.Equals(ct.ContractorId) && !w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && ((w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description())))).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault(),
                HiringStatus = ct.HiringData != null ? ct.StatusContractorNavigation.Code.Equals(StatusContractorEnum.CONTRATADO.Description()) ? HiringStatusEnum.CONTRATADO.Description() : HiringStatusEnum.CONTRATANDO.Description() : HiringStatusEnum.ENESPERA.Description(),
                AssignmentUser = ct.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.CONTRACTUALCONTRATO.Description())).Select(s => s.User.Id).ToList(),
                MinuteGnenerated = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s).FirstOrDefault() != null ? ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() : HiringStatusEnum.PENDIENTE.Description(),
                ComiteGenerated = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.SOLICITUDCOMITE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s).FirstOrDefault() != null ? ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.SOLICITUDCOMITE.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() : HiringStatusEnum.PENDIENTE.Description(),
                PreviusStudy = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.ESTUDIOSPREVIOS.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s).FirstOrDefault() != null ? ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.ESTUDIOSPREVIOS.Description())).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() : HiringStatusEnum.PENDIENTE.Description(),
                Gender = ct.Contractor.Genero,
                ContractValue = ct.EconomicdataContractor.OrderByDescending(o => o.Consecutive).Select(s => s.TotalValue).FirstOrDefault(),
                Nacionality = ct.Contractor.Nacionalidad,
                ExpeditionPlace = ct.Contractor.LugarExpedicion,
                InitialContractDate = ct.HiringData.FechaRealDeInicio,
                FinalContractDate = ct.HiringData.FechaFinalizacionConvenio,
                Cdp = ct.HiringData.Cdp,
                BankEntity = ct.Contractor.EntidadCuentaBancariaNavigation.BankName,
                Level = ct.HiringData.Nivel,
                Eps = ct.Contractor.Eps.ToString().ToLower(),
                Afp = ct.Contractor.Afp.ToString().ToLower(),
                Arl = ct.Contractor.Arl.ToString().ToLower(),
            })
             .AsNoTracking()
             .FirstOrDefaultAsync();

            if (resultByContract != null)
            {
                return ApiResponseHelper.CreateResponse(resultByContract);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<ContractorByContractDto>(Resource.INFORMATIONEMPTY);
            }
        }

        public async Task<FilesDto?> GetDocumentPdf(Guid contractId, Guid contractorId)
        {
            var result = _context.DetailFile
                 .Where(x => x.File.ContractId.Equals(contractId) && x.ContractorId.Equals(contractorId));

            return await result.Select(fl => new FilesDto
            {
                Id = fl.Id,
                FilesName = fl.File.FilesName,
                Filedata = fl.File.Filedata,
                DocumentType = fl.File.DocumentType,
                DescriptionFile = fl.File.DescriptionFile
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        }

        public async Task<bool> Delete(Guid id)
        {
            var resultData = _context.DetailContractor.Where(x => x.ContractorId.Equals(id)).FirstOrDefault();
            Guid getIdStatus = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.INHABILITADO.Description())).Select(S => S.Id).FirstOrDefault();

            if (resultData != null)
            {
                resultData.StatusContractor = getIdStatus;
                var result = _context.DetailContractor.Update(resultData);
                await _context.SaveChangesAsync();

            }
            return true;
        }

        public async Task<IGenericResponse<string>> SavePersonalInformation(PersonalInformation model)
        {
            var getStatusContractor = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.CONTRATANDO.Description())).Select(s => s.Id).FirstOrDefault();

            var getDetail = _context.DetailContractor.Where(x => x.ContractorId.Equals(model.ContractorPersonalInformation.Id)).FirstOrDefault();
            var getData = _context.Contractor.Where(x => x.Id.Equals(model.ContractorPersonalInformation.Id)).FirstOrDefault();
            var getAcademicInformation = _context.AcademicInformation.Where(x => x.Contractor.Equals(model.ContractorPersonalInformation.Id)).FirstOrDefault();

            if (getData != null)
            {

                getDetail.StatusContractor = getStatusContractor;
                getDetail.Consecutive = getDetail.Consecutive;

                var map = _mapper.Map(model.ContractorPersonalInformation, getData);
                if (getAcademicInformation != null)
                {
                    var mapContractorStudyDto = _mapper.Map(model.AcademicInformation, getAcademicInformation);
                    _context.AcademicInformation.UpdateRange(mapContractorStudyDto);
                }
                else
                {
                    var mapContractorStudyDto = _mapper.Map<List<AcademicInformation>>(model.AcademicInformation);
                    foreach (var item in mapContractorStudyDto)
                    {
                        item.Id = Guid.NewGuid();
                    }
                    _context.AcademicInformation.AddRange(mapContractorStudyDto);

                }
                _context.Contractor.Update(map);
                _context.DetailContractor.Update(getDetail);
                await _context.SaveChangesAsync();
            }
            else
            {

                await _context.SaveChangesAsync();
            }
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);

        }


        public async Task<IGenericResponse<string>> AddNewness(NewnessContractorDto model)
        {
            try
            {
                var getDataNewness = _context.NewnessContractor
                .Include(i => i.NewnessTypeNavigation)
                .Where(x => x.Id.Equals(model.Id) && x.NewnessTypeNavigation.Code.Equals(model.NewnessCode)).FirstOrDefault();

                var getnewnessType = _context.NewnessType.Where(x => x.Code.Equals(model.NewnessCode)).Select(s => s.Id).FirstOrDefault();
                if (getDataNewness == null)
                {
                    var getStatusContractor = Guid.Empty;
                    if (model.NewnessCode.Equals(NewnessTypeCodeEnum.RECONTRATAR.Description()))
                    {
                        getStatusContractor = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.ENREVISIÓN.Description())).Select(s => s.Id).FirstOrDefault();
                    }
                    else
                    {
                        getStatusContractor = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.INHABILITADO.Description())).Select(s => s.Id).FirstOrDefault();
                    }

                    var getContractor = _context.DetailContractor
                        .Include(i => i.Element)
                        .Where(x => x.ContractorId.Equals(Guid.Parse(model.ContractorId)) && x.ContractId.Equals(Guid.Parse(model.ContractId))).FirstOrDefault();

                    if (getContractor != null)
                    {
                        getContractor.StatusContractor = getStatusContractor;
                        var result = _context.DetailContractor.Update(getContractor);
                    }
                    if (getDataNewness == null)
                    {
                        var map = _mapper.Map<NewnessContractor>(model);
                        map.Id = Guid.NewGuid();
                        map.Consecutive = 1;
                        _context.NewnessContractor.Add(map);
                    }
                }

                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<string>("Error "+ ex.Message);

            }


        }

        public async Task<IGenericResponse<string>> AddNewnessList(List<NewnessContractorDto> modelList)
        {
            try
            {
                List<NewnessContractor> newnessContractorList = new();
                foreach (var item in newnessContractorList)
                {
                    item.Id = Guid.NewGuid();
                    //var getnewnessType = _context.NewnessContractor.Where(x => x.ContractorId.Equals(model.con)).Select(s => s.Id).FirstOrDefault();
                    //var getStatusContractor = Guid.Empty;

                    //var getContractor = _context.DetailContractor
                    //    .Include(i => i.Element)
                    //    .Where(x => x.ContractorId.Equals(Guid.Parse(model.ContractorId)) && x.ContractId.Equals(Guid.Parse(model.ContractId))).FirstOrDefault();

                    //if (getContractor != null)
                    //{
                    //    getContractor.StatusContractor = getStatusContractor;
                    //    var result = _context.DetailContractor.Update(getContractor);
                    //}
       
                }
                var mapList = _mapper.Map<List<NewnessContractor>>(modelList);
                _context.NewnessContractor.AddRange(mapList);

                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<string>("Error " + ex.Message);

            }


        }


        public static string ToCamelCase(string str)
        {
            var words = str.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);

            words = words
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();

            return string.Join(string.Empty, words);
        }

        public async Task<List<ContractsContarctorDto>> getContractsByContractor(string contractorId)
        {
            return await _context.DetailContractor
            .Include(i => i.Contractor)
            .Include(i => i.Contract)
            .Where(w => w.ContractorId.Equals(Guid.Parse(contractorId)))
            .Select(s => new ContractsContarctorDto()
            {
                Id = s.Contract.Id.ToString(),
                CompanyName = s.Contract.CompanyName,
                ProjectName = s.Contract.ProjectName
            })
            .AsNoTracking()
            .ToListAsync();


        }

        public async Task<bool> UpdateAsignment(AsignElementOrCompoenteDto model)
        {
            if (model.Id != null)
            {
                try
                {
                    List<DetailContractor> asignDataListUpdate = new List<DetailContractor>();
                    for (int i = 0; i < model.ContractorId.Length; i++)
                    {
                        if (model.Type == TIPOASIGNACIONELEMENTO)
                        {
                            var contractorUpdate = _context.DetailContractor.FirstOrDefault(d => d.ContractId == model.ContractId && d.ContractorId.Equals(model.ContractorId[i]));
                            if (contractorUpdate != null)
                            {
                                contractorUpdate.ElementId = model.Id;
                                if (!string.IsNullOrEmpty(model.ActivityId))
                                {
                                    contractorUpdate.ActivityId = Guid.Parse(model.ActivityId);
                                }
                                asignDataListUpdate.Add(contractorUpdate);
                            }
                        }
                        else
                        {
                            var contractorUpdate = _context.DetailContractor.FirstOrDefault(d => d.ContractId == model.ContractId && d.ContractorId.Equals(model.ContractorId[i]));
                            if (contractorUpdate != null)
                            {
                                contractorUpdate.ComponentId = model.Id;
                                asignDataListUpdate.Add(contractorUpdate);
                            }
                        }
                    }
                    _context.DetailContractor.UpdateRange(asignDataListUpdate);
                    var res = await _context.SaveChangesAsync();
                    return res != 0 ? true : false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error", ex);
                }
            }
            return false;
        }

        public async Task<List<HistoryContractorDto>> GetHistoryContractor()
        {
            try
            {
                return await _context.Contractor.
                Select(ct => new HistoryContractorDto
                {
                    Id = ct.Id,
                    Nombre = ct.Nombres + " " + ct.Apellidos,
                    Identificacion = ct.Identificacion,
                    FechaNacimiento = ct.FechaNacimiento,
                    Direccion = ct.Direccion,
                    Telefono = ct.Telefono,
                    Correo = ct.Correo
                })
                  .AsNoTracking()
                  .ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }

        }

        public ValidateFileDto ValidateDocumentUpload(Guid contractId, Guid contractorId)
        {
            ValidateFileDto validate = new();

            var getDataFile = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile)
                 .Where(x => x.File.ContractId.Equals(contractId) && x.ContractorId.Equals(contractorId)).ToList();
            var getDetailContractor = _context.DetailContractor.OrderByDescending(o => o.Consecutive).Where(w => w.ContractId.Equals(contractId) && w.ContractorId.Equals(contractorId)).Select(s => s.Id).FirstOrDefault();
            var termContractList = _context.TermContract
                .Include(i => i.TermTypeNavigation)
                .Where(x => x.DetailContractor.Equals(getDetailContractor)).ToList();
            validate.ActivateTermPayments = termContractList.Any(f => f.TermTypeNavigation.Code.Equals("DCNM") && f.TermDate >= DateTime.Now);
            validate.ActivateTermContract = termContractList.Any(f => f.TermTypeNavigation.Code.Equals("DCCT") && f.TermDate >= DateTime.Now);

            validate.Hv = getDataFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(w => w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) && !w.StatusFile.Code.Equals(StatusFileEnum.REMITIDO.Description())) != null ? true : false;
            validate.Secop = getDataFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(w => w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()) && !w.StatusFile.Code.Equals(StatusFileEnum.REMITIDO.Description())) != null ? true : false;
            validate.Exam = getDataFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(w => w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) && !w.StatusFile.Code.Equals(StatusFileEnum.REMITIDO.Description())) != null ? true : false;
            validate.Dct = getDataFile.OrderByDescending(o => o.RegisterDate).FirstOrDefault(w => w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) && !w.StatusFile.Code.Equals(StatusFileEnum.REMITIDO.Description())) != null ? true : false;


            return validate;


        }

        public async Task<IGenericResponse<string>> SaveModifyMinute(ChangeContractContractorDto economicDataModel)
        {
            try
            {
                var getDetailId = _context.DetailContractor.Where(w => w.ContractId.Equals(Guid.Parse(economicDataModel.ContractId)) && w.ContractorId.Equals(Guid.Parse(economicDataModel.ContractorId))).OrderByDescending(o => o.Consecutive).FirstOrDefault();
                var getEconomicData = _context.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(getDetailId.Id)).OrderByDescending(o => o.Consecutive).FirstOrDefault();
                var getModify = _context.ChangeContractContractor.Where(w => w.DetailContractorId.Equals(getDetailId.Id)).OrderByDescending(o => o.Consecutive).FirstOrDefault();

                if (economicDataModel.IsAddition == true)
                {
                    var getData = _context
                           .EconomicdataContractor
                           .Include(i => i.DetailContractor)
                           .OrderByDescending(o => o.Consecutive)
                           .FirstOrDefault(x => x.DetailContractor.ContractorId.Equals(Guid.Parse(economicDataModel.ContractorId)) && x.DetailContractor.ContractId.Equals(Guid.Parse(economicDataModel.ContractId)));
                    if (getData != null)
                    {
                        EconomicdataContractor economicdataContractor = new();
                        economicdataContractor.Consecutive = getData.Consecutive + 1;
                        economicdataContractor.DetailContractorId = getData.DetailContractorId;
                        economicdataContractor.TotalValue = economicDataModel.TotalValue;
                        economicdataContractor.UnitValue = economicDataModel.UnitValue;
                        economicdataContractor.Debt = economicDataModel.Debt;
                        economicdataContractor.RegisterDate = economicDataModel.RegisterDate;
                        economicdataContractor.ModifyDate = economicDataModel.RegisterDate;
                        economicdataContractor.Freed = getData.Freed;
                        economicdataContractor.CashPayment = false;
                        economicdataContractor.Missing = 0;
                        economicdataContractor.Debt = economicDataModel.TotalValue;
                        economicdataContractor.AdditionalValue = economicDataModel.TotalValue - getData.TotalValue;


                        economicdataContractor.Id = Guid.NewGuid();
                        _context.EconomicdataContractor.Add(economicdataContractor);
                    }
                    await _context.SaveChangesAsync();

                }

                var mapChangeContarctor = _mapper.Map<ChangeContractContractor>(economicDataModel);
                mapChangeContarctor.Id = Guid.NewGuid();
                mapChangeContarctor.DetailContractorId = getDetailId.Id;
                mapChangeContarctor.Consecutive += 1;
                mapChangeContarctor.NoAddition = economicDataModel.NoAddition;
                mapChangeContarctor.EconomicdataContractor = getEconomicData.Id;
                _context.ChangeContractContractor.Add(mapChangeContarctor);
                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
            }


        }

        public async Task<IGenericResponse<List<ContractorsPrePayrollDto>>> GetContractorByContractPrePayroll(string contractId)
        {
            if (string.IsNullOrEmpty(contractId) || !contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorsPrePayrollDto>>(Resource.GUIDNOTVALID);

            var getStatusFileProcess = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.ENPROCESO.Description())).FirstOrDefault()?.StatusFileDescription;

            var getStatusFiles = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile);
            IQueryable<DetailContractor> contractor = _context.DetailContractor.Where(x => x.ContractId.Equals(Guid.Parse(contractId)))
                 .Include(dt => dt.Contractor)
                 .Include(dt => dt.HiringData)
                 .Include(i => i.Contract)
                     .ThenInclude(i => i.StatusContract)
               .Where(w => w.Contractor.DetailContractor.Where(ww => ww.ContractId.Equals(Guid.Parse(contractId))).OrderByDescending(o => o.Consecutive).Select(sd => sd.StatusContractorNavigation.Code).FirstOrDefault() == StatusContractorEnum.CONTRATADO.Description()); ;


            var resultByContract = await contractor.Select(ct => new ContractorsPrePayrollDto
            {
                Id = ct.Contractor.Id,
                Nombre = ct.Contractor.Nombres + " " + ct.Contractor.Apellidos,
                Identificacion = ct.Contractor.Identificacion,
                FechaNacimiento = ct.Contractor.FechaNacimiento,
                Telefono = ct.Contractor.Telefono,
                Celular = ct.Contractor.Celular,
                Correo = ct.Contractor.Correo,
                Direccion = ct.Contractor.Direccion,
                StatusContractor = ct.StatusContractorNavigation.StatusContractorDescription,
                HiringStatus = ct.HiringData != null ? ct.StatusContractorNavigation.Code.Equals(StatusContractorEnum.CONTRATADO.Description()) ? HiringStatusEnum.CONTRATADO.Description() : HiringStatusEnum.CONTRATANDO.Description() : HiringStatusEnum.ENESPERA.Description(),
                AssignmentUser = ct.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.CONTRACTUALCONTRATO.Description())).Select(s => s.User.Id).ToList(),
                Gender = ct.Contractor.Genero,
                ContractValue = ct.EconomicdataContractor.OrderByDescending(o => o.Consecutive).Select(s => s.TotalValue).FirstOrDefault(),
                Nacionality = ct.Contractor.Nacionalidad,
                ExpeditionPlace = ct.Contractor.LugarExpedicion,
                InitialContractDate = ct.HiringData.FechaRealDeInicio,
                FinalContractDate = ct.HiringData.FechaFinalizacionConvenio,
                Cdp = ct.HiringData.Cdp,
                BankEntity = ct.Contractor.EntidadCuentaBancariaNavigation.BankName,
                Level = ct.HiringData.Nivel,
                Eps = ct.Contractor.Eps.ToString().ToLower(),
                Afp = ct.Contractor.Afp.ToString().ToLower(),
                Arl = ct.Contractor.Arl.ToString().ToLower(),
                PaymentsCant = ct.ContractorPayments.Select(s => s.Consecutive).Count()
            })
             .AsNoTracking()
             .ToListAsync();

            if (resultByContract != null)
            {
                return ApiResponseHelper.CreateResponse(resultByContract);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<List<ContractorsPrePayrollDto>>(Resource.INFORMATIONEMPTY);
            }
        }

        public async Task<ContractorDto?> GetById(string contractorId)
        {
            var result = _context.Contractor.Where(x => x.Id.Equals(Guid.Parse(contractorId)));
            return await result.Select(us => new ContractorDto
            {
                Id = us.Id,
                Identificacion = us.Identificacion
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<List<NewnessContractorDto>?> GetNewnessContractor(Guid contractId, Guid contractorId)
        {
            var result = _context.NewnessContractor
                 .Where(x => x.ContractId.Equals(contractId) && x.ContractorId.Equals(contractorId));

            return await result.Select(fl => new NewnessContractorDto
            {
                Id = fl.Id,
                NewnessDescripcion = fl.NewnessDescripcion,
                ContractorId = fl.ContractorId.ToString(),
                NewnessType = fl.NewnessTypeNavigation.NewnessDescription,
                Consecutive = fl.Consecutive,
                RegisterDate = fl.RegisterDate
            })
            .AsNoTracking()
            .ToListAsync();

        }

        public async Task<bool> GetStatusContractor(string contractorId, string contractId)
        {
            var result = _context.DetailContractor
                .Include(i => i.StatusContractorNavigation)
                .Where(x => x.ContractorId.Equals(Guid.Parse(contractorId)) && x.ContractId.Equals(Guid.Parse(contractId))).FirstOrDefault();
            if (result.StatusContractorNavigation.Code.Equals(StatusContractorEnum.CONTRATADO.Description()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region METODS PRIVATE

        private async Task<bool> Update(AddPasswordContractorDto model)
        {
            if (model.Id != null)

            {
                var userupdate = _context.Contractor.Where(x => x.Id.Equals(model.Id) && x.Identificacion.Equals(model.DocumentodeIdentificacion)).FirstOrDefault();
                var map = _mapper.Map(model, userupdate);
                _context.Contractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            return false;
        }

        private void UpdateResource(Guid contractorId, Guid contractId)
        {
            var getData = _context.DetailContractor.Where(x => x.ContractId.Equals(contractId) && x.ContractorId.Equals(contractorId))
                .Include(i => i.Element)
                .Include(i => i.Contract)
                .FirstOrDefault();
            if (getData.Id != null)
            {
                //if (getData.Element != null)
                //{
                //    getData.ElementId = null;
                //    _context.ElementComponent.Update(elemento);
                //}
                //elemento.Recursos += getData.EconomicdataContractor..OrderByDescending(o => o.Consecutive).Select(s => s.Debt);
            }
        }


        #endregion
    }
}
