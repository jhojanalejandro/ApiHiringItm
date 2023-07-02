using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IElementosComponenteCore
    {
        Task<bool> SaveElement(ElementComponentDto model);
        Task<List<ElementComponentDto>?> GetElementsByComponent(Guid? id);
        Task<ElementComponentDto> GetById(Guid id);
    }
}
