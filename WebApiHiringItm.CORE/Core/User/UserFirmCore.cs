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
using WebApiHiringItm.CORE.Helpers.Enums.File;
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

        public async Task<List<UserFileDto>> GetAllFirms()
        {
            var result = _context.UserFile.ToList();
            var map = _mapper.Map<List<UserFileDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<TypeUserFileDto>> GetAllTypeUserFile()
        {
            var result = _context.UserFileType.ToList();
            var map = _mapper.Map<List<TypeUserFileDto>>(result);
            return await Task.FromResult(map);
        }
        public async Task<List<RollDto>> GetAllRolls()
        {
            var result = _context.Roll.ToList();
            var map = _mapper.Map<List<RollDto>>(result);
            return await Task.FromResult(map);
        }


        public async Task<UserFileDto> GetByIdFirm(string id)
        {
            var result = _context.UserFile
                .Where(x => x.UserId.Equals(Guid.Parse(id)) && x.UserFileTypeNavigation.Code.Equals(TypeUserFileEnum.FIRMA.Description()));
            result.Select(s => new UserFileDto
            {
                FileData = s.FileData,
                Id = s.Id,

            })
            .AsNoTracking()
            .FirstOrDefault();
            var map = _mapper.Map<UserFileDto>(result);
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


        public async Task<bool> SaveUserDocument(UserFileDto modelFirm)
        {
            var typeUserFileId = _context.UserFileType.Where(x => x.Code.Equals(TypeUserFileEnum.FIRMA.Description())).Select(s => s.Id).FirstOrDefault();

            if (!modelFirm.IsOwner)
            {
                var typeUserFileIdImage = _context.UserFileType.Where(x => x.Code.Equals(TypeUserFileEnum.IMAGENCORREOS.Description())).Select(s => s.Id).FirstOrDefault();
                var getUser = _context.UserT
                .Where(w => w.Id.Equals(modelFirm.UserId)).FirstOrDefault();
                if (!modelFirm.UserFileType.Equals(typeUserFileId))
                {
                    var getImageMesagge = _context.UserFile.Where(x => x.UserFileType.Equals(typeUserFileIdImage) && x.UserId.Equals(modelFirm.UserId)).FirstOrDefault();
                    if (getImageMesagge != null)
                    {
                        getImageMesagge.FileData = modelFirm.FileData;
                        getImageMesagge.RollId = modelFirm.RollId;
                        _context.UserFile.Update(getImageMesagge);

                    }
                    else
                    {
                        var map = _mapper.Map<UserFile>(modelFirm);
                        map.Id = Guid.NewGuid();
                        _context.UserFile.Add(map);

                    }
                } else {
                    var getFirm = _context.UserFile.Where(x => x.UserFileType.Equals(typeUserFileId) && x.UserId.Equals(modelFirm.UserId)).FirstOrDefault();
                    if (getFirm != null)
                    {
                        getFirm.FileData = modelFirm.FileData;
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
