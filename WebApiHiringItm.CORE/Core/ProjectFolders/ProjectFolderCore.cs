using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;
        private readonly IComponenteCore _componente;
        private readonly IElementosComponenteCore _elementos;

        public ProjectFolderCore(Hiring_V1Context context, IMapper mapper, IComponenteCore componente, IElementosComponenteCore elementos)
        {
            _context = context;
            _mapper = mapper;
            _componente = componente;
            _elementos = elementos;
        }
        #region METODOS PUBLICOS

        public async Task<List<ProjectFolderDto>> GetAll()
        {
            var result = _context.ProjectFolder.Where(x => x.Id > 0).ToList();
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
                    //item.DetalleContrato = await GetDetailById(item.Id);
                }
            }
            return await Task.FromResult(map);
        }

        public async Task<List<ProjectFolderDto>> GetAllInProgess()
        {
            var result = _context.ProjectFolder.Where(x => x.Execution == true).ToList();
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
                    //item.DetalleContrato = await GetDetailById(item.Id);
                }
            }
            return await Task.FromResult(map);
        }
        public async Task<List<DetalleContratoDto>> GetDetailById(int idContrato)
        {

            var result = _context.DetalleContrato.Where(x => x.Idcontrato == idContrato).ToList();

            var mapp = _mapper.Map<List<DetalleContratoDto>>(result);

            return await Task.FromResult(mapp);

        }

        public async Task<DetalleContratoDto> GetDetailByIdLastDate(int idContrato)
        {

            var result = _context.DetalleContrato.Where(x => x.Idcontrato == idContrato).Select(x => new DetalleContratoDto()
            {
                Idcontrato = x.Id,
                FechaContrato = x.FechaContrato,
                FechaFinalizacion = x.FechaFinalizacion,
                TipoContrato = x.TipoContrato,
                Modificacion = x.Modificacion
            })
                .OrderByDescending(x => x.Idcontrato)
                .AsNoTracking()
                .FirstOrDefault();

            return await Task.FromResult(result);
        }



        public async Task<ProjectFolderDto> GetById(int id)
        {
            var result = _context.ProjectFolder.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<ProjectFolderDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.ProjectFolder.Where(x => x.Id == id).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.ProjectFolder.Remove(resultData);
                    await _context.SaveChangesAsync();

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
            return false;
        }

        public async Task<bool> Create(RProjectForlderDto model)
        {
            var getData = _context.ProjectFolder.Where(x => x.Id == model.Id).FirstOrDefault();

            if (getData == null)
            {
                var map = _mapper.Map<ProjectFolder>(model);
                _context.ProjectFolder.Add(map);
                var res = await _context.SaveChangesAsync();

                if (map.Id != null)
                {
                    DetalleContratoDto detalle = model.DetalleContratoDto;
                    detalle.Idcontrato = map.Id;
                    if (!await CreateDetail(detalle))
                        return false;
                }

                return res != 0 ? true : false;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.ProjectFolder.Update(map);
                if (model.DetalleContratoDto.Idcontrato != 0 && model.DetalleContratoDto.Update)
                {
                    DetalleContratoDto detalle = model.DetalleContratoDto;
                    if (!await CreateDetail(detalle))
                        return false;
                }
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            } 
            return false;

        }

        public async Task<bool> UpdateCost(ProjectFolderCostsDto model)
        {
            var getData = _context.ProjectFolder.Where(x => x.Id == model.Id).FirstOrDefault();

            if (getData != null)
            {
                var map = _mapper.Map(model, getData);
                _context.ProjectFolder.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }

            return false;

        }
        #endregion
        #region METODOS PRIVADOS
        private async Task<bool> CreateDetail(DetalleContratoDto model)
        {
            var getData = _context.DetalleContrato.Where(x => x.FechaFinalizacion == model.FechaFinalizacion).FirstOrDefault();

            if (getData == null)
            {
                var map = _mapper.Map<DetalleContrato>(model);
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
            return false;

        }
        #endregion

    }
}
