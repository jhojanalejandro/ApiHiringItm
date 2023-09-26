﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.PdfDto;

namespace WebApiHiringItm.CORE.Core.Contractors.Interface
{
    public interface IContractorPaymentsCore
    {
        Task<List<ContractorPaymentsDto>> GetAll();
        Task<ContractorPaymentsDto> GetById(string id);
        Task<IGenericResponse<List<ContractorPaymentsDto>>> GetPaymentsContractorList(string contractId, string contractorId);
        Task<IGenericResponse<string>> SaveContractorPayment(List<ContractorPaymentsDto> modelContractorPayments);
        Task<IGenericResponse<string>> DeleteContractorPayment(string idPayment);
        Task<IGenericResponse<ChargeAccountDto>> GetChargeAccount(string contractId, string contractorId);
        Task<IGenericResponse<EntityHealthResponseDto>> GetEmptityHealthContractor(string contractorId);


    }
}
