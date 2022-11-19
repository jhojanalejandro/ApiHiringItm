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
        DbSet<Component> Component { get; set; }
        DbSet<Contractor> Contractor { get; set; }
        DbSet<ContractorPayments> ContractorPayments { get; set; }
        DbSet<Files> Files { get; set; }
        DbSet<FolderContractor> FolderContractor { get; set; }
        DbSet<HiringData> HiringData { get; set; }
        DbSet<Planning> Planning { get; set; }
        DbSet<ProjectFolder> ProjectFolder { get; set; }
        DbSet<Roll> Roll { get; set; }
        DbSet<SharedData> SharedData { get; set; }
        DbSet<UserT> UserT { get; set; }
        DbSet<Componente> Componentes { get; set; }
        DbSet<ElementosComponente> ElementosComponentes { get; set; }

    }
}
