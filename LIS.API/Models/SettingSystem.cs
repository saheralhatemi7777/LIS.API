using System.ComponentModel.DataAnnotations;

namespace مشروع_ادار_المختبرات.Models
{
    public class SettingSystem
    {
        [Key]
        public int SettingID { get; set; }   // لو Identity لا ترسله

        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Addrees { get; set; }
        public string? Email { get; set; }
        public string? Descraption { get; set; }
        public string? Image { get; set; }
    }

}
