using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using مشروع_ادار_المختبرات.Models;

namespace مشروع_ادار_المختبرات.Models
{
    public class RecordPatients
    {
        [Key]
        public int RecurdId { get; set; }


        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        

        [ForeignKey("RequestTest")]
        public DateTime RequestDate { get; set; }


        [ForeignKey("Technician")]
        public int TechnicianiD { get; set; }


        public   virtual Patient Patient { get; set; }
        public virtual ICollection<RecordRequestTest> RecordRequestTests { get; set; } = new List<RecordRequestTest>();

    }

}
