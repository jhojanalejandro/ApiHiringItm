using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ContractorDto
    {

        public int Id { get; set; }
        public int IdUser { get; set; }
        public int? IdFolder { get; set; }
        public string NombreCompleto { get; set; }
        public string DocumentoDeIdentidificacion { get; set; }
        public string ClaveUsuario { get; set; }
        public string LugarDeExpedicion { get; set; }
        public DateTime? Fechanacimiento { get; set; }
        public string Municipio { get; set; }
        public string Comuna { get; set; }
        public string Barrio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Sexo { get; set; }
        public string Nacionalidad { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Eps { get; set; }
        public string Pension { get; set; }
        public string Arl { get; set; }
        public string Cuentabancaria { get; set; }
        public string Tipodecuenta { get; set; }
        public string Entidadcuentabancaria { get; set; }

    }
}
