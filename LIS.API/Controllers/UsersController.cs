using APiUsers.Data;
using APiUsers.DTOs;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {


        public UsersController(AppDbContext app1,IReopsitory<Users> reopsitory)
        {
            _reopsitory = reopsitory;
            _context=app1;
        }

        private readonly AppDbContext _context;
        private readonly IReopsitory<Users> _reopsitory;

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var usersDto = await _context.users
                .Include(u => u.Roles)
                .Select(u => new
                {u.UserID,
                    u.FullName,
                    u.Email,
                    u.Password,
                    u.IsActive,
                    u.RoleId,
                    RoleName = u.Roles.Name
                }).ToListAsync();

            return Ok(usersDto);
        }

        [HttpGet("GetUserBbyRole")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllByRole()
        {

            var result = await _reopsitory.FindAsync(u => u.RoleId==3);
            return Ok(result.ToList());

        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var user = await _reopsitory.FindByidAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("Name")]
        public async Task<IActionResult> SearshByName(string Name)
        {
            var keyword = Name.Trim().ToLower();

            var result = await _reopsitory.selectone(user =>
                user.FullName.ToLower().Contains(keyword) ||
                user.FullName.ToLower().StartsWith(keyword)
            );

            if (result == null)
            {
                return NotFound("لم يتم العثور على نتائج مشابهة.");
            }

            return Ok(result);
        }


        [HttpPost("AddUsers")]
        public async Task<ActionResult<Users>> AddUsers(DTOCreateUsers dTO)
        {
            // تحقق من الإيميل المكرر
            if (await _context.users.AnyAsync(u => u.Email == dTO.Email))
            {
                return BadRequest("الإيميل مستخدم بالفعل من قبل مستخدم آخر");
            }

            // تحقق من الـ RoleId
            var roleExists = await _context.roles.AnyAsync(r => r.RoleId == dTO.RoleId);
            if (!roleExists)
            {
                return BadRequest("RoleId غير موجود");
            }

            // توليد Salt و Hash
            var salt = GenerateSalt();
            dTO.Salt = salt;
            dTO.Password = HashPassword(dTO.Password, salt);

            var user = new Users
            {
                FullName = dTO.FullName,
                Email = dTO.Email,
                Password = dTO.Password,
                Salt = salt,
                RoleId = dTO.RoleId,
                IsActive = dTO.IsActive,
            };

            try
            {
                _reopsitory.AddOne(user); // داخليًا لازم يعمل SaveChangesAsync
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            // جلب المستخدم مع بيانات الدور من قاعدة البيانات مباشرة
            var user = await _context.users
                .Include(u => u.Roles)  // تحميل علاقة الدور مع المستخدم
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return NotFound("المستخدم غير موجود");

            var hashedPassword = HashPassword(dto.Password, user.Salt);
            if (user.Password != hashedPassword)
                return Unauthorized("كلمة المرور غير صحيحة");

            // تأكد من وجود الدور أو افتراض "User"
            var roleName = user.Roles?.Name ?? "User";

            // إنشاء Claims مع الدور الصحيح
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, roleName)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsAReallyLongSecretKeyForJwtToken12345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "LIS",
                audience: "sa@gmail.com",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                userId = user.UserID,
                fullName = user.FullName,
                email = user.Email,
                role = roleName,
                isActive = user.IsActive
            });
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] DTOEditUsers dTO)
        {
            if (id == 0)
                return BadRequest();

            // تحقق من تطابق id مع dTO.UserID
            if (id != dTO.UserID)
                return BadRequest("معرف المستخدم في الرابط لا يطابق معرف المستخدم في البيانات.");

            var user = await _reopsitory.FindByidAsync(id);
            if (user == null)
                return NotFound();


            user.FullName = dTO.FullName;
            user.Email = dTO.Email;
            user.RoleId = dTO.RoleId;
            user.IsActive = dTO.IsActive;
            
            _reopsitory.UpdateOne(user);
            return NoContent();
        }


        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _reopsitory.FindByidAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _reopsitory.DeleteOne(user);
            return Ok(user);
        }

        [HttpGet("byiD")]
        public async Task<IActionResult> SearshByiD(int iD)
        {
            var Result = await _reopsitory.selectone(name => name.UserID == iD);
            if (Result == null)
            {
                return NotFound("patient inValid");
            }
            return Ok(Result);
        }

        private string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            return Convert.ToBase64String(sha256.ComputeHash(combined));
        }

        [HttpPut("EditmyAccount")]
        public async Task<IActionResult> EditmyAccount(int id, [FromBody] DTOEditUsers dTO)
        {
            if (id == 0 || id != dTO.UserID)
                return BadRequest("معرف المستخدم غير صحيح.");

            var user = await _reopsitory.FindByidAsync(id);
            if (user == null)
                return NotFound("المستخدم غير موجود.");

            // تحديث البيانات
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(dTO.Password, salt);

            user.FullName = dTO.FullName;
            user.Email = dTO.Email;
            user.Salt = salt;
            user.Password = hashedPassword;
            user.RoleId = dTO.RoleId;
            user.IsActive = dTO.IsActive;

             _reopsitory.UpdateOne(user); 

            return NoContent();
        }

    }


    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
