using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Globalization;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.Contrato;
using WebApiHiringItm.MODEL.Dto.PdfDto;
using WebApiHiringItm.MODEL.Dto.Security;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.Contractors
{
    public class ContractorPaymentCore : IContractorPaymentsCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesCore _fileCore;


        public ContractorPaymentCore(HiringContext context, IMapper mapper, IFilesCore filesCore)
        {
            _context = context;
            _mapper = mapper;
            _fileCore = filesCore;
        }

        public async Task<List<ContractorPaymentsDto>> GetAll()
        {
            var result = _context.ContractorPayments.ToList();
            var map = _mapper.Map<List<ContractorPaymentsDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<ContractorPaymentsDto> GetById(string id)
        {
            var result = _context.ContractorPayments.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
            var map = _mapper.Map<ContractorPaymentsDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<IGenericResponse<string>> DeleteContractorPayment(string idPayment)
        {
            var getData = _context.ContractorPayments.Where(x => x.Id.Equals(Guid.Parse(idPayment))).FirstOrDefault();
            if (getData != null)
            {
                var result = _context.ContractorPayments.Remove(getData);
                await _context.SaveChangesAsync();
            }
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.DELETESUCCESS);

        }

        public async Task<IGenericResponse<string>> SaveContractorPayment(List<ContractorPaymentsDto> modelContractorPayments)
        {
            List<ContractorPayments> paymentListAdd = new List<ContractorPayments>();
            List<ContractorPayments> paymentListUpdate = new List<ContractorPayments>();
            var getDetailContractor = _context.DetailContractor.Where(w => w.ContractId.Equals(Guid.Parse(modelContractorPayments[0].ContractId))).ToList();

            string formataDate = "yyyy-MM-dd";
            CultureInfo cultura = new CultureInfo("es-ES"); // Por ejemplo, usando la cultura española

            for (var i = 0; i < modelContractorPayments.Count; i++)
            {
                string FromDate = string.Format("{0:" + formataDate + "}", modelContractorPayments[i].FromDate);
                string ToDate = string.Format("{0:" + formataDate + "}", modelContractorPayments[i].ToDate);

                var getData = _context.ContractorPayments.Where(x => x.FromDate.ToString() == FromDate && x.ToDate.ToString() == ToDate && x.DetailContractorNavigation.ContractorId.Equals(Guid.Parse(modelContractorPayments[i].ContractorId))).FirstOrDefault();
                var getDetail = getDetailContractor.OrderByDescending(o => o.Consecutive).FirstOrDefault(f => f.ContractorId.Equals(Guid.Parse(modelContractorPayments[i].ContractorId)));
                if (getData != null)
                {
                    modelContractorPayments[i].DetailContractor = getData.DetailContractor;
                    modelContractorPayments[i].Consecutive = getData.Consecutive;
                    modelContractorPayments[i].EconomicdataContractor = getData.EconomicdataContractor;
                    var mapData = _mapper.Map(modelContractorPayments[i], getData);
                    paymentListUpdate.Add(getData);
                }
                else
                {
                    var getEconomicdataContractorId = _context.EconomicdataContractor.Where(w => w.DetailContractorId.Equals(getDetail.Id)).Select(s => s.Id).FirstOrDefault();

                    var mapContractorPayment = _mapper.Map<ContractorPayments>(modelContractorPayments[i]);
                    mapContractorPayment.DetailContractor = getDetail.Id;
                    mapContractorPayment.Id = Guid.NewGuid();
                    mapContractorPayment.EconomicdataContractor = getEconomicdataContractorId;
                    paymentListAdd.Add(mapContractorPayment);
                }
            }
            if (paymentListUpdate.Count > 0)
                _context.ContractorPayments.UpdateRange(paymentListUpdate);
            if (paymentListAdd.Count > 0)
                _context.ContractorPayments.AddRange(paymentListAdd);
            await _context.SaveChangesAsync();
            if (paymentListUpdate.Count > 0)
            {
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.UPDATESUCCESSFULL);
            }
            else if (paymentListAdd.Count > 0)
            {
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.PAYMENTERROR);
            }
        }


        public async Task<IGenericResponse<List<ContractorPaymentsDto>>> GetPaymentsContractorList(string contractId, string contractorId)
        {
            if (string.IsNullOrEmpty(contractId) || !contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorPaymentsDto>>(Resource.GUIDNOTVALID);

            if (string.IsNullOrEmpty(contractorId) || !contractorId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorPaymentsDto>>(Resource.GUIDNOTVALID);

            var result = await _context.ContractorPayments
                        .Where(p => p.DetailContractorNavigation.ContractorId.Equals(Guid.Parse(contractorId)) && p.DetailContractorNavigation.ContractId.Equals(Guid.Parse(contractId))).ToListAsync();
            var map = _mapper.Map<List<ContractorPaymentsDto>>(result);
            var operationResult = new GenericResponse<List<ContractorPaymentsDto>>();

            if (map != null && map.Count > 0)
            {
                return ApiResponseHelper.CreateResponse(map);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<List<ContractorPaymentsDto>>(Resource.INFORMATIONEMPTY);
            }
        }


        public async Task<IGenericResponse<EntityHealthResponseDto>> GetEmptityHealthContractor(string contractorId)
        {

            if (string.IsNullOrEmpty(contractorId) || !contractorId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<EntityHealthResponseDto>(Resource.GUIDNOTVALID);

            var result = _context.Contractor
                        .Include(i => i.EpsNavigation)
                        .Include(i => i.ArlNavigation)
                        .Include(i => i.AfpNavigation)
                        .Where(p => p.Id.Equals(Guid.Parse(contractorId)));
            var response = result.Select(ec => new EntityHealthResponseDto
            {
                IdAfp = ec.Afp.ToString(),
                IdEps = ec.Eps.ToString(),
                IdArl = ec.Arl.ToString(),
                Afp = ec.AfpNavigation.EntityHealthDescription,
                CodeAfp = ec.EpsNavigation.Code,
                CodeEps = ec.EpsNavigation.Code,
                CodeArl = ec.ArlNavigation.Code,
                Arl = ec.ArlNavigation.EntityHealthDescription,
                Eps = ec.EpsNavigation.EntityHealthDescription,
            })
             .AsNoTracking()
             .FirstOrDefault();
            var operationResult = new GenericResponse<EntityHealthResponseDto>();

            if (response != null)
            {
                return ApiResponseHelper.CreateResponse(response);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<EntityHealthResponseDto>(Resource.INFORMATIONEMPTY);
            }
        }

        public async Task<IGenericResponse<ChargeAccountDto>> GetChargeAccount(string contractId, string contractorId)
        {
            if (!contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<ChargeAccountDto>(Resource.GUIDNOTVALID);

            if (!contractorId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<ChargeAccountDto>(Resource.GUIDNOTVALID);

            var result = _context.ContractorPayments
                .Where(x => x.DetailContractorNavigation.ContractorId.Equals(Guid.Parse(contractorId)) && x.DetailContractorNavigation.ContractId.Equals(Guid.Parse(contractId))).OrderByDescending(d => d.Consecutive);

            var hiringData = result.Select(report => new ChargeAccountDto
            {
                ChargeAccountNumber = report.DetailContractorNavigation.ContractorPayments.Count(),
                ContractorName = report.DetailContractorNavigation.Contractor.Nombres + " " + report.DetailContractorNavigation.Contractor.Apellidos,
                ContractNumber = report.DetailContractorNavigation.Contract.NumberProject,
                ContractorIdentification = report.DetailContractorNavigation.Contractor.Identificacion,
                ExpeditionIdentification = report.DetailContractorNavigation.Contractor.LugarExpedicion,
                PhoneNumber = report.DetailContractorNavigation.Contractor.Celular,
                AccountType = report.DetailContractorNavigation.Contractor.TipoCuenta,
                AccountNumber = report.DetailContractorNavigation.Contractor.CuentaBancaria,
                BankingEntity = report.DetailContractorNavigation.Contractor.EntidadCuentaBancariaNavigation.BankName,
                ContractName = report.DetailContractorNavigation.Contract.CompanyName,
                Direction = report.DetailContractorNavigation.Contractor.Direccion,
                PeriodExecutedInitialDate = report.FromDate,
                PeriodExecutedFinalDate = report.ToDate,
                ElementName = report.DetailContractorNavigation.Element.NombreElemento,
                TotalValue = Math.Ceiling(report.Paymentcant)
            })
            .AsNoTracking()
            .FirstOrDefault();

            return ApiResponseHelper.CreateResponse(hiringData);
        }

        public async Task<IGenericResponse<string>> SaveContractorSecurity(ContractorPaymentSecurityDto contractorPaymentSecurityModel)
        {
            var getDetailContractor = _context.DetailContractor.Where(x => x.ContractId.Equals(contractorPaymentSecurityModel.ContractorFile.ContractId) && x.ContractorId.Equals(contractorPaymentSecurityModel.ContractorFile.ContractorId)).FirstOrDefault();

            var getData = _context.ContractorPaymentSecurity.Where(x => x.ContractorPayments.Equals(contractorPaymentSecurityModel.ContractorPayments) && x.PaymentPeriodDate.Equals(contractorPaymentSecurityModel.PaymentPeriodDate)).FirstOrDefault();
            if (getData != null)
            {
                var mapContractorPaymentSecurityUpdate = _mapper.Map(contractorPaymentSecurityModel, getData);
                mapContractorPaymentSecurityUpdate.Id = getData.Id;
                mapContractorPaymentSecurityUpdate.DetailContractor = getData.DetailContractor;
                _context.ContractorPaymentSecurity.Update(mapContractorPaymentSecurityUpdate);

            }
            else
            {
                var mapContractorPaymentSecurity = _mapper.Map<ContractorPaymentSecurity>(contractorPaymentSecurityModel);
                mapContractorPaymentSecurity.Id = Guid.NewGuid();
                mapContractorPaymentSecurity.DetailContractor = getDetailContractor.Id;
                _context.ContractorPaymentSecurity.Add(mapContractorPaymentSecurity);

            }
            if (contractorPaymentSecurityModel.ContractorFile != null)
            {
                await _fileCore.AddFilePayrollContractor(contractorPaymentSecurityModel.ContractorFile, contractorPaymentSecurityModel.Consecutive);
            }
            await _context.SaveChangesAsync();
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);

        }

        public async Task<IGenericResponse<List<PosContractualDto>>> GetContractorSecurity(string contractId)
        {
            if (!contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<PosContractualDto>>(Resource.GUIDNOTVALID);

            var monthPayment = DateTime.Now.Month - 1;
          
            var getDetailcContractor = _context.DetailContractor
            .Where(w => w.ContractId.Equals(Guid.Parse(contractId)));

            getDetailcContractor = getDetailcContractor.Where(w => w.StatusContractorNavigation.Code.Equals(StatusContractorEnum.CONTRATADO.Description()));

            var resultByContract = await getDetailcContractor.Select(ct => new PosContractualDto
            {
                Id = ct.Contractor.Id,
                Nombre = ct.Contractor.Nombres + " " + ct.Contractor.Apellidos,
                Identificacion = ct.Contractor.Identificacion,
                FechaNacimiento = ct.Contractor.FechaNacimiento,
                Telefono = ct.Contractor.Telefono,
                Celular = ct.Contractor.Celular,
                Correo = ct.Contractor.Correo,
                Direccion = ct.Contractor.Direccion,
                ElementId = ct.ElementId.ToString().ToLower(),
                ComponentId = ct.ComponentId.ToString().ToLower(),
                ActivityId = ct.ActivityId.ToString().ToLower(),
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
                RegisterDate = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id)).Select(s => s.RegisterDate).FirstOrDefault()  :  null,
                PaymentsCant = ct.ContractorPayments.Select(s => s.Consecutive).FirstOrDefault(),
                StatusContractor = ct.StatusContractorNavigation.StatusContractorDescription,
                PeriodPaymented = ct.ContractorPayments.Select(s => s).FirstOrDefault() != null ? ct.ContractorPayments.OrderByDescending(o => o.Consecutive).Select(s => s.FromDate.ToString("yyyy-MM-dd")).FirstOrDefault() + "  "+ ct.ContractorPayments.Select(s => s.FromDate.ToString("yyyy-MM-dd")).FirstOrDefault() : "NO REGISTRADO",
                PaymentCant = ct.ContractorPayments.Select(s => s).FirstOrDefault() != null ? ct.ContractorPayments.OrderByDescending(o => o.Consecutive).Select(s => s.Paymentcant.ToString("N0")).FirstOrDefault() : "0",
                Debt = ct.EconomicdataContractor.OrderByDescending(o => o.Consecutive).Select(s => s.Debt.ToString("N0")).FirstOrDefault(),
            })
              .AsNoTracking()
              .ToListAsync();

            return ApiResponseHelper.CreateResponse(resultByContract);
        }

        public async Task<IGenericResponse<List<ContractorNominaDto>>?> GetContractorNomina(string contractId)
        {
            try
            {
                if (!contractId.IsGuid())
                    return ApiResponseHelper.CreateErrorResponse<List<ContractorNominaDto>>(Resource.GUIDNOTVALID);

                var monthPayment = DateTime.Now.Month - 1;

                var getDetailcContractor = _context.DetailContractor
                .Where(w => w.ContractId.Equals(Guid.Parse(contractId)));

                getDetailcContractor = getDetailcContractor.Where(w => w.StatusContractorNavigation.Code.Equals(StatusContractorEnum.CONTRATADO.Description()));

                var resultByContract = await getDetailcContractor.Select(ct => new ContractorNominaDto
                {
                    Id = ct.Contractor.Id,
                    Nombre = ct.Contractor.Nombres + " " + ct.Contractor.Apellidos,
                    Identificacion = ct.Contractor.Identificacion,
                    FechaNacimiento = ct.Contractor.FechaNacimiento,
                    Telefono = ct.Contractor.Telefono,
                    Celular = ct.Contractor.Celular,
                    Correo = ct.Contractor.Correo,
                    Direccion = ct.Contractor.Direccion,
                    ElementId = ct.ElementId.ToString()!.ToLower(),
                    ComponentId = ct.ComponentId.ToString()!.ToLower(),
                    ActivityId = ct.ActivityId.ToString()!.ToLower(),
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
                    Eps = ct.Contractor.Eps.ToString()!.ToLower(),
                    Afp = ct.Contractor.Afp.ToString()!.ToLower(),
                    Arl = ct.Contractor.Arl.ToString()!.ToLower(),
                    PaymentPension = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.PaymentPension).FirstOrDefault() : 0,
                    PaymentArl = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.PaymentArl).FirstOrDefault() : 0,
                    PaymentEps = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.PaymentEps).FirstOrDefault() : 0,
                    RegisterDate = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id)).Select(s => s.RegisterDate).FirstOrDefault() : null,
                    PayrollNumber = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id)).OrderByDescending(o => o.Consecutive).Select(s => s.PayrollNumber).FirstOrDefault() : "NO REGISTRADO",
                    PaymentPeriodDate = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id) && w.PaymentPeriodDate.Month == monthPayment).Select(s => s.PaymentPeriodDate).FirstOrDefault() : null,
                    CorrectArlPayment = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id) && w.PaymentPeriodDate.Month == monthPayment).OrderByDescending(o => o.Consecutive).Select(s => s.CorrectArlPayment).FirstOrDefault() == true ? "PAGO CORRECTO" : "PAGO NO CORRECTO" : "PAGO NO REGISTRADO",
                    CorrectAfpPayment = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id) && w.PaymentPeriodDate.Month == monthPayment).OrderByDescending(o => o.Consecutive).Select(s => s.CorrectAfpPayment).FirstOrDefault() == true ? "PAGO CORRECTO" : "PAGO NO CORRECTO" : "PAGO NO REGISTRADO",
                    CorrectEpsPayment = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id) && w.PaymentPeriodDate.Month == monthPayment).OrderByDescending(o => o.Consecutive).Select(s => s.CorrectEpsPayment).FirstOrDefault() == true ? "PAGO CORRECTO" : "PAGO NO CORRECTO" : "PAGO NO REGISTRADO",
                    CorrectSheet = ct.ContractorPaymentSecurity.Select(s => s).FirstOrDefault() != null ? ct.ContractorPaymentSecurity.Where(w => w.DetailContractor.Equals(ct.Id) && w.PaymentPeriodDate.Month == monthPayment).OrderByDescending(o => o.Consecutive).Select(s => s.CorrectSheet).FirstOrDefault() == true ? "PAGO CORRECTO" : "PAGO NO CORRECTO" : "PAGO NO REGISTRADO",

                })
                  .AsNoTracking()
                  .ToListAsync();

                return ApiResponseHelper.CreateResponse(resultByContract);
            }
            catch(Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<List<ContractorNominaDto>>(ex.Message);
            }
           
        }

        public async Task<IGenericResponse<List<ContractorPaymentListDto>>> GetPaymentsContractors(string contractorId)
        {

            if (!contractorId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorPaymentListDto>>(Resource.GUIDNOTVALID);

            var result = _context.ContractorPayments
                .Where(x => x.DetailContractorNavigation.ContractorId.Equals(Guid.Parse(contractorId))).OrderByDescending(d => d.Consecutive);

            var hiringData = result.Select(report => new ContractorPaymentListDto
            {
                FromDate = report.FromDate,
                ToDate = report.ToDate,
                DescriptionPayment = report.DescriptionPayment,
                Paymentcant = report.Paymentcant.ToString("N0"),
                Consecutive = report.Consecutive,
                ProjectName = report.DetailContractorNavigation.Contract.ProjectName
            })
            .AsNoTracking()
            .ToList();

            return ApiResponseHelper.CreateResponse(hiringData);
        }
    }
}
