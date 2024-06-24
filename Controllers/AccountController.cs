using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Entity;
using Cross_WebApplication.Models;
using Cross_WebApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cross_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        public AccountController(UserManager<ApplicationUser> userManager, IJwtService jwtService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var token = await _jwtService.GenerateToken(user);
                    return Ok(new { Token = token });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("signup")] 
        public async Task<IActionResult> SignUp([FromBody] IdentityUsers user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser appUser = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email
                    };

                    IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                    if (result.Succeeded)
                    {
                        // Create user to User table
                        var userDto = new User
                        {
                            Name = user.Name,
                            Email = user.Email,
                            Phone = user.Phone,
                            Surname = user.Surname,
                            UserName = user.UserName
                        };
                        await _unitOfWork.Users.AddAsync(userDto);
                        return Ok("User successfully created");
                    }
                    else
                    {                     
                        return BadRequest(String.Join(", ", result.Errors.Select(x=>x.Description)));
                    }
                }
                return BadRequest(String.Join(", ", ModelState.Values.SelectMany(x => x.Errors)));
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }


        }
    }
}
