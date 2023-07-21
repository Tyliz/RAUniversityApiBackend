using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.Student;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Services
{
	public class StudentsService : IStudentsService
	{
		#region Properties

		private readonly DBUniversityContext _context;

		#endregion

		#region Constructors

		public StudentsService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<IEnumerable<Student>> GetAll()
		{
			IEnumerable<Student> students = new List<Student>();

			if (_context.Students != null)
			{
				students = await _context.Students
					.Where(student => !student.IsDeleted)
					.ToListAsync();
			}

			return students;
		}

		public IEnumerable<Student> GetAllSync()
		{
			IEnumerable<Student> students = new List<Student>();

			if (_context.Students != null)
			{
				students = _context.Students
					.Where(student => !student.IsDeleted)
					.ToList();
			}

			return students;
		}

		public async Task<IEnumerable<Student>> GetStudentsByCourse(int IdCourse)
		{
			IEnumerable<Student> students = new List<Student>();

			if (_context.Students != null)
			{
				students = await _context.Students
					.Include(student => student.Courses.Where(course => !course.IsDeleted))
					.Where(student => 
						!student.IsDeleted &&
						student.Courses.Any(course => course.Id == IdCourse)
					)
					.ToListAsync();
			}

			return students;
		}

		public async Task<Student> Get(int id)
		{
			if (_context.Students != null)
			{
				Student? student = await _context.Students
					.Include(student => student.Courses)
					.FirstOrDefaultAsync(student => student.Id == id);

				if (student != null ) return student;
			}

			throw new StudentNotExistException();
		}

		public async Task Update(Student student)
		{
			Student originalStudent = await Get(student.Id);

			if (_context.Courses == null)
				throw new StudentException("Entity set 'DBUniversityContext.Courses' is null.");

			originalStudent.IdUserUpdatedBy = 1; // TODO: Take from session
			originalStudent.UpdatedAt = DateTime.Now;
			originalStudent.Name = student.Name;
			originalStudent.Surname = student.Surname;
			originalStudent.DateOfBird = student.DateOfBird;


			List<Course> courses = student.Courses != null ? new(student.Courses) : new();

			if (originalStudent.Courses != null)
				originalStudent.Courses.Clear();
			else
				originalStudent.Courses = new List<Course>();

			foreach (var course in courses)
			{
				Course existingCourse = await _context.Courses.FindAsync(course.Id) ??
					throw new StudentException($"Course with ID '{course.Id}' not found.");

				originalStudent.Courses.Add(existingCourse);
			}

			_context.Entry(originalStudent).State = EntityState.Modified;

			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					await _context.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw new StudentException(ex.Message);
				}
			}
		}

		public async Task<Student> Create(Student student)
		{
			if (_context.Students == null)
				throw new StudentException("Entity set 'DBUniversityContext.Students' is null.");

			if (_context.Courses == null)
				throw new StudentException("Entity set 'DBUniversityContext.Courses' is null.");

			student.IdUserCreatedBy = 1; // TODO: Take from session
			student.CreatedAt = DateTime.Now;
			student.UpdatedAt = null;
			student.DeletedAt = null;
			student.IsDeleted = false;

			List<Course> courses = student.Courses != null ? new(student.Courses) : new();

			student.Courses = new List<Course>();

			foreach (var course in courses)
			{
				Course existingCourse = await _context.Courses.FindAsync(course.Id) ??
					throw new StudentException($"Course with ID '{course.Id}' not found.");

				student.Courses.Add(existingCourse);
			}

			_context.Students.Add(student);

			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					await _context.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw new StudentException(ex.Message);
				}
			}

			student.Courses.Clear();
			return student;
		}

		public async Task Delete(int id)
		{
			if (_context.Students == null)
			{
				throw new StudentNotExistException();
			}
			Student student = await _context.Students.FindAsync(id) ?? throw new StudentNotExistException();

			if (student.IsDeleted) throw new StudentNotExistException();

			// Physical deletion
			//_context.Students.Remove(student);

			// Logical deletion
			try
			{
				student.UpdatedAt = DateTime.Now;
				student.DeletedAt = DateTime.Now;
				student.IsDeleted = true;
				_context.Entry(student).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new StudentException("An error occurred while updating the student.");
			} // End Logical deletion

			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Student>> GetOldersThan18()
		{
			IEnumerable<Student> students = new List<Student>();

			if (_context.Students != null)
			{
				students = await _context.Students
					.Where(student => GetAge(student.DateOfBird) >= 18)
					.ToListAsync();
			}

			return students;
		}

		public async Task<IEnumerable<Student>> GetStudenWithAtLeastOneCourse()
		{
			List<Student> students = new();

			if (_context.Students != null)
			{
				students = await _context.Students.ToListAsync();

				students = students
					.Where(student => student.Courses != null && student.Courses.Any())
					.ToList();
			}

			return students;
		}

		public async Task<IEnumerable<Student>> GetStudentsWithNoCourses()
		{
			IEnumerable<Student> students = new List<Student>();

			if (_context.Students != null)
			{
				students = await _context.Students
					.Include(student => student.Courses)
					.Where(student => !student.IsDeleted && student.Courses.Count == 0)
					.ToListAsync();
			}

			return students;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Get Age from Date of Bird
		/// Original code from: https://stackoverflow.com/a/1404
		/// </summary>
		/// <returns></returns>
		private static int GetAge(DateTime dateOfBird)
		{
			DateTime today = DateTime.Today;

			// Calculate the age.
			int age = today.Year - dateOfBird.Year;

			// Go back to the year in which the person was born in case of a leap year
			if (dateOfBird.Date > today.AddYears(-age)) age--;

			return age;
		}

		#endregion
	}
}
