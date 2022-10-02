using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.ProjectFolders
{
    public class ProjectFolderCore : IProjectFolder
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;

        public ProjectFolderCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProjectFolderDto>> GetAll()
        {
            var result = _context.ProjectFolder.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<ProjectFolderDto>>(result);
            return await Task.FromResult(map);
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

        public async Task<bool> Create(ProjectFolderDto model)
        {
            var getData = _context.ProjectFolder.Where(x => x.Id == model.Id).FirstOrDefault();

            if (getData == null)
            {
                var map = _mapper.Map<ProjectFolder>(model);
                _context.ProjectFolder.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.ProjectFolder.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;

        }
    }
}
