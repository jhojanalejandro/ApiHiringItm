namespace WebApiHiringItm.MODEL.Dto.Componentes
{
    public class ElementosComponenteDto
    {
        public Guid? Id { get; set; }
        public string NombreElemento { get; set; }
        public string? ObligacionesGenerales { get; set; }
        public string? ObligacionesEspecificas { get; set; }
        public string? TipoElemento { get; set; }
        public Guid IdComponente { get; set; }
        public int? CantidadContratistas { get; set; }
        public int CantidadDias { get; set; }
        public decimal ValorUnidad { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal? Recursos { get; set; }
        public decimal ValorPorDia { get; set; }
        public string? Cpc { get; set; }
        public string? NombreCpc { get; set; }
        public Guid? IdDetalle { get; set; }
        public bool? Modificacion { get; set; }
        public string Consecutivo { get; set; }
        public decimal? ValorTotalContratista { get; set; }
        public decimal? ValorPorDiaContratista { get; set; }
        public string? ObjetoElemento { get; set; }

    }
}
