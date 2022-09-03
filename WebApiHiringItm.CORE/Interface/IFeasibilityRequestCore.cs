using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Interface
{
    public interface IFeasibilityRequestCore
    {
        Task<List<FeasibilityRequestDto>> GetAll();
        Task<FeasibilityRequestDto> GetById(int id);
        Task<bool> Update(FeasibilityRequestDto model);
        Task<bool> Delete(int id);
        Task<int> Create(FeasibilityRequestDto model);

    }
}
