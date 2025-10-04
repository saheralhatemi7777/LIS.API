using APiUsers.Data;
using APiUsers.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorActivityDtoController : ControllerBase
    {

        public DoctorActivityDtoController(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        private readonly AppDbContext _context;

        //اي بي اي الاحصائيات
        [HttpGet("MostActiveDoctors")]
        public async Task<IActionResult> GetMostActiveDoctors()
        {

                var doctorsActivity =await  _context.testResult
                .Include(tr => tr.LabTechnician)
                .GroupBy(tr => new { tr.LabTechnician.UserID, tr.LabTechnician.FullName })
                .Select(g => new DoctorActivityDto
                {
                    DoctorId = g.Key.UserID,
                    DoctorName = g.Key.FullName,
                    ActivityCount = g.Count() 
                 })
                .OrderByDescending(d => d.ActivityCount)
                .Take(20) 
                .ToListAsync();

            return Ok(doctorsActivity);
        }

    }
}
