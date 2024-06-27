using Cross_WebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cross_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public RoleController() { }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok(AppConstant.RoleList);
        }
    }
}
