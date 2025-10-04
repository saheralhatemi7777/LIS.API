using APiUsers.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Ready_made_TestsController : ControllerBase
    {

        public Ready_made_TestsController(AppDbContext app1)
        {
            _Context = app1;
        }

        private readonly AppDbContext _Context;
        [HttpGet("GetAllDataRequestTest")]

        public async Task<IActionResult> GetAllDataGroupedByPatient()
        {
            var data = await _Context.RequestTest
                .Include(rt => rt.Test)
                .Include(rt => rt.Request)
                .ThenInclude(r => r.Patient)
                .Include(rt => rt.Request)
                .ThenInclude(r => r.User) // نفترض أن هناك كيان Doctor
                .Where(rt => rt.Request.Status == "تم اصدار التحليل✅")
                .ToListAsync();

            var groupedByPatient = data
                .GroupBy(rt => new { rt.Request.Patient.PatientID, rt.Request.Patient.FullName })
                .Select(g => new
                {
                    PatientID = g.Key.PatientID,
                    PatientName = g.Key.FullName,
                    Requests = g
                        .GroupBy(rt => rt.RequestID)
                        .Select(rq => new
                        {
                            RequestID = rq.Key,
                            RequestTestID = rq.Select(x => x.RequestTestID).FirstOrDefault(),
                            Status = rq.Select(x => x.Request.Status).FirstOrDefault(),
                            CreatedAt = rq.Select(x => x.Request.CreatedAt).FirstOrDefault(),
                            DoctorName = rq.Select(x => x.Request.User.FullName).FirstOrDefault(), // اسم الطبيب
                    Tests = rq.Select(x => new
                            {
                                Name = x.Test.TestNameEn,
                                Samples = x.Test.SampleType,
                                Price = x.Test.Testprice
                            })
                            .OrderBy(t => t.Name)
                            .ToList()
                        })
                        .OrderBy(r => r.CreatedAt)
                        .ToList()
                })
                .OrderBy(p => p.PatientName)
                .ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(
                groupedByPatient,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

            return Content(json, "application/json");
        }

        [HttpGet("GetAllDataGroupedByPatientName")]
        public async Task<IActionResult> GetAllDataGroupedByPatientName(string name)
        {
            var data = await _Context.RequestTest
                .Include(rt => rt.Test)
                .Include(rt => rt.Request)
                    .ThenInclude(r => r.Patient)
                .Include(rt => rt.Request)
                    .ThenInclude(r => r.User)
                .Where(rt => rt.Request.Status == "تم اصدار التحليل" &&
                             (string.IsNullOrEmpty(name) || rt.Request.Patient.FullName.Contains(name)))
                .ToListAsync();

            var groupedByPatient = data
                .GroupBy(rt => new { rt.Request.Patient.PatientID, rt.Request.Patient.FullName })
                .Select(g => new
                {
                    PatientID = g.Key.PatientID,
                    PatientName = g.Key.FullName,
                    Requests = g
                        .GroupBy(rt => rt.RequestID)
                        .Select(rq => new
                        {
                            RequestID = rq.Key,
                            RequestTestID = rq.Select(x => x.RequestTestID).FirstOrDefault(),
                            Status = rq.Select(x => x.Request.Status).FirstOrDefault(),
                            CreatedAt = rq.Select(x => x.Request.CreatedAt).FirstOrDefault(),
                            DoctorName = rq.Select(x => x.Request.User.FullName).FirstOrDefault(),
                            Tests = rq.Select(x => new
                            {
                                Name = x.Test.TestNameEn,
                                Samples = x.Test.SampleType,
                                Price = x.Test.Testprice
                            })
                            .OrderBy(t => t.Name)
                            .ToList()
                        })
                        .OrderBy(r => r.CreatedAt)
                        .ToList()
                })
                .OrderBy(p => p.PatientName)
                .ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(
                groupedByPatient,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

            return Content(json, "application/json");
        }


    }
}
