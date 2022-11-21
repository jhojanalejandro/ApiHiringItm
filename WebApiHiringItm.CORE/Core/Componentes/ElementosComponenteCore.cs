using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
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
                var exist = _context.ElementosComponente.Where(w => w.Id == item.Id).FirstOrDefault();
                if (exist != null)
                {
                    _context.ElementosComponente.Add(item);
                }
                else
                {
                    _context.ElementosComponente.Update(item);
                }
            }
            await _save.SaveChangesDB();
            return true;
        }

        public async Task<List<ElementosComponenteDto>?> Get(int id)
        {
            var result = _context.ElementosComponente.Where(x => x.IdComponenete == id).ToList();
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

        public async Task<List<ElementosComponenteDto>?> GetByContractId(int id)
        {
            var result = _context.ElementosComponente
                .Include(x => x.IdComponeneteNavigation)
                .ThenInclude(X => X.IdContratoNavigation)
                .Where(x => x.IdComponeneteNavigation.IdContratoNavigation.Id == id)
                .ToList();
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
