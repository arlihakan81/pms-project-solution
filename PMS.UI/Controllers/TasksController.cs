using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using PMS.Application.DTOs;
using System.Security.Claims;

namespace PMS.UI.Controllers
{
    [Authorize]
    public class TasksController(IHttpClientFactory httClientFactory) : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory = httClientFactory;

        [HttpGet("tasks")]
        public async Task<IActionResult> Index()
        {
            var me = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var apiClient = _httpClientFactory.CreateClient("PMSApiClient");            
            var tasks = await apiClient.GetFromJsonAsync<List<TaskDTO>>($"{apiClient.BaseAddress}/tasks/user/{me}");
            return View(tasks);
        }
    }
}
