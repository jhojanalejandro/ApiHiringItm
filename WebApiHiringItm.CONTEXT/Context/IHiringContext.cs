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
        DbSet<Component> Component { get; set; }
        DbSet<Contractor> Contractor { get; set; }
        DbSet<ContractorPayments> ContractorPayments { get; set; }
        DbSet<DetailContract> DetailContract { get; set; }
        DbSet<DetailFile> DetailFile { get; set; }
        DbSet<EconomicdataContractor> EconomicdataContractor { get; set; }
        DbSet<ElementComponent> ElementComponent { get; set; }
        DbSet<Files> Files { get; set; }
        DbSet<Folder> Folder { get; set; }
        DbSet<HiringData> HiringData { get; set; }
        DbSet<Activity> Activity { get; set; }
        DbSet<NewnessContractor> NewnessContractor { get; set; }
        DbSet<ContractFolder> ContractFolder { get; set; }
        DbSet<Roll> Roll { get; set; }
        DbSet<SharedData> SharedData { get; set; }
        DbSet<UserFirm> UserFirm { get; set; }
        DbSet<UserT> UserT { get; set; }
        DbSet<ElementType> ElementType { get; set; }
        //DbSet<FileType> FileType { get; set; }
        DbSet<FolderType> FolderType { get; set; }
        DbSet<NewnessType> NewnessType { get; set; }
        DbSet<StatusContractor> StatusContractor { get; set; }
        DbSet<StatusContract> StatusContract { get; set; }
        DbSet<StatusFile> StatusFile { get; set; }
        DbSet<CpcType> CpcType { get; set; }
    }
}
