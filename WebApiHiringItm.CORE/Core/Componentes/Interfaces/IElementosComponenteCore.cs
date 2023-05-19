using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IElementosComponenteCore
    {
        Task<bool> Add(ElementComponentDto model);
        Task<List<ElementComponentDto>?> Get(Guid? id);
        Task<ElementComponentDto> GetById(Guid id);
    }
}
