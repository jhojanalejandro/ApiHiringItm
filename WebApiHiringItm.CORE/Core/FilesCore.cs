using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core
{
    public class FilesCore: IFilesCore
    {
        private readonly Hiring_V1Context _context;
        private readonly IMapper _mapper;

        public FilesCore(Hiring_V1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FilesDto>> GetAll()
        {
            var result = _context.Files.Where(x => x.Id > 0).ToList();
            var map = _mapper.Map<List<FilesDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<FilesDto> GetById(int id)
        {
            var result = _context.Files.Where(x => x.Id == id).FirstOrDefault();
            var map = _mapper.Map<FilesDto>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var resultData = _context.Files.Where(x => x.Id == id).FirstOrDefault();
                if (resultData != null)
                {
                    var result = _context.Files.Remove(resultData);
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

        public async Task<bool> Create(FilesDto model)
        {
            var getData = _context.Files.Where(x => x.Id == model.Id).FirstOrDefault();
            //Convert pdf in Binary formate
            //int lenght = model.FilesName.ContentLength;
            //byte[] data = new byte[lenght];
            //model.FilesName.InputStream.Read(data, 0, lenght);
            if (getData == null)
            {
                var map = _mapper.Map<Files>(model);
                _context.Files.Add(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                _context.Files.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;

        }
    }
}
