using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APiUsers.Data;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APiUsers.DTOs.DTOSTests;
using APiUsers.DTOs;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
            public TestController(AppDbContext dbContext, IReopsitory<Test> reopsitory)
            {
                _reopsitory=reopsitory;
                _context=dbContext;
            }


            private readonly IReopsitory<Test> _reopsitory;
            private readonly AppDbContext _context;

        [HttpGet("test")]
        public async Task<ActionResult<IEnumerable<Test>>> Index()
        {
            var test = await _context.Test
           .Include(c => c.TestCategory)
           .Select(c => new DTOTest
           {
                     TestId=c.TestId,
                     TestNameEn=c.TestNameEn,
                     TestNameAr=c.TestNameAr,
                     SampleType=c.SampleType,
                     CategoryNameEn=c.TestCategory.CategoryNameEn,
                     CategoryNameAr=c.TestCategory.CategoryNameAr,
                     NormalRange=c.NormalRange,
               Testprice =c.Testprice
           }).ToListAsync();

            if (test == null)
            {
                return NotFound();
            }
            return Ok(test);
        }

        [HttpPost("AddTestType")]
        public async Task<IActionResult> AddNewTestType([FromBody] DTOAddTestType dto)
        {
            if (dto == null)
            {
                return BadRequest("البيانات غير صحيحة");
            }

            var entity = new Test
            {
                TestId = dto.TestId,
                TestNameEn = dto.TestNameEn,
                TestNameAr = dto.TestNameAr,
                SampleType = dto.SampleType,
                NormalRange = dto.NormalRange,
                Testprice = dto.Testprice,
                CategoryId = dto.CategoryId
            };

            _reopsitory.AddOne(entity);
            await _context.SaveChangesAsync();
            return Ok(entity); 
        }


        [HttpPut("EditTest")]
        public async Task<IActionResult> EditeTest(int id,DTOAddTestType dTO)
        {
            if(id == 0)
            {
                return BadRequest("تحقق من صحة الطلب");
            }
            var result = await _reopsitory.FindByidAsync(id);
            if(result == null)
            {
                return BadRequest("لا يوجد بيانات");
            }

            result.TestNameEn =dTO.TestNameEn;
            result.TestNameAr = dTO.TestNameAr;
            result.SampleType = dTO.SampleType;
            result.NormalRange = dTO.NormalRange;
            result.Testprice = dTO.Testprice;
            result.CategoryId = dTO.CategoryId;
            _reopsitory.UpdateOne(result);

            return Ok(result);
        }


        [HttpDelete("DeleteTest")]
        public  async Task<IActionResult> DeleteTest(int id)
        {
            if(id == 0)
            {
                return BadRequest("تحقق من صحة الطلب");
            }
            var result = await _reopsitory.FindByidAsync(id);
            if(result == null)
            {
                return NotFound("لا يوجد بيانات ");
            }
            _reopsitory.DeleteOne(result);
            return Ok(result);

        }


        [HttpGet("GetTestById")]
        public async Task<IActionResult> GetTestById(int id)
        {
            if( id == 0)
            {
                return BadRequest("طلب خاطى");
            }
            var result = await _reopsitory.FindByidAsync(id);
            if(result ==null)
            {
                return NotFound("لا يوجد بيانات");
            }

            return Ok(result);
        }

        [HttpGet("GetTestListById")]
        public async Task<IActionResult> GetTestListById([FromQuery] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("لم يتم إرسال أي Ids");

            var result = await _reopsitory.FindListByIdAsync(ids);
            if (result == null || !result.Any())
                return NotFound("لا يوجد بيانات");

            return Ok(result);
        }



        [HttpGet("GetTestByName")]
        public async Task<IActionResult> GetTest(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return BadRequest("تحقق من صحة الطلب");

            var keyword = Name.Trim();

            // استخدام EF.Functions.Like لتحسين الأداء
            var result = await _reopsitory.selectone(test =>
                EF.Functions.Like(test.TestNameEn, $"%{keyword}%") ||
                EF.Functions.Like(test.TestNameAr, $"{keyword}%"));

            if (result == null)
                return NotFound("لا يوجد نتائج مطابقة");

            return Ok(result);
        }


        [HttpGet("GetTestByListId")]
        public async Task<IActionResult> GetTestByListId([FromQuery] List<int> id)
        {
            if (id == null || !id.Any()) // لاحظ ! قبل Any
            {
                return BadRequest("طلب خاطئ");
            }

            var result = await _reopsitory.FindByListidAsync(id);
            if (result == null || !result.Any())
            {
                return NotFound("لا يوجد بيانات");
            }

            return Ok(result);
        }


    }
}
