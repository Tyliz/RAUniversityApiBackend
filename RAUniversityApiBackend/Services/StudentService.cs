using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services
{
	public class StudentService
	{
		#region Properties

		private readonly DBUniversityContext _context;

		#endregion

		#region Constructors

		public StudentService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<List<Student>> GetOldersThan18()
		{
			List<Student> students = new();

			if (_context.Students != null)
			{
				students = await _context.Students.ToListAsync();

				students = students
					.Select(student => student)
					.Where(student => GetAge(student.DateOfBird) >= 18)
					.ToList();
			}

			return students;
		}

		public async Task<List<Student>> GetStudenWithAtLeastOneCourse()
		{
			List<Student> students = new();

			if (_context.Students != null)
			{
				students = await _context.Students.ToListAsync();

				students = students
					.Select(student => student)
					.Where(student => student.Courses != null && student.Courses.Any())
					.ToList();
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
