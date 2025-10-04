using System.ComponentModel.DataAnnotations;

namespace مشروع_ادار_المختبرات.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        public string Name { get; set; }

        public  ICollection<Users>User{ get; set; }

       
    }
}
