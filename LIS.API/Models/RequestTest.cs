using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using مشروع_ادار_المختبرات.Models;

namespace مشروع_ادار_المختبرات.Models
{
    public class RequestTest
    {
        [Key]
        public int RequestTestID { get; set; }  

        [ForeignKey("Request")]
        public int RequestID { get; set; }

        [ForeignKey("Test")]
        public int TestID { get; set; }    
        
        public virtual Recuests? Request { get; set; }  
        public virtual Test? Test { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();

    }
}
