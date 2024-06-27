using Cross_WebApplication.Context;
using Cross_WebApplication.Models;
using CrossApplication.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cross_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = AppConstant.Role.Reader)]
        public async Task<IActionResult> GetEvents([FromQuery] EventRequestDto eventParameters)
        {
            try
            {
                var events = await _unitOfWork.Events.GetAllEventsAsync(eventParameters);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("process/{id}")]
        [Authorize(Policy = AppConstant.Role.Contributor)]
        public async Task<IActionResult> MarkAsProcessed(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _unitOfWork.Events.MarkAsProcessedAsync(id, userId);
                return Ok(new {message = "Event processed"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
