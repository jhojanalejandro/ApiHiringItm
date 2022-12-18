using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.CuentaCobroDto
{
    public class CuentaCobroDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public string LugarExpedicion { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string TipoCuenta { get; set; }
        public string EntidadCuentaBancaria { get; set; }
        public string CuentaBancaria { get; set; }
        public ContractorPayments Componentes { get; set; }

    }
}
