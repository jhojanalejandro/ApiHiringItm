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
        DbSet<UserT> UserT { get; set; }
        DbSet<Roll> Roll { get; set; }
        DbSet<Folder> Folder { get; set; }
        DbSet<FeasibilityRequest> FeasibilityRequest { get; set; }

    }
}
