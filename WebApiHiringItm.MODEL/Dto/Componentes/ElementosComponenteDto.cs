namespace WebApiHiringItm.MODEL.Dto.Componentes
{
    public class ElementosComponenteDto
    {
        public int Id { get; set; }
        public int IdComponenete { get; set; }
        public int CantidadContratistas { get; set; }
        public int CantidadDias { get; set; }
        public decimal ValorUnidad { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorPorDia { get; set; }
    }
}
