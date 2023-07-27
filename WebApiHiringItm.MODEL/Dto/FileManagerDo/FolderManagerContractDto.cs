

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
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string Type { get; set; }

    }
}
