using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Models
{
    public class PlaningRequest
    {
        public int Id { get; set; }
        public string[] Component { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? Total { get; set; }
        public decimal? HonorariosMensuales { get; set; }
        public string Profesional { get; set; }
        public string Laboral { get; set; }
        public decimal? ValorTotal { get; set; }
        public string Objeto { get; set; }
    }
}
