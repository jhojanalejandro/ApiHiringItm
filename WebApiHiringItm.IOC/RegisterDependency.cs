using Microsoft.Extensions.DependencyInjection;
using WebApiHiringItm.CORE.Core.ExcelCore.interfaces;
using WebApiHiringItm.CORE.Core.ExcelCore;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.CORE.Core.User;
using WebApiHiringItm.CORE.Core.File.Interface;
using WebApiHiringItm.CORE.Core.File;
using WebApiHiringItm.CORE.Core.ProjectFolders;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Core.Contractors;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.Viability.Interface;
using WebApiHiringItm.CORE.Core.Viability;
using WebApiHiringItm.CORE.Core.FoldersContractor.Interface;
using WebApiHiringItm.CORE.Core.FoldersContractor;

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
            services.AddScoped<IFolderContractorCore, FolderContractorCore>();

        }
    }
}
