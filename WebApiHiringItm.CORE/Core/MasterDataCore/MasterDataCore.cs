using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.MasterDataCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.MasterDataDto;
using WebApiHiringItm.MODEL.Dto.Security;
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

        public async Task<List<DocumentTypeDto>> GetDocumentType()
        {
            var result = _context.DocumentType.ToList();
            var map = _mapper.Map<List<DocumentTypeDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<DocumentTypeDto>> GetAllFileType()
        {
            var result = _context.Files.ToList();
            var map = _mapper.Map<List<DocumentTypeDto>>(result);
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
        }

        public async Task<List<StatusFileDto>> GetStatusFile()
        {
            var result = _context.StatusFile.OrderBy(o => o.ConsecutiveStatus).ToList();
            var map = _mapper.Map<List<StatusFileDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<MinuteTypeDto>> GetAllMinuteType()
        {
            var result = _context.MinuteType.ToList();
            var map = _mapper.Map<List<MinuteTypeDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<BanksDto>> GetBanks()
        {
            var result = _context.Banks.ToList();
            var map = _mapper.Map<List<BanksDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<RubroTypeDto>> GetAllRubroType()
        {
            var result = _context.RubroType.ToList();
            var map = _mapper.Map<List<RubroTypeDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<TermTypeDto>> GetAllTermType()
        {
            var result = _context.TermType.ToList();
            var map = _mapper.Map<List<TermTypeDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<List<AssignmentTypeDto>> GetAllAssignmentType()
        {
            var result = _context.AssignmentType.ToList();
            var map = _mapper.Map<List<AssignmentTypeDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<List<DetailTypeDto>> GetAllDetailType()
        {
            var result = _context.DetailType.ToList();
            var map = _mapper.Map<List<DetailTypeDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<List<NewnessTypeDto>> GetNewnessType()
        {
            var result = await _context.NewnessType.ToListAsync();
            var map = _mapper.Map<List<NewnessTypeDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<EntityHealthDto>> GetEmptityHealth()
        {
            var result = await _context.EntityHealth.ToListAsync();
            var map = _mapper.Map<List<EntityHealthDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<IGenericResponse<string>> SaveBank(BanksDto banksDtoModel)
        {
            var getBankc = _context.Banks.Where(w => w.BankName.Equals(banksDtoModel.BankName)).FirstOrDefault();
            if (getBankc == null)
            {
                var mapBank = _mapper.Map<Banks>(banksDtoModel);
                mapBank.Id = Guid.NewGuid();
                _context.Banks.Add(mapBank);
                await _context.SaveChangesAsync();
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.BANKEXIST);

            }
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
        }

        public async Task<IGenericResponse<string>> SaveCpcType(CpcTypeDto CpcTypeDto)
        {
            var getCpcType = _context.CpcType.Where(w => w.CpcNumber.Equals(CpcTypeDto.CpcNumber)).FirstOrDefault();
            if (getCpcType == null)
            {
                var mapCpcType = _mapper.Map<CpcType>(CpcTypeDto);
                mapCpcType.Id = Guid.NewGuid();
                _context.CpcType.Add(mapCpcType);
                await _context.SaveChangesAsync();
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.BANKEXIST);

            }
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
        }


        public async Task<IGenericResponse<string>> SaveRubro(RubroTypeDto rubroType)
        {
            var getCpcType = _context.RubroType.Where(w => w.RubroNumber.Equals(rubroType.RubroNumber)).FirstOrDefault();
            if (getCpcType == null)
            {
                var mapRubro = _mapper.Map<RubroType>(rubroType);
                mapRubro.Id = Guid.NewGuid();
                _context.RubroType.Add(mapRubro);
                await _context.SaveChangesAsync();
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.BANKEXIST);

            }
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
        }
    }
}
