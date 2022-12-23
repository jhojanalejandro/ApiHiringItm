using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.CONTEXT.Context
{
    public interface IHiring_V1Context
    {
        DbSet<Componente> Componente { get; set; }
        DbSet<Contractor> Contractor { get; set; }
        DbSet<ContractorPayments> ContractorPayments { get; set; }
        DbSet<DetalleContrato> DetalleContrato { get; set; }
        DbSet<EconomicdataContractor> EconomicdataContractor { get; set; }
        DbSet<ElementosComponente> ElementosComponente { get; set; }
        DbSet<Files> Files { get; set; }
        DbSet<FolderContractor> FolderContractor { get; set; }
        DbSet<HiringData> HiringData { get; set; }
        DbSet<Planning> Planning { get; set; }
        DbSet<ProfessionalRol> ProfessionalRol { get; set; }
        DbSet<ProjectFolder> ProjectFolder { get; set; }
        DbSet<Roll> Roll { get; set; }
        DbSet<SharedData> SharedData { get; set; }
        DbSet<UserFirm> UserFirm { get; set; }
        DbSet<UserT> UserT { get; set; }
    }
}
