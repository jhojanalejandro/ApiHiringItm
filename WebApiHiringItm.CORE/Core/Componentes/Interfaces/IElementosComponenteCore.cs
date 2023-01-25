using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IElementosComponenteCore
    {
        Task<bool> Add(ElementosComponenteDto model);
        Task<List<ElementosComponenteDto>?> Get(Guid? id);
        Task<ElementosComponenteDto> GetById(Guid id);
    }
}
