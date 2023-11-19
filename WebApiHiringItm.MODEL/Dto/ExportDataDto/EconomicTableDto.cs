using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.ExportDataDto
{
    public class EconomicTableDto
    {
        public string? Id { get; set; }
        public List<ElementComponent>? Elements { get; set; }
        public List<ComponentDto>? Components { get; set; }
        public List<ActivityDto>? Activities { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? OperatingExpenses { get; set; }
        public decimal? SubTotal { get; set; }
        public string ProjecName { get; set; }
        public string CompanyName { get; set; }

    }
}
