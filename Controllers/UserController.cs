using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Entity;
using Cross_WebApplication.Repository.Abstract;
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

        [HttpPost("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody]User user)
        {
            try
            {
                var updateDefinition = Builders<User>.Update
                            .Set(u => u.Name, user.Name)
                            .Set(u => u.Surname, user.Surname)
                            .Set(u => u.Role, user.Role)
                            .Set(u => u.Phone, user.Phone)
                            ;
                await _unitOfWork.Users.UpdateAsync(user, updateDefinition);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(string email, [FromBody] string roleName)
        {
            try
            {
                var identityUser = await _userManager.FindByEmailAsync(email);
                if (identityUser == null)
                {
                    return NotFound();
                }

                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    // Create the role if it doesn't exist
                    var newRole = new ApplicationRole(roleName);
                    await _roleManager.CreateAsync(newRole);
                }

                var user = await _unitOfWork.Users.GetByEmailAsync(email);

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
    }
}
