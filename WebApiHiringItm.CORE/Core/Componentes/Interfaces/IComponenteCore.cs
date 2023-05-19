using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IComponenteCore
    {
        Task<bool> Add(ComponenteDto model);
        Task<List<ComponenteDto>?> Get(Guid id);
        Task<bool> Delete(Guid id);
        Task<ComponenteDto> GetById(Guid id);
        Task<bool> AddActivity(ActivityDto model);
        Task<List<ActivityDto>?> GetActivityByComponent(Guid id);
        Task<ActivityDto> GetActivityById(Guid id);

    }
}
