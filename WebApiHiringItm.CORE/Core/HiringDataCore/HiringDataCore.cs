using AutoMapper;
using System.Data.Entity;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.HiringDataCore.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.HiringDataCore
{
    public class HiringDataCore : IHiringDataCore
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

        public async Task<HiringDataDto> GetById(int contractorId)
        {
            var result = _context.HiringData.FirstOrDefault(x => x.ContractorId == contractorId);
            var map = _mapper.Map<HiringDataDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<MinutaDto> GetByIdMinuta(int[] id)
        {
            var hiring = _context.DetailProjectContractor
                .Include(hd => hd.HiringData).FirstOrDefault(x => x.ContractorId == id[0]);
            var elemento = _context.ElementosComponente.FirstOrDefault(x => x.Id == id[1]);
            MinutaDto minutaDto = new MinutaDto();
            var hiringMap = _mapper.Map<HiringDataDto>(hiring);
            var elementoMap = _mapper.Map<ElementosComponenteDto>(elemento);

            minutaDto.HiringDataDto = hiringMap;
            minutaDto.elementosComponenteDto = elementoMap;
            var map = _mapper.Map<MinutaDto>(minutaDto);
            return await Task.FromResult(map);
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

        public async Task<bool> Create(List<HiringDataDto> model)
        {
            try
            {
                List<HiringData> hiringDataListUpdate = new List<HiringData>();
                List<HiringData> hiringDataListAdd = new List<HiringData>();

                var map = _mapper.Map<List<HiringData>>(model);
                for(var i= 0; i< map.Count; i++)
                {
                    var getData = _context.HiringData.FirstOrDefault(x => x.ContractorId == model[i].ContractorId);
                    if (getData != null)
                    {
                        model[i].Id = getData.Id;
                        var mapData = _mapper.Map(model[i], getData);
                        hiringDataListUpdate.Add(mapData);
                        map.Remove(map[i]);
                        i--;
                    }
                    else
                    {
                        hiringDataListAdd.Add(map[i]);
                    }
                }
                if (hiringDataListUpdate.Count > 0)
                    _context.HiringData.UpdateRange(hiringDataListUpdate);
                if (hiringDataListAdd.Count > 0)
                    _context.HiringData.AddRange(hiringDataListAdd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error", ex);
                return true;

            }
            return false;
        }

    }
}
