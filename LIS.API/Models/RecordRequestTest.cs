using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace مشروع_ادار_المختبرات.Models
{
    public class RecordRequestTest
    {
       
            [Key]
            public int Id { get; set; }

            public int RecordId { get; set; }
            public int RequestTestId { get; set; }

            [ForeignKey("RecordId")]
            public virtual RecordPatients Record { get; set; }

            [ForeignKey("RequestTestId")]
            public virtual RequestTest RequestTest { get; set; }
        
    }
}
