using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IComponenteCore
    {
        Task<List<ComponenteDto>?> GetComponentsByContract(Guid id);
        Task<bool> Delete(Guid id);
        Task<ComponenteDto> GetByIdComponent(Guid id, Guid activityId, Guid elementId);
        Task<bool> AddActivity(ActivityDto model);
        Task<List<ActivityDto>?> GetActivityByComponent(Guid id);
        Task<ActivityDto> GetActivityById(Guid id);
        Task<IGenericResponse<string>> SaveComponentContract(ComponenteDto model);

    }
}
