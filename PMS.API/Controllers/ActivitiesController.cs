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
    public class ActivitiesController(IActivityRepository activityRepository, IMapper mapper) : ControllerBase
    {
        private readonly IActivityRepository _activityRepository = activityRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("task/{taskId:guid}")]
        public async Task<ActionResult<List<ActivityDTO>>> GetActivitiesByTaskIdAsync([FromRoute] Guid taskId)
        {
            var activities = await _activityRepository.GetActivitiesByTaskAsync(taskId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Belirtilen göreve ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityDTO, Activity>(activities);

            return Ok(result);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<List<ActivityDTO>>> GetActivitiesByUserIdAsync([FromRoute] Guid userId)
        {
            var activities = await _activityRepository.GetActivitiesByUserAsync(userId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Kullanıcıya ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityDTO, Activity>(activities);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ActivityDTO>> GetActivityByIdAsync([FromRoute] Guid id)
        {
            var activity = await _activityRepository.GetActivityAsync(id);
            if (activity is null)
            {
                return NotFound("Aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityDTO, Activity>(activity);

            return Ok(result);
        }

        [HttpGet("organization/{organizationId:guid}")]
        public async Task<ActionResult<List<ActivityDTO>>> GetAllActivitiesByOrganizationIdAsync([FromRoute] Guid organizationId)
        {
            var activities = await _activityRepository.GetAllActivitiesAsync(organizationId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Organizasyona ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityDTO, Activity>(activities);

            return Ok(result);
        }

        [HttpGet("project/{projectId:guid}/last3")]
        public async Task<ActionResult<List<ActivityDTO>>> GetLast3ActivitiesByProjectIdAsync([FromRoute] Guid projectId)
        {
            var activities = await _activityRepository.GetLast3ActivitiesAsync(projectId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Projeye ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityDTO, Activity>(activities);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivityAsync([FromBody] ActivityDTO activityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = _mapper.Map<Activity, ActivityDTO>(activityDto);
            await _activityRepository.AddActivityAsync(activity);
            return Ok("Aktiviteniz başarıyla oluşturuldu");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateActivityAsync([FromRoute] Guid id, [FromBody] ActivityDTO activityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingActivity = await _activityRepository.GetActivityAsync(id);
            if (existingActivity is null)
            {
                return NotFound("Aktivite bulunamadı");
            }

            existingActivity.UserId = activityDto.UserId;
            existingActivity.TaskId = activityDto.TaskId;
            existingActivity.CommentId = activityDto.CommentId;
            existingActivity.UpdatedAt = DateTime.Now;
            await _activityRepository.UpdateActivityAsync(existingActivity);
            return Ok("Aktiviteniz başarıyla güncellendi");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteActivityAsync([FromRoute] Guid id)
        {
            var existingActivity = await _activityRepository.GetActivityAsync(id);
            if (existingActivity is null)
            {
                return NotFound("Aktivite bulunamadı");
            }

            await _activityRepository.DeleteActivityAsync(id);
            return Ok("Aktiviteniz başarıyla silindi");
        }


    }
}
