using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CONTEXT.Context
{
    public interface IHiringContext
    {
        DbSet<AcademicInformation> AcademicInformation { get; set; }
        DbSet<Activity> Activity { get; set; }
        DbSet<AssigmentContract> AssigmentContract { get; set; }
        DbSet<Banks> Banks { get; set; }
        DbSet<ChangeContractContractor> ChangeContractContractor { get; set; }
        DbSet<Component> Component { get; set; }
        DbSet<ContractFolder> ContractFolder { get; set; }
        DbSet<Contractor> Contractor { get; set; }
        DbSet<ContractorPayments> ContractorPayments { get; set; }
        DbSet<CpcType> CpcType { get; set; }
        DbSet<DetailContract> DetailContract { get; set; }
        DbSet<DetailContractor> DetailContractor { get; set; }
        DbSet<DetailFile> DetailFile { get; set; }
        DbSet<DocumentType> DocumentType { get; set; }
        DbSet<EconomicdataContractor> EconomicdataContractor { get; set; }
        DbSet<ElementComponent> ElementComponent { get; set; }
        DbSet<ElementType> ElementType { get; set; }
        DbSet<EmptityHealth> EmptityHealth { get; set; }
        DbSet<Files> Files { get; set; }
        DbSet<Folder> Folder { get; set; }
        DbSet<FolderType> FolderType { get; set; }
        DbSet<HiringData> HiringData { get; set; }
        DbSet<MinuteType> MinuteType { get; set; }
        DbSet<NewnessContractor> NewnessContractor { get; set; }
        DbSet<NewnessType> NewnessType { get; set; }
        DbSet<Roll> Roll { get; set; }
        DbSet<RubroType> RubroType { get; set; }
        DbSet<SharedData> SharedData { get; set; }
        DbSet<StatusContract> StatusContract { get; set; }
        DbSet<StatusContractor> StatusContractor { get; set; }
        DbSet<StatusFile> StatusFile { get; set; }
        DbSet<TermContractor> TermContractor { get; set; }
        DbSet<AssignmentType> AssignmentType { get; set; }
        DbSet<DetailType> DetailType { get; set; }
        DbSet<UserFileType> UserFileType { get; set; }
        DbSet<UserFile> UserFile { get; set; }
        DbSet<UserT> UserT { get; set; }
    }
}
