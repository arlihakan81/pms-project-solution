using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using Thunder.Application.Mapping;

namespace PMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController(IProjectRepository projectRepo, IMapper mapper) : ControllerBase
    {
        private readonly IProjectRepository _projectRepo = projectRepo;
        private readonly IMapper _mapper = mapper;

        [HttpGet("organization/{organizationId:guid}")]
        public async Task<ActionResult<List<ProjectDTO>>> GetProjectsAsync([FromRoute] Guid organizationId)
        {
            var projects = await _projectRepo.GetProjectsAsync(organizationId);
            if (projects is null || projects.Count == 0)
            {
                return NotFound("Bu organizasyona ait proje bulunamadı.");
            }
            var result = _mapper.Map<ProjectDTO, Project>(projects);
            return Ok(result);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<List<ProjectDTO>>> GetProjectsByUserAsync([FromRoute] Guid userId)
        {
            var projects = await _projectRepo.GetProjectsByUserAsync(userId);
            if (projects is null || projects.Count == 0)
            {
                return NotFound("Kullanıcıya atanmış proje bulunamadı.");
            }
            var result = _mapper.Map<ProjectDTO, Project>(projects);
            return Ok(result);
        }

        [HttpGet("{projectId:guid}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectAsync([FromRoute] Guid projectId)
        {
            var project = await _projectRepo.GetProjectAsync(projectId);
            if (project is null)
            {
                return NotFound("Proje bulunamadı.");
            }
            var result = _mapper.Map<ProjectDTO, Project>(project);
            return Ok(result);
        }

        [HttpGet("upcoming-deadlines/{organizationId:guid}")]
        public async Task<ActionResult<List<ProjectDTO>>> GetUpcomingDeadlineProjectsAsync([FromRoute] Guid organizationId)
        {
            var projects = await _projectRepo.GetUpcomingDeadlineProjectsAsync(organizationId);
            if (projects is null || projects.Count == 0)
            {
                return NotFound("Yaklaşan teslim tarihine sahip proje bulunamadı.");
            }
            var result = _mapper.Map<ProjectDTO, Project>(projects);
            return Ok(result);
        }

        [HttpGet("organization/{organizationId:guid}/date-range/{startDate:datetime}/{endDate:datetime}")]
        public async Task<ActionResult<List<ProjectDTO>>> GetProjectsByDateRangeAsync([FromRoute] Guid organizationId, [FromRoute] DateTime startDate, [FromRoute] DateTime endDate)
        {
            var projects = await _projectRepo.GetProjectsByDateRangeAsync(organizationId, startDate, endDate);
            if (projects is null || projects.Count == 0)
            {
                return NotFound("Belirtilen tarih aralığında proje bulunamadı.");
            }
            var result = _mapper.Map<ProjectDTO, Project>(projects);
            return Ok(result);
        }

        [HttpPost("assign/{projectId:guid}/user/{userId:guid}")]
        public async Task<IActionResult> AssignProjectToUserAsync([FromRoute] Guid projectId, [FromRoute] Guid userId)
        {
            var project = await _projectRepo.GetProjectAsync(projectId);
            if (project is null)
            {
                return NotFound("Proje bulunamadı.");
            }

            await _projectRepo.AssignToUserAsync(projectId, userId);
            return Ok("Proje başarıyla kullanıcıya atandı.");
        }

        [HttpPost("unassign/{projectId:guid}/user/{userId:guid}")]
        public async Task<IActionResult> UnassignProjectFromUserAsync([FromRoute] Guid projectId, [FromRoute] Guid userId)
        {
            var project = await _projectRepo.GetProjectAsync(projectId);
            if (project is null)
            {
                return NotFound("Proje bulunamadı.");
            }
            await _projectRepo.UnassignFromUserAsync(projectId, userId);
            return Ok("Proje başarıyla kullanıcıdan kaldırıldı.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectDTO projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _projectRepo.IsUniqueProjectTitleAsync(projectDto.OrganizationId, projectDto.Title))
            {
                return Conflict("Bu başlığa sahip bir proje zaten mevcut.");
            }

            var project = _mapper.Map<Project, ProjectDTO>(projectDto);
            await _projectRepo.AddProjectAsync(project);
            return Ok("Proje başarıyla oluşturuldu");
        }

        [HttpPut("{projectId:guid}")]
        public async Task<IActionResult> UpdateProjectAsync([FromRoute] Guid projectId, [FromBody] ProjectDTO projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProject = await _projectRepo.GetProjectAsync(projectId);
            if (existingProject is null)
            {
                return NotFound("Güncellenmek istenen proje bulunamadı.");
            }

            if (await _projectRepo.IsUniqueProjectTitleAsync(projectId, projectDto.OrganizationId, projectDto.Title))
            {
                return Conflict("Bu başlığa sahip bir proje zaten mevcut.");
            }

            existingProject.Title = projectDto.Title;
            existingProject.Description = projectDto.Description;
            existingProject.StartDate = projectDto.StartDate;
            existingProject.Deadline = projectDto.Deadline;
            existingProject.Budget = projectDto.Budget;
            existingProject.Status = projectDto.Status;
            existingProject.Privacy = projectDto.Privacy;
            existingProject.CoverImageUrl = projectDto.CoverImageUrl;
            existingProject.UpdatedAt = DateTime.Now;

            await _projectRepo.UpdateProjectAsync(existingProject);
            return Ok("Proje başarıyla güncellendi.");
        }

        [HttpDelete("{projectId:guid}")]
        public async Task<IActionResult> DeleteProjectAsync([FromRoute] Guid projectId)
        {
            var existingProject = await _projectRepo.GetProjectAsync(projectId);
            if (existingProject is null)
            {
                return NotFound("Silinmek istenen proje bulunamadı.");
            }

            await _projectRepo.DeleteProjectAsync(projectId);
            return Ok("Proje başarıyla silindi.");
        }


    }
}
