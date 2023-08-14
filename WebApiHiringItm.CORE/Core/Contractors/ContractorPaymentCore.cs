using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.Contractors
{
    public class ContractorPaymentCore : IContractorPaymentsCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;


        public ContractorPaymentCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            for (var i = 0; i < modelContractorPayments.Count; i++)
            {
                var getData = _context.ContractorPayments.Where(x => x.FromDate == modelContractorPayments[i].FromDate && x.ToDate == modelContractorPayments[i].ToDate && x.DetailContractorNavigation.ContractorId.Equals(Guid.Parse(modelContractorPayments[i].ContractorId))).FirstOrDefault();
                var getDetail = getDetailContractor.OrderByDescending(o => o.Consecutive).FirstOrDefault(f => f.ContractorId.Equals(Guid.Parse(modelContractorPayments[i].ContractorId)));
                if (getData != null)
                {
                    modelContractorPayments[i].DetailContractor = getData.DetailContractor;
                    var mapData = _mapper.Map(modelContractorPayments[i], getData);
                    paymentListUpdate.Add(getData);
                }
                else
                {
                    var mapContractorPayment = _mapper.Map<ContractorPayments>(modelContractorPayments[i]);
                    mapContractorPayment.DetailContractor = getDetail.Id;
                    mapContractorPayment.Id = Guid.NewGuid();
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
                return ApiResponseHelper.CreateResponse<string>(null,true,Resource.UPDATESUCCESSFULL);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.REGISTERSUCCESSFULL);
            }
        }


        public async Task<IGenericResponse<List<ContractorPaymentsDto>>> GetPaymentsContractorList(string contractId, string contractorId)
        {
            if (string.IsNullOrEmpty(contractId) || !contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorPaymentsDto>>(Resource.GUIDNOTVALID);

            if (string.IsNullOrEmpty(contractorId) || !contractorId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<List<ContractorPaymentsDto>>(Resource.GUIDNOTVALID);

            var result = _context.ContractorPayments
                        .Where(p => p.DetailContractorNavigation.ContractorId.Equals(Guid.Parse(contractorId)) && p.DetailContractorNavigation.ContractId.Equals(Guid.Parse(contractId))).ToList();
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
    }
}
