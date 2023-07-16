using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.Exceptions.Course;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.Course;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly ICoursesService _service;

		public CoursesController(ICoursesService coursesService)
		{
			_service = coursesService;
		}

		// GET: api/Courses
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
		{
			IEnumerable<Course> courses = await _service.GetAll();
			return Ok(courses);
		}

		// GET: api/Courses/Category/1
		[HttpGet("Category/{idCategoria}")]
		public async Task<ActionResult<IEnumerable<Course>>> GetByCategory(int idCategoria)
		{
			IEnumerable<Course> courses = await _service.GetByCategory(idCategoria);
			return Ok(courses);
		}

		// GET: api/Courses/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Course>> GetCourse(int id)
		{
			try
			{
				Course course = await _service.Get(id);
				return course;
			}
			catch (CourseNotExistException)
			{
				return NotFound();
			}
			catch (CourseException ex)
			{
				return Problem(ex.Message);
			}
		}

		// PUT: api/Courses/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCourse(int id, Course course)
		{
			if (id != course.Id) return BadRequest();

			try
			{
				await _service.Update(course);
				return NoContent();
			}
			catch (CourseNotExistException)
			{
				return NotFound();
			}
			catch (CourseException ex)
			{
				return Problem(ex.Message);
			}
		}

		// POST: api/Courses
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Course>> PostCourse(CourseCreateViewModel courseCreateVM)
		{
			try
			{
				Course newCourse = new()
				{
					Name = courseCreateVM.Name,
					ShortDescription = courseCreateVM.ShortDescription,
					LongDescription = courseCreateVM.LongDescription,
					Requirements = courseCreateVM.Requirements,
					Level = courseCreateVM.Level,
				};
				IEnumerable<int> categories = courseCreateVM.Categories;
				foreach (int IdCategory in categories)
				{
					newCourse.Categories.Add(new() {
						Id = IdCategory,
					});
				}

				newCourse = await _service.Create(newCourse);

				return CreatedAtAction("GetCourse", new { id = newCourse.Id }, newCourse);
			}
			catch (CourseException ex)
			{
				return Problem(ex.Message);
			}
		}

		// DELETE: api/Courses/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCourse(int id)
		{
			try
			{
				await _service.Delete(id);
				return NoContent();
			}
			catch (CourseNotExistException)
			{
				return NotFound();
			}
			catch (CourseException ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
