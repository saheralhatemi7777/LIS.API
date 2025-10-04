using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace مشروع_ادار_المختبرات.Models
{
    public class Recuests
    {
        [Key]
        public int RecuestID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public int UserID { get; internal set; }

        [ForeignKey("PatientID")]
        public virtual Patient? Patient { get; set; }

        public virtual Users?  User { get; set; }
        public virtual ICollection<RequestTest> RequestTests { get; set; } = new List<RequestTest>();
    }
}
