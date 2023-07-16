using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.Student;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.Student;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		private readonly IStudentsService _service;

		public StudentsController(IStudentsService studentsService)
		{
			_service = studentsService;
		}

		// GET: api/Students
		[HttpGet]
		public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudents()
		{
			IEnumerable<Student> students = await _service.GetAll();
			IEnumerable<StudentViewModel> studentViewModels = students
				.Select(student => StudentViewModel.Create(student));

			return Ok(studentViewModels);
		}

		// GET: api/Students/NoCourses
		[HttpGet("NoCourses")]
		public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudentsWithNoCourses()
		{
			IEnumerable<Student> students = await _service.GetStudentsWithNoCourses();
			IEnumerable<StudentViewModel> studentViewModels = students
				.Select(student => StudentViewModel.Create(student));

			return Ok(studentViewModels);
		}

		// GET: api/Students/NoCourses
		[HttpGet("Course/{idCourse}")]
		public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudentsByCourse(int idCourse)
		{
			IEnumerable<Student> students = await _service.GetStudentsByCourse(idCourse);
			IEnumerable<StudentViewModel> studentViewModels = students
				.Select(student => StudentViewModel.Create(student));

			return Ok(studentViewModels);
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
				return NotFound(ex.Message);
			}
		}

		// PUT: api/Students/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
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
				return NotFound(ex.Message);
			}
		}

		// POST: api/Students
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
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
				return Problem(ex.Message);
			}
		}

		// DELETE: api/Students/5
		[HttpDelete("{id}")]
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
				return NotFound(ex.Message);
			}
		}
	}
}
