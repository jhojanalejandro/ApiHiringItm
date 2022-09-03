using AutoMapper;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core
{
    public class FeasibilityRequestCore: IFeasibilityRequestCore
    {
        private readonly hiring_V1Context _context;
        private readonly IMapper _mapper;


        public FeasibilityRequestCore(hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<FeasibilityRequestDto>> GetAll()
        {
            var result = _context.FeasibilityRequest.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<FeasibilityRequestDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FeasibilityRequestDto> GetById(int id)
        {
            var result = _context.FeasibilityRequest.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<FeasibilityRequestDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Update(FeasibilityRequestDto model)
        {
            try
            {
                if (model.Id != 0)

                {
                    var map = _mapper.Map<FeasibilityRequest>(model);
                    await _context.BulkInsertAsync(_context.FeasibilityRequest,options => options.InsertKeepIdentity = true);
                    var res = _context.BulkSaveChangesAsync(bulk => bulk.BatchSize = 100);
                    if (res.IsCompleted)
                    {
                        return true;
                    }

                }

            }
            catch (Exception e)
            {

                new Exception("Error", e);
            }
            return false;
        }


        public async Task<bool> Updates(string model)
        {
            try
            {
                if (model != null)

                {
                    var map = _mapper.Map<FeasibilityRequest>(model);
                    await _context.BulkInsertAsync(_context.FeasibilityRequest, options => options.InsertKeepIdentity = true);
                    var res = _context.BulkSaveChangesAsync(bulk => bulk.BatchSize = 100);
                    if (res.IsCompleted)
                    {
                        return true;
                    }

                }

            }
            catch (Exception e)
            {

                new Exception("Error", e);
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var FeasibilityRequest = _context.FeasibilityRequest.Where(x => x.Id == id).FirstOrDefault();
            if (FeasibilityRequest != null)
            {

                var result = _context.FeasibilityRequest.Remove(FeasibilityRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> Create(FeasibilityRequestDto model)
        {
            var FeasibilityRequestGet = _context.FeasibilityRequest.Where(x => x.Id == model.Id).FirstOrDefault();
            if (FeasibilityRequestGet == null)
            {
                var map = _mapper.Map<FeasibilityRequest>(model);
                var res = _context.FeasibilityRequest.Add(map);
                await _context.SaveChangesAsync();
                return map.Id != 0 ? map.Id : 0;
            }
            else
            {
                var FeasibilityRequestupdate = _context.FeasibilityRequest.Where(x => x.Id == model.Id).FirstOrDefault();
                var map = _mapper.Map(model, FeasibilityRequestupdate);
                var res = _context.FeasibilityRequest.Update(map);
                await _context.SaveChangesAsync();
                if (res.State != 0)
                {
                    return 0;
                }
            }
            return 0;
        }




    }
}
