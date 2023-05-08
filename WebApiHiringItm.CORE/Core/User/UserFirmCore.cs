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


        public async Task<bool> SaveUserFirm(UserFirmDto modelFirm)
        {
            if (!modelFirm.IsOwner)
            {
                var getUser = _context.UserT
                                .Where(w => w.Id.Equals(modelFirm.UserId)).FirstOrDefault();
                var getFirm = _context.UserFirm.Where(w => w.UserId.Equals(modelFirm.UserId)).FirstOrDefault();
                if (getFirm != null)
                {
                    getFirm.FirmData = modelFirm.FirmData;
                    _context.UserFirm.Update(getFirm);
                }
                else
                {
                    modelFirm.OwnerFirm = getUser.UserName;
                    modelFirm.UserCharge = getUser.Professionalposition;
                    var map = _mapper.Map<UserFirm>(modelFirm);
                    map.Id = Guid.NewGuid();

                    _context.UserFirm.Add(map);

                }
                _context.SaveChanges();
                return true;
            }
            return false;

        }

    }
}
