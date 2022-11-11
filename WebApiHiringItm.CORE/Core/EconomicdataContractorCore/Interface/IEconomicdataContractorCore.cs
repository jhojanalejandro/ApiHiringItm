using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.EconomicdataContractorCore.Interface
{
    public interface IEconomicdataContractorCore
    {
        Task<List<EconomicdataContractorDto>> GetAll();
        Task<EconomicdataContractorDto> GetById(int id);
        Task<int> Create(EconomicdataContractorDto model);
        Task<bool> Delete(int id);
    }
}
