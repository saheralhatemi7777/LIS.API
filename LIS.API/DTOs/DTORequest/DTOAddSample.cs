using System.ComponentModel.DataAnnotations;

namespace APiUsers.DTOs.DTOSample
{
    public class DTOAddRecuests
    {
        [Key]
        public int RecuestTestID { get; set; }

        [Required]
        public int PatientID { get; set; }

        public string Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserID { get; set; }

    }
}
