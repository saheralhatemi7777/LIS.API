using System.ComponentModel.DataAnnotations;

namespace APiUsers.DTOs.DTOSettingSystem
{
   
        public class DTOSettingSystem
        {
            [Key]
            public int SettingID { get; set; }   

            public string? Name { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Addrees { get; set; }
            public string? Email { get; set; }
            public string? Descraption { get; set; }
            public string? Image { get; set; }
        }
    
}
