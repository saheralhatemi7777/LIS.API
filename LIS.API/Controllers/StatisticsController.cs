using APiUsers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        // 1️⃣ عدد التحاليل لكل طبيب
        [HttpGet("DoctorStats")]
        public async Task<IActionResult> GetDoctorStats()
        {
            var data = await _context.RequestTest
                .Include(rt => rt.Request)
                .ThenInclude(r => r.User)
                .Where(rt => rt.Request.User != null && rt.Request.User.RoleId == 2) // طبيب
                .GroupBy(rt => rt.Request.User.FullName)
                .Select(g => new
                {
                    DoctorName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(d => d.Count)
                .ToListAsync();

            return Ok(data);
        }

        // 2️⃣ أكثر التحاليل طلبًا
        [HttpGet("TestStats")]
        public async Task<IActionResult> GetTestStats()
        {
            var data = await _context.RequestTest
                .Include(rt => rt.Test)
                .GroupBy(rt => rt.Test.TestNameAr)
                .Select(g => new
                {
                    TestName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(t => t.Count)
                .ToListAsync();

            return Ok(data);
        }

        // 3️⃣ عدد التحاليل لكل فني (مثال على إضافة إحصائية للفنيين)
        // 3️⃣ المستخدم الأكثر تفاعلاً
        [HttpGet("MostActiveUsers")]
        public async Task<IActionResult> GetMostActiveUsers()
        {
            var data = await _context.recuests
                .Where(r => r.User != null) // التأكد من وجود مستخدم مرتبط
                .GroupBy(r => new { r.UserID, r.User.FullName })
                .Select(g => new
                {
                    UserId = g.Key.UserID,
                    UserName = g.Key.FullName,
                    RequestsCount = g.Count() // عدد الطلبات لكل مستخدم
        })
                .OrderByDescending(u => u.RequestsCount)
                .ToListAsync();

            return Ok(data);
        }

        // 4️⃣ الإيرادات
        [HttpGet("RevenueStats")]
        public async Task<IActionResult> GetRevenueStats()
        {
            var totalRevenue = await _context.RequestTest
                .Include(rt => rt.Test)
                .SumAsync(rt => rt.Test.Testprice);

            var otherCosts = 500; // مثال تكاليف ثابتة أخرى
            return Ok(new { Revenue = totalRevenue, OtherCosts = otherCosts });
        }
    }
}
