using APiUsers.Data;
using APiUsers.DTOs;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {


        public OperationsController(AppDbContext context,IReopsitory<Operations> reopsitory)
        {
            _reopsitory = reopsitory;
            _context = context;
        }
        private readonly AppDbContext _context;
        public IReopsitory<Operations> _reopsitory { get; set; }

        [HttpGet("GetAllOperations")]
        public async Task<IActionResult> GetAllOperations()
        {
            var response = await _context.Operations
                                       .Include(c => c.Users)
                                       .Include(p => p.Patients)
                                       .Select(g => new DTOShowOperations
                                       {
                                           OperationId = g.OperationId,
                                           ActionDate = g.ActionDate,
                                           UserName = g.Users != null ? g.Users.FullName : "N/A",
                                           ActionType = g.ActionType,
                                           PateintName = g.Patients.FullName,
                                           TableName = g.TableName
                                       }).ToListAsync();
                                   
                  if (!response.Any())
                  {
                     return NotFound("No operations found");
                  }

                   return Ok(response);

        }


        [HttpGet("GetAllOperationsbyname")]
        public async Task<IActionResult> GetAllOperationsbyname(string name)
        {
            var response = await _context.Operations.Where(u=>u.Users.FullName == name)
                                       .Include(c => c.Users)
                                       .Include(p => p.Patients)
                                       .Select(g => new DTOShowOperations
                                       {
                                           OperationId = g.OperationId,
                                           ActionDate = g.ActionDate,
                                           UserName = g.Users != null ? g.Users.FullName : "N/A",
                                           ActionType = g.ActionType,
                                           PateintName = g.Patients.FullName,
                                           TableName = g.TableName
                                       }).ToListAsync();

            if (!response.Any())
            {
                return NotFound("No operations found");
            }

            return Ok(response);

        }

        [HttpPost("AddOperations")]
        
        public async Task<IActionResult> AddOperations(DTOAddOperations dTO)
        {
            if(dTO == null)
            {
                return BadRequest("Not Found Data");
            }

            var data = new Operations
            {
                OperationId = dTO.OperationId,
                UserId = dTO.UserId,
                ActionDate = dTO.ActionDate,
                TableName = dTO.TableName,
                RecordId = dTO.RecordId,
                ActionType = dTO.ActionType,
            };
            _reopsitory.AddOne(data);
            return Ok(data);
        }
    }
}
