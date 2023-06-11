using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface;
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


        public async Task<List<FolderDto>> GetAllById(Guid contractorId, Guid contractId)
        {
            var result = _context.Folder.Where(x => x.ContractorId.Equals(contractorId) && x.ContractId == contractId).ToList();
            var map = _mapper.Map<List<FolderDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FolderDto> GetById(string id)
        {
            var result = _context.Folder.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
            var map = _mapper.Map<FolderDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var resultData = _context.Folder.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.Folder.Remove(resultData);
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

        public async Task<bool> Create(FolderDto model)
        {
            var getData = _context.Folder.Where(x => x.Id == model.Id).FirstOrDefault();

            if (getData == null)
            {
                var map = _mapper.Map<Folder>(model);
                map.Id = Guid.NewGuid();
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
