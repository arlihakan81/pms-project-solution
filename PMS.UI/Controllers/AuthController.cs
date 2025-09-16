using PMS.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Security.Claims;
using PMS.Application.Interfaces;

namespace PMS.UI.Controllers
{
    [Authorize]
    public class AuthController(IAppUserRepository appUserRepository, IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly IAppUserRepository _appUserRepository = appUserRepository;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var apiClient = _httpClientFactory.CreateClient("PMSApiClient");
            var response = await apiClient.PostAsJsonAsync<LoginRequest>($"{apiClient.BaseAddress}/auth/login", request);
            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response as string first to see what's actually coming back
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw response: {responseString}");
                var token = responseString.Trim('"'); // Remove quotes if present

                if (string.IsNullOrEmpty(token))
                {
                    // Handle null token case
                    throw new Exception("Token is null or empty. Raw response: " + responseString);
                }
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpContext.Response.Cookies.Append("jwt_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
            }
            else
            {
                // Handle error response
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Login failed: {response.StatusCode} - {errorContent}");
            }
            var user = await _appUserRepository.GetUserByEmailAsync(request.Email);
            var claims = new List<Claim>
            {
                new ("avatar", user!.Avatar ?? string.Empty),
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.FirstName+" "+user.LastName),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Role, user.Role.ToString()),
                new ("organizationId", user.OrganizationId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Set expiration time as needed
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var apiClient = _httpClientFactory.CreateClient("PMSApiClient");
                var response = await apiClient.PostAsJsonAsync<RegisterRequest>($"{apiClient.BaseAddress}/auth/register", request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("login");
                }
            }
            return View(request);
        }

        [HttpGet("forbidden")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwt_token");
            return RedirectToAction("login");
        }
    }
}
