using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        #region FIELDS
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        IQueryable<DetailProjectContractor> hiringResult;
        #endregion

        public HiringDataCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region PUBLIC METHODS
        public async Task<List<HiringDataDto>> GetAll()
        {
            var result = _context.HiringData.Where(x => x.Id != null).ToList();
            var map = _mapper.Map<List<HiringDataDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<HiringDataDto?> GetById(Guid contractorId, Guid contractId)
        {
            var hiringResult = _context.DetailProjectContractor
                 .Include(x => x.HiringData)
                 .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId.Equals(x.ContractId));
            
            return await hiringResult.Select(hd => new HiringDataDto
            {
                Id = hd.HiringData.Id,
                FechaRealDeInicio = hd.HiringData.FechaRealDeInicio,
                FechaFinalizacionConvenio = hd.HiringData.FechaFinalizacionConvenio,
                Contrato = hd.HiringData.Contrato,
                Compromiso = hd.HiringData.Compromiso,
                FechaExaPreocupacional = hd.HiringData.FechaExaPreocupacional,
                SupervisorItm = hd.HiringData.SupervisorItm,
                CargoSupervisorItm = hd.HiringData.CargoSupervisorItm,
                FechaDeComite = hd.HiringData.FechaDeComite,
                RequierePoliza = hd.HiringData.RequierePoliza,
                NoPoliza = hd.HiringData.NoPoliza,
                VigenciaInicial = hd.HiringData.VigenciaInicial,
                VigenciaFinal = hd.HiringData.VigenciaFinal,
                FechaExpedicionPoliza = hd.HiringData.FechaExpedicionPoliza,
                ValorAsegurado = hd.HiringData.ValorAsegurado,
                Nivel = hd.HiringData.Nivel,
                Caso = hd.HiringData.Caso,
                NombreRubro = hd.Contract.NombreRubro,
                FuenteRubro = hd.Contract.FuenteRubro,
                Cdp = hd.HiringData.Cdp,
                NumeroActa = hd.HiringData.NumeroActa
            })
             .AsNoTracking()
             .FirstOrDefaultAsync();
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

        public async Task<bool> Delete(Guid id)
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
                List<DetailProjectContractor> detailDataListAdd = new List<DetailProjectContractor>();
                var getData = _context.DetailProjectContractor
                    .Where(x => x.ContractId == model[0].ContractId)
                    .Include(dt => dt.HiringData)
                    .ToList();

                var hd = _context.HiringData.ToList();
                var map = _mapper.Map<List<HiringData>>(model);
                for (var i = 0; i < map.Count; i++)
                {
                    var hiring = getData.FirstOrDefault(h => h.ContractorId.Equals(map[i].ContractorId));
                    var hdata = hd.FirstOrDefault(x => x.Id == hiring.HiringDataId);

                    if (hdata != null)
                    {
                        model[i].Id = hdata.Id;
                        var mapData = _mapper.Map(model[i], hdata);
                        hiringDataListUpdate.Add(mapData);
                        map.Remove(map[i]);
                        i--;
                    }
                    else
                    {
                        if (getData != null)
                        {
                            DetailProjectContractor detailProjectContractor = new DetailProjectContractor();
                            map[i].Id = Guid.NewGuid();
                            detailProjectContractor.HiringDataId = map[i].Id;
                            detailProjectContractor.ContractorId = map[i].ContractorId;
                            detailProjectContractor.ContractId = model[i].ContractId;
                            detailProjectContractor.ElementId = hiring.ElementId;
                            detailProjectContractor.ComponentId = hiring.ComponentId;
                            detailProjectContractor.Id = hiring.Id;
                            detailDataListAdd.Add(detailProjectContractor);
                            hiringDataListAdd.Add(map[i]);
                        }
                        else
                        {
                            return false;
                        }

                    }
                }

                if (hiringDataListUpdate.Count > 0)
                    _context.HiringData.UpdateRange(hiringDataListUpdate);
                if (hiringDataListAdd.Count > 0)
                    _context.HiringData.AddRange(hiringDataListAdd);
                await _context.SaveChangesAsync();

                if (detailDataListAdd.Count > 0)
                    return await updateDetails(detailDataListAdd);

                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error", ex);
            }
            return false;
        }
        #endregion
        
        #region PRIVATE METHODS
        private async Task<bool> updateDetails(List<DetailProjectContractor> detailProjectContractors)
        {
            try
            {
                _context.DetailProjectContractor.UpdateRange(detailProjectContractors);
                var result = await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }

            return false;
        }
        #endregion
    }
}
