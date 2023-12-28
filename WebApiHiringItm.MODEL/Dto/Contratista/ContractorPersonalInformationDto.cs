using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class PersonalInformation
    {
        public ContractorPersonalInformationDto ContractorPersonalInformation { get; set; }
        public List<AcademicInformationDto?> AcademicInformation { get; set; }
    }
    public class ContractorPersonalInformationDto
    {
        public Guid? Id { get; set; }
        public string Identificacion { get; set; }
        public string LugarExpedicion { get; set; }
        public string Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Barrio { get; set; }
        public string? Telefono { get; set; }
        public string Celular { get; set; }
        public string CuentaBancaria { get; set; }
        public string? TipoCuenta { get; set; }
        public Guid? EntidadCuentaBancaria { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public Guid Eps { get; set; }
        public Guid Arl { get; set; }
        public Guid Afp { get; set; }
        public bool EnableChangePassword { get; set; }


    }

    public class AcademicInformationDto
    {
        public Guid? Id { get; set; }
        public string AcademicInformationType { get; set; }
        public string CollegeDegree { get; set; }
        public string Institution { get; set; }
        public Guid? Contractor { get; set; }
    }

}
