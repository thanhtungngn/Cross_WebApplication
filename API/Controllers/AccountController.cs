using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Entity;
using Cross_WebApplication.Models;
using Cross_WebApplication.Models.DTO;
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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        public AccountController(UserManager<ApplicationUser> userManager, IJwtService jwtService, IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
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

        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp([FromBody] IdentityUsers user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var defaultRole = AppConstant.Role.Reader;

                    ApplicationUser appUser = new ApplicationUser
                    {
                        Email = user.Email,
                        UserName = user.Email
                    };

                    IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                    if (result.Succeeded)
                    {
                        var identityUser = await _userManager.FindByEmailAsync(user.Email);

                        var roleExists = await _roleManager.RoleExistsAsync(defaultRole);
                        if (!roleExists && AppConstant.RoleList.Contains(defaultRole))
                        {
                            // Create the role if it doesn't exist
                            var newRole = new ApplicationRole(defaultRole);
                            await _roleManager.CreateAsync(newRole);
                        }

                        await _userManager.AddToRoleAsync(identityUser, defaultRole);
                        // Create user to User table
                        var userDto = new User
                        {
                            Id = identityUser.Id,
                            Name = user.Name,
                            Email = user.Email,
                            Phone = user.Phone,
                            Surname = user.Surname,
                            Role = AppConstant.Role.Reader
                        };
                        await _unitOfWork.Users.AddAsync(userDto);
                        return Ok(new { message = "User successfully created" });
                    }
                    else
                    {
                        return BadRequest(String.Join(", ", result.Errors.Select(x => x.Description)));
                    }
                }
                return BadRequest(String.Join(", ", ModelState.Values.SelectMany(x => x.Errors)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }


        }


        [HttpPost("SignupAdmin")]
        public async Task<IActionResult> SignUpAdmin([FromBody] IdentityUsers user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var defaultRole = AppConstant.Role.Admin;

                    ApplicationUser appUser = new ApplicationUser
                    {
                        Email = user.Email,
                        UserName = user.Email
                    };

                    IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                    if (result.Succeeded)
                    {
                        var identityUser = await _userManager.FindByEmailAsync(user.Email);

                        var roleExists = await _roleManager.RoleExistsAsync(defaultRole);
                        if (!roleExists && AppConstant.RoleList.Contains(defaultRole))
                        {
                            // Create the role if it doesn't exist
                            var newRole = new ApplicationRole(defaultRole);
                            await _roleManager.CreateAsync(newRole);
                        }


                        await _userManager.AddToRoleAsync(identityUser, defaultRole);
                        // Create user to User table
                        var userDto = new User
                        {
                            Id = identityUser.Id,
                            Name = user.Name,
                            Email = user.Email,
                            Phone = user.Phone,
                            Surname = user.Surname,
                            Role = AppConstant.Role.Admin
                        };
                        await _unitOfWork.Users.AddAsync(userDto);
                        return Ok(new { message = "User successfully created" });
                    }
                    else
                    {
                        return BadRequest(String.Join(", ", result.Errors.Select(x => x.Description)));
                    }
                }
                return BadRequest(String.Join(", ", ModelState.Values.SelectMany(x => x.Errors)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
    }
}
