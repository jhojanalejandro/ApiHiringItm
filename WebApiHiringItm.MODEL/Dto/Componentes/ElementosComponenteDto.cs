namespace WebApiHiringItm.MODEL.Dto.Componentes
{
    public class ElementosComponenteDto
    {
        public int Id { get; set; }
        public string NombreElemento { get; set; }
        public string TipoElemento { get; set; }
        public int IdComponente { get; set; }
        public int CantidadContratistas { get; set; }
        public int CantidadDias { get; set; }
        public decimal ValorUnidad { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorPorDia { get; set; }
        public string Cpc { get; set; }
        public string NombreCpc { get; set; }
        public bool? Adicion { get; set; }
        public decimal? Recursos { get; set; }
        public string Consecutivo { get; set; }

    }
}
