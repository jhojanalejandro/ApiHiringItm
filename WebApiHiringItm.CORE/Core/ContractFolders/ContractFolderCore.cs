using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.StatusContract;
using WebApiHiringItm.MODEL.Dto.Contrato;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.ContractFolders
{
    public class ContractFolderCore : IProjectFolder
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly IComponenteCore _componente;
        private readonly IElementosComponenteCore _elementos;
        private const string MODULONOMINA = "Nomina";
        private const string TIPOCONTRATOINICIO = "Inicial";

        #region Builder
        public ContractFolderCore(HiringContext context, IMapper mapper, IComponenteCore Component, IElementosComponenteCore elementos)
        {
            _context = context;
            _mapper = mapper;
            _componente = Component;
            _elementos = elementos;
        }

        #endregion
       
        #region PUBLIC METHODS

        public async Task<List<ContractListDto>> GetAllHistory()
        {
            var result = _context.ContractFolder
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
                StatusContract = contract.StatusContract.StatusContract1,
                FechaContrato = contract.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                FechaFinalizacion = contract.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault()
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<ContractListDto>> GetAllActivate()
        {
            var getStatusContract = _context.StatusContract.Where(x => x.Code.Equals(StatusContractEnum.TERMINADO.Description())).ToList();
            var result = _context.ContractFolder.Where(x => x.Id != null && x.Activate == true && !x.StatusContractId.Equals(getStatusContract));
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
                StatusContract = contract.StatusContract.StatusContract1,
                FechaContrato = contract.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                FechaFinalizacion = contract.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault(),
                ValorSubTotal = contract.ValorSubTotal,
                GastosOperativos = contract.GastosOperativos,

            }).AsNoTracking()
              .ToListAsync();
            //var map = _mapper.Map<List<ContractFolderDto>>(result);
            //if (result.Count != 0)
            //{
            //    foreach (var item in map)
            //    {
            //        item.Componentes = await _componente.Get(item.Id);
            //        if (item.Componentes.Count != 0)
            //        {
            //            foreach (var element in item.Componentes)
            //            {
            //                element.Elementos = await _elementos.Get(element.Id);
            //            }
            //        }
            //    }
            //}
            //return await Task.FromResult(map);
        }

        public async Task<List<ContractListDto>> GetAllInProgess(string typeModule)
        {
            var getStatusContract = _context.StatusContract.Where(x =>  x.Code.Equals(StatusContractEnum.TERMINADO.Description())).Select(s => s.Id).FirstOrDefault();

            IQueryable<ContractFolder> result; 
            if (typeModule.Equals(MODULONOMINA))
            {
                result = _context.ContractFolder.Where(x => x.Activate == true && !x.StatusContractId.Equals(getStatusContract)  && x.EnableProject == true);
            }
            else
            {
                result = _context.ContractFolder.Where(x => x.Activate == true && !x.StatusContractId.Equals(getStatusContract));

            }
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
                StatusContract = contract.StatusContract.StatusContract1,
                FechaContrato = contract.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                FechaFinalizacion = contract.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault(),
                ValorSubTotal = contract.ValorSubTotal,
                GastosOperativos = contract.GastosOperativos,

            }).AsNoTracking()
                .ToListAsync();
            //var map = _mapper.Map<List<ContractFolderDto>>(result);
            //if (result.Count != 0)
            //{
            //    foreach (var item in map)
            //    {
            //        item.Componentes = await _componente.Get(item.Id);
            //        if (item.Componentes.Count != 0)
            //        {
            //            foreach (var element in item.Componentes)
            //            {
            //                element.Elementos = await _elementos.Get(element.Id);
            //            }
            //        }
            //        //item.DetailContract = await GetDetailsById(item.Id);
            //    }
            //}
            //return await Task.FromResult(map);
        }
        public async Task<List<DetalleContratoDto>> GetDetailByIdList(Guid ContractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(ContractId)).ToList();
            var mapp = _mapper.Map<List<DetalleContratoDto>>(result);

            return await Task.FromResult(mapp);

        }

        public async Task<DetalleContratoDto> GetDetailById(Guid ContractId)
        {

            var result = _context.DetailContract.Where(x => x.ContractId.Equals(ContractId) && x.TipoContrato.Equals(TIPOCONTRATOINICIO)).FirstOrDefault();
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

        public async Task<bool> Create(RProjectForlderDto model)
        {
            var getData = _context.ContractFolder.FirstOrDefault(x => x.Id == model.Id);

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

        public async Task<bool> UpdateState(Guid id)
        {
            var getData = _context.ContractFolder.FirstOrDefault(x => x.Id.Equals(id));

            if (getData != null)
            {
                getData.EnableProject = true;
                getData.CompanyName = getData.CompanyName;
                getData.Activate = getData.Activate;
                _context.ContractFolder.Update(getData);
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
            var resultData = _context.ContractFolder.FirstOrDefault(x => x.Id == id);
            if (resultData != null)
            {
                resultData.Activate = false;
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
                    Rubro = s.Rubro,
                    NombreRubro = s.NombreRubro,
                    FuenteRubro = s.FuenteRubro,
                    FechaContrato = s.DetailContract.Select(s => s.FechaContrato).FirstOrDefault(),
                    FechaFinalizacion = s.DetailContract.Select(s => s.FechaFinalizacion).FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();
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
