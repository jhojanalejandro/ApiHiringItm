using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IElementosComponenteCore
    {
        Task<IGenericResponse<string>> SaveElement(ElementComponentDto model);
        Task<List<ElementComponentDto>?> GetElementsByComponent(Guid? id);
        Task<ElementComponentDto> GetElementById(Guid id);
    }
}
