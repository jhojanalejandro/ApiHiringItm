using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class DetailProjectContractorDto
    {
        public string ContractorId { get; set; }
        public string Convenio { get; set; }
        public string CompanyName { get; set; }
        public string NombreComponente { get; set; }
        public string Cpc { get; set; }
        public string? Nombre { get; set; }
        public string? Identificacion { get; set; }
        public string? ObjetoElemento { get; set; }
        public string? ObjetoConvenio { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal UnitValue { get; set; }
        public string? NombreElemento { get; set; }
        public string? Cdp { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public string? User { get; set; }
        public string? Email { get; set; }
        public string? GeneralObligation { get; set; }
        public string? SpecificObligation { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Arl { get; set; }
        public string? Afp { get; set; }
        public string? Eps { get; set; }
        public string? ContractorMail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ContractorAddress { get; set; }
        public string? EmailManager { get; set; }
        public string? UserManager { get; set; }
        public string? IdentificationManager { get; set; }
        public string? ChargeManager { get; set; }



    }
}
