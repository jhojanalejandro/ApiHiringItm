using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.EconomicdataContractorCore.Interface
{
    public interface IEconomicdataContractorCore
    {
        Task<List<EconomicdataContractorDto>> GetAll();
        Task<List<EconomicdataContractorDto>> GetEconiomicDataById(EconomicDataRequest economicData);
        Task<bool> Delete(string id);
        Task<IGenericResponse<string>> AddEconomicData(List<EconomicdataContractorDto> model);
        Task<ContractorPaymentsDto> GetPaymentByIdContractAndContractor(string contractId, string contractorId);
    }
}
