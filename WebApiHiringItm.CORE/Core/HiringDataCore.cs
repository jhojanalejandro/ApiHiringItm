using AutoMapper;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core
{
    public class HiringDataCore: IHiringDataCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;


        public HiringDataCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<HiringDataDto>> GetAll()
        {
            var result = _context.HiringData.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<HiringDataDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<HiringDataDto> GetById(int id)
        {
            var result = _context.HiringData.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<HiringDataDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Update(HiringDataDto model)
        {
            try
            {
                if (model.Id != 0)

                {
                    var map = _mapper.Map<HiringDataDto>(model);
                    await _context.BulkInsertAsync(_context.HiringData, options => options.InsertKeepIdentity = true);
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
                    var map = _mapper.Map<HiringDataDto>(model);
                    await _context.BulkInsertAsync(_context.HiringData, options => options.InsertKeepIdentity = true);
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
            var getData = _context.HiringData.Where(x => x.Id == id).FirstOrDefault();
            if (getData != null)
            {

                var result = _context.HiringData.Remove(getData);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> Create(HiringDataDto model)
        {
            var getData = _context.HiringData.Where(x => x.Id == model.Id).FirstOrDefault();
            if (getData == null)
            {
                var map = _mapper.Map<HiringData>(model);
                var res = _context.HiringData.Add(map);
                await _context.SaveChangesAsync();
                return map.Id != 0 ? map.Id : 0;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                var res = _context.HiringData.Update(map);
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
