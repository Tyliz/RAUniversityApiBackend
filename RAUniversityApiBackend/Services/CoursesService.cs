using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.Course;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Services
{
	public class CoursesService : ICoursesService
	{
		#region Properties

		private readonly DBUniversityContext _context;

		#endregion

		#region Constructors

		public CoursesService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<IEnumerable<Course>> GetAll()
		{
			IEnumerable<Course> courses = new List<Course>();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Where(course => !course.IsDeleted)
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.ToListAsync();
			}

			return courses;
		}

		public async Task<Course> Get(int id)
		{
			if (_context.Courses != null)
			{
				Course? course = await _context.Courses
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.FirstOrDefaultAsync(course => !course.IsDeleted && course.Id == id);

				if (course != null) return course;
			}

			throw new CourseNotExistException();
		}

		public async Task Update(Course course)
		{
			Course originalCourse = await Get(course.Id);

			if (_context.Categories == null)
				throw new CourseException("Entity set 'DBUniversityContext.Categories' is null.");

			originalCourse.IdUserUpdatedBy = 1; // TODO: take from session
			originalCourse.UpdatedAt = DateTime.Now;
			originalCourse.Name = course.Name;
			originalCourse.ShortDescription = course.ShortDescription;
			originalCourse.LongDescription = course.LongDescription;
			originalCourse.Requirements = course.Requirements;
			originalCourse.Level = course.Level;

			originalCourse.Categories.Clear();
			foreach (var category in course.Categories)
			{
				Category existingCategory = await _context.Categories.FindAsync(category.Id) ??
					throw new CourseException($"Category with ID '{category.Id}' not found.");

				originalCourse.Categories.Add(existingCategory);
			}

			_context.Entry(originalCourse).State = EntityState.Modified;

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
					throw new CourseException(ex.Message);
				}
			}
		}

		public async Task<Course> Create(Course course)
		{
			if (_context.Courses == null)
				throw new CourseException("Entity set 'DBUniversityContext.Courses' is null.");

			if (_context.Categories == null)
				throw new CourseException("Entity set 'DBUniversityContext.Categories' is null.");

			// TODO: Obtener el ID del usuario actual desde la sesión
			course.IdUserUpdatedBy = 1;
			course.CreatedAt = DateTime.Now;
			course.UpdatedAt = null;
			course.DeletedAt = null;
			course.IsDeleted = false;

			List<Category> categories = new(course.Categories);
			course.Categories = new List<Category>();
			foreach (var category in categories)
			{
				Category existingCategory = await _context.Categories.FindAsync(category.Id) ??
					throw new CourseException($"Category with ID '{category.Id}' not found.");

				course.Categories.Add(existingCategory);
			}

			_context.Courses.Add(course);

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
					throw new CourseException(ex.Message);
				}
			}

			course.Categories.Clear();
			return course;
		}

		public async Task Delete(int id)
		{
			if (_context.Courses == null)
			{
				throw new CourseNotExistException();
			}
			Course course = await _context.Courses.FindAsync(id) ?? throw new CourseNotExistException();

			if (course.IsDeleted) throw new CourseNotExistException();

			// Physical deletion
			//_context.Courses.Remove(course);

			// Logical deletion
			try
			{
				course.UpdatedAt = DateTime.Now;
				course.DeletedAt = DateTime.Now;
				course.IsDeleted = true;
				_context.Entry(course).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new CourseException("An error occurred while updating the course.");
			} // End Logical deletion

			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Course>> GetWithoutsStudents()
		{
			IEnumerable<Course> courses = new List<Course>();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Where(course =>
						course.Students == null ||
						!course.Students.Any()
					)
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.ToListAsync();
			}

			return courses;
		}

		public async Task<IEnumerable<Course>> GetByLevelWithAtLeastOneStudent(CourseLevel courseLevel)
		{
			IEnumerable<Course> courses = new List<Course>();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Where(course =>
						course.Level.Equals(courseLevel) &&
						course.Students != null &&
						course.Students.Count() > 0
					)
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.ToListAsync();
			}

			return courses;
		}

		public async Task<IEnumerable<Course>> GetWithoutThemes()
		{
			IEnumerable<Course> courses = new List<Course>();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Include(course =>
						course.Chapters
							.Where(chapter => !chapter.IsDeleted)
					)
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.Where(course => 
						!course.IsDeleted &&
						(
							course.Chapters == null ||
							course.Chapters.Count == 0 ||
							course.Chapters.All(chapter => string.IsNullOrEmpty(chapter.Themes))
						)
					)
					.ToListAsync();
			}

			return courses;
		}

		public async Task<IEnumerable<Course>> GetByCategory(int idCategory)
		{
			IEnumerable<Course> courses = new List<Course>();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.Where(course =>
						course.Categories.Any(category => category.Id == idCategory)
					)
					.ToListAsync();
			}

			return courses;
		}

		public async Task<IEnumerable<Course>> GetByStudent(int IdStudent)
		{
			IEnumerable<Course> courses = new List<Course>();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Include(course => course.Students.Where(student => !student.IsDeleted))
					.Where(course =>
						course.Students.Any(student => student.Id == IdStudent)
					)
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.ToListAsync();
			}

			return courses;
		}

		public async Task<IEnumerable<Course>> GetByLevelCategory(CourseLevel courseLevel, int idCategory)
		{
			IEnumerable<Course> courses = new List<Course>();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Where(course =>
						course.Level.Equals(courseLevel) &&
						course.Categories.Any(category => category.Id == idCategory)
					)
					.Include(course => course.Categories.Where(category => !category.IsDeleted))
					.ToListAsync();
			}

			return courses;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
