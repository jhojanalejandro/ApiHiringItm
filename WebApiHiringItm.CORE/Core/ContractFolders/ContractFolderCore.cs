using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Assignment;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContract;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContractor;
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
        private const string MODULONOMINA = "Nomina";

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
            var result = _context.ContractFolder
                .Include(i => i.RubroNavigation)
                .Include(i => i.StatusContract);

            return await result.Select(contract => new ContractListDto
            {
                Id = contract.Id,
                CompanyName = contract.CompanyName,
                ProjectName = contract.ProjectName,
                Activate = contract.Activate,
                EnableProject = contract.EnableProject,
                ContractorsCant = contract.ContractorsCant,
                ValorContrato = contract.ValorContrato,
                NumberProject = contract.NumberProject,
                Project = contract.Project,
                Rubro = contract.RubroNavigation.RubroNumber,
                NombreRubro = contract.RubroNavigation.Rubro,
                FuenteRubro = contract.RubroNavigation.RubroOrigin,
                StatusContract = contract.StatusContract.StatusContractDescription,
                FechaContrato = contract.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                FechaFinalizacion = contract.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault()
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<ContractListDto>> GetAllActivate()
        {
            var getStatusContract = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.TERMINADO.Description())).ToList();
            var result = _context.ContractFolder.Where(x => x.Activate  && !x.StatusContractId.Equals(getStatusContract));
            var resultDetail = _context.DetailContract.ToList();
            return await result.Select(contract => new ContractListDto
            {
                Id = contract.Id,
                CompanyName = contract.CompanyName,
                ProjectName = contract.ProjectName,
                Activate = contract.Activate,
                EnableProject = contract.EnableProject,
                ContractorsCant = contract.ContractorsCant,
                ValorContrato = contract.ValorContrato,
                NumberProject = contract.NumberProject,
                Project = contract.Project,
                StatusContract = contract.StatusContract.StatusContractDescription,
                StatusContractId = contract.StatusContract.Id.ToString().ToLower(),
                FechaContrato = contract.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                FechaFinalizacion = contract.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault(),
                ValorSubTotal = contract.ValorSubTotal,
                GastosOperativos = contract.GastosOperativos,
                Rubro = contract.RubroNavigation.RubroNumber,
                RubroId = contract.RubroNavigation.Id.ToString().ToLower(),
                NombreRubro = contract.RubroNavigation.Rubro,
                FuenteRubro = contract.FuenteRubro,
                ObjectContract = contract.ObjectContract

            }).AsNoTracking()
              .ToListAsync();
        }

        public async Task<List<ContractListDto>> GetAllInProgess(string typeModule)
        {
            var getStatusContract = _context.StatusContract.Where(x =>  x.Code.Equals(StatusContractEnum.TERMINADO.Description())).Select(s => s.Id).FirstOrDefault();
            var getStatusContractInprogess = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.ENPROCESO.Description())).Select(s => s.Id).FirstOrDefault();

            IQueryable<ContractFolder> result; 
            if (typeModule.Equals(MODULONOMINA))
            {
                result = _context.ContractFolder.Where(x => x.Activate && !x.StatusContractId.Equals(getStatusContract)  && x.EnableProject && x.StatusContractId.Equals(getStatusContractInprogess));
            }
            else
            {
                result = _context.ContractFolder.Where(x => x.Activate && !x.StatusContractId.Equals(getStatusContract) && x.StatusContractId.Equals(getStatusContractInprogess));

            }
            return await result.Select(contract => new ContractListDto
            {
                Id = contract.Id,
                CompanyName = contract.CompanyName,
                ProjectName = contract.ProjectName,
                Activate = contract.Activate,
                ObjectContract = contract.ObjectContract,
                EnableProject = contract.EnableProject,
                ContractorsCant = contract.ContractorsCant,
                ValorContrato = contract.ValorContrato,
                NumberProject = contract.NumberProject,
                Project = contract.Project,
                StatusContract = contract.StatusContract.StatusContractDescription,
                FechaContrato = contract.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                FechaFinalizacion = contract.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault(),
                ValorSubTotal = contract.ValorSubTotal,
                GastosOperativos = contract.GastosOperativos,
                Rubro = contract.RubroNavigation.RubroNumber,
                NombreRubro = contract.RubroNavigation.Rubro,
                FuenteRubro = contract.FuenteRubro,
                IsAssigmentUser = contract.AssigmentContract.Where(w => w.AssignmentTypeNavigation.Code.Equals(AssignmentEnum.RESPONSABLECONTRATO.Description())).Select(s => s.User).ToList().Count > 0 ? "ASIGNADO" : "NO ASIGNADO",
            }).AsNoTracking()
              .ToListAsync();

        }
        public async Task<List<DetalleContratoDto>> GetDetailByIdList(Guid ContractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(ContractId)).ToList();
            var mapp = _mapper.Map<List<DetalleContratoDto>>(result);

            return await Task.FromResult(mapp);

        }

        public async Task<DetalleContratoDto> GetDetailByIdContract(Guid ContractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(ContractId)).FirstOrDefault();
            var mapp = _mapper.Map<DetalleContratoDto>(result);

            return await Task.FromResult(mapp);

        }
        public async Task<DetalleContratoDto?> GetDetailByIdLastDate(Guid contractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(contractId)).Select(x => new DetalleContratoDto()
            {
                ContractId = x.ContractId,
                FechaContrato = x.FechaContrato,
                FechaFinalizacion = x.FechaFinalizacion,
                TipoContrato = x.TipoContrato,
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

        public async Task<bool> SaveContract(RProjectForlderDto model)
        {
            var getData = _context.ContractFolder.FirstOrDefault(x => x.Id.Equals(model.Id));

            if (getData == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<ContractFolder>(model);
                _context.ContractFolder.Add(map);
                var res = await _context.SaveChangesAsync();
                if (res != null)
                {
                    DetalleContratoDto detalle = model.DetalleContratoDto;
                    detalle.ContractId = map.Id;
                    if (!await CreateDetail(detalle))
                        return false;
                }
                return res != 0 ? true : false;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                if (model.DetalleContratoDto.ContractId != null && model.DetalleContratoDto.Update)
                {
                    DetalleContratoDto detalle = model.DetalleContratoDto;
                    if (!await CreateDetail(detalle))
                        return false;
                }
                _context.ContractFolder.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            } 
        }

        public async Task<bool> UpdateStateContract(Guid id)
        {
            var getData = _context.ContractFolder.FirstOrDefault(x => x.Id.Equals(id));
            var getStatusContractId = _context.StatusContractor.FirstOrDefault(x => x.Code.Equals(StatusContractorEnum.CONTRATADO.Description()));
            var getContracts = _context.DetailContractor.Where(w => w.ContractId.Equals(id)).ToList();
            foreach(var item in getContracts)
            {
                item.StatusContractor = getStatusContractId.Id;
            }

            if (getData != null)
            {
                getData.EnableProject = true;
                _context.ContractFolder.Update(getData);
                _context.DetailContractor.UpdateRange(getContracts);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }

            return false;

        }
        public async Task<bool> UpdateCost(ProjectFolderCostsDto model)
        {
            var getData = _context.ContractFolder.FirstOrDefault(x => x.Id == model.Id);

            if (getData != null)
            {
                var map = _mapper.Map(model, getData);
                _context.ContractFolder.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }

            return false;

        }

        public async Task<bool> Delete(Guid id)
        {
            var getStatusContract = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.TERMINADO.Description())).Select(s => s.Id).FirstOrDefault();
            var resultData = _context.ContractFolder.FirstOrDefault(x => x.Id == id);
            if (resultData != null)
            {
                resultData.Activate = false;
                resultData.StatusContractId = getStatusContract;
                var result = _context.ContractFolder.Update(resultData);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ContractFolderDto>> GetAllProjectsRegistered()
        {
            var projects = _context.ContractFolder
               .Include(i => i.DetailContract);
            return await projects
                .Select(s => new ContractFolderDto
                {
                    Id = s.Id,
                    Project = s.Project,
                    CompanyName = s.CompanyName,
                    ProjectName = s.ProjectName,
                    ObjectContract = s.ObjectContract,
                    Activate = s.EnableProject,
                    EnableProject = s.EnableProject,
                    ContractorsCant = s.ContractorsCant,
                    ValorContrato = s.ValorContrato,
                    GastosOperativos = s.GastosOperativos,
                    ValorSubTotal = s.ValorSubTotal,
                    NumberProject = s.NumberProject,
                    Rubro = s.RubroNavigation.RubroNumber,
                    NombreRubro = s.RubroNavigation.Rubro,
                    FuenteRubro = s.RubroNavigation.RubroOrigin,
                    FechaContrato = s.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                    FechaFinalizacion = s.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AssignmentUser(List<AssignmentUserDto> modelAssignment)
        {
            var getContractAssignment = _context.AssigmentContract.Where(x => x.ContractId == modelAssignment[0].Id).ToList();
            List<AssignmentUserDto> updateAssignment = new();
            List<AssignmentUserDto> addAssignment = new();
            if (getContractAssignment.Count > 0)
            {
                updateAssignment = modelAssignment.Where(w =>
                         getContractAssignment.Any(a => a.UserId.Equals(w.UserId) && a.ContractId.Equals(w.ContractId))).ToList();
                addAssignment = modelAssignment.Where(w =>
                         getContractAssignment.Any(a => a.UserId != w.UserId && a.ContractId.Equals(w.ContractId))).ToList();

                if (updateAssignment.Count > 0)
                {
                    var map = _mapper.Map(updateAssignment, getContractAssignment);
                    _context.AssigmentContract.UpdateRange(map);
                }
                if (addAssignment.Count > 0)
                {
                    var mapAdd = _mapper.Map<List<AssigmentContract>>(addAssignment);
                    _context.AssigmentContract.AddRange(mapAdd);
                }

            }
            else
            {
                var mapAdd = _mapper.Map<List<AssigmentContract>>(modelAssignment);
                _context.AssigmentContract.AddRange(mapAdd);
            }
            var res = await _context.SaveChangesAsync();
            return res != 0 ? true : false;

        }

        public async Task<bool> SaveTermFileContract(TermContractDto modelTermContract)
        {
            var getDetailConttract = _context.DetailContract.FirstOrDefault(x => x.ContractId.Equals(modelTermContract.DetailContract));
            var getTermContract = _context.TermContract.FirstOrDefault(x => x.DetailContract.Equals(getDetailConttract.Id));

            if (getTermContract == null)
            {
                modelTermContract.Id = Guid.NewGuid();
                modelTermContract.DetailContract = getDetailConttract.Id;
                var mapTermContract = _mapper.Map<TermContract>(modelTermContract);
                _context.TermContract.Add(mapTermContract);
            }
            else
            {
                modelTermContract.Id = getTermContract.Id;
                modelTermContract.DetailContract = getTermContract.DetailContract;
                var mapTermContractUpdate = _mapper.Map(modelTermContract, getTermContract);
                _context.TermContract.Update(mapTermContractUpdate);
            }
            var res = await _context.SaveChangesAsync();
            return res != 0 ? true : false;
        }
        #endregion

        #region METODOS PRIVADOS
        private async Task<bool> CreateDetail(DetalleContratoDto model)
        {
            var getData = _context.DetailContract.FirstOrDefault(x => x.FechaFinalizacion == model.FechaFinalizacion && x.ContractId.Equals(model.ContractId));

            if (getData == null)
            {
                var map = _mapper.Map<DetailContract>(model);
                map.Id = Guid.NewGuid();
                _context.DetailContract.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else{
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.DetailContract.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
        }

      
        #endregion

    }
}
