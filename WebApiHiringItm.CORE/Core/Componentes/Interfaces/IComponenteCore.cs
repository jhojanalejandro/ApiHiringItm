using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.CORE.Core.Componentes.Interfaces
{
    public interface IComponenteCore
    {
        Task<List<ComponenteDto>?> GetComponentsByContract(Guid id);
        Task<IGenericResponse<ComponenteDto>> GetByIdComponent(string id, string? activityId, string? elementId);
        Task<IGenericResponse<string>> AddActivity(ActivityDto model);
        Task<List<ActivityDto>?> GetActivityByComponent(Guid id);
        Task<ActivityDto> GetActivityById(Guid id);
        Task<IGenericResponse<string>> SaveComponentContract(ComponenteDto model);
        Task<IGenericResponse<string>> DeleteComponentContract(Guid id);

    }
}
