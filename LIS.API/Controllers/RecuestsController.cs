using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APiUsers.Data;
using مشروع_ادار_المختبرات.Models;
using APiUsers.Repository.Base;
using APiUsers.DTOs;
using APiUsers.DTOs.DTOSample;
using APiUsers.DTOs.DTORequestTest;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class RecuestsController : ControllerBase
    {

        public RecuestsController(AppDbContext dbContext,IReopsitory<Recuests> reopsitory)
        {
            
            _reopsitory = reopsitory;
            _context = dbContext;
        }
        private readonly IReopsitory<Recuests> _reopsitory;
        private readonly AppDbContext _context;
        [HttpGet("GetAllRecuests")]
        public async Task<ActionResult<IEnumerable<DTORequest>>> GetAllRecuests()
        {
            var sampleDtos = await _context.recuests
               .Include(s => s.Patient)
               .Include(s => s.User)
               .Select(s => new DTORequest
               {
                   RecuestTestID = s.RecuestID,
                   PatientID = s.PatientID,
                   Status = s.Status,
                   Notes = s.Notes,
                   CreatedAt = s.CreatedAt,
                   PatientName = s.Patient.FullName,
                   usernName = s.User != null ? s.User.FullName : null,
                   UserID = s.UserID
               }).Where(s => s.Status=="طلب تحليل")
               .ToListAsync();

            if (sampleDtos == null || !sampleDtos.Any())
            {
                return NotFound("لا يوجد بيانات");
            }

            return Ok(sampleDtos);
        }


        [HttpPost("AddNewRecuests")]
        public async Task<IActionResult> AddRecuests(DTOAddRecuests dTO)
        {

            if (dTO == null)
                return BadRequest("البيانات غير موجودة");
                
            var sample = new Recuests
            {
                PatientID = dTO.PatientID,
                Status = dTO.Status,
                Notes = dTO.Notes,
                CreatedAt = dTO.CreatedAt,
                UserID =dTO.UserID
            };

            try
            {
                _reopsitory.AddOne(sample);
            }
            catch (DbUpdateException ex)
            {
                var sqlError = ex.InnerException?.Message ?? ex.Message;
                return BadRequest("SQL Error: " + sqlError);
            }

            return Ok(sample.RecuestID);
        }


        [HttpDelete("DeleteRecuests")]
        public async Task<ActionResult<IEnumerable<Recuests>>> DeleteRecuests(int id)
        {
            if(id == 0)
            {
                return BadRequest("Insert Number of ID Please");
            }
            var sample = await _reopsitory.FindByidAsync(id);
            if(sample==null)
            {
                return NotFound();
            }

           _reopsitory.DeleteOne(sample);
            return Ok(sample);
        }


        [HttpPut("EditRecuests")]
        public async Task<IActionResult> Edit(int id, DTORequest dTO)
        {
            if(id==0)
            {
                return BadRequest("Insert number of ID Please ");
            }

            var sample = await _reopsitory.FindByidAsync(id);

            if (sample == null || dTO == null)
            {
                return NotFound("Invalid Data");
            }
            
            sample.PatientID = dTO.PatientID;
            sample.Status = dTO.Status;
            sample.Notes = dTO.Notes;
            sample.CreatedAt = dTO.CreatedAt;
        
            _reopsitory.UpdateOne(sample);
            return Ok(sample);
        }

        [HttpPut("EditRecuestsStatus")]
        public async Task<IActionResult> EditRecuestsStatus(int id, [FromBody] string status)
        {
            if (id == 0)
                return BadRequest("Insert number of ID Please");

            var sample = await _reopsitory.FindByidAsync(id);

            if (sample == null)
                return NotFound("Invalid Data");

            // تحديث الحالة فقط
            sample.Status = status;

            _reopsitory.UpdateOne(sample);

            return Ok(new { message = "تم تحديث حالة الطلب بنجاح", status = sample.Status });
        }


        [HttpGet("Name")]
        public async Task<ActionResult> SearshByName(string Name)
        {
            if(Name == null)
            {
                return BadRequest("Insert Text of Barcode Please");
            }

            var Result = await _reopsitory.selectone(patient =>
                patient.Patient.FullName.ToLower().Trim().Contains(Name) ||
                patient.Patient.FullName.ToLower().Trim().StartsWith(Name));
            if (Result == null)
            {
                return NotFound("patient inValid");
            }
            return Ok(Result);
        }

        [HttpGet("GetRecuestsByiD")]
        public async Task<IActionResult> GetRecuestsByiD(int id)
        {
                 var sample = await _context.recuests
                .Include(s => s.Patient)
                .Where(s => s.RecuestID == id)
                .Select(s => new DTORequest
                {
                    RecuestTestID = s.RecuestID,
                    PatientID = s.PatientID,
                    Status = s.Status,
                    Notes = s.Notes,
                    CreatedAt = s.CreatedAt,
                })
                .FirstOrDefaultAsync();

            if (sample == null)
                
               return NotFound($"لم يتم العثور على العينة برقم ID = {id}");

            return Ok(sample);
        }

       // [HttpGet("MostUsedSamples")]
        //public async Task<IActionResult> GetMostUsedSamples()
        //{
        //    var result = await _context.recuests
        //        .GroupBy(s => s.SampleType.Name)
        //        .Select(g => new {
        //            SampleTypeName = g.Key,
        //            Count = g.Count()
        //        })
        //        .OrderByDescending(x => x.Count)
        //        .Take(5)
        //        .ToListAsync();

        //    return Ok(result);
        //}

    }
}
