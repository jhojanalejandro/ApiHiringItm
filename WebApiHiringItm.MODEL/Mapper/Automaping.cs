using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Mapper
{
    public class Automaping : Profile
    {
        public Automaping()
        {
            CreateMap<UserTDto, UserT>().ReverseMap();
            CreateMap<HiringDataDto, HiringData>().ReverseMap();
            CreateMap<ContractorDto, Contractor>().ReverseMap();
            CreateMap<ContractorStudyDto, ContractorStudy>().ReverseMap();
            CreateMap<FilesDto, Files>().ReverseMap();
            CreateMap<ProjectFolderDto, ProjectFolder>().ReverseMap();
            CreateMap<UserUpdatePasswordDto, UserT>().ReverseMap();

        }
    }
}
