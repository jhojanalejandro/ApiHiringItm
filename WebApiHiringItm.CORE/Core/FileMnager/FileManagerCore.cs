using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FileMnager.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.FolderType;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContract;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.FileManagerDo;

namespace WebApiHiringItm.CORE.Core.FileMnager
{
    public class FileManagerCore: IFileManagerCore
    {
        private const string FILETYPE = "file";
        private const string FOLDERTYPE = "folder";
        #region BUILD
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        public FileManagerCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion
        #region PUBLIC METHODS
        public async Task<FileManagerDto> GetFolderFilesContract(Guid id)
        {
            FileManagerDto fileManagerDto = new FileManagerDto();
            fileManagerDto.Folders = await GetFolderContract(id);
            fileManagerDto.FolderContract = await GetFoldersContract(id);
            return fileManagerDto;
        }
        public async Task<FolderManagerContractDto> GetAllContract()
        {
            FolderManagerContractDto folderManagerContractDto = new FolderManagerContractDto();
            var getStatusContract = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.TERMINADO.Description())).Select(s => s.Id).FirstOrDefault();
            var getStatusContractInprogess = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.ENPROCESO.Description())).Select(s => s.Id).FirstOrDefault();
            var contractor = _context.DetailContract
                .Include(dt => dt.Contract)
                .Where(w => !w.Contract.StatusContractId.Equals(getStatusContract) && w.Contract.Activate);
            var contracts =  await contractor.Select(ct => new FolderContractContractorDto
            {
                Type = FOLDERTYPE,
                Id = ct.Contract.Id.ToString(),
                CompanyName = ct.Contract.CompanyName,
                ProjectName = ct.Contract.ProjectName,
                ProjectNumber = ct.Contract.NumberProject
            })
             .AsNoTracking()
             .ToListAsync();
            folderManagerContractDto.Folders = contracts;
            return folderManagerContractDto;
        }


        public async Task<List<FilesDto>?> GetFileContract(Guid id)
        {

            var getFiles = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Where(w => w.File.ContractId.Equals(id));

            return await getFiles.Select(ct => new FilesDto
            {
                Id = ct.Id,
                Type = FILETYPE,
                FilesName = ct.File.FilesName,
                Filedata = ct.File.Filedata,
                FileType = ct.File.FileType,
                DocumentTypes = ct.File.DocumentTypeNavigation.DocumentType1,
                DescriptionFile = ct.File.DescriptionFile,
                UserId = ct.UserId,
            })
             .AsNoTracking()
             .ToListAsync();
        }

        #endregion
        #region PRIVATE METHODS
        private async Task<List<FolderContractorDto>> GetFolderContract(Guid id)
        {

            var contractor = _context.DetailProjectContractor
                .Include(dt => dt.Contractor)
                .Include(dt => dt.Contract)
                    .ThenInclude(i => i.Files)
                .Where(x => x.ContractId.Equals(id));
            return await contractor.Select(ct => new FolderContractorDto
            {
                Type = FOLDERTYPE,
                Id = ct.Contractor.Id.ToString(),
                Nombre = ct.Contractor.Nombre + " " + ct.Contractor.Apellido,
                Identificacion = ct.Contractor.Identificacion,
            })
             .AsNoTracking()
             .ToListAsync();
        }


        private async Task<List<FolderContractDto>?> GetFoldersContract(Guid id)
        {

            var getFiles = _context.Folder
                .Where(w => w.ContractId.Equals(id) && !w.ContractorId.HasValue && w.TypeFolder.Equals(FolderTypeEnum.CONTRATO.Description()));

            return await getFiles.Select(fc => new FolderContractDto
            {
                Id = fc.Id,
                TypeFolder = FILETYPE,
                FolderName = fc.FolderName,
            })
             .AsNoTracking()
             .ToListAsync();
        }




        #endregion
    }
}
