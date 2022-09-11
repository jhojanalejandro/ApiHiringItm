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
        DbSet<Contractor> Contractor { get; set; }
        DbSet<ContractorStudy> ContractorStudy { get; set; }
        DbSet<Files> Files { get; set; }
        DbSet<Hiringdata> Hiringdata { get; set; }
        DbSet<ProjectFolder> ProjectFolder { get; set; }
        DbSet<RecursiveContractor> RecursiveContractor { get; set; }
        DbSet<Roll> Roll { get; set; }
        DbSet<UserT> UserT { get; set; }

    }
}
