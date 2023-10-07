using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ChangeContractContractorDto
    {
        public Guid Id { get; set; }
        public Guid DetailContractorId { get; set; }
        public Guid EconomicdataContractorId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal UnitValue { get; set; }
        public decimal Debt { get; set; }
        public Guid? CpcId { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid MinuteType { get; set; }
        public int? Consecutive { get; set; }
        public bool? IsAddition { get; set; }
        public string ContractorId { get; set; }
        public string ContractId { get; set; }
        public string PerfilRequeridoAcademico { get; set; }
        public string PerfilRequeridoExperiencia { get; set; }
        public string ElementName { get; set; }
        public string SpecificObligations { get; set; }
        public string GeneralObligations { get; set; }
        public int? CantDays { get; set; }
        public decimal? Resources { get; set; }
        public decimal? ValueDay { get; set; }
        public bool? IsModify { get; set; }
        public string ElementObject { get; set; }
        public DateTime? InitialAdditionDate { get; set; }
        public DateTime? FinalAdditionDate { get; set; }
        public string? NoAddition { get; set; }


    }
}
