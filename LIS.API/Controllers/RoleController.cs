using APiUsers.DTOs.DTORole;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IReopsitory<Roles> _reopsitory;
        public RoleController(IReopsitory<Roles> reopsitory)
        {
            _reopsitory = reopsitory;
        }

        [HttpGet("GetAllData")]
        public async Task<IActionResult> GetAllData()
        {
            var role = await _reopsitory.GetAllAsync();
            if(role == null)
            {
                return NotFound("لا يوجد بيانات ");
            }
            return Ok(role);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(DTORole role)
        {
            if(role == null)
            {
                return BadRequest("طلب اضافة خاطئ");
            }
            var result = new Roles
            {
                Name = role.Name,
            };
            _reopsitory.AddOne(result);
            return Ok(result);
        }

        [HttpPut("EditRoles")]
        public async Task<IActionResult> EditRole(int id,DTORole role)
        {
            if(id == 0)
            {
                return BadRequest("لا يوجد قيمه");
            }
            var _resultRole = await _reopsitory.FindByidAsync(id);
            if(_resultRole == null)
            {
                return BadRequest("لا يوجد بيانات");
            }

            _resultRole.Name = role.Name;
            _reopsitory.UpdateOne(_resultRole);

            return Ok(_resultRole);
        }

        [HttpDelete("DeleteRoles")]

        public async Task<IActionResult> DeleteRole(int id)
        {
            if(id==0)
            {
                return BadRequest("طلب خطاء");
            }
            var _resultRole = await _reopsitory.FindByidAsync(id);
            if(_resultRole == null)
            {
                return NotFound("لا يوجد بيانات");
            }
            _reopsitory.DeleteOne(_resultRole);
            return Ok(_resultRole);
        }
    }
}
