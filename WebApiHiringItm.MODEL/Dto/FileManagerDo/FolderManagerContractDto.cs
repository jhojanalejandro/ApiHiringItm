

namespace WebApiHiringItm.MODEL.Dto.FileManagerDo
{
    public class FolderManagerContractDto
    {
        public string[] Path { get; set; }
        public List<FolderContractContractorDto>? Folders { get; set; }
    }

    public class FolderContractContractorDto
    {
        public string Id { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreProyecto { get; set; }
        public string NumeroContrato { get; set; }
        public string Type { get; set; }

    }
}
