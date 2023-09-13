using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.HiringDataCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
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
        IQueryable<DetailContractor> hiringResult;
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

        public async Task<HiringDataDto?> GetByIdHinringData(Guid contractorId, Guid contractId)
        {
            var hiringResult = _context.DetailContractor
                 .Include(x => x.HiringData)
                 .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId.Equals(contractId));
            var getType = _context.DetailContractor
                    .Include(x => x.HiringData)
                    .Where(x => x.ContractorId.Equals(contractorId) && x.ContractId.Equals(contractId));
            return await hiringResult.Select(hd => new HiringDataDto
            {
                Id = hd.HiringData.Id,
                FechaRealDeInicio = hd.HiringData.FechaRealDeInicio,
                FechaFinalizacionConvenio = hd.HiringData.FechaFinalizacionConvenio,
                Contrato = hd.HiringData.Contrato,
                Compromiso = hd.HiringData.Compromiso,
                FechaExaPreocupacional = hd.HiringData.FechaExaPreocupacional,
                SupervisorItm = hd.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.UserName).FirstOrDefault(),
                CargoSupervisorItm = hd.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Professionalposition).FirstOrDefault(),
                FechaDeComite = hd.HiringData.FechaDeComite,
                RequierePoliza = hd.HiringData.RequierePoliza,
                NoPoliza = hd.HiringData.NoPoliza,
                VigenciaInicial = hd.HiringData.VigenciaInicial,
                VigenciaFinal = hd.HiringData.VigenciaFinal,
                FechaExpedicionPoliza = hd.HiringData.FechaExpedicionPoliza,
                ValorAsegurado = hd.HiringData.ValorAsegurado,
                Nivel = hd.HiringData.Nivel,
                Caso = hd.HiringData.Caso,
                NombreRubro = hd.Contract.RubroNavigation.Rubro,
                FuenteRubro = hd.Contract.FuenteRubro,
                Cdp = hd.HiringData.Cdp,
                NumeroActa = hd.HiringData.NumeroActa,
                SupervisorId = hd.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Id.ToString()).FirstOrDefault(),
            })
             .AsNoTracking()
             .FirstOrDefaultAsync();
        }

        public async Task<bool> Updates(string model)
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
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var getData = _context.HiringData.Where(x => x.Id.Equals(id)).FirstOrDefault();
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

        public async Task<bool> SaveHiringData(List<HiringDataDto> model)
        {
            var getStatusId = _context.StatusContractor.ToList();

            List<HiringData> hiringDataListUpdate = new List<HiringData>();
            List<HiringData> hiringDataListAdd = new List<HiringData>();
            List<DetailContractor> detailDataListAdd = new List<DetailContractor>();
            var getData = _context.DetailContractor
                .Where(x => x.ContractId == model[0].ContractId)
                .Include(dt => dt.HiringData)
                .ToList();

            var hd = _context.HiringData.ToList();
            var mapHiring = _mapper.Map<List<HiringData>>(model);
            for (var i = 0; i < mapHiring.Count; i++)
            {
                var hiring = getData.FirstOrDefault(h => h.ContractorId.Equals(mapHiring[i].ContractorId));
                var hdata = hd.FirstOrDefault(x => x.Id == hiring.HiringDataId);

                if (hdata != null)
                {
                    model[i].Id = hdata.Id;
                    var mapData = _mapper.Map(model[i], hdata);
                    hiringDataListUpdate.Add(mapData);
                    mapHiring.Remove(mapHiring[i]);
                    i--;
                }
                else
                {
                    if (getData != null)
                    {
                        var stattusId = getStatusId.Find(f => f.StatusContractorDescription.Equals(model[i].StatusContractor)).Id;
                        DetailContractor DetailContractor = new DetailContractor();
                        mapHiring[i].Id = Guid.NewGuid();
                        DetailContractor.HiringDataId = mapHiring[i].Id;
                        DetailContractor.ContractorId = mapHiring[i].ContractorId;
                        DetailContractor.ContractId = model[i].ContractId;
                        DetailContractor.ElementId = hiring.ElementId;
                        DetailContractor.ComponentId = hiring.ComponentId;
                        DetailContractor.ActivityId = hiring.ActivityId;
                        DetailContractor.StatusContractor = stattusId;
                        DetailContractor.Id = hiring.Id;
                        detailDataListAdd.Add(DetailContractor);
                        hiringDataListAdd.Add(mapHiring[i]);
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
        #endregion
        
        #region PRIVATE METHODS
        private async Task<bool> updateDetails(List<DetailContractor> detailProjectContractors)
        {
            _context.DetailContractor.UpdateRange(detailProjectContractors);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
