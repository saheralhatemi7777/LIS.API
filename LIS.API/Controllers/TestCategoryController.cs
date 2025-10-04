using APiUsers.DTOs.TestCategory;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCategoryController : ControllerBase
    {

        public TestCategoryController(IReopsitory<TestCategory> reopsitory)
        {
            _reopsitory = reopsitory;
        }
        public readonly IReopsitory<TestCategory> _reopsitory;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var TestCategory = await _reopsitory.GetAllAsync();
            if(TestCategory == null)
            {
                return BadRequest("لا يوجد بيانات ");
            }
            
            return Ok(TestCategory);
        }

        [HttpPost("AddNewTestCategory")]
        public async Task<IActionResult> AddNewTestCategory(TestCategory category)
        {
            if (category == null)
            {
                return BadRequest("تحقق من صحة البيانات");
            }

              _reopsitory.AddOne(category);

            return Ok(category);
        }

        [HttpPut("EditCategory")]
        public async Task<IActionResult> EditCategory(int id ,DTOTestCategory dTOTestCategory)
        {
            var result = await _reopsitory.FindByidAsync(id);
            if(result == null)
            {
                return BadRequest($"لا يوجد بيانات بهذ الرقم :{id}");
            }

            result.CategoryNameEn = dTOTestCategory.CategoryNameEn;
            result.CategoryNameAr = dTOTestCategory.CategoryNameAr;

            _reopsitory.UpdateOne(result);
            return Ok(result);
        }

        [HttpGet("GetTestCategoryBy")]
       public async Task<IActionResult> GetTestCategoryBy(int id)
        {
            if(id == null)
            {
                return BadRequest("طلب خاطئ");
            }
            var response = await _reopsitory.FindByidAsync(id);
            if(response == null)
            {
                return NotFound("لا يوجد بيانات");
            }
            return Ok(response);
        }


       [HttpDelete]

       public async Task<IActionResult> DeleteTestCategorys(int id)
       {    

            if(id == null)
            {
                return BadRequest($"لا يوجد بيانات بهذ الرقم {id}");
            }

            var result = await _reopsitory.FindByidAsync(id);

            if(result == null)
            {
                return NotFound("لا يوجد بيانات لحذفها");
            }

            _reopsitory.DeleteOne(result);  
            return Ok(result);
       }

        [HttpGet("Name")]
        public async Task<IActionResult> SearshByName(string Name)
        {
            var keyword = Name.Trim().ToLower();

            var result = await _reopsitory.selectone(test =>
                test.CategoryNameAr.ToLower().Contains(keyword) ||
                test.CategoryNameEn.ToLower().StartsWith(keyword)
            );

            if (result == null)
            {
                return NotFound("لم يتم العثور على نتائج مشابهة.");
            }

            return Ok(result);
        }

    }
}
