using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.Chapter;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.Chapter;

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
		public async Task<ActionResult<IEnumerable<ChapterViewModel>>> GetChapters()
		{
			IEnumerable<Chapter> chapters = await _service.GetAll();
			IEnumerable<ChapterViewModel> chaptersViewModel = chapters
				.Select(chapter => ChapterViewModel.Create(chapter));

			return Ok(chaptersViewModel);
		}

		// GET: api/Chapters/Course/1
		[HttpGet("Course/{idCourse}")]
		public async Task<ActionResult<IEnumerable<ChapterViewModel>>> GetByCourse(int idCourse)
		{
			IEnumerable<Chapter> chapters = await _service.GetByCourse(idCourse);
			IEnumerable<ChapterViewModel> chaptersViewModel = chapters
				.Select(chapter => ChapterViewModel.Create(chapter));

			return Ok(chaptersViewModel);
		}

		// GET: api/Chapters/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ChapterViewModel>> GetChapter(int id)
		{
			try
			{
				Chapter chapter = await _service.Get(id);
				return ChapterViewModel.Create(chapter);
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
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> PutChapter(int id, ChapterUpdateViewModel chapter)
		{
			if (id != chapter.Id) return BadRequest();

			try
			{
				await _service.Update(Chapter.Create(chapter));
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
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<ActionResult<ChapterViewModel>> PostChapter(ChapterCreateViewModel chapter)
		{
			try
			{
				Chapter newChapter = await _service.Create(Chapter.Create(chapter));

				return CreatedAtAction(
					"GetChapter",
					new { id = newChapter.Id },
					ChapterViewModel.Create(newChapter)
				);
			}
			catch (ChapterException ex)
			{
				return Problem(ex.Message);
			}
		}

		// DELETE: api/Chapters/5
		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
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
