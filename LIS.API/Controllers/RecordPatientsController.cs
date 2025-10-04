using APiUsers.Data;
using APiUsers.DTOs;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordPatientsController : ControllerBase
    {

        public RecordPatientsController(AppDbContext app1,IReopsitory<RecordPatients> reopsitory)
        {
             _reopsitory = reopsitory;
             _context = app1;
        }

        private readonly AppDbContext _context;

        private readonly IReopsitory<RecordPatients> _reopsitory;

        [HttpGet("GetAllRecurde")]
        public async Task<ActionResult<IEnumerable<DTORecordPatients>>> GetAllRecords()
        {
            var result = await _context.RecordPatients
            .Include(r => r.RecordRequestTests).ToListAsync();
            return Ok(result);
        }


        [HttpPost("AddRecurd")]
        public async Task<ActionResult> AddRecurd(DTORecordPatients recordPatients)
        {
            if (recordPatients == null)
            {
                return BadRequest("تحقق من صحة الطلب");
            }

            // إنشاء سجل جديد
            var newRecord = new RecordPatients
            {
                PatientID = recordPatients.PatientID,
                RequestDate = recordPatients.RequestDate,
                TechnicianiD = recordPatients.TechnicianiD,
            };

            // التحقق إذا المريض عنده سجل مسبق
            var existingRecord = await _context.RecordPatients.FirstOrDefaultAsync(c=>c.PatientID == newRecord.PatientID);

            if (existingRecord != null)
            {
                return BadRequest("المريض معه سجل بالفعل");
            }

            // إضافة السجل الجديد
            _reopsitory.AddOne(newRecord);

            return Ok(newRecord.RecurdId); // يرجع فقط رقم السجل
        }

        [HttpGet("GOToRecurdByID")]
        public async Task<ActionResult> AddRecurd(int Patientid)
        {
            var response = await _context.RecordPatients
                                .Include(c => c.Patient)
                                .FirstOrDefaultAsync(c => c.PatientID == Patientid);
            if (response == null)
            {
                return BadRequest("NotFound Data");
            }
            return Ok(response.RecurdId);
        }
    }
}
