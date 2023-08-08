namespace WebApiHiringItm.MODEL.Dto.Componentes
{
    public class ElementComponentDto
    {
        public Guid? Id { get; set; }
        public string NombreElemento { get; set; }
        public string? ObligacionesGenerales { get; set; }
        public string? ObligacionesEspecificas { get; set; }
        public Guid? TipoElemento { get; set; }
        public Guid ComponentId { get; set; }
        public int? CantidadContratistas { get; set; }
        public int CantidadDias { get; set; }
        public decimal ValorUnidad { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal? Recursos { get; set; }
        public decimal ValorPorDia { get; set; }
        public Guid? CpcId { get; set; }
        public Guid? IdDetalle { get; set; }
        public bool? Modificacion { get; set; }
        public string Consecutivo { get; set; }
        public decimal? ValorTotalContratista { get; set; }
        public decimal? ValorPorDiaContratista { get; set; }
        public string? ObjetoElemento { get; set; }
        public Guid? ActivityId { get; set; }
        public int? CantidadEnable { get; set; }

    }
}
