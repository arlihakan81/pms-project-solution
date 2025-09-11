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
    public class TasksController(ITaskRepository taskRepository, IMapper mapper) : ControllerBase
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly IMapper _mapper = mapper;

        // Task-related endpoints will be implemented here in the future.

        [HttpGet("project/{projectId:guid}")]
        public async Task<ActionResult<List<TaskDTO>>> GetTasksByProjectIdAsync([FromRoute] Guid projectId)
        {
            var tasks = await _taskRepository.GetTasksAsync(projectId);
            if (tasks is null || tasks.Count == 0)
            {
                return NotFound("Belirtilen projeye ait görev bulunamadı");
            }
            var result = _mapper.Map<TaskDTO, TaskItem>(tasks); 
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDTO>> GetTaskByIdAsync([FromRoute] Guid id)
        {
            var task = await _taskRepository.GetTaskAsync(id);
            if (task is null)
            {
                return NotFound("Görev bulunamadı");
            }
            var result = _mapper.Map<TaskDTO, TaskItem>(task);
            return Ok(result);
        }

        [HttpGet("overdue/{organizationId:guid}")]
        public async Task<ActionResult<List<TaskDTO>>> GetOverdueTasksAsync([FromRoute] Guid organizationId)
        {
            var tasks = await _taskRepository.GetOverdueTasksAsync(organizationId);
            if (tasks is null || tasks.Count == 0)
            {
                return NotFound("Gecikmiş görev bulunamadı");
            }
            var result = _mapper.Map<TaskDTO, TaskItem>(tasks);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] TaskDTO taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _taskRepository.IsUniqueTaskTitleAsync(taskDto.ProjectId, taskDto.Title))
                return Conflict("Bu başlıkta zaten bir görev mevcut");


            var task = _mapper.Map<TaskItem, TaskDTO>(taskDto);
            await _taskRepository.AddTaskAsync(task);
            return Ok("Görev başarıyla oluşturuldu");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTaskAsync([FromRoute] Guid id, [FromBody] TaskDTO taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTask = await _taskRepository.GetTaskAsync(id);
            if (existingTask is null)
            {
                return NotFound("Görev bulunamadı");
            }

            if (await _taskRepository.IsUniqueTaskTitleAsync(id, taskDto.ProjectId, taskDto.Title))
                return Conflict("Bu başlıkta zaten bir görev mevcut");

            existingTask.Title = taskDto.Title;
            existingTask.Description = taskDto.Description;
            existingTask.Status = taskDto.Status;
            existingTask.DurationInDays = taskDto.DurationInDays;
            existingTask.Priority = taskDto.Priority;
            existingTask.StartDate = taskDto.StartDate;
            existingTask.Status = taskDto.Status;
            existingTask.UserId = taskDto.UserId;
            existingTask.ProjectId = taskDto.ProjectId;
            existingTask.UpdatedAt = DateTime.Now;

            await _taskRepository.UpdateTaskAsync(existingTask);
            return Ok("Görev başarıyla güncellendi");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] Guid id)
        {
            var existingTask = await _taskRepository.GetTaskAsync(id);
            if (existingTask is null)
            {
                return NotFound("Görev bulunamadı");
            }

            await _taskRepository.DeleteTaskAsync(id);
            return Ok("Görev başarıyla silindi");
        }


    }
}
