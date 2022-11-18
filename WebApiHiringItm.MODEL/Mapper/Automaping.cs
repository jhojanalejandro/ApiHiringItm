using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Componentes;
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
            CreateMap<AddPasswordContractorDto, Contractor>().ReverseMap();
            CreateMap<FilesDto, Files>().ReverseMap();
            CreateMap<ProjectFolderDto, ProjectFolder>().ReverseMap();
            CreateMap<UserUpdatePasswordDto, UserT>().ReverseMap();
            CreateMap<FolderContractorDto, FolderContractor>().ReverseMap();
            CreateMap<GetFileDto, Files>().ReverseMap();
            CreateMap<ContractorPayments, ContractorPaymentsDto>().ReverseMap();
            CreateMap<EconomicdataContractor, EconomicdataContractorDto>().ReverseMap(); 
            CreateMap<ComponenteDto, Componente>().ReverseMap();
            CreateMap<ElementosComponenteDto, ElementosComponente>().ReverseMap();
        }
    }
}
