using System.ComponentModel.DataAnnotations;

namespace APiUsers.DTOs.DTOTestRsults
{
    public class DTOSaveTestResult
    {
        [Key]
        public int ResultID { get; set; }   

        public int RequestTestID { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public int LabTechniciansUserID { get; set; }
        public List<int> TestId { get; set; }
        public List<string> ResultValue { get; set; }
        public int Requestid { get; set; }

    }
}
