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
    public class CommentsController(ICommentRepository commentRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<List<CommentDTO>>> GetCommentsByUserIdAsync([FromRoute] Guid userId)
        {
            var comments = await _commentRepository.GetCommentByUserAsync(userId);
            if (comments is null || comments.Count == 0)
            {
                return NotFound("Kullanıcıya ait yorum bulunamadı");
            }
            var result = _mapper.Map<CommentDTO, Comment>(comments);
            return Ok(result);
        }

        [HttpGet("task/{taskId:guid}")]
        public async Task<ActionResult<List<CommentDTO>>> GetCommentsByTaskIdAsync([FromRoute] Guid taskId)
        {
            var comments = await _commentRepository.GetCommentsByTaskAsync(taskId);
            if (comments is null || comments.Count == 0)
            {
                return NotFound("Belirtilen göreve ait yorum bulunamadı");
            }
            var result = _mapper.Map<CommentDTO, Comment>(comments);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommentAsync([FromBody] CommentDTO createCommentDto)
        {
            var comment = _mapper.Map<Comment, CommentDTO>(createCommentDto);
            await _commentRepository.AddCommentAsync(comment);
            return Ok("Yorumunuz başarıyla oluşturuldu");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] Guid id, [FromBody] CommentDTO updateCommentDto)
        {
            var existingComment = await _commentRepository.GetCommentAsync(id);
            if (existingComment is null)
            {
                return NotFound("Yorum bulunamadı");
            }
            existingComment.Content = updateCommentDto.Content;
            await _commentRepository.UpdateCommentAsync(existingComment);
            return Ok("Yorumunuz başarıyla güncellendi");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCommentAsync([FromRoute] Guid id)
        {
            var existingComment = await _commentRepository.GetCommentAsync(id);
            if (existingComment is null)
            {
                return NotFound("Yorum bulunamadı");
            }
            await _commentRepository.DeleteCommentAsync(id);
            return Ok("Yorumunuz başarıyla silindi");
        }



    }
}
