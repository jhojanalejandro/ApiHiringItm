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

        public async Task<List<ExecutionReportDto>> GetExecutionReport(Guid contractId, Guid contractorId)
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
                    ContractorIdentification = report.Contractor.Identificacion,
                    ContractNumber = report.Contract.NumberProject,
                    SupervisorContract = report.HiringData.SupervisorItm,
                    SupervisorIdentification = report.HiringData.IdentificacionSupervisor,
                    PeriodExecuted = "del"+ "  " + report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault()+"  " + "al"+"  " + report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
                    SpecificObligations = report.Element.ObligacionesEspecificas,
                    ElementObject = report.Element.ObjetoElemento,
                    TotalValue = report.Contractor.EconomicdataContractor.Select(s => s.TotalValue.ToString()).FirstOrDefault(),
                    TotalValuePeriod = report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate.ToString()).Select(s => s.Paymentcant.ToString()).FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<ChargeAccountDto>> GetChargeAccount(Guid contractId, Guid contractorId)
        {
            return null;
            //var result = _context.DetailProjectContractor
            //    .Include(i => i.Contract)
            //    .Include(i => i.Element)
            //    .Include(i => i.HiringData)
            //    .Include(i => i.Contractor)
            //        .ThenInclude(ti => ti.ContractorPayments)
            //    .Include(i => i.Contractor)
            //        .ThenInclude(ti => ti.EconomicdataContractor)
            //    .Where(x => contractorList.Contreactor.Contains(x.Contractor.Id) && x.ContractId.Equals(contractorList.ContractId));

            //return await result.Select(report => new ExecutionReportDto
            //{
            //    ContractorName = report.Contractor.Nombre + " " + report.Contractor.Apellido,
            //    ContractorIdentification = report.Contractor.Identificacion,
            //    ContractNumber = report.Contract.NumberProject,
            //    SupervisorContract = report.HiringData.SupervisorItm,
            //    SupervisorIdentification = report.HiringData.IdentificacionSupervisor,
            //    PeriodExecuted = "del" + report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault() + " " + "al" + " " + report.Contractor.ContractorPayments.OrderByDescending(d => d.FromDate).Select(s => s.FromDate.ToString()).FirstOrDefault(),
            //    SpecificObligations = report.Element.ObligacionesEspecificas,
            //    ElementObject = report.Element.ObjetoElemento,
            //    TotalValue = report.Contractor.EconomicdataContractor.Where(w => w.ContractId.Equals(contractorList.ContractId)).Select(s => s.TotalValue.ToString()).FirstOrDefault(),
            //    TotalValuePeriod = report.Contractor.ContractorPayments.Where(w => w.ContractId.Equals(contractorList.ContractId)).OrderByDescending(d => d.Paymentcant.ToString()).Select(s => s.FromDate.ToString()).FirstOrDefault()
            //})
            //.AsNoTracking()
            //.ToListAsync();
        }
    }
}
