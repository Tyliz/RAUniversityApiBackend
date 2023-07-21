using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.Course;
using RAUniversityApiBackend.Goblal;
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
		private readonly ILogger<CoursesController> _logger;
		private string Name
		{
			get
			{
				return nameof(CoursesController);
			}
		}

		public CoursesController(ICoursesService coursesService, ILogger<CoursesController> logger)
		{
			_service = coursesService;
			_logger = logger;
		}

		// GET: api/Courses
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetCourses()
		{
			try
			{
				IEnumerable<Course> courses = await _service.GetAll();
				IEnumerable<CourseViewModel> coursesViewModel = courses
					.Select(course => CourseViewModel.Create(course));

				return Ok(coursesViewModel);
			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetCourses)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerGetCourses), ex, message);

				return Ok(Array.Empty<CourseViewModel>());
			}
		}

		// GET: api/Courses/Category/1
		[HttpGet("Category/{idCategoria}")]
		public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetByCategory(int idCategoria)
		{
			try
			{
				IEnumerable<Course> courses = await _service.GetByCategory(idCategoria);
				IEnumerable<CourseViewModel> coursesViewModel = courses
					.Select(course => CourseViewModel.Create(course));

				return Ok(coursesViewModel);
			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetByCategory)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerGetByCategory), ex, message);

				return Ok(Array.Empty<CourseViewModel>());
			}
		}

		// GET: api/Courses/Student/1
		[HttpGet("Student/{idStudent}")]
		public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetByStudent(int idStudent)
		{
			try
			{
				IEnumerable<Course> courses = await _service.GetByStudent(idStudent);
				IEnumerable<CourseViewModel> coursesViewModel = courses
					.Select(course => CourseViewModel.Create(course));

				return Ok(coursesViewModel);
			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetByStudent)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerGetByStudent), ex, message);

				return Ok(Array.Empty<CourseViewModel>());
			}
		}

		// GET: api/Courses/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CourseViewModel>> GetCourse(int id)
		{
			try
			{
				Course course = await _service.Get(id);
				return CourseViewModel.Create(course);
			}
			catch (CourseNotExistException)
			{
				return NotFound();
			}
			catch (CourseException ex)
			{
				string message = $"{Name} - {nameof(GetCourse)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerGetCourses), ex, message);

				return Problem(ex.Message);
			}
		}

		// GET: api/Courses/NoThemes
		[HttpGet("NoThemes")]
		public async Task<ActionResult<CourseViewModel>> GetWithoutThemes()
		{
			try
			{
				IEnumerable<Course> courses = await _service.GetWithoutThemes();
				IEnumerable<CourseViewModel> coursesViewModel = courses
					.Select(course => CourseViewModel.Create(course));

				return Ok(coursesViewModel);
			}
			catch (CourseNotExistException)
			{
				return NotFound();
			}
			catch (CourseException ex)
			{
				string message = $"{Name} - {nameof(GetWithoutThemes)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerGetWithoutThemes), ex, message);

				return Problem(ex.Message);
			}
		}

		// PUT: api/Courses/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> PutCourse(int id, CourseUpdateViewModel course)
		{
			if (id != course.Id) return BadRequest();

			try
			{
				await _service.Update(Course.Create(course));
				return NoContent();
			}
			catch (CourseNotExistException)
			{
				return NotFound();
			}
			catch (CourseException ex)
			{
				string message = $"{Name} - {nameof(PutCourse)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerPutCourse), ex, message);

				return Problem(ex.Message);
			}
		}

		// POST: api/Courses
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<ActionResult<CourseViewModel>> PostCourse(CourseCreateViewModel course)
		{
			try
			{
				Course newCourse = await _service.Create(Course.Create(course));
				return CreatedAtAction(
					"GetCourse",
					new { id = newCourse.Id },
					CourseViewModel.Create(newCourse)
				);
			}
			catch (CourseException ex)
			{
				string message = $"{Name} - {nameof(PostCourse)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerPostCourse), ex, message);

				return Problem(ex.Message);
			}
		}

		// DELETE: api/Courses/5
		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
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
				string message = $"{Name} - {nameof(DeleteCourse)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CoursesControllerDeleteCourse), ex, message);

				return Problem(ex.Message);
			}
		}
	}
}
