using Microsoft.Extensions.DependencyInjection;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.CORE.Core.User;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Core.FileCore;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Core.Contractors;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.HiringDataCore.Interface;
using WebApiHiringItm.CORE.Core.HiringDataCore;
using WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface;
using WebApiHiringItm.CORE.Core.FoldersContractorCore;
using WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces;
using WebApiHiringItm.CORE.Core.ExportToExcel;
using WebApiHiringItm.CORE.Core.EconomicdataContractorCore.Interface;
using WebApiHiringItm.CORE.Core.EconomicdataContractorCore;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Core.Componentes;
using WebApiHiringItm.CORE.Helpers.InterfacesHelpers;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.Core.User;
using WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore;
using WebApiHiringItm.CORE.Core.PdfDataCore;
using WebApiHiringItm.CORE.Core.MasterDataCore.Interface;
using WebApiHiringItm.CORE.Core.MasterDataCore;
using WebApiHiringItm.CORE.Core.ImportExcelCore.Interface;
using WebApiHiringItm.CORE.Core.ImportExcelCore;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Core.MessageHandlingCore;
using WebApiHiringItm.CORE.Core.ContractFolders;
using WebApiHiringItm.CORE.Core.FileMnager.Interface;
using WebApiHiringItm.CORE.Core.FileMnager;

namespace WebApiHiringItm.IOC.Dependencies
{
    public class RegisterDependency
    {
        public static void RegistrarDependencias(IServiceCollection services)
        {
            services.AddScoped<IUserCore, UserCore>();
            services.AddScoped<IHiringDataCore, HiringDataCore>();
            services.AddScoped<IFilesCore, FilesCore>();
            services.AddScoped<IContractorCore, ContractorCore>();
            services.AddScoped<IProjectFolder, ContractFolderCore>();
            services.AddScoped<IFolderContractorCore, FolderContractorCore>();
            services.AddScoped<IExportToExcelCore, ExportToExcelCore>();
            services.AddScoped<IEconomicdataContractorCore, EconomicdataContractorCore>();
            services.AddScoped<IComponenteCore, ComponenteCore>();
            services.AddScoped<ISaveChangesExitHelper, SaveChangesExitHelper>();
            services.AddScoped<IElementosComponenteCore, ElementosComponenteCore>();
            services.AddScoped<IUserFirmCore, UserFirmCore>();
            services.AddScoped<IContractorPaymentsCore, ContractorPaymentCore>();
            services.AddScoped<IPdfDataCore, PdfDataCore>();
            services.AddScoped<IMasterDataCore, MasterDataCore>();
            services.AddScoped<IImportExcelCore, ImportExcelCore>();
            services.AddScoped<IMessageHandlingCore, MessageHandlingCore>();
            services.AddScoped<IFileManagerCore, FileManagerCore>();

        }
    }
}
