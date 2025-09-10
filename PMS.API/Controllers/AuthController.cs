using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using PMS.Application.Requests;
using PMS.Domain.Entities;
using PMS.Domain.Enums;

namespace PMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAppUserRepository appUserRepo) : ControllerBase
    {
        private readonly IAppUserRepository _appUserRepo = appUserRepo;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _appUserRepo.GetUserByEmailAsync(request.Email);
            if(user is null)
            {
                return Unauthorized("Geçersiz e-posta veya parola.");
            }

            if(new PasswordHasher<AppUser>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Geçersiz e-posta veya parola.");
            }

            return Ok(_appUserRepo.GenerateJwtToken(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _appUserRepo.IsEmailUniqueAsync(request.Email))
            {
                return Conflict("Bu e-posta adresi zaten kayıtlı.");
            }

            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = Enumeration.AppRole.Member,
                PasswordHash = new PasswordHasher<AppUser>().HashPassword(null!, request.Password)
            };

            await _appUserRepo.AddUserAsync(user);

            return Ok("You have been registered");
        }

    }
}
