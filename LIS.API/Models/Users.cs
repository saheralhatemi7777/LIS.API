using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace مشروع_ادار_المختبرات.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        [Required(ErrorMessage ="يرجاء ادخال الاســم")]
        [MaxLength(50,ErrorMessage ="يجب ان يكون طول الاسم بين 10 الى 30 حرفا")]

        public string FullName { get; set; }

        [Required(ErrorMessage ="البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage ="صيغة بريد غير صالحة")]
        public string  Email { get; set; }

        
        [MaxLength(20,ErrorMessage ="يجب ان يكون طول كلمة المرور بين 10 الى 15 رقم")]
        public string Password { get; set; }
        public string? Salt { get; set; }
        [Required]
        [ForeignKey("Roles")]
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public virtual Roles Roles { get; set; }
        public  ICollection<Patient>? SupervisedPatients { get; set; }

       
    }
}
