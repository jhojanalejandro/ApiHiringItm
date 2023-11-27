using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ContractorsPrePayrollDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string? Identificacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string Correo { get; set; }
        public string? StatusContractor { get; set; }
        public string? HiringStatus { get; set; }
        public List<Guid> AssignmentUser { get; set; }
        public int? CantDays { get; set; }
        public decimal? ContractValue { get; set; }
        public string? Nacionality { get; set; }
        public string? ExpeditionPlace { get; set; }
        public string? Gender { get; set; }
        public DateTime? InitialContractDate { get; set; }
        public DateTime? FinalContractDate { get; set; }
        public string? Cdp { get; set; }
        public string? No { get; set; }
        public int? Level { get; set; }
        public string? BankEntity { get; set; }
        public string? Eps { get; set; }
        public string? Afp { get; set; }
        public string? Arl { get; set; }
        public int? PaymentsCant { get; set; }

    }
}
