﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class ExecutionReportDto
    {
        public string ContractorName { get; set; }
        public string ContractorIdentification { get; set; }
        public string ContractNumber { get; set; }
        public string ContractInitialDate { get; set; }
        public string SupervisorContract { get; set; }
        public string SupervisorIdentification { get; set; }
        public string PeriodExecuted { get; set; }
        public string SpecificObligations { get; set; }
        public string ElementObject { get; set; }
        public string? TotalValue { get; set; }
        public string? TotalValuePeriod { get; set; }


    }
}
