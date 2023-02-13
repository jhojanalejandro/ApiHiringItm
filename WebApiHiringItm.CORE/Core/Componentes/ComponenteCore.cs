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
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;
        private readonly ISaveChangesExitHelper _save;
        #endregion

        #region Builder
        public ComponenteCore(Hiring_V1Context context, IMapper mapper, ISaveChangesExitHelper save)
        {
            _context = context;
            _mapper = mapper;
            _save = save;
        }
        #endregion

        #region Methods
        public async Task<bool> Add(ComponenteDto model)
        {
            var exist = _context.Componente.FirstOrDefault(x => x.Id == model.Id);

            if (exist == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<Componente>(model);
                _context.Componente.Add(map);
                _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
            else
            {
                var mapUpdate = _mapper.Map(model,exist);
                _context.Componente.Update(mapUpdate);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

                return await Task.FromResult(true);
            }
        }

        public async Task<bool> AddActivity(ActivityDto model)
        {
            var exist = _context.Actividad.FirstOrDefault(x => x.Id == model.Id);

            if (exist == null)
            {
                var map = _mapper.Map<Actividad>(model);
                _context.Actividad.Add(map);
                _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
            else
            {
                var mapUpdate = _mapper.Map(model, exist);
                _context.Actividad.Update(mapUpdate);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

                return await Task.FromResult(true);
            }
        }
        public async Task<List<ComponenteDto>?> Get(Guid id)
        {
            try
            {
                var result = _context.Componente.Where(x => x.IdContrato == id).ToList();
                if (result.Count != 0)
                {
                    var map = _mapper.Map<List<ComponenteDto>>(result);
                    map.ForEach(e =>
                    {
                        var element = _context.ElementosComponente.Where(d => d.IdComponente == e.Id).ToList();
                        e.Elementos = _mapper.Map<List<ElementosComponenteDto>>(element);
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
        public async Task<List<ActivityDto>?> GetActivity(Guid id)
        {
            var result = _context.Actividad.Where(x => x.IdComponente.Equals(id)).ToList();
            var mapctivity = _mapper.Map<List<ActivityDto>>(result);
            return await Task.FromResult(mapctivity);
        }
        public async Task<ComponenteDto> GetById(Guid id)
        {
            var result = _context.Componente.FirstOrDefault(x => x.Id.Equals(id));
            var map = _mapper.Map<ComponenteDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var resultData = _context.ElementosComponente.Where(x => x.IdComponente == id).ToList();
                List<ElementosComponente?> elementComponent = new List<ElementosComponente?>();
                if (resultData != null)
                {
                    foreach (var item in resultData)
                    {
                        var element = _context.ElementosComponente.FirstOrDefault(x => x.Id == item.Id);
                        elementComponent.Add(element);
                    }
                }

                var componentData = _context.Componente.FirstOrDefault(x => x.Id == id);
                if (resultData != null)
                {
                    var result = _context.Componente.Remove(componentData);
                }
                if (elementComponent.Count > 0)
                {
                    _context.ElementosComponente.RemoveRange(elementComponent);
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
