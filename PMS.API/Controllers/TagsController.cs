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
    public class TagsController(ITagRepository tagRepository, IMapper mapper) : ControllerBase
    {
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("organization/{organizationId:guid}")]
        public async Task<ActionResult<List<TagDTO>>> GetTagsAsync([FromRoute] Guid organizationId)
        {
            var tags = await _tagRepository.GetTagsAsync(organizationId);
            if (tags == null || tags.Count == 0)
            {
                return NotFound("Belirtilen organizasyona ait etiket bilgisi bulunamadı");
            }
            var result = _mapper.Map<TagDTO, Tag>(tags);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TagDTO>> GetTagAsync([FromRoute] Guid id)
        {
            var tag = await _tagRepository.GetTagAsync(id);
            if (tag == null)
            {
                return NotFound("Belirtilen etiket bilgisi bulunamadı");
            }
            var result = _mapper.Map<TagDTO, Tag>(tag);
            return Ok(result);
        }

        [HttpGet("task/{taskId:guid}")]
        public async Task<ActionResult<List<TagDTO>>> GetTagsByTaskAsync([FromRoute] Guid taskId)
        {
            var tags = await _tagRepository.GetTagsByTaskAsync(taskId);
            if (tags == null || tags.Count == 0)
            {
                return NotFound("Belirtilen göreve ait etiket bilgisi bulunamadı");
            }
            var result = _mapper.Map<TagDTO, Tag>(tags);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTagAsync([FromBody] TagDTO tagDto)
        {
            if (tagDto == null)
            {
                return BadRequest("Etiket bilgisi boş olamaz");
            }

            if (await _tagRepository.IsUniqueTagNameAsync(tagDto.OrganizationId, tagDto.Name))
            {
                return Conflict("Aynı isimde bir etiket zaten mevcut. Etiket isimleri benzersiz olmalıdır.");
            }

            var tag = _mapper.Map<Tag, TagDTO>(tagDto);
            await _tagRepository.AddTagAsync(tag);
            return Ok("Etiket başarıyla oluşturuldu");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTagAsync([FromRoute] Guid id, [FromBody] TagDTO tagDto)
        {
            if (tagDto == null || id != tagDto.Id)
            {
                return BadRequest("Etiket bilgisi geçersiz");
            }

            var existingTag = await _tagRepository.GetTagAsync(id);
            if (existingTag == null)
            {
                return NotFound("Güncellenmek istenen etiket bulunamadı");
            }

            if (await _tagRepository.IsUniqueTagNameAsync(tagDto.OrganizationId, tagDto.Name, id))
            {
                return Conflict("Aynı isimde bir etiket zaten mevcut. Etiket isimleri benzersiz olmalıdır.");
            }
            existingTag.Name = tagDto.Name;
            existingTag.Color = tagDto.Color;
            existingTag.Icon = tagDto.Icon;
            await _tagRepository.UpdateTagAsync(existingTag);
            return Ok("Etiket başarıyla güncellendi");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTagAsync([FromRoute] Guid id)
        {
            var existingTag = await _tagRepository.GetTagAsync(id);
            if (existingTag == null)
            {
                return NotFound("Silinmek istenen etiket bulunamadı");
            }
            await _tagRepository.DeleteTagAsync(id);
            return Ok("Etiket başarıyla silindi");
        }

    }
}
