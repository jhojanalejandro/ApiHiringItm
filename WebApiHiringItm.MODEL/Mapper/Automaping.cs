using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Dto.MasterDataDto;
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
            CreateMap<ComponenteDto, Component>().ReverseMap();
            CreateMap<ElementComponentDto, ElementComponent>().ReverseMap();
            CreateMap<DetalleContratoDto, DetailContract>().ReverseMap();
            CreateMap<UserFirmDto, UserFile>().ReverseMap();
            CreateMap<RProjectForlderDto, ContractFolder>().ReverseMap();
            CreateMap<ProjectFolderCostsDto, ContractFolder>().ReverseMap();
            CreateMap<DetailFileDto, DetailFile>().ReverseMap();
            CreateMap<Activity, ActivityDto>().ReverseMap();
            CreateMap<AuthDto, UserT>().ReverseMap();
            CreateMap<AuthDto, Contractor>().ReverseMap();
            CreateMap<NewnessContractorDto, NewnessContractor>().ReverseMap();
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
            CreateMap<EmptityHealthDto, EmptityHealth>().ReverseMap();
            CreateMap<RollDto, Roll>().ReverseMap();
            CreateMap<TypeUserFileDto, TypeUserFile>().ReverseMap();


        }
    }
}
