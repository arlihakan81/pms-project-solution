using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace PMS.UI.Controllers
{
    [Authorize]
    public class ProjectsController(IHttpClientFactory httpClient) : Controller
    {
        private readonly IHttpClientFactory _httpClient = httpClient;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var apiClient = _httpClient.CreateClient("PMSApiClient");
            var me = User.FindFirst("organizationId")?.Value;
            var response = await apiClient.GetFromJsonAsync<List<ProjectDTO>>($"{apiClient.BaseAddress}/projects/organization/{me}");
            return View(response);
        }

        [HttpGet("project-create")]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ProjectDTO project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }

            var apiClient = _httpClient.CreateClient("PMSApiClient");
            var response = await apiClient.PostAsJsonAsync<ProjectDTO>($"{apiClient.BaseAddress}/projects", project);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Proje oluşturulurken bir hata oluştu.");
                return View(project);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("project-edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var apiClient = _httpClient.CreateClient("PMSApiClient");
            var project = await apiClient.GetFromJsonAsync<ProjectDTO>($"{apiClient.BaseAddress}/projects/{id}");
            return View(project);
        }

        [HttpPost("project-edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, ProjectDTO project)
        {
            var apiClient = _httpClient.CreateClient("PMSApiClient");
            var response = await apiClient.PutAsJsonAsync($"{apiClient.BaseAddress}/projects/{id}", project);

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(project);
        }

        [HttpGet("project-detail/{id:guid}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var apiClient = _httpClient.CreateClient("PMSApiClient");
            var project = await apiClient.GetFromJsonAsync<ProjectDTO>($"{apiClient.BaseAddress}/projects/{id}");
            return View(project);
        }

        [HttpGet("projects/{id:guid}/tasks")]
        public async Task<IActionResult> Tasks(Guid id)
        {
            var apiClient = _httpClient.CreateClient("PMSApiClient");
            var tasks = await apiClient.GetFromJsonAsync<List<TaskDTO>>($"{apiClient.BaseAddress}/tasks/project/{id}");            
            return View(tasks);
        }

    }


}
