using System.ComponentModel.DataAnnotations;

namespace APiUsers.DTOs
{
    public class DTOAddTestType
    {
        [Key]
        public int TestId { get; set; }
        public string TestNameEn { get; set; }
        public string TestNameAr { get; set; }
        public string SampleType { get; set; }
        public string NormalRange { get; set;}
        public int Testprice { get; set;  }
        public int CategoryId { get; set;    }


    }
}
