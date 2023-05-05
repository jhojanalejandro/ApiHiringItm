using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CORE.Core.User
{
    public class UserFirmCore : IUserFirmCore
    {
        private readonly HiringContext _context;
        private readonly IMapper _mapper;

        public UserFirmCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserFirmDto>> GetAllFirms()
        {
            var result = _context.UserFirm.ToList();
            var map = _mapper.Map<List<UserFirmDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<UserFirmDto> GetByIdFirm(string id)
        {
            var result = _context.UserFirm.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
            var map = _mapper.Map<UserFirmDto>(result);
            return await Task.FromResult(map);
        }


        public async Task<bool> DeleteFirm(string id)
        {
            var getData = _context.UserFirm.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
            if (getData != null)
            {

                var result = _context.UserFirm.Remove(getData);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateFirm(UserFirmDto model)
        {
            var getData = _context.UserFirm.Where(x => x.Id == model.Id).FirstOrDefault();
            if (getData == null)
            {
                var map = _mapper.Map<UserFirm>(model);
                map.Id = Guid.NewGuid();
                var res = _context.UserFirm.Add(map);
                var result = await _context.SaveChangesAsync();
                return result != null ? true : false;
            }
            else
            {
                model.Id = getData.Id;
                var map = _mapper.Map(model, getData);
                var res = _context.UserFirm.Update(map);
                await _context.SaveChangesAsync();
                if (res.State != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
