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
        private readonly IHiringContext _context;
        private readonly IMapper _mapper;
        private readonly ISaveChangesExitHelper _save;
        #endregion

        #region Builder
        public ElementosComponenteCore(IHiringContext context, IMapper mapper, ISaveChangesExitHelper save)
        {
            _context = context;
            _mapper = mapper;
            _save = save;
        }
        #endregion

        #region Methods
        public async Task<bool> Add(ElementosComponenteDto model)
        {
            var exist = _context.ElementosComponente.Where(w => w.Id == model.Id).FirstOrDefault();
            if (exist == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<ElementosComponente>(model);
                _context.ElementosComponente.Add(map);
            }
            else
            {
                var mapUpdate = _mapper.Map(model, exist);
                _context.ElementosComponente.Update(mapUpdate);
            }
            await _save.SaveChangesDB();
            return true;
        }

        public async Task<List<ElementosComponenteDto>?> Get(Guid? id)
        {
            var result = _context.ElementosComponente.Where(x => x.IdComponente == id).ToList();
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

        public async Task<ElementosComponenteDto> GetById(Guid id)
        {
            var result = _context.ElementosComponente.Where(x => x.Id.Equals(id)).FirstOrDefault();
            var map = _mapper.Map<ElementosComponenteDto>(result);
            return await Task.FromResult(map);
        }



        #endregion
    }
}
