using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IComponenteCore
    {
        Task<bool> Add(ComponenteDto model);
        Task<List<ComponenteDto>?> Get(int id);
        Task<bool> Delete(int id);
    }
}
