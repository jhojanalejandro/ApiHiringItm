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
        private readonly IHiring_V1Context _context;
        private readonly IMapper _mapper;
        private readonly ISaveChangesExitHelper _save;
        #endregion

        #region Builder
        public ComponenteCore(IHiring_V1Context context, IMapper mapper, ISaveChangesExitHelper save)
        {
            _context = context;
            _mapper = mapper;
            _save = save;
        }
        #endregion

        #region Methods
        public async Task<bool> Add(ComponenteDto model)
        {
            var map = _mapper.Map<Componente>(model);
            var exist = _context.Componente.Where(x => x.Id == model.Id).FirstOrDefault();

            if (exist == null)
            {
                _context.Componente.Add(map);
                await _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
            else
            {
                _context.Componente.Update(map);
                await _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
        }

        public async Task<List<ComponenteDto>?> Get(int id)
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

        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.ElementosComponente.Where(x => x.IdComponente == id).ToList();
                foreach (var item in resultData)
                {
                    var element = _context.ElementosComponente.Where(x => x.Id == item.Id).FirstOrDefault();
                    _context.ElementosComponente.Remove(element);
                    await _save.SaveChangesDB();
                    return await Task.FromResult(true);
                }
                var componentData = _context.Componente.Where(x => x.Id == id).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.Componente.Remove(componentData);
                    await _save.SaveChangesDB();
                    return await Task.FromResult(true);

                }
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
