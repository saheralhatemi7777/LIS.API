using APiUsers.Data;
using APiUsers.DTOs.DTOSettingSystem;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using مشروع_ادار_المختبرات.Models;

namespace APiUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingSystemController : ControllerBase
    {

        public SettingSystemController(AppDbContext app1,IReopsitory<SettingSystem> reopsitory)
        {
            _reopsitory = reopsitory;
            _context = app1;
        }
        private readonly AppDbContext _context;
        private readonly IReopsitory<SettingSystem> _reopsitory;

        [HttpGet("GetAllSettingData")]
        public async Task<IActionResult> GetAllSettingData()
        {
            var respnse = await _reopsitory.GetAllAsync();
            var firstSetting = respnse?.FirstOrDefault();

            if (firstSetting == null)
            {
                return BadRequest();
            }
           
            return Ok(firstSetting); 
        }

        [HttpGet("GetAllSettingDataByid")]
        public async Task<IActionResult> GetAllSettingDataByid(int id)
        {
            var respnse = await _reopsitory.FindByidAsync(id);
           
            if (respnse == null)
            {
                return BadRequest();
            }

            return Ok(respnse);
        }

        [HttpPost("AddSettingSystem")]
        public async Task<IActionResult> Create(DTOSettingSystem setting)
        {
            var existing = await _context.SettingSystem.FirstOrDefaultAsync();
            if (existing == null)
            {
                var request1 = new SettingSystem
                {
                    Name = setting.Name,
                    PhoneNumber = setting.PhoneNumber,
                    Email = setting.Email,
                    Addrees = setting.Addrees,
                    Image = setting.Image,
                    Descraption = setting.Descraption,

                };
                _reopsitory.AddOne(request1);
            }
            else
            {
                existing.Name = setting.Name;
                existing.PhoneNumber = setting.PhoneNumber;
                existing.Email = setting.Email;
                existing.Addrees = setting.Addrees;
                existing.Image = setting.Image;
                existing.Descraption = setting.Descraption;
                _reopsitory.UpdateOne(existing);
            }
            return Ok(setting);
        }

       

    }
}
