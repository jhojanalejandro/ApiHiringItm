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
        public string? Convenio { get; set; }
        public string? CompanyName { get; set; }
        public string? NombreComponente { get; set; }
        public string? Cpc { get; set; }
        public string? Nombre { get; set; }
        public string? Identificacion { get; set; }
        public string? DescriptionProject { get; set; }
        public string? ObjetoConvenio { get; set; }
        public decimal? ValorTotal { get; set; }
        public string? NombreElemento { get; set; }
        public string? Rubro { get; set; }
        public string? NombreRubro { get; set; }
        public string? FuenteRubro { get; set; }
        public string? Cdp { get; set; }

    }
}
