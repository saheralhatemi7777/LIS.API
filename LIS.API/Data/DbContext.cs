using APiUsers.DTOs.DTOSTests;
using Microsoft.EntityFrameworkCore;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        //المشرفين
        public DbSet<Users> users { get; set; }
        //المرضى
        public DbSet<Patient> patients { get; set; }
        //الطلبات
        public DbSet<Recuests> recuests { get; set; }
        //الرتبه
        public DbSet<Roles> roles { get; set; }
        //التحاليل
        public DbSet<Test> Test { get; set; }
        //جدول فئات التحاليل
        public DbSet<TestCategory> TestCategories { get; set; }
        //الجدول الوسيط
        public DbSet<RequestTest> RequestTest { get; set; }
        //نتائج التحاليل
        public DbSet<TestResult> testResult { get; set; } 
        //اعدادت النظام
        public DbSet<SettingSystem> SettingSystem { get; set; }
        //السجلات
        public DbSet<RecordPatients> RecordPatients { get; set; }

        public DbSet<RecordRequestTest> RecordRequestTests { get; set; }

        public DbSet<Operations> Operations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.Test)
                .WithMany()
                .HasForeignKey(tr => tr.TestId)
                .OnDelete(DeleteBehavior.Restrict); // الحل هنا

        }


    }
}
