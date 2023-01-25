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
    public class FolderContractorCore : IFolderContractorCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;

        public FolderContractorCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<FolderContractorDto>> GetAllById(Guid contractorId, Guid contractId)
        {
            var result = _context.FolderContractor.Where(x => x.ContractorId.Equals(contractorId) && x.ContractId == contractId).ToList();
            var map = _mapper.Map<List<FolderContractorDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FolderContractorDto> GetById(int id)
        {
            var result = _context.FolderContractor.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<FolderContractorDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.FolderContractor.Where(x => x.Id == id).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.FolderContractor.Remove(resultData);
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

        public async Task<bool> Create(FolderContractorDto model)
        {
            var getData = _context.FolderContractor.Where(x => x.Id == model.Id).FirstOrDefault();

            if (getData == null)
            {
                var map = _mapper.Map<FolderContractor>(model);
                _context.FolderContractor.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.FolderContractor.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;

        }

    }
}
