using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.InterfacesHelpers;
using WebApiHiringItm.CORE.Properties;
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

        #region PUBLIC METHODS
        public async Task<IGenericResponse<string>> SaveElement(ElementComponentDto modelElement)
        {
            if (modelElement.ComponentId == Guid.Empty)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.GUIDNOTVALID);

            var getElement = _context.ElementComponent.Where(w => w.Id.Equals(modelElement.Id)).FirstOrDefault();
            if (getElement == null)
            {
                modelElement.Id = Guid.NewGuid();
                var mapElement = _mapper.Map<ElementComponent>(modelElement);
                _context.ElementComponent.Add(mapElement);
            }
            else
            {
                var mapUpdate = _mapper.Map(modelElement, getElement);
                _context.ElementComponent.Update(mapUpdate);
            }
            await _save.SaveChangesDB();
            return ApiResponseHelper.CreateResponse<string>(Resource.REGISTERSUCCESSFULL);
        }

        public async Task<List<ElementComponentDto>?> GetElementsByComponent(Guid? id)
        {

            var result = _context.ElementComponent.Where(x => x.ComponentId == id);

            return result.Select(s => new ElementComponentDto
            {
                NombreElemento = s.NombreElemento,
                Id = s.Id,
                CantidadContratistas = s.CantidadContratistas,
                CantidadEnable = s.CantidadContratistas - s.DetailContractor.Select(s => s.ElementId.Equals(s.Id)).ToList().Count ,

            }).AsNoTracking()
            .ToList();

        }

        public async Task<ElementComponentDto?> GetElementById(Guid id)
        {
            var result = _context.ElementComponent.Where(x => x.Id.Equals(id));
            return await result.Select(element => new ElementComponentDto
            {
                NombreElemento = element.NombreElemento,
                CpcNumber = element.Cpc.CpcNumber,
                CpcId = element.CpcId,
                NombreCpc = element.Cpc.CpcName,
                ObjetoElemento = element.ObjetoElemento,
                ValorPorDia = element.ValorPorDia,
                ValorTotal = element.ValorTotal,
                ValorUnidad = element.ValorUnidad,
                Recursos = element.DetailContractor.Select(s => s.Contract.DetailContract.Select(s => s.ResourceContract).FirstOrDefault()).FirstOrDefault()!,
                Consecutivo = element.Consecutivo,
                ObligacionesEspecificas = element.ObligacionesEspecificas,
                ObligacionesGenerales = element.ObligacionesGenerales,
                PerfilRequeridoAcademico = element.PerfilAcademicoRequerido,
                PerfilRequeridoExperiencia = element.PerfilExperienciaRequerido,
                CantidadDias = element.CantidadDias,

            }).AsNoTracking()
           .FirstOrDefaultAsync();

        }

        #endregion
    }
}
