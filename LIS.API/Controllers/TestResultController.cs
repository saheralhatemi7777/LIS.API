using APiUsers.Data;
using APiUsers.DTOs;
using APiUsers.DTOs.DTOTestRsults;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        private readonly IReopsitory<TestResult> _reopsitory;
        private readonly AppDbContext _context;
        public TestResultController(AppDbContext app1,IReopsitory<TestResult> reopsitory)
        {
            _reopsitory = reopsitory;
            _context = app1;
        }
       
        [HttpGet("GetAllTestResult")]
        public async Task<IActionResult> GetAllTestResult()
        {
            var testResultsGrouped = await _context.testResult
           .Include(tr => tr.RequestTest)
           .ThenInclude(rt => rt.Request)
           .ThenInclude(r => r.Patient)
           .ThenInclude(p => p.Supervisor)
           .Include(tr => tr.Test)
           .Include(tr => tr.LabTechnician)
           .GroupBy(tr => new
           {
               tr.RequestTest.Request.Patient.PatientID,//تجميع حسب المريض + الطلب
               tr.RequestTest.Request.RecuestID
               }) 
               .Select(g => new
               {
                     PatientId = g.Key.PatientID,
                     PatientName = g.Select(x => x.RequestTest.Request.Patient.FullName).FirstOrDefault(),
                     RequestID = g.Key.RecuestID,
                     Status = g.Select(x => x.RequestTest.Request.Status).FirstOrDefault(),
                     SupervisorName = g.Select(x =>
              x.RequestTest.Request.Patient.Supervisor != null
            ? x.RequestTest.Request.Patient.Supervisor.FullName
            : null).FirstOrDefault(),
            LabTechnicianName = g.Select(x =>
           x.LabTechnician != null ? x.LabTechnician.FullName : null).FirstOrDefault(),
                   CreateAt = g.Select(x =>
                  x.CreatedAt ).FirstOrDefault(),

                   // ترتيب الاختبارات داخل الطلب
                   Tests = g.OrderBy(x => x.CreatedAt)
             .Select(x => new
             {
                 TestId = x.TestId,
                 TestName = x.Test.TestNameEn,
                 ReferenceRange = x.Test.NormalRange,
                 ResultValue = x.ResultValue,
                 CreatedAt = x.CreatedAt,
                 }).ToList()
                })
                .OrderBy(x => x.PatientName)   // ترتيب المرضى
               .ThenBy(x => x.RequestID)      // ترتيب الطلبات لكل مريض
             .ToListAsync();

            if (!testResultsGrouped.Any())
            {
                return NotFound("لا يوجد بيانات لعرضها");
            }

            return Ok(testResultsGrouped);

        }

        [HttpPost("AddNewTestResults")]
        public async Task<IActionResult> AddNewTestResults(DTOSaveTestResult testResult)
        {
            if (testResult == null || testResult.TestId == null || !testResult.TestId.Any())
                return BadRequest("يجب إرسال بيانات صحيحة للتحاليل.");

            
            await _reopsitory.AddTestsResultToRequestAsync(
                testResult.RequestTestID,
                testResult.TestId,
                testResult.ResultValue,
                testResult.Requestid,
                testResult.CreatedAt,
                testResult.LabTechniciansUserID
            );
            
            return Ok(testResult); 
        }

        [HttpGet("GetAllTestResultByRequestTestId")]
        public async Task<IActionResult> GetAllTestResultByRequestTestId(int id)
        {
            var testResultsGrouped = await _context.testResult
                .Where(tr => tr.RequestTestID == id)
                .Include(tr => tr.RequestTest)
                .ThenInclude(rt => rt.Request)
                .ThenInclude(r => r.Patient)
                .ThenInclude(p => p.Supervisor)
                .Include(tr => tr.Test)
                .Include(tr => tr.LabTechnician)
                .GroupBy(tr => tr.RequestTest.RequestID)
                .Select(g => new RequestResultDto
                {
                    RequestID = g.Key,
                    PatientName = g.Select(x => x.RequestTest.Request.Patient.FullName).FirstOrDefault(),
                    Status = g.Select(x => x.RequestTest.Request.Status).FirstOrDefault(),
                    SupervisorName = g.Select(x => x.RequestTest.Request.Patient.Supervisor != null
                                                   ? x.RequestTest.Request.Patient.Supervisor.FullName
                                                   : null).FirstOrDefault(),
                    LabTechnicianName = g.Select(x => x.LabTechnician != null ? x.LabTechnician.FullName : null).FirstOrDefault(),
                    CreateAt = g.Select(x => x.CreatedAt).FirstOrDefault(),

                    Tests = g.Select(x => new TestResultDto
                    {
                        TestId = x.TestId,
                        TestName = x.Test.TestNameEn,
                        ReferenceRange = x.Test.NormalRange,
                        ResultValue = x.ResultValue,
                        CreatedAt = x.CreatedAt
                    }).ToList()
                })
                .ToListAsync();

            if (testResultsGrouped == null || !testResultsGrouped.Any())
                return NotFound("لا يوجد بيانات لعرضها");

            // 🔹 إعادة JSON نظيف بدون $values و $id
            return new JsonResult(testResultsGrouped, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
        }
    }

}
