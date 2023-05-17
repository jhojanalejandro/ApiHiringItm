using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.MasterDataCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.Folder;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.MasterDataDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.MasterDataCore
{
    public class MasterDataCore : IMasterDataCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;

        public MasterDataCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FilesDto>> GetAllMinutesType()
        {
            var result = _context.Files.ToList();
            var map = _mapper.Map<List<FilesDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<TypeFileDto>> GetAllFileType()
        {
            var result = _context.Files.ToList();
            var map = _mapper.Map<List<TypeFileDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<CpcTypeDto>> GetAllCpcType()
        {
            var result = _context.CpcType.ToList();
            var map = _mapper.Map<List<CpcTypeDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<ElementTypeDto>> GetAllElementType()
        {
            var result = _context.ElementType.ToList();
            var map = _mapper.Map<List<ElementTypeDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<List<StatusContractDto>> GetSatatusContract()
        {
            var result = _context.StatusContract.ToList();
            var map = _mapper.Map<List<StatusContractDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FilesDto> GetById(string id)
        {
            var result = _context.Files.FirstOrDefault(x => x.Id.Equals(Guid.Parse(id)));
            var resultFile = _context.DetailFile.FirstOrDefault(df => df.FileId.Equals(Guid.Parse(id)));
            var mapDf = _mapper.Map<DetailFileDto>(resultFile);
            var map = _mapper.Map<FilesDto>(result);
            map.DetailFile = mapDf;
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var resultData = _context.Files.FirstOrDefault(x => x.Id.Equals(Guid.Parse(id)));
                if (resultData != null)
                {
                    var result = _context.Files.Remove(resultData);
                    await _context.SaveChangesAsync();

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
            return false;
        }

        public async Task<bool> Create(TypeFileDto model)
        {
            var getData = _context.FileType.FirstOrDefault(x => x.Id.Equals(model.Id));
            if (getData == null)
            {
                var map = _mapper.Map<FileType>(model);
                map.Id = Guid.NewGuid();
                _context.FileType.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.FileType.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
        }
    }
}
