using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IComponenteCore
    {
        Task<List<ComponentDto>?> GetComponentsByContract(Guid id);
        Task<IGenericResponse<ComponentDto>> GetByIdComponent(string id, string? activityId, string? elementId);
        Task<IGenericResponse<string>> AddActivity(ActivityDto model);
        Task<List<ActivityDto>?> GetActivityByComponent(Guid id);
        Task<ActivityDto> GetActivityById(Guid id);
        Task<IGenericResponse<string>> SaveComponentContract(ComponentDto model);
        Task<IGenericResponse<string>> DeleteComponentContract(Guid id);
        Task<IGenericResponse<string>> DeleteActivityContract(string activityContract);

    }
}
