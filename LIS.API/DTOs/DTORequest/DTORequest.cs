using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.DTOs
{
    public class DTORequest
    {

        [Key]
        public int RecuestTestID { get; set; }

        [Required]
        public int PatientID { get; set; }

        public string Status { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? usernName { get; internal set; }

        public string? PatientName { get; internal set; }

        public int UserID { get; internal set; }

    }
}
