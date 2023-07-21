using Microsoft.Extensions.Configuration;
using WebApiHiringItm.MODEL.Entities;
using Aspose.Cells;
using System.Data;
using WebApiHiringItm.CONTEXT.Context;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using WebApiHiringItm.MODEL.Models;
using WebApiHiringItm.CORE.Helpers;
using System.Net;
using MimeKit;
using MailKit.Security;
using AutoMapper;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using Microsoft.Extensions.Options;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.CORE.Helpers.Enums;
using MimeKit.Utils;
using WebApiHiringItm.CORE.Helpers.Enums.StatusFile;
using WebApiHiringItm.CORE.Helpers.Enums.Hiring;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using System.Xml.Linq;
using System.Diagnostics.Contracts;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.CORE.Helpers.GenericValidation;

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

        public async Task<IGenericResponse<List<ContractorByContractDto>>> GetContractorByContract(string contractId)
        {
            if (string.IsNullOrEmpty(contractId) || !contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorByContractDto>>(Resource.GUIDNOTVALID);

            var getStatusFileProcess = _context.StatusFile.Where(w => w.Code.Equals(StatusFileEnum.ENPROCESO.Description())).FirstOrDefault()?.StatusFileDescription;

            var getStatusFiles = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile);
            var contractor = _context.DetailContractor.Where(x => x.ContractId.Equals(contractId))
                .Include(dt => dt.Contractor)
                    .ThenInclude(i => i.Files)
                .Include(dt => dt.HiringData)
                .Include(i => i.Contract)
                    .ThenInclude(i => i.StatusContract);
           
            var resultByContract = await contractor.Select(ct => new ContractorByContractDto
            {
                Id = ct.Contractor.Id,
                Nombre = ct.Contractor.Nombre + " " + ct.Contractor.Apellido,
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
                LegalProccess = getStatusFiles.Where(w => w.File.ContractId.Equals(ct.ContractId) && w.File.ContractorId.Equals(ct.ContractorId) && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))).ToList().Count >= 3
                ? "APROBADO"
                : getStatusFiles.Where(w => w.File.ContractId.Equals(ct.ContractId) && w.File.ContractorId.Equals(ct.ContractorId) && !w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault() == null 
                    ? getStatusFileProcess
                    : getStatusFiles.Where(w => w.File.ContractId.Equals(ct.ContractId) && w.File.ContractorId.Equals(ct.ContractorId) && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))).ToList().Count < 3
                        ? "EN PROCESO"
                        : getStatusFiles.Where(w => w.File.ContractId.Equals(ct.ContractId) && w.File.ContractorId.Equals(ct.ContractorId) && !w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description()) && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))).OrderByDescending(o => o.RegisterDate).Select(s => s.StatusFile.StatusFileDescription).FirstOrDefault(),
                HiringStatus = ct.HiringData != null ? HiringStatusEnum.CONTRATANDO.Description() : HiringStatusEnum.ENESPERA.Description(),
                AssignmentUser = ct.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.RESPONSABLECONTRATO.Description())).Select(s => s.User.Id).ToList()

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

        //public async Task<ChargeAccountDto?> ChargeAccountGetById(Guid contractorId, Guid ContractId)
        //{
        //    var result = _context.ContractorPayments
        //        .Include(co => co.Contractor)
        //            .ThenInclude(x => x.DetailContractor)
        //                .ThenInclude(x => x.Contract)
        //            .ThenInclude(x => x.DetailContractor)
        //                .ThenInclude(x => x.Element)
        //         .Where(x => x.Contractor.Id.Equals(contractorId) && x.Contract.Id.Equals(ContractId)).OrderByDescending(x => x.FromDate);

        //    if (result != null)
        //    {
        //        return await result.Select(cb => new ChargeAccountDto
        //        {
        //            Codigo = cb.Contractor.Codigo,
        //            Convenio = cb.Contractor.Convenio,
        //            Nombre = cb.Contractor.Nombre + "  " + cb.Contractor.Apellido,
        //            Identificacion = cb.Contractor.Identificacion,
        //            Direccion = cb.Contractor.Direccion,
        //            Departamento = cb.Contractor.Departamento,
        //            Municipio = cb.Contractor.Municipio,
        //            Barrio = cb.Contractor.Barrio,
        //            Telefono = cb.Contractor.Telefono,
        //            Celular = cb.Contractor.Celular,
        //            Correo = cb.Contractor.Correo,
        //            TipoAdministradora = cb.Contractor.TipoAdministradora,
        //            Administradora = cb.Contractor.Administradora,
        //            CuentaBancaria = cb.Contractor.CuentaBancaria,
        //            TipoCuenta = cb.Contractor.TipoCuenta,
        //            EntidadCuentaBancaria = cb.Contractor.EntidadCuentaBancaria,
        //            ContractId = cb.Contract.Id,
        //            From = cb.FromDate,
        //            To = cb.ToDate,
        //            Company = cb.Contract.CompanyName,
        //            Paymentcant = cb.Paymentcant,
        //            ContractNumber = cb.Contract.NumberProject,
        //            LugarExpedicion = cb.Contractor.LugarExpedicion,
        //            NombreElemento = cb.Contract.DetailContractor.Select(ne => ne.Element.NombreElemento).FirstOrDefault()
        //        })
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync();
        //    }
        //    return null;

        //}

        public async Task<FilesDto?> GetDocumentPdf(Guid contractId, Guid contractorId)
        {
            var result = _context.Files
                 .Where(x => x.ContractId.Equals(contractId) && x.ContractorId.Equals(contractorId));

            return await result.Select(fl => new FilesDto
            {
                Id = fl.Id,
                FilesName = fl.FilesName,
                Filedata = fl.Filedata,
                DocumentType = fl.DocumentType,
                DescriptionFile = fl.DescriptionFile
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        }

        public async Task<bool> Delete(Guid id)
        {
            var resultData = _context.DetailContractor.Where(x => x.ContractorId.Equals(id)).FirstOrDefault();
            Guid? getIdStatus = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.INHABILITADO.Description())).FirstOrDefault()?.Id;

            if (resultData != null)
            {
                resultData.StatusContractor = getIdStatus;
                var result = _context.DetailContractor.Update(resultData);
                await _context.SaveChangesAsync();

            }
            return true;
        }

        public async Task<bool> SavePersonalInformation(PersonalInformation model)
        {
            var getStatusContractor = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.CONTRATANDO.Description())).FirstOrDefault()?.Id;

            var getDetail = _context.DetailContractor.Where(x => x.ContractorId.Equals(model.ContractorPersonalInformation.Id)).FirstOrDefault();
            var getData = _context.Contractor.Where(x => x.Id.Equals(model.ContractorPersonalInformation.Id)).FirstOrDefault();
            var getAcademicInformation = _context.AcademicInformation.Where(x => x.Contractor.Equals(model.ContractorPersonalInformation.Id)).FirstOrDefault();
            var getDataEmptityHealth = _context.EmptityHealth.Where(x => x.Contractor.Equals(model.ContractorPersonalInformation.Id)).FirstOrDefault();

            if (getData != null)
            {

                getDetail.StatusContractor = getStatusContractor;
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
                if (getDataEmptityHealth != null)
                {
                    var mapEmptityHealth = _mapper.Map(model.EmptityHealth, getDataEmptityHealth);
                    _context.EmptityHealth.UpdateRange(mapEmptityHealth);
                }
                else
                {
                    var mapEmptityHealth = _mapper.Map<List<EmptityHealth>>(model.EmptityHealth);
                    foreach (var item in mapEmptityHealth)
                    {
                        item.Id = Guid.NewGuid();
                    }
                    _context.EmptityHealth.AddRange(mapEmptityHealth);
                }

                _context.Contractor.Update(map);
                _context.DetailContractor.Update(getDetail);

                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {

                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
        }


        public async Task<bool> AddNewness(NewnessContractorDto model)
        {
            var getData = _context.NewnessContractor.Where(x => x.Id.Equals(model.Id)).FirstOrDefault();
            Guid? getStatusContractor = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.INHABILITADO.Description())).FirstOrDefault()?.Id;
            var getContractor = _context.DetailContractor.Where(x => x.Id.Equals(Guid.Parse(model.ContractorId)) && x.ContractId.Equals(Guid.Parse(model.ContractId))).FirstOrDefault();
            if (getContractor != null)
            {
                getContractor.StatusContractor = getStatusContractor;
                var result = _context.DetailContractor.Update(getContractor);
            }
            if (getData == null)
            {
                var map = _mapper.Map<NewnessContractor>(model);
                map.Id = Guid.NewGuid();

                _context.NewnessContractor.Add(map);
                UpdateResource(Guid.Parse(model.ContractorId), Guid.Parse(model.ContractId));
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.NewnessContractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
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
                CompanyName = s.Contract.CompanyName
            })
            .AsNoTracking()
            .ToListAsync();


        }

        //public async Task<Contractor> SendMessageById(int idFolder)
        //{
        //    var result = _context.Contractor.Where(x => x.IdFolder == idFolder).ToList();
        //    foreach (var resultItem in result) { 

        //    }
        //    var map = _mapper.Map<Contractor>(result);
        //    return await Task.FromResult(map);
        //}


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
                    Nombre = ct.Nombre + " " + ct.Apellido,
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
                ElementComponent elemento = new();
                elemento = getData.Element;
                elemento.Recursos += getData.EconomicdataNavigation.Debt;
                _context.ElementComponent.Update(elemento);
            }
        }


        #endregion
    }
}
