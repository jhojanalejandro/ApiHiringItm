using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ExportDataDto
{
    public class ReportExportDto
    {
        public string? Consecutive { get; set; }
        public string? ContractorLastName { get; set; }
        public string? ContractorName { get; set; }
        public string? ContractorIdentification { get; set; }
        public string? ContractorMail { get; set; }

        public string? ContractNumber { get; set; }
        public string? ProjectName { get; set; }
        public decimal? ContractValue { get; set; }
        public string? NoAddition { get; set; }

        public DateTime? InitialPeriod { get; set; }
        public DateTime? FinalPeriod { get; set; }
        public decimal? TotalValuePeriodPayment { get; set; }

        public decimal? AfpValue { get; set; }
        public decimal? EpsValue { get; set; }
        public decimal? ArlValue { get; set; }

        public int? CantDays { get; set; }
        public string? AreaCode { get; set; }
        public string? AreaName { get; set; }
        public string? PayrollNumber { get; set; }
        public string? News { get; set; }


    }
}
