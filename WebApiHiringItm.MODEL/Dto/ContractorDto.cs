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
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documentodeidentificacion { get; set; }
        public string Lugardeexpedicion { get; set; }
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
        public string ClaveUsuario { get; set; }

    }
}
