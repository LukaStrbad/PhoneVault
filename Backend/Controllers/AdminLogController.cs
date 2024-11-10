using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class AdminLogController:ControllerBase
    {
        private readonly AdminLogService _adminlogService;
        public AdminLogController(AdminLogService adminlogService)
        {
            _adminlogService = adminlogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminLog>>> GetAllAdminLogs()
        {
            var logs=await _adminlogService.GetAllAdminLogs();
            if (!(logs == null))
            {
                return Ok(logs);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminLog>> GetAdminLogById(int id)
        {
            var log=await _adminlogService.GetAdminLogById(id);
            if(!(log == null))
            {
                return Ok(log);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddAdminLog(AdminLogDTO log)
        {
            await _adminlogService.AddAdminLog(log);
            return Ok(log);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAdminLog(AdminLog log)
        {
            await _adminlogService.UpdateAdminLog(log);
            return Ok(log);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdminLog(int id)
        {
            if (!(id == null))
            {
                await _adminlogService.DeleteAdminLog(id);
                return Ok();
            }
            else{
                return NotFound();
            }
        }


    }
}
