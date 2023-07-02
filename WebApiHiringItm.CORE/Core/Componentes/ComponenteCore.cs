using AutoMapper;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.CORE.Helpers.InterfacesHelpers;
using WebApiHiringItm.MODEL.Dto.Componentes;
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
        public async Task<bool> Add(ComponenteDto model)
        {
            var exist = _context.Component.FirstOrDefault(x => x.Id == model.Id);

            if (exist == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<Component>(model);
                _context.Component.Add(map);
                _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
            else
            {
                var mapUpdate = _mapper.Map(model,exist);
                _context.Component.Update(mapUpdate);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
        }

        public async Task<bool> AddActivity(ActivityDto model)
        {
            var exist = _context.Activity.FirstOrDefault(x => x.Id == model.Id);

            if (exist == null)
            {
                var map = _mapper.Map<Activity>(model);
                map.Id = Guid.NewGuid();
                _context.Activity.Add(map);
                await _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
            else
            {
                var mapUpdate = _mapper.Map(model, exist);
                _context.Activity.Update(mapUpdate);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;            }
        }
        public async Task<List<ComponenteDto>?> GetComponentsByContract(Guid contractId)
        {
            try
            {
                var result = _context.Component.Where(x => x.ContractId == contractId).ToList();
                if (result.Count != 0)
                {
                    var map = _mapper.Map<List<ComponenteDto>>(result);
                    map.ForEach(e =>
                    {
                        var element = _context.ElementComponent.Where(w => w.ComponentId.Equals(e.Id) && w.ActivityId == null).ToList();
                        e.Elementos = _mapper.Map<List<ElementComponentDto>>(element);
                        var activity = _context.Activity.Where(d => d.ComponentId == e.Id).ToList();
                        e.Activities = _mapper.Map<List<ActivityDto>>(activity);
                        e.Activities.ForEach(eA =>
                        {
                            var element = _context.ElementComponent.Where(w => w.ComponentId.Equals(e.Id) && w.ActivityId.Equals(eA.Id)).ToList();
                            eA.Elementos = _mapper.Map<List<ElementComponentDto>>(element);

                        });
                    });
                    return await Task.FromResult(map);
                }
                else
                {
                    return new List<ComponenteDto>();
                }
            }
            catch (Exception e)
            {
                return new List<ComponenteDto>(); throw;
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
        public async Task<ComponenteDto> GetById(Guid id)
        {
            var result = _context.Component.FirstOrDefault(x => x.Id.Equals(id));
            var map = _mapper.Map<ComponenteDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var resultData = _context.ElementComponent.Where(x => x.ComponentId == id).ToList();
                List<ElementComponent?> elementComponent = new List<ElementComponent?>();
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
                return await Task.FromResult(true);
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
