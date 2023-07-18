using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.PdfDto;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;

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
                ContractorName = report.Contractor.Nombre + " " + report.Contractor.Apellido,
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
                TotalValue = report.EconomicdataNavigation.TotalValue,
                TotalValuePeriod = report.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<ChargeAccountDto?> GetChargeAccount(Guid contractId, Guid contractorId)
        {
            var result = _context.DetailContractor
                .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId.Equals(contractId));

            return await result.Select(report => new ChargeAccountDto
            {
                ChargeAccountNumber = report.ContractorPayments.Count().ToString(),
                ContractorName = report.Contractor.Nombre + " " + report.Contractor.Apellido,
                ContractNumber = report.Contract.NumberProject,
                ContractorIdentification = report.Contractor.Identificacion,
                ExpeditionIdentification = report.Contractor.LugarExpedicion,
                PhoneNumber = report.Contractor.Celular,
                AccountType = report.Contractor.TipoCuenta,
                AccountNumber = report.Contractor.CuentaBancaria,
                BankingEntity = report.Contractor.EntidadCuentaBancariaNavigation.BankName,
                ContractName = report.Contract.CompanyName,
                Direction = report.Contractor.Direccion,
                PeriodExecutedInitialDate = report.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                PeriodExecutedFinalDate = report.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                elementName = report.Element.NombreElemento,
                TotalValue = report.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<MinuteExtensionDto?> GetminuteExtension(Guid contractId, Guid contractorId)
        {
            var result = _context.DetailContractor
                .Include(i => i.Contract)
                .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId.Equals(contractId));

            return await result.Select(report => new MinuteExtensionDto
            {
                ContractorName = report.Contractor.Nombre + " " + report.Contractor.Apellido,
                ContractNumber = report.Contract.NumberProject,
                ContractorIdentification = report.Contractor.Identificacion,
                ContractName = report.Contract.CompanyName,
                PeriodInitialDate = report.HiringData.FechaRealDeInicio,
                PeriodFinalDate = report.HiringData.FechaFinalizacionConvenio,
                Object = report.Element.ObjetoElemento,
                TotalValueContract = report.EconomicdataNavigation.TotalValue,
                Supervisor = report.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.UserName).FirstOrDefault(),
                SupervisorCharge = report.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Professionalposition).FirstOrDefault(),
                SupervisorIdentification = report.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Identification).FirstOrDefault()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
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
                //Supervisor = report.HiringData.SupervisorItm,
                //SupervisorCharge = report.HiringData.CargoSupervisorItm,
                //SupervisorIdentification = report.HiringData.IdentificacionSupervisor
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<List<MinutaDto>> GetDataBill(ContractContractorsDto contractors)
        {
            try
            {
                var contractor = _context.DetailContractor.Where(x => x.ContractId == contractors.contractId)
                .Include(dt => dt.Contractor)
                .Include(hd => hd.HiringData)
                .Include(el => el.Element)
                .Include(el => el.Contract)
                    .ThenInclude(i => i.RubroNavigation)
                .Where(w => contractors.contractors.Contains(w.Contractor.Id.ToString()) && !w.StatusContractor.Equals(StatusContractorEnum.INHABILITADO.Description()));

                return await contractor.Select(ct => new MinutaDto
                {
                    ContractorId = ct.ContractorId,
                    FechaFinalizacionConvenio = ct.HiringData.FechaFinalizacionConvenio,
                    Contrato = ct.HiringData.Contrato,
                    Compromiso = ct.HiringData.Compromiso,
                    SupervisorItm = ct.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.UserName).FirstOrDefault(),
                    CargoSupervisorItm = ct.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Professionalposition).FirstOrDefault(),
                    IdentificacionSupervisor = ct.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Identification).FirstOrDefault(),
                    FechaRealDeInicio = ct.HiringData.FechaRealDeInicio,
                    FechaDeComite = ct.HiringData.FechaDeComite,
                    Rubro = ct.Contract.RubroNavigation.RubroNumber,
                    NombreRubro = ct.Contract.RubroNavigation.Rubro,
                    FuenteRubro = ct.Contract.RubroNavigation.RubroOrigin,
                    NumeroActa = ct.HiringData.NumeroActa,
                    NombreElemento = ct.Element.NombreElemento,
                    ObligacionesGenerales = ct.Element.ObligacionesGenerales,
                    ObligacionesEspecificas = ct.Element.ObligacionesEspecificas,
                    ValorUnidad = ct.EconomicdataNavigation.UnitValue,
                    ValorTotal = ct.EconomicdataNavigation.TotalValue,
                    Cpc = ct.Element.Cpc.CpcNumber,
                    NombreCpc = ct.Element.Cpc.CpcName,
                    ObjetoElemento = ct.Element.ObjetoElemento,
                    Nombre = ct.Contractor.Nombre + " " + ct.Contractor.Apellido,
                    Identificacion = ct.Contractor.Identificacion,
                    LugarExpedicion = ct.Contractor.LugarExpedicion,
                    FechaNacimiento = ct.Contractor.FechaNacimiento,
                    Direccion = ct.Contractor.Direccion,
                    Telefono = ct.Contractor.Telefono,
                    Celular = ct.Contractor.Celular,
                    Correo = ct.Contractor.Correo,

                })
                  .AsNoTracking()
                  .ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }

        }

        public async Task<List<PreviusStudyDto>> GetPreviusStudy(ContractContractorsDto contractors)
        {
            var result = _context.DetailContractor
                .Where(x => contractors.contractors.Contains(x.ContractorId.ToString()) && x.ContractId.Equals(contractors.contractId));
            var typeUserFileId = _context.UserFileType.Where(x => x.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.Id).FirstOrDefault();

            return await result.Select(study => new PreviusStudyDto
            {
                ElementObject = study.Element.ObjetoElemento,
                ContractorId = study.ContractorId.ToString(),
                ContractorName = study.Contractor.Nombre + " " + study.Contractor.Apellido,
                ContractorIdentification = study.Contractor.Identificacion,
                ContractNumber = study.Contract.NumberProject,
                SpecificObligations = study.Element.ObligacionesEspecificas,
                User = study.Contractor.User.UserName,
                UserCharge = study.Contractor.User.Professionalposition,
                UserFirm = study.Contractor.User.UserFile.Where(w => w.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.FileData).FirstOrDefault(),
                GeneralObligations = study.Element.ObligacionesGenerales,
                SupervisorItmName = study.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.UserName).FirstOrDefault(),
                SupervisorFirm = _context.AssigmentContract.Where(w => w.ContractId.Equals(study.ContractId) && w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).FirstOrDefault().User.UserFile.Where(w => w.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.FileData).FirstOrDefault(),
                SupervisorCharge = study.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Professionalposition).FirstOrDefault(),
                UserJuridic = _context.DetailFile.Where(w => w.File.ContractId.Equals(study.ContractId) && w.File.ContractorId.Equals(study.ContractorId) && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))).OrderByDescending(o => o.RegisterDate).Select(s => s.User.UserName).FirstOrDefault(),
                UserIdentification = study.Contractor.User.Identification,
                UserJuridicFirm = _context.DetailFile
                 .Where(w => w.File.Contract.Id.Equals(study.ContractId) && w.File.ContractorId.Equals(study.ContractorId) && w.UserId.HasValue && (w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description())))
                 .Select(s => s.User.UserFile.Select(s => s.FileData).FirstOrDefault()).FirstOrDefault(),
                ContractInitialDate = study.Contractor.HiringData.Select(s => s.FechaRealDeInicio).FirstOrDefault(),
                ContractFinalDate = study.Contractor.HiringData.Select(s => s.FechaFinalizacionConvenio).FirstOrDefault(),
                TotalValue = study.EconomicdataNavigation.TotalValue,
                UnifiedProfile = study.Element.PerfilRequerido
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<CommitteeRequestDto>> GetCommitteeRequest(ContractContractorsDto contractors)
        {
            var result = _context.DetailContractor
                .Include(i => i.Contract)
                .Include(i => i.Element)
                .Include(i => i.HiringData)
                .Include(i => i.Contractor)
                .Include(ti => ti.ContractorPayments.OrderByDescending(s => s.FromDate))
                .Where(x => contractors.contractors.Contains(x.ContractorId.ToString()) && x.ContractId.Equals(contractors.contractId));

            return await result.Select(study => new CommitteeRequestDto
            {
                ContractorId = study.ContractorId.ToString(),
                ContractorName = study.Contractor.Nombre + " " + study.Contractor.Apellido,
                ContractorIdentification = study.Contractor.Identificacion,
                ContractNumber = study.Contract.NumberProject,
                ElementName = study.Element.NombreElemento,
                ElementObject = study.Element.ObjetoElemento,
                User = study.Contractor.User.UserName,
                UserFirm = study.Contractor.User.UserFile.Where(w => w.UserFileType.Equals(TypeUserFileEnum.FIRMA.Description())).Select( s => s.FileData).FirstOrDefault(),
                UserIdentification = study.Contractor.User.Identification,
                ContractInitialDate = study.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                ContractFinalDate = study.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                TotalValue = study.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault(),
                ProfileRequire = study.Element.PerfilRequerido
            })
            .AsNoTracking()
            .ToListAsync();
        }
        #endregion

        #region PRIVATE METHODS

        //private async Task<string?> GetJuridicFirm(Guid contarctId, Guid contractorId)
        //{
           
        //    var getUser = _context.UserT
        //         .Include(i => i.Roll)
        //         .Where(w => w.Roll.Code.Equals(RollEnum.Juridico.Description())).ToList();
        //}
        #endregion
    }
}
