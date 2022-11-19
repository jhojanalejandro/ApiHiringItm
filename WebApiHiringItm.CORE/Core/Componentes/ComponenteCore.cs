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
            _save =  save;
        }
        #endregion

        #region Methods
        public async Task<bool> Add(ComponenteDto model)
        {
            var map = _mapper.Map<Componente>(model);
            var exist = _context.Componentes.Where(x => x.Id == model.Id).FirstOrDefault();

            if (exist != null)
            {
                _context.Componentes.Add(map);
                await _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
            else
            {
                _context.Componentes.Update(map);
                await _save.SaveChangesDB();
                return await Task.FromResult(true);
            }
        }

        public async Task<List<ComponenteDto>?> Get(int id)
        {           
            var result = _context.Componentes.Where(x => x.IdContrato == id).ToList();
            if (result.Count != 0)
            {
                var map = _mapper.Map<List<ComponenteDto>>(result);
                return await Task.FromResult(map);
            }
            else
            {
                return new List<ComponenteDto>();
            }
        }
        #endregion
    }
}
