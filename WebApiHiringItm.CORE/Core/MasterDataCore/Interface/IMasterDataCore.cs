using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.MasterDataDto;

namespace WebApiHiringItm.CORE.Core.MasterDataCore.Interface
{
    public interface IMasterDataCore
    {
        Task<List<FilesDto>> GetAllMinutesType();
        Task<List<TypeFileDto>> GetAllFileType();
        Task<List<ElementTypeDto>> GetAllElementType();
        Task<List<CpcTypeDto>> GetAllCpcType();
        Task<List<StatusContractDto>> GetSatatusContract();
    }
}
