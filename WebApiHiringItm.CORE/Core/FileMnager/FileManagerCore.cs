using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.FileMnager.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.FolderType;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContract;
using WebApiHiringItm.CORE.Helpers.Enums.StatusFile;
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
            fileManagerDto.ContractName = _context.ContractFolder.Where(w => w.Id.Equals(id)).Select(s => s.ProjectName).FirstOrDefault();
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
                .OrderBy(o => o.RegisterDate)
                .Where(w => !w.Contract.StatusContractId.Equals(getStatusContract) && w.Contract.Activate);
            var contracts =  await contractor
                .GroupBy(g => new
                {
                    g.Contract.Id,
                    g.Contract.CompanyName,
                    g.Contract.NumberProject,
                    g.Contract.ProjectName
                })
                .Select(ct => new FolderContractContractorDto
            {
                Type = FOLDERTYPE,
                Id = ct.Key.Id.ToString(),
                CompanyName = ct.Key.CompanyName,
                ProjectName = ct.Key.ProjectName,
                ProjectNumber = ct.Key.NumberProject
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
                DocumentTypes = ct.File.DocumentTypeNavigation.DocumentTypeDescription,
                DescriptionFile = ct.File.DescriptionFile,
                UserId = ct.UserId
            })
             .AsNoTracking()
             .ToListAsync();
        }

        #endregion
        #region PRIVATE METHODS
        private async Task<List<FolderContractorDto>> GetFolderContract(Guid id)
        {
            var getStatusFiles = _context.DetailFile
                .Include(i => i.File)
                    .ThenInclude(i => i.DocumentTypeNavigation)
                .Include(i => i.StatusFile);
            var contractor = _context.DetailContractor
                .Include(dt => dt.Contract)
                    .ThenInclude(i => i.Files)
                    .OrderBy(o => o.Contractor.Nombres)
                .Where(x => x.ContractId.Equals(id));
            return await contractor.Select(ct => new FolderContractorDto
            {
                Type = FOLDERTYPE,
                Id = ct.Contractor.Id.ToString(),
                ContractorName = ct.Contractor.Nombres + " " + ct.Contractor.Apellidos,
                ContractorIdentification = ct.Contractor.Identificacion,
                cantFile = getStatusFiles.Where(w => w.File.ContractId.Equals(ct.ContractId) && (w.ContractorId.Equals(ct.ContractorId) && ((w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.EXAMENESPREOCUPACIONALESCODE.Description())
                || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.HOJADEVIDACODE.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.DOCUMENTOSCONTRATACION.Description()) || w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.REGISTROSECOPCODE.Description()))))).ToList().Count
            })
             .AsNoTracking()
             .ToListAsync();
        }


        private async Task<List<FolderContractDto>?> GetFoldersContract(Guid id)
        {

            var getFiles = _context.Folder
                .Where(w => w.ContractId.Equals(id) && w.ContractorId == null && w.FolderTypeNavigation.Code.Equals(FolderTypeCodeEnum.CONTRATO.Description()));

            return await getFiles.Select(fc => new FolderContractDto
            {
                Id = fc.Id,
                FolderType = FILETYPE,
                FolderName = fc.FolderName,
            })
             .AsNoTracking()
             .ToListAsync();
        }




        #endregion
    }
}
