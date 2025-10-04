using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APiUsers.DTOs.DTOTestRsults
{
    public class DTOAddTestResults
    {
        [Key]
        public int ResultID { get; set; }   // المفتاح الأساسي

        public int RequestTestID { get; set; }  // يربط النتيجة بالطلب والتحليل

        public int TestId { get; set; }         // ربط التحليل

        public string ResultValue { get; set; } // نتيجة التحليل

        public DateTime CreatedAt { get; set; } = DateTime.Now; // وقت الادخال

        public int? LabTechniciansUserID { get; set; }  // من أدخل النتيجة

    }
}
