using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.FolderType;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.FoldersContractorCore
{
    public class FolderCore : IFolderContractorCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;

        public FolderCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<FolderDto>> GetAllFolderById(Guid contractorId, Guid contractId)
        {
            var getContractor = _context.Contractor.FirstOrDefault(f => f.Id.Equals(contractorId));
            return _context.Folder
                .Include(i => i.FolderTypeNavigation)
                .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId == contractId)
                .Select(f => new FolderDto
                {
                    Id = f.Id,
                    //UserId = f.UserId.HasValue ? f.UserId.Value : null,
                    FolderName = f.FolderName + " " + f.Consutive,
                    RegisterDate = f.RegisterDate,
                    ModifyDate = f.ModifyDate,
                    FolderDescription = f.FolderTypeNavigation.FolderTypeDescription,
                    Consutive = f.Consutive,
                    DescriptionProject = f.DescriptionProject,
                    ContractorName = getContractor.Nombres +" "+ getContractor.Apellidos
                })
                .AsNoTracking()
                .ToList();
            
        }

        public async Task<FolderDto> GetById(string id)
        {
            var result = _context.Folder.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
            var map = _mapper.Map<FolderDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<IGenericResponse<string>> Delete(string folderId)
        {
            try
            {
                var resultData = _context.Folder.Where(x => x.Id.Equals(Guid.Parse(folderId))).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.Folder.Remove(resultData);
                    await _context.SaveChangesAsync();
                }
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
            }
        }

        public async Task<bool> SaveFolderContract(FolderDto model)
        {
            var getData = _context.Folder.Where(x => x.Id == model.Id).FirstOrDefault();
            model.FolderType = _context.FolderType.Where(w => w.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description())).Select(s => s.Id).FirstOrDefault();
            if (getData == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<Folder>(model);
                _context.Folder.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.Folder.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
        }

    }
}
