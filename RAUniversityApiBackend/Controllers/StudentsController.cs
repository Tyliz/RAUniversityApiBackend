using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.Student;
using RAUniversityApiBackend.Global;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.Student;
using System.Diagnostics;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		private readonly IStudentsService _service;
		private readonly ILogger<StudentsController> _logger;
		private string Name
		{
			get
			{
				return nameof(StudentsController);
			}
		}

		public StudentsController(IStudentsService studentsService, ILogger<StudentsController> logger)
		{
			_service = studentsService;
			_logger = logger;
		}

		// GET: api/Students
		[HttpGet]
		public async Task<IActionResult> GetStudents()
		{
			try
			{
				Stopwatch sw = Stopwatch.StartNew();

				IEnumerable<Student> studentsSync = _service.GetAllSync();
				IEnumerable<StudentViewModel> studentViewModelsSync = studentsSync
					.Select(student => StudentViewModel.Create(student));


				sw.Stop();
				var syncResult = new
				{
					students = studentViewModelsSync,
					time = sw.Elapsed,
				};

				sw.Restart();
				IEnumerable<Student> students = await _service.GetAll();
				IEnumerable<StudentViewModel> studentViewModels = students
					.Select(student => StudentViewModel.Create(student));
				


				sw.Stop();
				var asyncResult = new {
					students = studentViewModels,
					time = sw.Elapsed,
				};

				return Ok(new
				{
					syncResult, // 1st time: "time": "00:00:02.6016816"; 4th "time": "00:00:00.0782683"
					asyncResult, // 1st"time": "00:00:00.1430757"; 4th "time": "00:00:00.0162782"
				});

			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetStudents)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.StudentsControllerGetStudents), ex, message);

				return Ok(Array.Empty<StudentViewModel>());
			}
		}

		// GET: api/Students/NoCourses
		[HttpGet("NoCourses")]
		public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudentsWithNoCourses()
		{
			try
			{
				IEnumerable<Student> students = await _service.GetStudentsWithNoCourses();
				IEnumerable<StudentViewModel> studentViewModels = students
					.Select(student => StudentViewModel.Create(student));

				return Ok(studentViewModels);
			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetStudentsWithNoCourses)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.StudentsControllerGetStudentsWithNoCourses), ex, message);

				return Ok(Array.Empty<StudentViewModel>());
			}
		}

		// GET: api/Students/NoCourses
		[HttpGet("Course/{idCourse}")]
		public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudentsByCourse(int idCourse)
		{
			try
			{
				IEnumerable<Student> students = await _service.GetStudentsByCourse(idCourse);
				IEnumerable<StudentViewModel> studentViewModels = students
					.Select(student => StudentViewModel.Create(student));

				return Ok(studentViewModels);
			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetStudentsByCourse)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.StudentsControllerGetStudentsByCourse), ex, message);

				return Ok(Array.Empty<StudentViewModel>());
			}
		}

		// GET: api/Students/5
		[HttpGet("{id}")]
		public async Task<ActionResult<StudentViewModel>> GetStudent(int id)
		{
			try
			{
				Student student = await _service.Get(id);
				return StudentViewModel.Create(student);
			}
			catch (StudentNotExistException)
			{
				return NotFound();
			}
			catch (StudentException ex)
			{
				string message = $"{Name} - {nameof(GetStudent)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.StudentsControllerGetStudent), ex, message);

				return NotFound(ex.Message);
			}
		}

		// PUT: api/Students/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> PutStudent(int id, StudentUpdateViewModel student)
		{
			if (id != student.Id) return BadRequest();

			try
			{
				await _service.Update(Student.Create(student));
				return NoContent();
			}
			catch (StudentNotExistException)
			{
				return NotFound();
			}
			catch (StudentException ex)
			{
				string message = $"{Name} - {nameof(PutStudent)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.StudentsControllerPutStudent), ex, message);

				return NotFound(ex.Message);
			}
		}

		// POST: api/Students
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<ActionResult<StudentViewModel>> PostStudent(StudentCreateViewModel student)
		{
			try
			{
				Student newStudent = await _service.Create(Student.Create(student));

				return CreatedAtAction(
					"GetStudent",
					new { id = newStudent.Id },
					StudentViewModel.Create(newStudent)
				);
			}
			catch (StudentException ex)
			{
				string message = $"{Name} - {nameof(PostStudent)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.StudentsControllerPostStudent), ex, message);

				return Problem(ex.Message);
			}
		}

		// DELETE: api/Students/5
		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> DeleteStudent(int id)
		{
			try
			{
				await _service.Delete(id);
				return NoContent();
			}
			catch (StudentNotExistException ex)
			{
				return NotFound(ex.Message);
			}
			catch (StudentException ex)
			{
				string message = $"{Name} - {nameof(DeleteStudent)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.StudentsControllerDeleteStudent), ex, message);

				return NotFound(ex.Message);
			}
		}
	}
}
