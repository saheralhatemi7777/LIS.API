using APiUsers.Data;
using APiUsers.DTOs.DTORequestTest;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestTestController : ControllerBase
    {

        public RequestTestController (AppDbContext appDb,IReopsitory<RequestTest> reopsitory)
        {
            _Repository=reopsitory;
            _Context = appDb;
        }


        public readonly IReopsitory<RequestTest> _Repository;
        public readonly AppDbContext _Context;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var requestTestsGrouped = await _Context.RequestTest
                .Include(rt => rt.Test)
                .Include(rt => rt.Request)
                .ThenInclude(r => r.Patient)
                .Where(rt => rt.Request.Status == "قيد التحليل")
                .GroupBy(rt => rt.RequestID)
                .Select(g => new
                {
                    RequestTestID =g.Select(rt => rt.RequestTestID).FirstOrDefault(),
                    RequestID = g.Key,
                    PatientName = g.Select(x => x.Request.Patient.FullName).FirstOrDefault(),
                    Status = g.Select(x => x.Request.Status).FirstOrDefault(),
                    CreatedAt = g.Select(x => x.Request.CreatedAt).FirstOrDefault(),
                    Tests = g.Select(x => new
                    {
                        Name = x.Test.TestNameEn,
                        Samples = x.Test.SampleType,
                        Price = x.Test.Testprice
                    }).ToList(),
                    
                }).ToListAsync();

            // تسلسل (Serialize) النتيجة مع JSON مرتب
            var json = System.Text.Json.JsonSerializer.Serialize(
                requestTestsGrouped,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true // يخلي JSON مرتب
        });

            return Content(json, "application/json");
        }

        [HttpGet("GetByid")]
        public async Task<IActionResult> GetByid(int id)
        {
            var requestTestsGrouped = await _Context.RequestTest
                 .Include(rt => rt.Test)
                 .Include(rt => rt.Request)
                 .ThenInclude(r => r.Patient)
                 .Where(rt => rt.RequestID==id)
                 .GroupBy(rt => rt.RequestID)
                 .Select(g => new
                 {
                     RequestTestID = g.Select(rt => rt.RequestTestID).FirstOrDefault(),
                     RequestID = g.Key,
                     PatientName = g.Select(x => x.Request.Patient.FullName).FirstOrDefault(),
                     Status = g.Select(x => x.Request.Status).FirstOrDefault(),
                     CreatedAt = g.Select(x => x.Request.CreatedAt).FirstOrDefault(),
                     Tests = g.Select(x => new
                     {
                         testid =x.TestID,
                         Name = x.Test.TestNameEn,
                         Samples = x.Test.SampleType,
                         Price = x.Test.Testprice,
                         Roung =x.Test.NormalRange
                     }).ToList(),
                 }).ToListAsync();


                var json = System.Text.Json.JsonSerializer.Serialize(
                requestTestsGrouped,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true // يخلي JSON مرتب
                });

            return Content(json, "application/json");
        }

        [HttpGet("GetAllDataRequestTest")]

        public async Task<IActionResult> GetAllDataRequestTest()
        {
            var requestTestsGrouped = await _Context.RequestTest
                .Include(rt => rt.Test)
                .Include(rt => rt.Request)
                .ThenInclude(r => r.Patient)
                 .Where(rt => rt.Request.Status == "طلب تحليل")
                .GroupBy(rt => rt.RequestID)
                .Select(g => new
                {
                    RequestTestID = g.Select(rt => rt.RequestTestID).FirstOrDefault(),
                    RequestID = g.Key,
                    PatientName = g.Select(x => x.Request.Patient.FullName).FirstOrDefault(),
                    Status = g.Select(x => x.Request.Status).FirstOrDefault(),
                    CreatedAt = g.Select(x => x.Request.CreatedAt).FirstOrDefault(),
                    Tests = g.Select(x => new
                    {
                        Name = x.Test.TestNameEn,
                        Samples = x.Test.SampleType,
                        Price = x.Test.Testprice
                    }).ToList(),

                }).ToListAsync();

            // تسلسل (Serialize) النتيجة مع JSON مرتب
            var json = System.Text.Json.JsonSerializer.Serialize(
                requestTestsGrouped,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true // يخلي JSON مرتب
                });

            return Content(json, "application/json");
        }

        [HttpGet("GetAllDataRequestTestbyid")]

        public async Task<IActionResult> GetAllDataRequestTestbyid(int id)
        {
            var requestTestsGrouped = await _Context.RequestTest
                .Include(rt => rt.Test)
                .Include(rt => rt.Request)
                .ThenInclude(r => r.Patient).Where(c => c.RequestID == id)
                .Where(rt => rt.Request.Status == "طلب تحليل")
                .GroupBy(rt => rt.RequestID)
                .Select(g => new
                {
                    RequestTestID = g.Select(rt => rt.RequestTestID).FirstOrDefault(),
                    RequestID = g.Key,
                    PatientName = g.Select(x => x.Request.Patient.FullName).FirstOrDefault(),
                    Status = g.Select(x => x.Request.Status).FirstOrDefault(),
                    CreatedAt = g.Select(x => x.Request.CreatedAt).FirstOrDefault(),
                    Tests = g.Select(x => new
                    {
                        Name = x.Test.TestNameEn,
                        Samples = x.Test.SampleType,
                        Price = x.Test.Testprice
                    }).ToList(),

                }).ToListAsync();

            // تسلسل (Serialize) النتيجة مع JSON مرتب
            var json = System.Text.Json.JsonSerializer.Serialize(
                requestTestsGrouped,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true // يخلي JSON مرتب
                });

            return Content(json, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRequestTest([FromBody] DTORequestTest requestTests)
        {
            if (requestTests == null || requestTests.TestIds == null || !requestTests.TestIds.Any())
            {
                return BadRequest("يجب إرسال قائمة أرقام التحاليل.");
            }

            // استدعاء الدالة التي تضيف التحاليل للطلب، مع إرجاع قائمة بالـIDs التي تم إضافتها
            var addedRecordIds = await _Repository.AddTestsToRequestAsyncS(requestTests.RequestID, requestTests.TestIds);

            // إرجاع القائمة
            return Ok(addedRecordIds);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequestTest (int id)
        {
            if(id == 0)
            {
                return BadRequest("طلب عمليه خاطئة");
            }
            var request = await _Repository.FindByidAsync(id);
            if(request == null)
            {
                return NotFound("لا يوجد بيانات حاليا");
            }

            _Repository.DeleteOne(request);
            return Ok(request);
        }
    }
}
