using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services
{
	public class CourseService
	{
		#region Properties

		private readonly DBUniversityContext _context;

		#endregion

		#region Constructors

		public CourseService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<List<Course>> GetWithoutsStudents()
		{
			List<Course> courses = new();

			if (_context.Courses != null)
			{
				courses = await _context.Courses.ToListAsync();

				courses = courses
					.Select(course => course)
					.Where(course =>
						course.Students == null ||
						!course.Students.Any()
					)
					.ToList();
			}

			return courses;
		}

		public async Task<List<Course>> GetByLevelWithAtLeastOneStudent(CourseLevel courseLevel)
		{
			List<Course> courses = new();

			if (_context.Courses != null)
			{
				courses = await _context.Courses.ToListAsync();

				courses = courses
					.Select(course => course)
					.Where(course => 
						course.Level.Equals(courseLevel) &&
						course.Students != null &&
						course.Students.Any()
					)
					.ToList();
			}

			return courses;
		}

		public async Task<List<Course>> GetByLevelCategory(CourseLevel courseLevel, int idCategory)
		{
			List<Course> courses = new();

			if (_context.Courses != null)
			{
				courses = await _context.Courses.ToListAsync();

				courses = courses
					.Select(course => course)
					.Where(course => 
						course.Level.Equals(courseLevel) &&
						course.Categories != null &&
						course.Categories.Any(category => category.Equals(idCategory))
					)
					.ToList();
			}

			return courses;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
