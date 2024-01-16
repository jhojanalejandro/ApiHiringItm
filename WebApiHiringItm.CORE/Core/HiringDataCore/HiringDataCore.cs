using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.HiringDataCore.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.PdfDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.HiringDataCore
{
    public class HiringDataCore : IHiringDataCore
    {
        #region FIELDS
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
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

        public async Task<IGenericResponse<HiringDataDto>> GetByIdHinringData(string contractorId, string contractId)
        {
            if (!contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<HiringDataDto>(Resource.GUIDNOTVALID);

            if (!contractorId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<HiringDataDto>(Resource.GUIDNOTVALID);

            var hiringResult = _context.DetailContractor
                 .Include(x => x.HiringData)
                 .Where(x => x.ContractorId.Equals(Guid.Parse(contractorId)) && x.ContractId.Equals(Guid.Parse(contractId)));
            var getType = _context.DetailContractor
                    .Include(x => x.HiringData)
                    .Where(x => x.ContractorId.Equals(Guid.Parse(contractorId)) && x.ContractId.Equals(Guid.Parse(contractId)));

            var hiringData = await hiringResult.Select(hd => new HiringDataDto
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
                ActividadContratista = hd.HiringData.ActividadContratista,
                SupervisorId = hd.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.SUPERVISORCONTRATO.Description())).Select(s => s.User.Id.ToString()).FirstOrDefault(),
            })
             .AsNoTracking()
             .FirstOrDefaultAsync();
            return ApiResponseHelper.CreateResponse(hiringData);

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

        public async Task<IGenericResponse<string>> SaveHiringData(List<HiringDataDto> model)
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
                    if (hiring != null)
                    {
                        if (model[i].StatusContractor == null)
                        {
                            model[i].StatusContractor = _context.StatusContractor.Where(w => w.Code.Equals(StatusContractorEnum.CONTRATANDO.Description())).Select(s => s.StatusContractorDescription).FirstOrDefault();
                        }
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
                        DetailContractor.Consecutive = hiring.Consecutive;
                        DetailContractor.Id = hiring.Id;
                        detailDataListAdd.Add(DetailContractor);
                        hiringDataListAdd.Add(mapHiring[i]);
                    }

                }
            }

            if (hiringDataListUpdate.Count > 0)
                _context.HiringData.UpdateRange(hiringDataListUpdate);
            if (hiringDataListAdd.Count > 0)
                _context.HiringData.AddRange(hiringDataListAdd);
            await _context.SaveChangesAsync();

            if (detailDataListAdd.Count > 0)
                _context.DetailContractor.UpdateRange(detailDataListAdd);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
        }

        public async Task<ContractorDateDto?> GetDateContractById(string contractorId, string contractId)
        {
            var getData = _context.DetailContractor.Where(x => x.ContractorId.Equals(Guid.Parse(contractorId)) && x.ContractId.Equals(Guid.Parse(contractId)));
            return getData.Select(data => new ContractorDateDto
            {
                DateContract =  data.HiringData.FechaRealDeInicio,
                FinalDateContract = data.HiringData.FechaFinalizacionConvenio
            }).AsNoTracking()
            .FirstOrDefault();
        }
        #endregion

        #region PRIVATE METHODS

        #endregion
    }
}
