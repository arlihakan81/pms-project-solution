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
    public class ActivityLogsController(IActivityLogRepository activityRepository, IMapper mapper) : ControllerBase
    {
        private readonly IActivityLogRepository _activityRepository = activityRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("task/{taskId:guid}")]
        public async Task<ActionResult<List<ActivityLogDTO>>> GetActivityLogsByTaskAsync([FromRoute] Guid taskId)
        {
            var activities = await _activityRepository.GetActivityLogsByTaskAsync(taskId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Belirtilen göreve ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityLogDTO, ActivityLog>(activities);

            return Ok(result);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<List<ActivityLogDTO>>> GetActivityLogsByUserAsync([FromRoute] Guid userId)
        {
            var activities = await _activityRepository.GetActivityLogsByUserAsync(userId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Kullanıcıya ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityLogDTO, ActivityLog>(activities);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ActivityLogDTO>> GetActivityLogAsync([FromRoute] Guid id)
        {
            var activity = await _activityRepository.GetActivityLogAsync(id);
            if (activity is null)
            {
                return NotFound("Aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityLogDTO, ActivityLog>(activity);

            return Ok(result);
        }

        [HttpGet("organization/{organizationId:guid}")]
        public async Task<ActionResult<List<ActivityLogDTO>>> GetAllActivityLogsAsync([FromRoute] Guid organizationId)
        {
            var activities = await _activityRepository.GetAllActivityLogsAsync(organizationId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Organizasyona ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityLogDTO, ActivityLog>(activities);

            return Ok(result);
        }

        [HttpGet("project/{projectId:guid}/last3")]
        public async Task<ActionResult<List<ActivityLogDTO>>> GetLast3ActivityLogsByProjectIdAsync([FromRoute] Guid projectId)
        {
            var activities = await _activityRepository.GetLast3ActivityLogsAsync(projectId);
            if (activities is null || activities.Count == 0)
            {
                return NotFound("Projeye ait aktivite bulunamadı");
            }
            var result = _mapper.Map<ActivityLogDTO, ActivityLog>(activities);

            return Ok(result);
        }



    }
}
