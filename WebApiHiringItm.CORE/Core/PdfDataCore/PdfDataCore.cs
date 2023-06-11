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
            var result = _context.DetailProjectContractor
                .Include(i => i.Contract)
                .Include(i => i.Element)
                .Include(i => i.HiringData)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.ContractorPayments)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.EconomicdataContractor)
                .Where(x => x.Contractor.Id.Equals(contractorId) && x.ContractId.Equals(contractId));

            return await result.Select(report => new ExecutionReportDto
            {
                ContractorName = report.Contractor.Nombre + " " + report.Contractor.Apellido,
                ContractInitialDate = report.HiringData.FechaRealDeInicio.ToString(),
                ContractFinalDate = report.HiringData.FechaFinalizacionConvenio.ToString(),
                ContractorIdentification = report.Contractor.Identificacion,
                ContractNumber = report.Contract.NumberProject,
                SupervisorContract = report.HiringData.SupervisorItm,
                SupervisorIdentification = report.HiringData.IdentificacionSupervisor,
                PeriodExecutedInitialDate = report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                PeriodExecutedFinalDate = report.Contractor.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                SpecificObligations = report.Element.ObligacionesEspecificas,
                ElementObject = report.Element.ObjetoElemento,
                TotalValue = report.Contractor.EconomicdataContractor.Select(s => s.TotalValue.ToString()).FirstOrDefault(),
                TotalValuePeriod = report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<ChargeAccountDto?> GetChargeAccount(Guid contractId, Guid contractorId)
        {
            var result = _context.DetailProjectContractor
                .Include(i => i.Contract)
                .Include(i => i.Element)
                .Include(i => i.HiringData)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.ContractorPayments.OrderByDescending(s => s.FromDate))
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.EconomicdataContractor)
                .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId.Equals(contractId));

            return await result.Select(report => new ChargeAccountDto
            {
                ChargeAccountNumber = report.Contractor.ContractorPayments.Count().ToString(),
                ContractorName = report.Contractor.Nombre + " " + report.Contractor.Apellido,
                ContractNumber = report.Contract.NumberProject,
                ContractorIdentification = report.Contractor.Identificacion,
                ExpeditionIdentification = report.Contractor.LugarExpedicion,
                PhoneNumber = report.Contractor.Celular,
                TypeAccount = report.Contractor.TipoCuenta,
                AccountNumber = report.Contractor.CuentaBancaria,
                BankingEntity = report.Contractor.EntidadCuentaBancaria,
                ContractName = report.Contract.CompanyName,
                Direction = report.Contractor.Direccion,
                PeriodExecutedInitialDate = report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                PeriodExecutedFinalDate = report.Contractor.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                elementName = report.Element.NombreElemento,
                TotalValue = report.Contractor.ContractorPayments.Where(w => w.ContractId.Equals(contractId)).OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public async Task<MinuteExtensionDto?> GetminuteExtension(Guid contractId, Guid contractorId)
        {
            var result = _context.DetailProjectContractor
                .Include(i => i.Contract)
                .Include(i => i.Element)
                .Include(i => i.HiringData)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.ContractorPayments.OrderByDescending(s => s.FromDate))
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.EconomicdataContractor)
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
                TotalValueContract = report.Contractor.EconomicdataContractor.Select(s => s.TotalValue).FirstOrDefault(),
                Supervisor = report.HiringData.SupervisorItm,
                SupervisorCharge = report.HiringData.CargoSupervisorItm,
                SupervisorIdentification = report.HiringData.IdentificacionSupervisor
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
                var contractor = _context.DetailProjectContractor.Where(x => x.ContractId == contractors.contractId)
                .Include(dt => dt.Contractor)
                    .ThenInclude(i => i.EconomicdataContractor)
                .Include(hd => hd.HiringData)
                .Include(el => el.Element)
                .Include(el => el.Contract)
                .Where(w => contractors.contractors.Contains(w.Contractor.Id.ToString()) && !w.Contractor.StatusContractor.Equals(StatusContractorEnum.INHABILITADO.Description()));

                return await contractor.Select(ct => new MinutaDto
                {
                    ContractorId = ct.ContractorId,
                    FechaFinalizacionConvenio = ct.HiringData.FechaFinalizacionConvenio,
                    Contrato = ct.HiringData.Contrato,
                    Compromiso = ct.HiringData.Compromiso,
                    SupervisorItm = ct.HiringData.SupervisorItm,
                    CargoSupervisorItm = ct.HiringData.CargoSupervisorItm,
                    IdentificacionSupervisor = ct.HiringData.IdentificacionSupervisor,
                    FechaRealDeInicio = ct.HiringData.FechaRealDeInicio,
                    FechaDeComite = ct.HiringData.FechaDeComite,
                    Rubro = ct.Contract.Rubro,
                    NombreRubro = ct.Contract.NombreRubro,
                    FuenteRubro = ct.Contract.FuenteRubro,
                    NumeroActa = ct.HiringData.NumeroActa,
                    NombreElemento = ct.Element.NombreElemento,
                    ObligacionesGenerales = ct.Element.ObligacionesGenerales,
                    ObligacionesEspecificas = ct.Element.ObligacionesEspecificas,
                    ValorUnidad = ct.Contractor.EconomicdataContractor.Select(s => s.UnitValue).FirstOrDefault(),
                    ValorTotal = ct.Contractor.EconomicdataContractor.Select(s => s.TotalValue).FirstOrDefault(),
                    Cpc = ct.Element.Cpc.CpcNumber,
                    NombreCpc = ct.Element.Cpc.CpcName,
                    ObjetoElemento = ct.Element.ObjetoElemento,
                    TipoContratacion = ct.Contractor.TipoContratacion,
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
            var result = _context.DetailProjectContractor
                .Include(i => i.Contract)
                .Include(i => i.Element)
                .Include(i => i.HiringData)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.ContractorPayments.OrderByDescending(s => s.FromDate))
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.EconomicdataContractor)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.User)
                        .ThenInclude(i => i.UserFirm)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.Files)
                        .ThenInclude(i => i.User)
                .Where(x => contractors.contractors.Contains(x.ContractorId.ToString()) && x.ContractId.Equals(contractors.contractId));

            return await result.Select(study => new PreviusStudyDto
            {
                ContractorId = study.ContractorId.ToString(),
                ContractorName = study.Contractor.Nombre + " " + study.Contractor.Apellido,
                ContractorIdentification = study.Contractor.Identificacion,
                ContractNumber = study.Contract.NumberProject,
                SpecificObligations = study.Element.ObligacionesEspecificas,
                User = study.Contractor.User.UserName,
                UserCharge = study.Contractor.User.Professionalposition,
                UserFirm = study.Contractor.User.UserFirm.Select(s => s.FirmData).FirstOrDefault(),
                GeneralObligations = study.Element.ObligacionesGenerales,
                SupervisorItmName = study.HiringData.SupervisorItm,
                SupervisorFirm = GetFirmSuperVisor(study.HiringData.IdentificacionSupervisor),
                UserJuridic = study.Contractor.Files.Select(s => s.User.UserName).FirstOrDefault(),
                UserIdentification = study.Contractor.User.Identification,
                UserJuridicFirm = GetJuridicFirm(contractors.contractId, study.ContractorId.Value),
                ContractInitialDate = study.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                ContractFinalDate = study.Contractor.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                TotalValue = study.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<CommitteeRequestDto>> GetCommitteeRequest(ContractContractorsDto contractors)
        {
            var result = _context.DetailProjectContractor
                .Include(i => i.Contract)
                .Include(i => i.Element)
                .Include(i => i.HiringData)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.ContractorPayments.OrderByDescending(s => s.FromDate))
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.EconomicdataContractor)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.User)
                        .ThenInclude(i => i.UserFirm)
                .Include(i => i.Contractor)
                    .ThenInclude(ti => ti.Files)
                        .ThenInclude(i => i.User)
                .Where(x => contractors.contractors.Contains(x.ContractorId.ToString()) && x.ContractId.Equals(contractors.contractId));

            return await result.Select(study => new CommitteeRequestDto
            {
                ContractorId = study.ContractorId.ToString(),
                ContractorName = study.Contractor.Nombre + " " + study.Contractor.Apellido,
                ContractorIdentification = study.Contractor.Identificacion,
                ContractNumber = study.Contract.NumberProject,
                User = study.Contractor.User.UserName,
                UserFirm = study.Contractor.User.UserFirm.Select(s => s.FirmData).FirstOrDefault(),
                UserIdentification = study.Contractor.User.Identification,
                ContractInitialDate = study.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                ContractFinalDate = study.Contractor.ContractorPayments.OrderByDescending(d => d.ToDate).Select(s => s.ToDate.ToString()).FirstOrDefault(),
                TotalValue = study.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
            })
            .AsNoTracking()
            .ToListAsync();
        }
        #endregion

        #region PRIVATE METHODS
        private string? GetFirmSuperVisor(string identification)
        {
            return _context.UserFirm
                 .Include(i => i.User)
                 .Where(w => w.User.Identification.Equals(identification))
                 .Select(s => s.FirmData)
                 .AsNoTracking()
                 .FirstOrDefault();
        }

        private string? GetJuridicFirm(Guid contarctId, Guid contractorId)
        {
            return  _context.Files
                 .Include(i => i.User)
                    .ThenInclude(i => i.UserFirm)
                 .Where(w => w.Contract.Equals(contarctId) && w.ContractorId.Equals(contractorId))
                 .Select(s => s.User.UserFirm.Select(s => s.FirmData).FirstOrDefault())
                 .AsNoTracking()
                 .FirstOrDefault();
        }
        #endregion
    }
}
