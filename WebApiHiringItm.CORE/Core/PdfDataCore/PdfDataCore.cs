using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.ContractFolders;
using WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.PdfDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.PdfDataCore
{
    public class PdfDataCore : IPdfDataCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;

        public PdfDataCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region PUBLIC METHODS
        public async Task<ExecutionReportDto?> GetExecutionReport(Guid contractId, Guid contractorId)
        {
            var result = _context.DetailContractor
                .Where(x => x.Contractor.Id.Equals(contractorId) && x.ContractId.Equals(contractId));

            return await result.Select(report => new ExecutionReportDto
            {
                ContractorName = report.Contractor.Nombres + " " + report.Contractor.Apellidos,
                ContractInitialDate = report.HiringData.FechaRealDeInicio.ToString(),
                ContractFinalDate = report.HiringData.FechaFinalizacionConvenio.ToString(),
                ContractorIdentification = report.Contractor.Identificacion,
                ContractNumber = report.Contract.NumberProject,
                SupervisorContract = report.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.UserName).FirstOrDefault(),
                SupervisorIdentification = report.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Identification).FirstOrDefault(),
                PeriodExecutedInitialDate = report.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                PeriodExecutedFinalDate = report.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                SpecificObligations = report.Element.ObligacionesEspecificas,
                ElementObject = report.Element.ObjetoElemento,
                TotalValue = Math.Ceiling(report.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(w.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.TotalValue).FirstOrDefault()),
                TotalValuePeriod = report.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<ResponsePdfDataDto<MinuteModifyDataDto>> GetminuteModifyData(ContractContractorsDto contractors)
        {
            ResponsePdfDataDto<MinuteModifyDataDto> minuteModifyDataDtoContractorsDto = new();

            minuteModifyDataDtoContractorsDto.PersonalInCharge = await GetPersonalContractual(Guid.Parse(contractors.contractId));
            minuteModifyDataDtoContractorsDto.DataContract = GetDataContract(Guid.Parse(contractors.contractId));
            minuteModifyDataDtoContractorsDto.GetDataContractors = await GetMinuteModify(contractors);
            return minuteModifyDataDtoContractorsDto;

        }

        public async Task<MacroMinuteDto?> GetminuteMacroContract(Guid contractId)
        {
            var result = _context.DetailContract
                .Include(i => i.Contract)
                .Where(x => x.ContractId.Equals(contractId));

            return await result.Select(report => new MacroMinuteDto
            {
                ContractNumber = report.Contract.NumberProject,
                ContractName = report.Contract.CompanyName,
                PeriodInitialDate = report.FechaContrato,
                PeriodFinalDate = report.FechaFinalizacion,
                Object = report.Contract.ObjectContract,
                TotalValueContract = report.Contract.ValorContrato,
                CompanyName = report.Contract.CompanyName
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<ResponsePdfDataDto<MinutaDto>> GetDataBill(ContractContractorsDto contractors)
        {
            ResponsePdfDataDto<MinutaDto> previusStudyContractors = new();
            previusStudyContractors.DataContract = GetDataContract(Guid.Parse(contractors.contractId));
            previusStudyContractors.GetDataContractors = await getDataMinuteContractor(contractors);
            previusStudyContractors.PersonalInCharge = await GetPersonalContractual(Guid.Parse(contractors.contractId));
            return previusStudyContractors;
        }

        public async Task<ResponsePdfDataDto<PreviusStudyDto>> GetPreviusStudy(ContractContractorsDto contractors)
        {
            ResponsePdfDataDto<PreviusStudyDto> previusStudyContractors = new();
            previusStudyContractors.DataContract = GetDataContract(Guid.Parse(contractors.contractId));
            previusStudyContractors.GetDataContractors = await GetPrevusStudyContractorsList(contractors);
            previusStudyContractors.PersonalInCharge = await GetPersonalContractual(Guid.Parse(contractors.contractId));
            return previusStudyContractors;
        }

        public async Task<ResponsePdfDataDto<CommiteeRequestDto>> GetCommitteeRequest(ContractContractorsDto contractors)
        {
            ResponsePdfDataDto<CommiteeRequestDto> commiteeRequestDto = new();
            var getDataContract = _context.DetailContract
                .Include(i => i.Contract)
                .Where(w => w.ContractId.Equals(Guid.Parse(contractors.contractId))).FirstOrDefault();
            commiteeRequestDto.DataContract = GetDataContract(Guid.Parse(contractors.contractId));
            commiteeRequestDto.GetDataContractors = await GetCommiteeContractorsList(contractors);
            commiteeRequestDto.PersonalInCharge = await GetPersonalContractual(Guid.Parse(contractors.contractId));
            return commiteeRequestDto;
        }

        #endregion

        #region PRIVATE METHODS

        private async Task<List<PersonalInChargeDto>> GetPersonalContractual(Guid contarctId)
        {

            var GetPersonal = _context.AssigmentContract
                 .Where(w => (w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.JURIDICONTRATO.Description()) || w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.CONTRACTUALCONTRATO.Description()) || w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())) && (w.ContractId.Equals(contarctId) || w.User.Roll.Code.Equals(RollEnum.JEFEUNIDADESTRATEGICA.Description())));

            var getJob = _context.UserT
            .Where(w => w.Roll.Code.Equals(RollEnum.JEFEUNIDADESTRATEGICA.Description()));

            var mapJob = getJob.Select(s => new PersonalInChargeDto
            {
                UserName = s.UserName,
                UserCharge = s.Professionalposition,
                UserIdentification = s.Identification,
                UserFirm = s.UserFile.Where(w => w.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.FileData).FirstOrDefault(),
                UserFirmType = s.UserFile.Where(w => w.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.FileType).FirstOrDefault(),
                UserChargeCode = s.Roll.Code,
            }).FirstOrDefault();

            var resp = await GetPersonal.Select(s => new PersonalInChargeDto
            {
                UserName = s.User.UserName,
                UserCharge = s.User.Professionalposition,
                UserIdentification = s.User.Identification,
                UserFirm = s.User.UserFile.Where(w => w.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.FileData).FirstOrDefault(),
                UserFirmType = s.User.UserFile.Where(w => w.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.FileType).FirstOrDefault(),
                UserChargeCode = s.User.Roll.Code,
            }).AsNoTracking()
            .ToListAsync();
            resp.Add(mapJob);
            return resp;
        }

        private async Task<List<PreviusStudyDto>> GetPrevusStudyContractorsList(ContractContractorsDto contractors)
        {

            var result = _context.DetailContractor
                .Where(x => contractors.contractors.Contains(x.ContractorId.ToString()) && x.ContractId.Equals(Guid.Parse(contractors.contractId)));
            var typeUserFileId = _context.UserFileType.Where(x => x.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.Id).FirstOrDefault();

            return await result.Select(study => new PreviusStudyDto
            {
                RequiredProfile = study.Element.PerfilRequeridoAcademico,
                ElementObject = study.Element.ObjetoElemento,
                ContractorId = study.ContractorId.ToString(),
                ContractorName = study.Contractor.Nombres + " " + study.Contractor.Apellidos,
                ContractorIdentification = study.Contractor.Identificacion,
                ContractNumber = study.Contract.NumberProject,
                SpecificObligations = study.Element.ObligacionesEspecificas,
                GeneralObligations = study.Element.ObligacionesGenerales,
                ContractInitialDate = study.Contractor.HiringData.Select(s => s.FechaRealDeInicio).FirstOrDefault(),
                ContractFinalDate = study.Contractor.HiringData.Select(s => s.FechaFinalizacionConvenio).FirstOrDefault(),
                TotalValue = Math.Ceiling(study.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(study.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.TotalValue).FirstOrDefault()),
                UnifiedProfile = study.Element.PerfilRequeridoAcademico + " " + study.Element.PerfilRequeridoExperiencia,
                RequiredProfileAcademic = study.Element.PerfilRequeridoAcademico,
                RequiredProfileExperience = study.Element.PerfilRequeridoExperiencia,
                ActivityContractor = study.HiringData.ActividadContratista,
                DutyContract = study.Contract.DutyContract,
                PoliceRequire = study.HiringData.RequierePoliza
            })
            .AsNoTracking()
            .ToListAsync();
        }

        private async Task<List<CommiteeRequestDto>> GetCommiteeContractorsList(ContractContractorsDto contractors)
        {

            var result = _context.DetailContractor
               .Include(i => i.Contract)
               .Include(i => i.Element)
               .Include(i => i.HiringData)
               .Include(i => i.Contractor)
               .Include(ti => ti.ContractorPayments.OrderByDescending(s => s.FromDate))
               .Where(x => contractors.contractors.Contains(x.ContractorId.ToString()) && x.ContractId.Equals(Guid.Parse(contractors.contractId)));

            return await result.Select(study => new CommiteeRequestDto
            {
                ContractorId = study.ContractorId.ToString(),
                ContractorName = study.Contractor.Nombres + " " + study.Contractor.Apellidos,
                ContractorIdentification = study.Contractor.Identificacion,
                ContractNumber = study.Contract.NumberProject,
                ElementName = study.Element.NombreElemento,
                ElementObject = study.Element.ObjetoElemento,
                User = study.Contractor.User.UserName,
                UserFirm = study.Contractor.User.UserFile.Where(w => w.UserFileType.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.FileData).FirstOrDefault(),
                UserIdentification = study.Contractor.User.Identification,
                ContractInitialDate = study.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                ContractFinalDate = study.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                TotalValue = Math.Ceiling(study.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant).FirstOrDefault()),
                ProfileRequire = study.Element.PerfilRequeridoAcademico != null && study.Element.PerfilRequeridoExperiencia != null ? study.Element.PerfilRequeridoAcademico  + " " + study.Element.PerfilRequeridoExperiencia : null,
                
            })
            .AsNoTracking()
            .ToListAsync();
        }

        private async Task<List<MinuteModifyDataDto>> GetMinuteModify(ContractContractorsDto contractors)
        {
            var result = _context.ChangeContractContractor
            .Include(i => i.DetailContractor)
            .Where(w => contractors.contractors.Contains(w.DetailContractor.Contractor.Id.ToString()) && w.DetailContractor.ContractId.Equals(Guid.Parse(contractors.contractId))).OrderByDescending(o => o.Consecutive);

            return await result.Select(report => new MinuteModifyDataDto
            {
                ContractorName = report.DetailContractor.Contractor.Nombres + " " + report.DetailContractor.Contractor.Apellidos,
                ContractNumber = report.DetailContractor.Contract.NumberProject,
                ContractorId = report.DetailContractor.Contractor.Id.ToString().ToLower(),
                ContractorIdentification = report.DetailContractor.Contractor.Identificacion,
                ContractName = report.DetailContractor.Contract.CompanyName,
                InitialDateContract = report.DetailContractor.HiringData.FechaRealDeInicio.Value,
                FinalDateContract = report.DetailContractor.HiringData.FechaFinalizacionConvenio.Value,
                ExtensionInitialDate = report.InitialAdditionDate,
                ExtensionFinalDate = report.FinalAdditionDate,
                Object = report.DetailContractor.Element.ObjetoElemento,
                UnitValueContract = Math.Ceiling(report.EconomicdataContractorNavigation.UnitValue),
                TotalValueContract = Math.Ceiling(report.EconomicdataContractorNavigation.TotalValue),
                Supervisor = report.DetailContractor.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.UserName).FirstOrDefault()!,
                SupervisorCharge = report.DetailContractor.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Professionalposition).FirstOrDefault()!,
                SupervisorIdentification = report.DetailContractor.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Identification).FirstOrDefault(),
                SpecificObligations = report.SpecificObligations,
                GeneralObligations = report.GeneralObligations,
                NumberModify = report.Consecutive,
                RubroContract = report.DetailContractor.Contract.RubroNavigation.RubroNumber,
                TypeModify = report.MinuteTypeNavigation.Code,
                AdditionValue =  report.EconomicdataContractorNavigation.AdditionalValue,
                InitialValue = report.EconomicdataContractorNavigation.TotalValue - report.EconomicdataContractorNavigation.AdditionalValue,
                Consecutive = report.Consecutive

            })
            .AsNoTracking()
            .ToListAsync();
        }
        
        private DataContractDto GetDataContract(Guid contractId)
        {
            DataContractDto dto = new DataContractDto();
            var getDataContract = _context.DetailContract
            .Include(i => i.Contract)
                .ThenInclude(ti => ti.RubroNavigation)
            .Where(w => w.ContractId.Equals(contractId)).OrderByDescending(o => o.Consecutive).FirstOrDefault();
            dto.RegisterDate = getDataContract.RegisterDate;
            dto.ContractNumber = getDataContract.Contract.NumberProject;
            dto.ProjectName = getDataContract.Contract.ProjectName;
            dto.CompanyName = getDataContract.Contract.CompanyName;
            dto.ContractObject = getDataContract.Contract.ObjectContract;
            dto.Rubro = getDataContract.Contract.RubroNavigation.RubroNumber;
            dto.RubroName = getDataContract.Contract.RubroNavigation.Rubro;
            dto.RubroOrigin = getDataContract.Contract.FuenteRubro;
            return dto;
        }
        
        private async Task<List<MinutaDto>> getDataMinuteContractor(ContractContractorsDto contractors)
        {
            var contractor = _context.DetailContractor.Where(x => x.ContractId.Equals(Guid.Parse(contractors.contractId)))
            .Where(w => contractors.contractors.Contains(w.Contractor.Id.ToString()) && !w.StatusContractor.Equals(StatusContractorEnum.INHABILITADO.Description()));

            return await contractor.Select(ct => new MinutaDto
            {
                ContractorId = ct.ContractorId,
                FinalContractDate = ct.HiringData.FechaFinalizacionConvenio,
                InitialDateContract = ct.HiringData.FechaRealDeInicio,
                Compromiso = ct.HiringData.Compromiso,
                ComiteDate = ct.HiringData.FechaDeComite,
                Rubro = ct.Contract.RubroNavigation.RubroNumber,
                RUbroName = ct.Contract.RubroNavigation.Rubro,
                RubroOrigin = ct.Contract.FuenteRubro,
                NumeroActa = ct.HiringData.NumeroActa,
                ElementName = ct.Element.NombreElemento,
                GeneralObligations = ct.Element.ObligacionesGenerales,
                SpecificObligations = ct.Element.ObligacionesEspecificas,
                UnitValue = Math.Ceiling(ct.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(ct.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.UnitValue).FirstOrDefault()),
                TotalValueContract = Math.Ceiling(ct.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(ct.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.TotalValue).FirstOrDefault()),
                Cpc = ct.Element.Cpc.CpcNumber,
                CpcName = ct.Element.Cpc.CpcName,
                ElementObject = ct.Element.ObjetoElemento,
                ContractorName = ct.Contractor.Nombres + " " + ct.Contractor.Apellidos,
                ContractorIdentification = ct.Contractor.Identificacion,
                ContractorExpeditionPlace = ct.Contractor.LugarExpedicion,
                BirthDate = ct.Contractor.FechaNacimiento,
                ContractorMail = ct.Contractor.Correo,
                ContractNumber = ct.HiringData.Contrato,
                ComiteGenerated = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.SOLICITUDCOMITE.Description())).Select(s => s).FirstOrDefault() != null ? true : false,
                PreviusStudy = ct.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.ESTUDIOSPREVIOS.Description())).Select(s => s).FirstOrDefault() != null ? true : false,
                RequirePolice = ct.HiringData.RequierePoliza,
            })
              .AsNoTracking()
              .ToListAsync();
        }
        #endregion
    }
}
