using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.Contrato;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.MasterDataDto;
using WebApiHiringItm.MODEL.Dto.Security;
using WebApiHiringItm.MODEL.Dto.Usuario;
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
            CreateMap<GetFilesPaymentDto, Files>().ReverseMap();
            CreateMap<ContractFolderDto, ContractFolder>().ReverseMap();
            CreateMap<UserUpdatePasswordDto, UserT>().ReverseMap();
            CreateMap<FolderDto, Folder>().ReverseMap();
            CreateMap<GetFileDto, Files>().ReverseMap();
            CreateMap<ContractorPayments, ContractorPaymentsDto>().ReverseMap();
            CreateMap<EconomicdataContractor, EconomicdataContractorDto>().ReverseMap(); 
            CreateMap<ComponentDto, Component>().ReverseMap();
            CreateMap<ElementComponentDto, ElementComponent>().ReverseMap();
            CreateMap<DetailContractDto, DetailContract>()
                .ForMember(c => c.Id, cd => cd.MapFrom(src => src.Id))
                .ForMember(c => c.UserId, cd => cd.MapFrom(src => Guid.Parse(src.UserId))).ReverseMap();
            CreateMap<UserFileDto, UserFile>().ReverseMap();
            CreateMap<RProjectForlderDto, ContractFolder>()
                .ForMember(c => c.Id, cd => cd.MapFrom(src => src.Id))
                .ForMember(c => c.Rubro, cd => cd.MapFrom(src => Guid.Parse(src.Rubro)))
                .ForMember(c => c.StatusContractId, cd => cd.MapFrom(src => Guid.Parse(src.StatusContractId)))
                .ForMember(c => c.ObjectContract, cd => cd.MapFrom(src => src.ObjectContract))
                .ForMember(c => c.ContractorsCant, cd => cd.MapFrom(src => src.ContractorsCant))
                .ForMember(c => c.Activate, cd => cd.MapFrom(src => src.Activate))
                .ForMember(c => c.EnableProject, cd => cd.MapFrom(src => src.EnableProject))
                .ForMember(c => c.CompanyName, cd => cd.MapFrom(src => src.CompanyName))
                .ForMember(c => c.ProjectName, cd => cd.MapFrom(src => src.ProjectName))
                .ForMember(c => c.Project, cd => cd.MapFrom(src => src.Project))
                .ForMember(c => c.FuenteRubro, cd => cd.MapFrom(src => src.FuenteRubro))
                .ForMember(c => c.NumberProject, cd => cd.MapFrom(src => src.NumberProject))
                .ForMember(c => c.DutyContract, cd => cd.MapFrom(src => src.DutyContract))
                .ReverseMap();
            CreateMap<ProjectFolderCostsDto, ContractFolder>().ReverseMap();
            CreateMap<DetailFileDto, DetailFile>().ReverseMap();
            CreateMap<Activity, ActivityDto>().ReverseMap();
            CreateMap<AuthDto, UserT>().ReverseMap();
            CreateMap<AuthDto, Contractor>().ReverseMap();
            CreateMap<NewnessContractorDto, NewnessContractor>()
                .ForMember(c => c.NewnessType, cd => cd.MapFrom(src => Guid.Parse(src.NewnessType)))
                .ReverseMap();
            //CreateMap<FileType, TypeFileDto>().ReverseMap();
            CreateMap<ElementTypeDto, ElementType>().ReverseMap();
            CreateMap<CpcTypeDto, CpcType>().ReverseMap();
            CreateMap<StatusContractDto, StatusContract>().ReverseMap();
            CreateMap<FileContractDto, Files>().ReverseMap();
            CreateMap<DocumentTypeDto, DocumentType>().ReverseMap();
            CreateMap<StatusFileDto, StatusFile>().ReverseMap();
            CreateMap<ContractorPersonalInformationDto, Contractor>().ReverseMap();
            CreateMap<MinuteTypeDto, MinuteType>().ReverseMap();
            CreateMap<BanksDto, Banks>().ReverseMap();
            CreateMap<RubroTypeDto, RubroType>().ReverseMap();
            CreateMap<AcademicInformationDto, AcademicInformation>().ReverseMap();
            CreateMap<EntityHealthDto, EntityHealth>().ReverseMap();
            CreateMap<RollDto, Roll>().ReverseMap();
            CreateMap<TypeUserFileDto, UserFileType>().ReverseMap();
            CreateMap<AssignmentUserDto, AssigmentContract>().ReverseMap();
            CreateMap<AssignmentTypeDto, AssignmentType>().ReverseMap();
            CreateMap<TermTypeDto, TermType>().ReverseMap();
            CreateMap<TermContractDto, TermContract>().ReverseMap();
            CreateMap<DetailTypeDto, DetailType>().ReverseMap();
            CreateMap<ChangeContractContractorDto, ChangeContractContractor>().ReverseMap();
            CreateMap<ContractorPaymentSecurityDto, ContractorPaymentSecurity>().ReverseMap();
            CreateMap<NewnessTypeDto, NewnessType>().ReverseMap();
            CreateMap<ObservationFileRequest, DetailFile>().ReverseMap();


        }
    }
}
