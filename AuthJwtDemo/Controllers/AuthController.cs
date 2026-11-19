using AuthJwtDemo.DTO;
using AuthJwtDemo.Models;
using AuthJwtDemo.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwtDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager; // For register user
        private readonly SignInManager<ApplicationUser> _signInManager; // For login
        private readonly ITokenService _tokenService; // For Token gen

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserRegisterDto requset)
        {
            // map dto to domain model

            var user = new ApplicationUser
            {
                UserName = requset.Username,
                Email = requset.Email
            };

            var result = await _userManager.CreateAsync(user, requset.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, "User");

            return Ok("Registartion Successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLoginDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if(user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return Ok(new
            {
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    username = user.UserName
                },
                token
            });
        }
    }
}
