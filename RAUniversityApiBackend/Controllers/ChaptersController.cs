using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.Chapter;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChaptersController : ControllerBase
	{
		private readonly IChaptersService _service;

		public ChaptersController(IChaptersService service)
		{
			_service = service;
		}

		// GET: api/Chapters
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Chapter>>> GetChapters()
		{
			IEnumerable<Chapter> chapters = await _service.GetAll();
			return Ok(chapters);
		}

		// GET: api/Chapters/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Chapter>> GetChapter(int id)
		{
			try
			{
				Chapter chapter = await _service.Get(id);
				return chapter;
			}
			catch (ChapterNotExistException)
			{
				return NotFound();
			}
			catch (ChapterException ex)
			{
				return Problem(ex.Message);
			}
		}

		// PUT: api/Chapters/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutChapter(int id, Chapter chapter)
		{
			if (id != chapter.Id) return BadRequest();

			try
			{
				await _service.Update(chapter);
				return NoContent();
			}
			catch (ChapterNotExistException)
			{
				return NotFound();
			}
			catch (ChapterException ex)
			{
				return Problem(ex.Message);
			}
		}

		// POST: api/Chapters
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Chapter>> PostChapter(Chapter chapter)
		{
			try
			{
				Chapter newChapter = await _service.Create(chapter);

				return CreatedAtAction("GetChapter", new { id = newChapter.Id }, newChapter);
			}
			catch (ChapterException ex)
			{
				return Problem(ex.Message);
			}
		}

		// DELETE: api/Chapters/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteChapter(int id)
		{
			try
			{
				await _service.Delete(id);
				return NoContent();
			}
			catch (ChapterNotExistException)
			{
				return NotFound();
			}
			catch (ChapterException ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
