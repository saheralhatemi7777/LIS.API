using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace مشروع_ادار_المختبرات.Models
{
    [Table("TestResults")]
    public class TestResult
    {

        [Key]
        public int ResultID { get; set; }   // المفتاح الأساسي

        [ForeignKey("RequestTest")]
        public int RequestTestID { get; set; }  // يربط النتيجة بالطلب والتحليل

        [ForeignKey("Test")]
        public int TestId { get; set; }         // ربط التحليل

        public string? ResultValue { get; set; } // نتيجة التحليل

        public DateTime CreatedAt { get; set; } = DateTime.Now; // وقت الادخال

        [ForeignKey("LabTechnician")]
        public int? LabTechniciansUserID { get; set; }  // من أدخل النتيجة

        // العلاقات?
        public virtual RequestTest? RequestTest { get; set; }
        public virtual Test? Test { get; set; }
        public virtual Users? LabTechnician { get; set; }

    }
}

