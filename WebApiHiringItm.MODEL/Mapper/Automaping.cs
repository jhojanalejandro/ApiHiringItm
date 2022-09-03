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
            CreateMap<FeasibilityRequestDto, FeasibilityRequest>().ReverseMap();

        }
    }
}
