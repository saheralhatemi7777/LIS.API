using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using مشروع_ادار_المختبرات.Models;

namespace مشروع_ادار_المختبرات.Models
{
    public class Operations
    {
        [Key]
        public int OperationId { get; set; }

        //رقم المستخدم الذي قام بالعمليه
        [ForeignKey("Users")]
        public int UserId { get; set; }
        //اسم العمليه التي قام بها 
        public string ActionType { get; set; }

        public string TableName { get; set; }

        [ForeignKey("Patients")]
        public int RecordId { get; set; }

        public DateTime ActionDate { get; set; }

        public virtual Users? Users { get; set; }
        public virtual Patient? Patients  { get; set; }
    }
}
