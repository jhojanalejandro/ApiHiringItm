using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.MODEL.Dto;
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
            var result = _context.UserFile.ToList();
            var map = _mapper.Map<List<UserFirmDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<TypeUserFileDto>> GetAllTypeUserFile()
        {
            var result = _context.TypeUserFile.ToList();
            var map = _mapper.Map<List<TypeUserFileDto>>(result);
            return await Task.FromResult(map);
        }
        public async Task<List<RollDto>> GetAllRolls()
        {
            var result = _context.Roll.Where(w => w.Code.Equals(RollEnum.RECTOR.Description())).ToList();
            var map = _mapper.Map<List<RollDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<UserFirmDto> GetByIdFirm(string id)
        {
            var result = _context.UserT
                .Include(i => i.UserFirm)
                .Where(x => x.Id.Equals(Guid.Parse(id)));
            result.Select(s => new UserFirmDto
            {
                FirmData = s.UserFirm.FirmData,
                Id = s.UserFirm.Id,

            })
            .AsNoTracking()
            .FirstOrDefault();
            var map = _mapper.Map<UserFirmDto>(result);
            return await Task.FromResult(map);
        }


        public async Task<bool> DeleteFirm(string id)
        {
            var getData = _context.UserFile.Where(x => x.Id.Equals(Guid.Parse(id))).FirstOrDefault();
            if (getData != null)
            {

                var result = _context.UserFile.Remove(getData);
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
                var getFirm = _context.UserFile.Where(w => w.Id.Equals(getUser.UserFirmId)).FirstOrDefault();
                if (getFirm != null)
                {
                    getFirm.FirmData = modelFirm.FirmData;
                    getFirm.RollId = modelFirm.RollId;
                    _context.UserFile.Update(getFirm);

                }
                else
                {
                    modelFirm.OwnerFirm = getUser.UserName;
                    modelFirm.RollId = modelFirm.RollId;
                    var map = _mapper.Map<UserFile>(modelFirm);
                    map.Id = Guid.NewGuid();
                    _context.UserFile.Add(map);
                    if (getUser  != null)
                    {
                        getUser.UserFirmId = map.Id;
                        _context.UserT.Update(getUser);
                    }

                }
                var resp = await _context.SaveChangesAsync();
                return resp > 0 ? true : false;
            }
            else
            {
                var map = _mapper.Map<UserFile>(modelFirm);
                map.Id = Guid.NewGuid();
                _context.UserFile.Add(map);
                var resp = await _context.SaveChangesAsync();
                return resp > 0 ? true : false;
            }

        }

    }
}
