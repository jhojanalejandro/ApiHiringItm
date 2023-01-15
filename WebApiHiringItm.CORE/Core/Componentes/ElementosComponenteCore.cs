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
        public async Task<bool> Add(ElementosComponenteDto model)
        {
            var exist = _context.ElementosComponente.Where(w => w.Id == model.Id).FirstOrDefault();
            if (exist == null)
            {
                var map = _mapper.Map<ElementosComponente>(model);
                _context.ElementosComponente.Add(map);
            }
            else
            {
                _context.ElementosComponente.Update(exist);
            }
            await _save.SaveChangesDB();
            return true;
        }

        public async Task<List<ElementosComponenteDto>?> Get(int id)
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

        public async Task<ElementosComponenteDto> GetById(int id)
        {
            var result = _context.ElementosComponente.Where(x => x.Id.Equals(id)).FirstOrDefault();
            var map = _mapper.Map<ElementosComponenteDto>(result);
            return await Task.FromResult(map);
        }



        #endregion
    }
}
