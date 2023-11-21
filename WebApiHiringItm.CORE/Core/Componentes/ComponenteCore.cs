using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using WebApiHiringItm.CORE.Helpers.InterfacesHelpers;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.Componentes
{
    public class ComponenteCore : IComponenteCore
    {
        #region Fields
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly ISaveChangesExitHelper _save;
        #endregion

        #region Builder
        public ComponenteCore(HiringContext context, IMapper mapper, ISaveChangesExitHelper save)
        {
            _context = context;
            _mapper = mapper;
            _save = save;
        }
        #endregion

        #region Methods
        public async Task<IGenericResponse<string>> SaveComponentContract(ComponentDto model)
        {
            var exist = _context.Component.FirstOrDefault(x => x.Id == model.Id);

            if (exist == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<Component>(model);
                _context.Component.Add(map);
                var resp = await _save.SaveChangesDB();
                if (resp)
                {
                    return ApiResponseHelper.CreateResponse(Resource.REGISTERSUCCESSFULL);
                }
                else
                {
                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.INFORMATIONEMPTY);
                }
            }
            else
            {
                var mapUpdate = _mapper.Map(model,exist);
                _context.Component.Update(mapUpdate);
                var resp = await _save.SaveChangesDB();
                if (resp)
                {
                    return ApiResponseHelper.CreateResponse(Resource.REGISTERSUCCESSFULL);
                }
                else
                {
                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.INFORMATIONEMPTY);
                }
            }
        }

        public async Task<IGenericResponse<string>> AddActivity(ActivityDto modelActivity)
        {
            if (modelActivity.ComponentId == Guid.Empty)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.GUIDNOTVALID);

            var exist = _context.Activity.FirstOrDefault(x => x.Id.Equals(modelActivity.Id));

            if (exist == null)
            {
                var map = _mapper.Map<Activity>(modelActivity);
                map.Id = Guid.NewGuid();
                _context.Activity.Add(map);
            }
            else
            {
                var mapUpdate = _mapper.Map(modelActivity, exist);
                _context.Activity.Update(mapUpdate);
            }
            var resp = await _save.SaveChangesDB();

            return ApiResponseHelper.CreateResponse<string>(Resource.REGISTERSUCCESSFULL);

        }

        public async Task<List<ComponentDto>?> GetComponentsByContract(Guid contractId)
        {
            try
            {
                var result = _context.Component.Where(x => x.ContractId == contractId).ToList();
                if (result.Count != 0)
                {
                    var map = _mapper.Map<List<ComponentDto>>(result);
                    foreach (var items in map)
                    {
                        var element = _context.ElementComponent.Where(w => w.ComponentId.Equals(items.Id) && w.ActivityId == null).ToList();
                        items.Elementos = _mapper.Map<List<ElementComponentDto>>(element);
                        var activity = _context.Activity.Where(d => d.ComponentId.Equals(items.Id)).ToList();
                        items.Activities = _mapper.Map<List<ActivityDto>>(activity);
                        foreach (var item in items.Activities)
                        {
                            item.Elementos = getElementsByActivity(item.Id.Value);

                        }
                    }

                    return await Task.FromResult(map);
                }
                else
                {
                    return new List<ComponentDto>();
                }
            }
            catch (Exception e)
            {
                return new List<ComponentDto>(); throw;
            }
        }
        public async Task<List<ActivityDto>?> GetActivityByComponent(Guid id)
        {
            var result = _context.Activity.Where(x => x.ComponentId.Equals(id)).ToList();
            var mapctivity = _mapper.Map<List<ActivityDto>>(result);
            return await Task.FromResult(mapctivity);
        }

        public async Task<ActivityDto?> GetActivityById(Guid id)
        {
            var result = _context.Activity.Where(x => x.Id.Equals(id)).FirstOrDefault();
            var mapctivity = _mapper.Map<ActivityDto>(result);
            return await Task.FromResult(mapctivity);
        }
        public async Task<IGenericResponse<ComponentDto>> GetByIdComponent(string id, string? activityId, string? elementId)
        {
            if (string.IsNullOrEmpty(id) || !id.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<ComponentDto>(Resource.GUIDNOTVALID);

            var result = _context.Component.FirstOrDefault(x => x.Id.Equals(Guid.Parse(id)));

            var map = _mapper.Map<ComponentDto>(result);
            var activity = _context.Activity;
            if (!string.IsNullOrEmpty(elementId) && elementId != "null")
            {
                List<ElementComponentDto> elementsList = new();
                var getElement = _context.ElementComponent.Where(w => w.Id.Equals(Guid.Parse(elementId))).FirstOrDefault();
                var mapElement = _mapper.Map<ElementComponentDto>(getElement);

                elementsList.Add(mapElement);
                map.Elementos = elementsList;
            }
            if (!string.IsNullOrEmpty(activityId) && activityId != "null")
            {
                activity.Where(w => w.Id.Equals(Guid.Parse(activityId))).ToList();
            }
            else
            {
                activity.Where(w => w.ComponentId.Equals(Guid.Parse(id))).ToList();
            }

            if (activity.ToList().Count() > 0 )
            {
                map.Activities = _mapper.Map<List<ActivityDto>>(activity);
                foreach (var item in map.Activities)
                {
                    item.Elementos = getElementsByActivity(item.Id.Value);
                }
            }
            if (map != null)
            {
                return ApiResponseHelper.CreateResponse(map);
            }
            else
            {
                return ApiResponseHelper.CreateErrorResponse<ComponentDto>(Resource.INFORMATIONEMPTY);
            }

        }

        public async Task<IGenericResponse<string>> DeleteComponentContract(Guid id)
        {
            var resultData = _context.ElementComponent.Where(x => x.ComponentId == id).ToList();
            List<ElementComponent> elementComponent = new List<ElementComponent>();
            if (resultData != null)
            {
                foreach (var item in resultData)
                {
                    var element = _context.ElementComponent.FirstOrDefault(x => x.Id == item.Id);
                    elementComponent.Add(element);
                }
            }

            var componentData = _context.Component.FirstOrDefault(x => x.Id == id);
            if (resultData != null)
            {
                var result = _context.Component.Remove(componentData);
            }
            if (elementComponent.Count > 0)
            {
                _context.ElementComponent.RemoveRange(elementComponent);
            }
            await _save.SaveChangesDB();
            return ApiResponseHelper.CreateResponse<string>(null, true,Resource.DELETESUCCESS);
        }

        public async Task<IGenericResponse<string>> DeleteActivityContract(string activityContract)
        {
            var resultData = _context.ElementComponent.Where(x => x.ActivityId.Equals(Guid.Parse(activityContract))).ToList();
            List<ElementComponent> elementComponent = new List<ElementComponent>();
            if (resultData != null)
            {
                foreach (var item in resultData)
                {
                    var element = _context.ElementComponent.FirstOrDefault(x => x.Id == item.Id);
                    elementComponent.Add(element);
                }
            }

            if (elementComponent.Count > 0)
            {
                _context.ElementComponent.RemoveRange(elementComponent);
            }

            var activityData = _context.Activity.FirstOrDefault(x => x.Id.Equals(Guid.Parse(activityContract)));
            if (activityData != null)
            {
                var result = _context.Activity.Remove(activityData);
            }
            await _save.SaveChangesDB();
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.DELETESUCCESS);
        }
        #endregion
        #region PRIVATE METHODS
        private  List<ElementComponentDto> getElementsByActivity(Guid activityId)
        {
            var element = _context.ElementComponent.Where(w =>  w.ActivityId.Equals(activityId)).OrderBy(o => o.Consecutivo);
            return element.Select(s => new ElementComponentDto()
            {
                Id = s.Id,  
                NombreElemento = s.NombreElemento,
                CantidadContratistas = s.CantidadContratistas,
                CantidadDias = s.CantidadDias,
                CpcId = s.CpcId,
                NombreCpc = s.Cpc.CpcName,
                Recursos = s.DetailContractor.Select(s => s.Contract).FirstOrDefault()!.RecursosAdicionales,
                ValorPorDia = s.ValorPorDia,
                ValorPorDiaContratista = s.ValorPorDiaContratista,
                ValorTotal = s.ValorTotal,
                ObjetoElemento = s.ObjetoElemento,
                ObligacionesEspecificas = s.ObligacionesEspecificas,
                ObligacionesGenerales = s.ObligacionesGenerales,
                Consecutivo = s.Consecutivo,
                TipoElemento = s.TipoElemento,
                CpcNumber = s.Cpc.CpcNumber,
                ValorUnidad = s.ValorUnidad,
                ValorTotalContratista = s.ValorTotalContratista,
                ComponentId = s.ComponentId.Value,
                ActivityId = s.ActivityId.Value,
                CantidadEnable = s.CantidadContratistas - _context.DetailContractor.Where(w => w.ElementId.Equals(s.Id)).Count(),
            })
            .AsNoTracking()
            .ToList();
        }
        #endregion
    }
}
