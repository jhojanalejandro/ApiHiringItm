using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Helpers.InterfacesHelpers;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.Componentes
{
    public class ElementosComponenteCore : IElementosComponenteCore
    {
        #region Fields
        private readonly IHiring_V1Context _context;
        private readonly IMapper _mapper;
        private readonly ISaveChangesExitHelper _save;
        #endregion

        #region Builder
        public ElementosComponenteCore(IHiring_V1Context context, IMapper mapper, ISaveChangesExitHelper save)
        {
            _context = context;
            _mapper = mapper;
            _save = save;
        }
        #endregion

        #region Methods
        public async Task<bool> Add(List<ElementosComponenteDto> model)
        {
            var map = _mapper.Map<List<ElementosComponente>>(model);
            foreach (var item in map)
            {
                var exist = _context.ElementosComponentes.Where(w => w.Id == item.Id).FirstOrDefault();
                if (exist != null)
                {
                    _context.ElementosComponentes.Add(item);
                }
                else
                {
                    _context.ElementosComponentes.Update(item);
                }
            }
            await _save.SaveChangesDB();
            return true;
        }

        public async Task<List<ElementosComponenteDto>?> Get(int id)
        {
            var result = _context.ElementosComponentes.Where(x => x.IdComponenete == id).ToList();
            if (result.Count != 0)
            {
                var map = _mapper.Map<List<ElementosComponenteDto>>(result);
                return await Task.FromResult(map);
            }
            else
            {
                return new List<ElementosComponenteDto>();
            }
        }
        #endregion
    }
}
