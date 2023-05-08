using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.ProjectFolders
{
    public class ProjectFolderCore : IProjectFolder
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly IComponenteCore _componente;
        private readonly IElementosComponenteCore _elementos;
        private const string MODULONOMINA = "Nomina";
        private const string TIPOCONTRATOINICIO = "Inicial";

        #region Builder
        public ProjectFolderCore(HiringContext context, IMapper mapper, IComponenteCore componente, IElementosComponenteCore elementos)
        {
            _context = context;
            _mapper = mapper;
            _componente = componente;
            _elementos = elementos;
        }

        #endregion
        #region PUBLIC METHODS

        public async Task<List<ProjectFolderDto>> GetAll()
        {
            var result = _context.ProjectFolder.Where(x => x.Id != null).ToList();
            var map = _mapper.Map<List<ProjectFolderDto>>(result);
            if (result.Count != 0)
            {
                foreach (var item in map)
                {
                    item.Componentes = await _componente.Get(item.Id);
                    if (item.Componentes.Count != 0)
                    {
                        foreach (var element in item.Componentes)
                        {
                            element.Elementos = await _elementos.Get(element.Id);
                        }
                    }
                }
            }
            return await Task.FromResult(map);
        }
        public async Task<List<ProjectFolderDto>> GetAllActivate()
        {
            var result = _context.ProjectFolder.Where(x => x.Id != null && x.Activate == true).ToList();
            var resultDetail = _context.DetalleContrato.Where(x => x.Id != null).ToList();

            var map = _mapper.Map<List<ProjectFolderDto>>(result);
            if (result.Count != 0)
            {
                foreach (var item in map)
                {
                    item.FechaFinalizacion = resultDetail.Where(f => f.ContractId.Equals(item.Id)).Select(s => s.FechaFinalizacion).FirstOrDefault();
                    item.FechaContrato = resultDetail.Where(f => f.ContractId.Equals(item.Id)).Select( s => s.FechaContrato).FirstOrDefault();
                    item.Componentes = await _componente.Get(item.Id);
                    if (item.Componentes.Count != 0)
                    {
                        foreach (var element in item.Componentes)
                        {
                            element.Elementos = await _elementos.Get(element.Id);
                        }
                    }
                }
            }
            return await Task.FromResult(map);
        }

        public async Task<List<ProjectFolderDto>> GetAllInProgess(string typeModule)
        {
            List<ProjectFolder> result; 
            if (typeModule.Equals(MODULONOMINA))
            {
                result = _context.ProjectFolder.Where(x => x.Activate == true && x.Execution == true && x.EnableProject == true).ToList();
            }
            else
            {
                result = _context.ProjectFolder.Where(x => x.Activate == true && x.Execution == true).ToList();

            }
            var map = _mapper.Map<List<ProjectFolderDto>>(result);
            if (result.Count != 0)
            {
                foreach (var item in map)
                {
                    item.Componentes = await _componente.Get(item.Id);
                    if (item.Componentes.Count != 0)
                    {
                        foreach (var element in item.Componentes)
                        {
                            element.Elementos = await _elementos.Get(element.Id);
                        }
                    }
                    //item.DetalleContrato = await GetDetailsById(item.Id);
                }
            }
            return await Task.FromResult(map);
        }
        public async Task<List<DetalleContratoDto>> GetDetailByIdList(Guid idContrato)
        {

            var result = _context.DetalleContrato.Where(x => x.ContractId.Equals(idContrato)).ToList();
            var mapp = _mapper.Map<List<DetalleContratoDto>>(result);

            return await Task.FromResult(mapp);

        }

        public async Task<DetalleContratoDto> GetDetailById(Guid idContrato)
        {

            var result = _context.DetalleContrato.Where(x => x.ContractId.Equals(idContrato) && x.TipoContrato.Equals(TIPOCONTRATOINICIO)).FirstOrDefault();
            var mapp = _mapper.Map<DetalleContratoDto>(result);

            return await Task.FromResult(mapp);

        }
        public async Task<DetalleContratoDto?> GetDetailByIdLastDate(Guid contractId)
        {

            var result = _context.DetalleContrato.Where(x => x.ContractId.Equals(contractId)).Select(x => new DetalleContratoDto()
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


        public async Task<ProjectFolderDto> GetById(Guid id)
        {
            var result = _context.ProjectFolder.FirstOrDefault(x => x.Id == id);
            var map = _mapper.Map<ProjectFolderDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Create(RProjectForlderDto model)
        {
            var getData = _context.ProjectFolder.FirstOrDefault(x => x.Id == model.Id);

            if (getData == null)
            {
                model.Id = Guid.NewGuid();
                var map = _mapper.Map<ProjectFolder>(model);
                _context.ProjectFolder.Add(map);
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
                _context.ProjectFolder.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            } 
            return false;

        }

        public async Task<bool> UpdateState(Guid id)
        {
            var getData = _context.ProjectFolder.FirstOrDefault(x => x.Id == id);

            if (getData != null)
            {
                getData.EnableProject = true;
                getData.CompanyName = getData.CompanyName;
                getData.Activate = getData.Activate;
                _context.ProjectFolder.Update(getData);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }

            return false;

        }
        public async Task<bool> UpdateCost(ProjectFolderCostsDto model)
        {
            var getData = _context.ProjectFolder.FirstOrDefault(x => x.Id == model.Id);

            if (getData != null)
            {
                var map = _mapper.Map(model, getData);
                _context.ProjectFolder.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }

            return false;

        }

        public async Task<bool> Delete(Guid id)
        {
            var resultData = _context.ProjectFolder.FirstOrDefault(x => x.Id == id);
            if (resultData != null)
            {
                resultData.Activate = false;
                var result = _context.ProjectFolder.Update(resultData);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ProjectFolderDto>> GetAllProjectsRegistered()
        {
            var projects = _context.ProjectFolder
               .Include(i => i.DetalleContrato);
            return await projects
                .Select(s => new ProjectFolderDto
                {
                    Id = s.Id,
                    Project = s.Project,
                    CompanyName = s.CompanyName,
                    ProjectName = s.ProjectName,
                    DescriptionProject = s.DescriptionProject,
                    Execution = s.Execution,
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
                    FechaContrato = s.DetalleContrato.Select(s => s.FechaContrato).FirstOrDefault(),
                    FechaFinalizacion = s.DetalleContrato.Select(s => s.FechaFinalizacion).FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
        #region METODOS PRIVADOS
        private async Task<bool> CreateDetail(DetalleContratoDto model)
        {
            var getData = _context.DetalleContrato.FirstOrDefault(x => x.FechaFinalizacion == model.FechaFinalizacion && x.ContractId.Equals(model.ContractId));

            if (getData == null)
            {
                var map = _mapper.Map<DetalleContrato>(model);
                map.Id = Guid.NewGuid();
                _context.DetalleContrato.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else{
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.DetalleContrato.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
        }

      
        #endregion

    }
}
