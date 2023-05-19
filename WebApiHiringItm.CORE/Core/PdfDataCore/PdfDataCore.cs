using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore;
using WebApiHiringItm.MODEL.Dto.PdfDto;

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
                
                return await result.Select(report  => new ExecutionReportDto
                {
                    ContractorName = report.Contractor.Nombre+ " "+ report.Contractor.Apellido,
                    ContractInitialDate = report.HiringData.FechaRealDeInicio.ToString(),
                    ContractFinalDate = report.HiringData.FechaFinalizacionConvenio.ToString(),
                    ContractorIdentification = report.Contractor.Identificacion,
                    ContractNumber = report.Contract.NumberProject,
                    SupervisorContract = report.HiringData.SupervisorItm,
                    SupervisorIdentification = report.HiringData.IdentificacionSupervisor,
                    PeriodExecutedInitialDate =  report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
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
                Supervisor =report.HiringData.SupervisorItm,
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
    }
}
