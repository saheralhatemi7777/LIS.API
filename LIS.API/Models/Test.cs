using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using مشروع_ادار_المختبرات.Models;

namespace مشروع_ادار_المختبرات.Models
{
    public class Test
    {
        [Key]
        public int TestId { get; set; }

        [Required, StringLength(200)]
        public string TestNameEn { get; set; }

        [Required, StringLength(200)]
        public string TestNameAr { get; set; }

        [Required, StringLength(200)]
        public string SampleType { get; set; }

        public string NormalRange { get; set; }

        public int Testprice { get; set; }

        [ForeignKey("TestCategory")]
        public int CategoryId { get; set; }
        public virtual TestCategory? TestCategory { get; set; }

        // علاقة واحد إلى كثير مع RecuestTest
        public virtual ICollection<RequestTest> RecuestTests { get; set; } = new List<RequestTest>();
    }
}
