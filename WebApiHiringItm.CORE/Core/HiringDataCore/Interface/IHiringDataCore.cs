using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.CORE.Core.HiringDataCore.Interface
{
    public interface IHiringDataCore
    {
        Task<List<HiringDataDto>> GetAll();
        Task<HiringDataDto> GetById(int id);
        Task<bool> Delete(int id);
        Task<int> Create(HiringDataDto model);

    }
}
