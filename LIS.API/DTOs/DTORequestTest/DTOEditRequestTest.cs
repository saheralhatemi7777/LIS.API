using System.ComponentModel.DataAnnotations;

namespace APiUsers.DTOs.DTORequestTest
{
    public class DTOEditRequestTest
    {
        [Key]
        public int RequestTestID { get; set; }

        public int RequestID { get; set; }

        public List<int> TestIds { get; set; }
    }
}
