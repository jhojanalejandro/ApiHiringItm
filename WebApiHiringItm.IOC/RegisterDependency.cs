using Microsoft.Extensions.DependencyInjection;
using WebApiHiringItm.CORE.Core;
using WebApiHiringItm.CORE.Core.ExcelCore.interfaces;
using WebApiHiringItm.CORE.Core.ExcelCore;
using WebApiHiringItm.CORE.Interface;

namespace WebApiHiringItm.IOC
{
    public class RegisterDependency
    {
        public static void RegistrarDependencias(IServiceCollection services)
        {
            services.AddScoped<IUserCore, UserCore>();
            services.AddScoped<IHiringDataCore, HiringDataCore>();
            services.AddScoped<IFilesCore, FilesCore>();
            services.AddScoped<IContractorCore, ContractorCore>();
            services.AddScoped<IProjectFolder, ProjectFolderCore>();
            services.AddScoped<IUploadExcelCore, UploadExcelCore>();
        }
    }
}
