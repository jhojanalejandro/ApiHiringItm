using Microsoft.Extensions.DependencyInjection;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.CORE.Core.User;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Core.FileCore;
using WebApiHiringItm.CORE.Core.ProjectFolders;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Core.Contractors;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.Viability.Interface;
using WebApiHiringItm.CORE.Core.Viability;
using WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface;
using WebApiHiringItm.CORE.Core.FoldersContractorCore;

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
            services.AddScoped<IFolderContractorCore, FolderContractorCore>();

        }
    }
}
