using System.ComponentModel.DataAnnotations;

namespace مشروع_ادار_المختبرات.Models
{
    public class TestCategory
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryNameEn { get; set; }
        [Required]
        [StringLength(100)]
        public string CategoryNameAr { get; set; }

        //public ICollection<Test>? tests { get; set; }
    }
}
