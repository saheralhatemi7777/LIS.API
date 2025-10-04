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
    public class PatientController : ControllerBase
    {

       

        public PatientController(AppDbContext app1,IReopsitory<Patient> reopsitory)
        {
            _reopsitory=reopsitory;
            _Context=app1;
        }


        private readonly IReopsitory<Patient> _reopsitory;
        private readonly AppDbContext _Context;
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Patient>>> GetAllpatients()
        {
            var result = await _Context.patients
           .Include(c => c.Supervisor)
           .Select(p => new DTOCreatePatient
           {
            userName=p.Supervisor.FullName,
            PatientID = p.PatientID,
            FullName = p.FullName,
            BirthDate = p.BirthDate,
            Gender = p.Gender,
            phoneNumber = p.phoneNumber,
            Password = p.Password,
            Address = p.Address,
            SupervisorID = p.SupervisorID,
            }) .ToListAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound("لا يوجد بيانات");
            }

            return Ok(result);

        }

        [HttpPost("Addpatients")]

        public async Task<ActionResult> Addpatients(DTOCreatePatient dTO)
        {
            if(dTO == null)
            {
                return NotFound("تاكد من صحة البيانات");
            }

            var response = await _Context.patients
                .FirstOrDefaultAsync(p => p.FullName.Trim() == dTO.FullName.Trim());
            if (response != null)
            {
                return BadRequest($"يوجد مريض بهذ الاسم بالفعل ويحمل رقم معرف{response.PatientID}");
            }
            else
            {
                var patient = new Patient
                {

                    FullName = dTO.FullName,
                    BirthDate = dTO.BirthDate,
                    Gender = dTO.Gender,
                    phoneNumber = dTO.phoneNumber,
                    Password = dTO.Password,
                    Address = dTO.Address,
                    SupervisorID = dTO.SupervisorID,

                };

                if (patient == null)
                {
                    return BadRequest();
                }
                _reopsitory.AddOne(patient);
                return Ok(patient);
            }
        }

        [HttpPut("patientid")]
        public async Task<IActionResult> Upatepatients(int patientid,DTOCreatePatient dTO)
        {
            if(patientid == 0)
            {
                return NotFound();
            }
            var patients = await _reopsitory.FindByidAsync(patientid);
            if (dTO == null || patients==null)
            {
                return NotFound("patientiD NotFound");
            }
            else
            {
                 patients.FullName = dTO.FullName;
                 patients.BirthDate= dTO.BirthDate;
                 patients.Gender= dTO.Gender;
                 patients.phoneNumber = dTO.phoneNumber;
                 patients.Password = dTO.Password;
                 patients.Address = dTO.Address;
                 patients.SupervisorID = dTO.SupervisorID;
            }
            
            _reopsitory.UpdateOne(patients);
            return Ok(patients);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetpatientsById(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var patients = await _reopsitory.FindByidAsync(id);
            if (patients == null)
            {
                return NotFound("patientiD NotFound");
            }
            
            return Ok(patients);

        }

        [HttpDelete("by")]
        public async Task<IActionResult> Deletepatients(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var patients = await _reopsitory.FindByidAsync(id);
            if (patients == null)
            {
                return NotFound("patientiD NotFound");
            }
            _reopsitory.DeleteOne(patients);
            return Ok(patients);

        }

        [HttpGet("Samples/Name")]

        public async Task<IActionResult> SearshByName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return BadRequest("يجب إدخال اسم للبحث.");

            var keyword = Name.Trim().ToLower();

            var result = await _reopsitory.selectAll(patient =>
                patient.FullName.ToLower().Contains(keyword) ||
                patient.FullName.ToLower().StartsWith(keyword)
            );

            if (result == null)
                return NotFound("لم يتم العثور على نتائج مشابهة.");

            return Ok(result);
        }
        [HttpPost("login")]

        public async Task<ActionResult<Patient>> Login([FromBody] LoginDtO dto)
        {
            var user = await _reopsitory.selectone(u => u.phoneNumber == dto.Phone || u.Password == dto.Password);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود");
            }

            if (user.Password != dto.Password || user.phoneNumber != dto.Phone)
            {
                return Unauthorized("كلمة  او رقم الهاتف غير صحيحا تاكد من البيانات");
            }

            return Ok(user);
        }

    }
    public class LoginDtO
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
