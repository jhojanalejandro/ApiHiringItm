using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.File;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContract;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
using WebApiHiringItm.CORE.Helpers.Enums.StatusFile;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Helpers.GenericValidation;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contrato;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.ContractFolders
{
    public class ContractFolderCore : IProjectFolder
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private const string MODULONOMINA = "NOMINA";
        private const string MODULOCONTRACTUAL = "CONTRACTUAL";

        #region Builder
        public ContractFolderCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion
       
        #region PUBLIC METHODS

        public async Task<List<ContractListDto>> GetAllHistory()
        {
            var result = _context.DetailContract.Include(i => i.Contract)
                .Include(i => i.RubroNavigation)
                .Include(i => i.StatusContract);

            return await result.Select(contract => new ContractListDto
            {
                Id = contract.ContractId,
                CompanyName = contract.Contract.CompanyName,
                ProjectName = contract.Contract.ProjectName,
                Activate = contract.Activate,
                EnableProject = contract.Contract.EnableProject,
                ContractorsCant = contract.ContractorsCant,
                ValorContrato = contract.ValorContrato,
                NumberProject = contract.Contract.NumberProject,
                Project = contract.Contract.Project,
                Rubro = contract.RubroNavigation.RubroNumber,
                NombreRubro = contract.RubroNavigation.Rubro,
                FuenteRubro = contract.FuenteRubro,
                StatusContract = contract.StatusContract.StatusContractDescription,
                FechaContrato = contract.FechaContrato,
                FechaFinalizacion = contract.FechaFinalizacion,
                DutyContract = contract.Contract.DutyContract
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<ContractListDto>> GetAllActivate()
        {
            var getStatusContract = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.TERMINADO.Description())).ToList();
            var result = _context.DetailContract.Include(i => i.Contract).Where(x => x.Activate  && !x.StatusContractId.Equals(getStatusContract));
            var resultDetail = _context.DetailContract.ToList();
            return await result.Select(contract => new ContractListDto
            {
                Id = contract.ContractId,
                CompanyName = contract.Contract.CompanyName,
                ProjectName = contract.Contract.ProjectName,
                Activate = contract.Activate,
                EnableProject = contract.Contract.EnableProject,
                ContractorsCant = contract.ContractorsCant,
                ValorContrato = contract.ValorContrato,
                NumberProject = contract.Contract.NumberProject,
                Project = contract.Contract.Project,
                StatusContract = contract.StatusContract.StatusContractDescription,
                StatusContractId = contract.StatusContract.Id.ToString().ToLower(),
                FechaContrato = contract.FechaContrato,
                FechaFinalizacion = contract.FechaFinalizacion,
                ValorSubTotal = contract.ValorSubTotal,
                GastosOperativos = contract.GastosOperativos,
                Rubro = contract.RubroNavigation.RubroNumber,
                RubroId = contract.RubroNavigation.Id.ToString().ToLower(),
                NombreRubro = contract.RubroNavigation.Rubro,
                FuenteRubro = contract.FuenteRubro,
                ObjectContract = contract.Contract.ObjectContract,
                DetailContractId = contract.Id.ToString(),
                DetailType = contract.DetailType.ToString(),
                DutyContract = contract.Contract.DutyContract,
                ResourceContract = contract.ResourceContract,
                AreaCode = contract.Contract.AreaCode

            }).AsNoTracking()
              .ToListAsync();
        }

        public async Task<List<ContractListDto>> GetAllInProgess(string typeModule)
        {

            IQueryable<DetailContract> result; 
            if (typeModule.Equals(MODULONOMINA))
            {
                result = _context.DetailContract
                    .Include(i => i.Contract).OrderByDescending(c => c.Consecutive).Where(x => x.Activate && !x.StatusContract.Code.Equals(StatusContractEnum.TERMINADO.Description())  && x.Contract.EnableProject && (x.StatusContract.Code.Equals(StatusContractEnum.ENPROCESO.Description()) || x.StatusContract.Code.Equals(StatusContractEnum.ENEJECUCIÓN.Description())));
            }
            else 
            {
                result = _context.DetailContract.Include(i => i.Contract).OrderByDescending(c => c.Consecutive).Where(x => x.Activate && !x.StatusContract.Code.Equals(StatusContractEnum.TERMINADO.Description()) && (x.StatusContract.Code.Equals(StatusContractEnum.ENPROCESO.Description()) || x.StatusContract.Code.Equals(StatusContractEnum.ENEJECUCIÓN.Description())));

            }
            return await result.Select(contract => new ContractListDto
            {
                Id = contract.ContractId,
                CompanyName = contract.Contract.CompanyName,
                ProjectName = contract.Contract.ProjectName,
                Activate = contract.Activate,
                ObjectContract = contract.Contract.ObjectContract,
                EnableProject = contract.Contract.EnableProject,
                ContractorsCant = contract.ContractorsCant,
                ValorContrato = contract.ValorContrato,
                NumberProject = contract.Contract.NumberProject,
                Project = contract.Contract.Project,
                StatusContract = contract.StatusContract.StatusContractDescription,
                FechaContrato = contract.Contract.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                FechaFinalizacion = contract.Contract.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault(),
                ValorSubTotal = contract.ValorSubTotal,
                GastosOperativos = contract.GastosOperativos,
                Rubro = contract.RubroNavigation.RubroNumber,
                NombreRubro = contract.RubroNavigation.Rubro,
                FuenteRubro = contract.FuenteRubro,
                DetailContractId = contract.Contract.DetailContract.OrderByDescending(o => o.Consecutive).Select(s => s.Id.ToString()).FirstOrDefault(),
                IsAssigmentUser = contract.Contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.CONTRACTUALCONTRATO.Description())).Select(s => s.User).ToList().Count > 0 ? "ASIGNADO" : "NO ASIGNADO",
                DetailType = contract.Contract.DetailContract.OrderByDescending(o => o.Consecutive).Select(s => s.DetailType.ToString()).FirstOrDefault(),
                DutyContract = contract.Contract.DutyContract
            }).AsNoTracking()
              .ToListAsync();

        }
        public async Task<List<DetailContractDto>> GetDetailByIdList(Guid ContractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(ContractId)).ToList();
            var mapp = _mapper.Map<List<DetailContractDto>>(result);

            return await Task.FromResult(mapp);

        }

        public async Task<DetailContractDto> GetDetailByIdContract(Guid ContractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(ContractId)).FirstOrDefault();
            var mapp = _mapper.Map<DetailContractDto>(result);

            return await Task.FromResult(mapp);

        }
        public async Task<DetailContractDto?> GetDetailByIdLastDate(Guid contractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(contractId)).Select(x => new DetailContractDto()
            {
                ContractId = x.ContractId,
                FechaContrato = x.FechaContrato,
                FechaFinalizacion = x.FechaFinalizacion,
                DetailType = x.DetailType,
            })
                .OrderByDescending(x => x.ContractId)
                .AsNoTracking()
                .FirstOrDefault();

            return await Task.FromResult(result);
        }


        public async Task<ContractFolderDto> GetById(Guid id)
        {
            var result = _context.ContractFolder.FirstOrDefault(x => x.Id.Equals(id));
            var map = _mapper.Map<ContractFolderDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<IGenericResponse<string>> SaveContract(RProjectForlderDto model)
        {
            if (string.IsNullOrEmpty(model.DetalleContrato.UserId) || !model.DetalleContrato.UserId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.GUIDNOTVALID);

            if (string.IsNullOrEmpty(model.DetalleContrato.StatusContractId) || !model.DetalleContrato.StatusContractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.GUIDNOTVALID);


            var getData = _context.DetailContract.Include(i => i.Contract).FirstOrDefault(x => x.ContractId.Equals(model.Id));
            int resp = 0;
            if (getData == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<ContractFolder>(model);
                _context.ContractFolder.Add(map);
                model.DetalleContrato.ContractId = map.Id;
                model.DetalleContrato.Consecutive = 1;
                CreateDetail(model.DetalleContrato);
                resp = await _context.SaveChangesAsync();

                if (resp != 0)
                {
                    return ApiResponseHelper.CreateResponse<string>(null,true,Resource.REGISTERSUCCESSFULL);
                }
                else
                {
                    return ApiResponseHelper.CreateErrorResponse<string>(Resource.INFORMATIONEMPTY);
                }
            }
            return await updateDetail(model.DetalleContrato, getData);

        }

        public async Task<IGenericResponse<string>> UpdateStateContract(string contractId)
        {
            if (string.IsNullOrEmpty(contractId) || !contractId.IsGuid())
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.GUIDNOTVALID);

            var getStatucContract = _context.StatusContract.Where(w => w.Code.Equals(StatusContractEnum.ENEJECUCIÓN.Description())).Select(s => s.Id).FirstOrDefault();
            var getData = _context.DetailContract.Include(i => i.Contract).FirstOrDefault(x => x.Id.Equals(Guid.Parse(contractId)));
            var getStatusContractorId = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.CONTRATADO.Description())).Select(s => s.Id).FirstOrDefault();
    
            if (validateActationContract(Guid.Parse(contractId)))
            {
                var getDetailContractors = _context.DetailContractor
                  .Where(w => w.ContractId.Equals(Guid.Parse(contractId)) &&
                  w.Contractor.DetailFile.Where(wd => wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description())).OrderByDescending(o => o.RegisterDate).Select(S => S.StatusFile.Code).FirstOrDefault()!.Equals(StatusFileEnum.APROBADO.Description())
                  && w.Contractor.DetailFile.Where(wd => wd.StatusFile.Category.Equals(CategoryEnum.CREADO.Description())
                  && wd.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.ESTUDIOSPREVIOS.Description())).OrderByDescending(o => o.RegisterDate).Select(S => S.StatusFile.Code).FirstOrDefault()!.Equals(StatusFileEnum.APROBADO.Description())).ToList();
                foreach (var item in getDetailContractors)
                {
                    if (item.StatusContractor != getStatusContractorId)
                    {
                        item.StatusContractor = getStatusContractorId;
                    }
                }


                if (getData != null && getDetailContractors.Count > 0)
                {
                    getData.Contract.EnableProject = true;
                    getData.StatusContractId = getStatucContract;
                    _context.ContractFolder.Update(getData.Contract);
                    _context.DetailContract.Update(getData);
                    _context.DetailContractor.UpdateRange(getDetailContractors);
                    await _context.SaveChangesAsync();
                    return ApiResponseHelper.CreateResponse<string>(null, true, Resource.UPDATESUCCESSFULL);

                }
            }
        
            return ApiResponseHelper.CreateErrorResponse<string>(Resource.NOBODYHIRING);


        }
        public async Task<IGenericResponse<string>> UpdateCost(ProjectFolderCostsDto model)
        {

            var getData = _context.DetailContract.OrderByDescending(o => o.Consecutive).FirstOrDefault(x => x.ContractId.Equals(model.Id));
            try
            {
                  if (getData != null)
                {
                    model.Id = getData.Id;
                    var map = _mapper.Map(model, getData);
                    _context.DetailContract.Update(map);
                }
                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.UPDATESUCCESSFULL);
            }
            catch(Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<string>(ex.Message);

            }


        }

        public async Task<bool> Delete(Guid id)
        {
            var getStatusContract = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.TERMINADO.Description())).Select(s => s.Id).FirstOrDefault();
            var resultData = _context.DetailContract.Include(i => i.Contract).FirstOrDefault(x => x.Id == id);
            if (resultData != null)
            {
                resultData.Activate = false;
                resultData.StatusContractId = getStatusContract;
                _context.DetailContract.Update(resultData);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ContractFolderDto>> GetAllProjectsRegistered()
        {
            var projects = _context.DetailContract
               .Include(i => i.Contract);
            return await projects
                .Select(s => new ContractFolderDto
                {
                    Id = s.Id,
                    Project = s.Contract.Project,
                    CompanyName = s.Contract.CompanyName,
                    ProjectName = s.Contract.ProjectName,
                    ObjectContract = s.Contract.ObjectContract,
                    Activate = s.Activate,
                    EnableProject = s.Contract.EnableProject,
                    ContractorsCant = s.ContractorsCant,
                    ValorContrato = s.ValorContrato,
                    GastosOperativos = s.GastosOperativos,
                    ValorSubTotal = s.ValorSubTotal,
                    NumberProject = s.Contract.NumberProject,
                    Rubro = s.RubroNavigation.RubroNumber,
                    NombreRubro = s.RubroNavigation.Rubro,
                    FuenteRubro = s.FuenteRubro,
                    FechaContrato = s.FechaContrato,
                    FechaFinalizacion = s.FechaFinalizacion
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IGenericResponse<string>> AssignmentUser(List<AssignmentUserDto> modelAssignment)
        {
            var IdUserList = modelAssignment.Select(s => s.UserId).ToList();
            var getContractAssignment = _context.AssigmentContract.Where(x => x.ContractId == modelAssignment[0].ContractId).ToList();
            List<AssigmentContract> updateAssignment = new();
            List<AssigmentContract> addAssignment = new();
            if (getContractAssignment.Count > 0 && getContractAssignment.Any(a => IdUserList.Contains(a.UserId)))
            {

                foreach (var item in modelAssignment)
                {
                    var getAssignment = getContractAssignment.Where(w => w.UserId.Equals(item.UserId)).FirstOrDefault();
                    var getAssignmentUpdate = getContractAssignment.Where(w => w.UserId.Equals(item.UserId)).FirstOrDefault();

                    if (getAssignment != null)
                    {
                        item.Id = getAssignment.Id;
                        var mapAssignment = _mapper.Map(item,getAssignment);

                        updateAssignment.Add(mapAssignment);
                    }
                    else
                    {
                        var mapAdd = _mapper.Map<AssigmentContract>(item);
                        addAssignment.Add(mapAdd);
                    }
                }


            }
            else
            {
                var mapAdd = _mapper.Map<List<AssigmentContract>>(modelAssignment);
                _context.AssigmentContract.AddRange(mapAdd);
            }
            if (updateAssignment.Count > 0)
            {
                _context.AssigmentContract.UpdateRange(updateAssignment);
            }
            if (addAssignment.Count > 0)
            {
                _context.AssigmentContract.AddRange(addAssignment);
            }
            await _context.SaveChangesAsync();
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);

        }

        public async Task<IGenericResponse<string>> SaveTermFileContract(TermContractDto modelTermContract)
        {

            var getTermContractList = _context.TermContract
                .Include(i => i.DetailContractorNavigation)
                    .ThenInclude(t => t.Contractor)
                .Include(i => i.TermTypeNavigation)
                .Where(w => w.DetailContractorNavigation.ContractId.Equals(Guid.Parse(modelTermContract.ContractId))).ToList();
            List<TermContract> termContractsList = new();
            List<TermContract> UpdatetermContractsList = new();
            if (modelTermContract.ContractorId.IsGuid() && !string.IsNullOrEmpty(modelTermContract.ContractorId))
            {
                var getTermContract = getTermContractList.FirstOrDefault(x => x.DetailContractorNavigation.ContractId.Equals(Guid.Parse(modelTermContract.ContractId)) && x.DetailContractorNavigation.ContractorId.Equals(Guid.Parse(modelTermContract.ContractorId)) && x.TermTypeNavigation.Id.Equals(modelTermContract.TermType));
                getTermContract.TermDate = modelTermContract.TermDate;

                UpdatetermContractsList.Add(getTermContract);
            }
            
            else
            {
                var getDetailConttract = _context.DetailContractor.Where(x => x.ContractId.Equals(Guid.Parse(modelTermContract.ContractId))).ToList();

                foreach (var item in getDetailConttract)
                {
                    var getTermContract = getTermContractList.Find(x => x.DetailContractor.Equals(item.Id) && x.TermType.Equals(modelTermContract.TermType));
                    if (getTermContract == null)
                    {
                        var mapTermContract = _mapper.Map<TermContract>(modelTermContract);
                        mapTermContract.Id = Guid.NewGuid();
                        mapTermContract.DetailContractor = item.Id;
                        termContractsList.Add(mapTermContract);

                    }
                    else
                    {
                        getTermContract.TermDate = modelTermContract.TermDate;
                        UpdatetermContractsList.Add(getTermContract);
                    }

                }

            }
            if (termContractsList.Count > 0)
            {
                _context.TermContract.AddRange(termContractsList);

            }
            if (UpdatetermContractsList.Count > 0)
            {
                _context.TermContract.UpdateRange(UpdatetermContractsList);

            }
            var res = await _context.SaveChangesAsync();
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
        }

        public async Task<List<ContractFolderDto>> GetStatusPostContractual()
        {
            var projects = _context.DetailContract
               .Include(i => i.Contract);
            return await projects
                .Select(s => new ContractFolderDto
                {
                    Id = s.Id,
                    Project = s.Contract.Project,
                    CompanyName = s.Contract.CompanyName,
                    ProjectName = s.Contract.ProjectName,
                    ObjectContract = s.Contract.ObjectContract,
                    Activate = s.Contract.EnableProject,
                    EnableProject = s.Contract.EnableProject,
                    ContractorsCant = s.ContractorsCant,
                    ValorContrato = s.ValorContrato,
                    GastosOperativos = s.GastosOperativos,
                    ValorSubTotal = s.ValorSubTotal,
                    NumberProject = s.Contract.NumberProject,
                    Rubro = s.RubroNavigation.RubroNumber,
                    NombreRubro = s.RubroNavigation.Rubro,
                    FuenteRubro = s.FuenteRubro,
                    FechaContrato = s.FechaContrato,
                    FechaFinalizacion = s.FechaFinalizacion
                })
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region METODOS PRIVADOS
        private void CreateDetail(DetailContractDto model)
        {
            var getDataDetail = _context.DetailContract.Where(x => x.ContractId.Equals(model.ContractId) && x.DetailType.Equals(model.DetailType)).OrderByDescending(o => o.Consecutive).FirstOrDefault();
            if (getDataDetail != null)
            {
                model.Consecutive = getDataDetail.Consecutive;
                model.RegisterDate = getDataDetail.RegisterDate;
                model.ModifyDate = DateTime.Now;
                var map = _mapper.Map(model, getDataDetail);
                _context.DetailContract.Update(map);

            } else
            {
                var map = _mapper.Map<DetailContract>(model);
                map.Id = Guid.NewGuid();
                _context.DetailContract.Add(map);
            }
        }

        private async Task<IGenericResponse<string>> updateDetail(DetailContractDto detailContract,DetailContract getData)
        {
            try
            {
                getData.RecursosAdicionales = detailContract.RecursosAdicionales;
                getData.RegisterDateContract = detailContract.RegisterDateContract;
                getData.StatusContractId = Guid.Parse(detailContract.StatusContractId);
                getData.RecursosAdicionales = detailContract.RecursosAdicionales;
                if (!string.IsNullOrEmpty(detailContract.Rubro) && detailContract.Rubro.IsGuid())
                    getData.Rubro = Guid.Parse(detailContract.Rubro);

                if (!string.IsNullOrEmpty(detailContract.FuenteRubro))
                    getData.FuenteRubro = detailContract.FuenteRubro;

                _context.DetailContract.Update(getData);
                await _context.SaveChangesAsync();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.INFORMATIONEMPTY);

            }

        }

        private bool validateActationContract(Guid contractId)
        {


            var getStatucContract = _context.StatusContract.Where(w => w.Code.Equals(StatusContractEnum.ENEJECUCIÓN.Description())).Select(s => s.Id).FirstOrDefault();
            var getData = _context.DetailContract.Include(i => i.Contract).FirstOrDefault(x => x.Id.Equals(contractId));
            var getStatusContractorId = _context.StatusContractor.Where(x => x.Code.Equals(StatusContractorEnum.CONTRATADO.Description())).Select(s => s.Id).FirstOrDefault();
            var getDetailContractors = _context.DetailFile
                .Include(i => i.Contractor)
                    .ThenInclude(i => i.DetailContractor)
                .OrderByDescending(o => o.RegisterDate)
                .Where(w => w.File.ContractId.Equals(contractId) &&
                w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.MINUTACODE.Description()) && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description())
                && w.StatusFile.Category.Equals(CategoryEnum.CREADO.Description())
                && w.File.DocumentTypeNavigation.Code.Equals(DocumentTypeEnum.ESTUDIOSPREVIOS.Description()) && w.StatusFile.Code.Equals(StatusFileEnum.APROBADO.Description())).ToList();

            if (getData != null && getDetailContractors.Count > 0)
            {
                return true;

            }
            return false;
        }
      
        #endregion

    }
}
