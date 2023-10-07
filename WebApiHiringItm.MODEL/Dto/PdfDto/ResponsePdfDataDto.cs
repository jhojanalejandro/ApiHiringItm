using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class ResponsePdfDataDto<T>
    {

        public List<PersonalInChargeDto> PersonalInCharge { get; set; }
        public List<T> GetDataContractors { get; set; }
        public DataContractDto DataContract { get; set; }

    }

    public class DataContractDto
    {
        public string ContractNumber { get; set; }
        public string ContractObject { get; set; }
        public DateTime RegisterDate { get; set; }
        public string ContractName { get; set; }
    }
}
