using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Entity;
using Cross_WebApplication.Models;
using Cross_WebApplication.Models.DTO;
using Cross_WebApplication.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Data;
using static Cross_WebApplication.Models.AppConstant;

namespace Cross_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Policy = AppConstant.Role.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _unitOfWork.Users.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserById")]
        [Authorize(Policy = AppConstant.Role.Reader)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var result = await _unitOfWork.Users.GetByIdAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = AppConstant.Role.Reader)]
        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile(string userId, [FromBody] UserDto userDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest("User not found");
                }
                var updateDefinition = Builders<User>.Update
                            .Set(u => u.Name, userDto.Name)
                            .Set(u => u.Surname, userDto.Surname)
                            .Set(u => u.Role, userDto.Role)
                            .Set(u => u.Phone, userDto.Phone);
                await _unitOfWork.Users.UpdateAsync(user, updateDefinition);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("AssignRoleToUser")]
        [Authorize(Policy = AppConstant.Role.Admin)]
        public async Task<IActionResult> AssignRoleToUser(string userId, [FromBody] string roleName)
        {
            try
            {
                var identityUser = await _userManager.FindByIdAsync(userId);
                if (identityUser == null)
                {
                    return NotFound();
                }

                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists && AppConstant.RoleList.Contains(roleName))
                {
                    // Create the role if it doesn't exist
                    var newRole = new ApplicationRole(roleName);
                    await _roleManager.CreateAsync(newRole);
                }

                var user = await _unitOfWork.Users.GetByIdAsync(userId);

                if (user != null)
                {
                    // Assign the user to the role
                    await _userManager.AddToRoleAsync(identityUser, roleName);
                    var updateDefinition = Builders<User>.Update
                              .Set(u => u.Role, roleName);
                    await _unitOfWork.Users.UpdateAsync(user, updateDefinition);

                    return Ok($"User {identityUser.UserName} successfully assigned to role {roleName}");
                }
                return BadRequest("User not found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("AddUser")]
        [Authorize(Policy = AppConstant.Role.Admin)]
        public async Task<IActionResult> AddUser(UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser appUser = new ApplicationUser
                    {
                        UserName = userDto.UserName,
                        Email = userDto.Email
                    };

                    IdentityResult result = await _userManager.CreateAsync(appUser, AppConstant.DefaultPassword);
                    if (result.Succeeded)
                    {
                        var identityUser = await _userManager.FindByEmailAsync(userDto.Email);
                        await _userManager.AddToRoleAsync(identityUser, userDto.Role);

                        var user = userDto.CopyToUserEntity();
                        user.Id = identityUser.Id;

                        await _unitOfWork.Users.AddAsync(user);
                        return Ok("User successfully created");
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

        [HttpDelete("DeleteUser")]
        [Authorize(Policy = AppConstant.Role.Admin)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var userIdentity = await _userManager.FindByIdAsync(userId);

                var result = await _userManager.DeleteAsync(userIdentity);

                if (result.Succeeded)
                {
                    await _unitOfWork.Users.DeleteAsync(userId);

                    return Ok("User successfully deleted");
                }
                else
                {
                    return BadRequest(String.Join(", ", result.Errors.Select(x => x.Description)));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
