using APiUsers.Data;
using APiUsers.DTOs;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordRequestTestController : ControllerBase
    {
        public RecordRequestTestController(AppDbContext appDb, IReopsitory<RecordRequestTest> reopsitory)
        {
            _Reopsitory = reopsitory;
            _context = appDb;
        }
        private IReopsitory<RecordRequestTest> _Reopsitory;
        private readonly AppDbContext _context;

        [HttpGet("GetAllRecordRequestTest")]

        public async Task<IActionResult> GetAllRecordRequestTest()
        {
            var rawData = await _context.RecordRequestTests
            .Include(rrt => rrt.Record)
            .ThenInclude(r => r.Patient)
            .ThenInclude(p => p.Supervisor)
            .Select(rrt => new
           {
              PatientId = rrt.Record.Patient.PatientID,
              FullName = rrt.Record.Patient.FullName,
              Phone = rrt.Record.Patient.phoneNumber,
              Address = rrt.Record.Patient.Address,
              Username = rrt.Record.Patient.Supervisor.FullName,
              RecordId = rrt.Record.RecurdId,
              CreateAt =rrt.Record.RequestDate,
           })
            .Distinct() // لو المريض عنده أكثر من اختبار في نفس السجل يمنع التكرار
            .ToListAsync();
            if(rawData ==null)
            {
                return BadRequest("لا يوجد بيانات");
            }
            return Ok(rawData);
        }

        [HttpGet("GetAllRecordRequestTestByName")]

        public async Task<IActionResult> GetAllRecordRequestTestByName(string name)
        {
            var rawData = await _context.RecordRequestTests
            .Include(rrt => rrt.Record)
            .ThenInclude(r => r.Patient)
            .ThenInclude(p => p.Supervisor).Where(rrt => rrt.Record.Patient.FullName.Contains(name))
            .Select(rrt => new
            {
                PatientId = rrt.Record.Patient.PatientID,
                FullName = rrt.Record.Patient.FullName,
                Phone = rrt.Record.Patient.phoneNumber,
                Address = rrt.Record.Patient.Address,
                Username = rrt.Record.Patient.Supervisor.FullName,
                RecordId = rrt.Record.RecurdId,
                CreateAt = rrt.Record.RequestDate,
            })
            .Distinct() // لو المريض عنده أكثر من اختبار في نفس السجل يمنع التكرار
            .ToListAsync();
            if (rawData == null)
            {
                return BadRequest("لا يوجد بيانات");
            }
            return Ok(rawData);
        }


        [HttpGet("GetAllRecordRequestByiDTest")]
        public async Task<IActionResult> GetAllRecordRequestByiDTest(int id)
        {
            var rawData = await _context.RecordRequestTests
                .Include(rrt => rrt.Record)
                    .ThenInclude(r => r.Patient)
                        .ThenInclude(p => p.Supervisor)
                .Include(rrt => rrt.RequestTest)
                    .ThenInclude(rt => rt.TestResults)
                .Include(rrt => rrt.RequestTest)
                    .ThenInclude(rt => rt.Request)
                .Include(rrt => rrt.RequestTest)
                    .ThenInclude(rt => rt.Test)
                .Where(c => c.RecordId == id)
                .ToListAsync();

            if (!rawData.Any())
                return NotFound("لا توجد بيانات لهذا السجل.");

            var firstEntry = rawData.First();

            var result = new PatientFullRecordDto
            {
                PatientId = firstEntry.Record.Patient.PatientID,
                FullName = firstEntry.Record.Patient.FullName,
                Phone = firstEntry.Record.Patient.phoneNumber,
                Address = firstEntry.Record.Patient.Address,
                username = firstEntry.Record.Patient.Supervisor.FullName,

                Records = new List<PatientFullRecordDto.RecordData>
        {
            // سيكون هناك سجل واحد لأن الـ filter على RecordId
            new PatientFullRecordDto.RecordData
            {
                RecordId = id,
                RequestDate = firstEntry.Record.RequestDate,
                Requests = rawData
                    // كل عنصر في rawData يمثل عنصر من RecordRequestTests المتعلقة بهذا السجل
                    .Select(rrt => rrt.RequestTest.Request)
                    .Distinct()  // للتأكد ما تتكرر الطلبات نفسها
                    .Select(req => new PatientFullRecordDto.RequestData
                    {
                        RequestId = req.RecuestID,
                        RequestDate = req.CreatedAt,
                        Status = req.Status,
                        Tests = rawData
                            .Where(rrt => rrt.RequestTest.Request.RecuestID == req.RecuestID)
                            .Select(rrt => new PatientFullRecordDto.TestData
                            {
                                TestId = rrt.RequestTest.Test.TestId,
                                TestNameEn = rrt.RequestTest.Test.TestNameEn,
                                TestNameAr = rrt.RequestTest.Test.TestNameAr,
                                SampleType = rrt.RequestTest.Test.SampleType,
                                NormalRange = rrt.RequestTest.Test.NormalRange,
                                TestPrice = rrt.RequestTest.Test.Testprice,
                                Results = rrt.RequestTest.TestResults
                                    .Select(res => new PatientFullRecordDto.TestResultData
                                    {
                                        ResultId = res.ResultID,
                                        ResultValue = res.ResultValue,
                                        CreatedAt = res.CreatedAt
                                    }).ToList()
                            }).ToList()
                    }).ToList()
            }
        }
            };

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> AddRecordRequestTest(DTOAddRecordRequestTest recordRequestTest)
        {
            if(recordRequestTest == null)
            {
                return BadRequest("طلب عمليه خاطئة");
            }

            
           await _Reopsitory.AddRecordRequestTest(recordRequestTest.RecordId, recordRequestTest.RequestTestId);
            return Ok(recordRequestTest.Id);
        }
    }
}
