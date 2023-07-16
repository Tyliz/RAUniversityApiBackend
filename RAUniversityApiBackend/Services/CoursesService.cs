using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.Course;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using System.Collections.Generic;

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
					.ToListAsync();
			}

			return courses;
		}

		public async Task<Course> Get(int id)
		{
			if (_context.Courses != null)
			{
				Course? course = await _context.Courses.FindAsync(id);

				if (course != null) return course;
			}

			throw new CourseNotExistException();
		}

		public async Task Update(Course course)
		{
			Course originalCourse = await Get(course.Id);

			try
			{
				originalCourse.IdUserUpdatedBy = 1; // TODO: take from session
				originalCourse.UpdatedAt = DateTime.Now;
				originalCourse.Name = course.Name;
				originalCourse.ShortDescription = course.ShortDescription;
				originalCourse.LongDescription = course.LongDescription;
				originalCourse.Requirements = course.Requirements;
				originalCourse.Level = course.Level;
				//originalCourse.Categories = course.Categories; / /TODO: make N:N update

				_context.Entry(originalCourse).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new CourseException("An error occurred while updating the course.");
			}
		}

		public async Task<Course> Create(Course course)
		{
			if (_context.Courses == null)
			{
				throw new CourseException("Entity set 'DBUniversityContext.Courses' is null.");
			}

			if (_context.Categories == null)
			{
				throw new CourseException("Entity set 'DBUniversityContext.Categories' is null.");
			}

			// TODO: Obtener el ID del usuario actual desde la sesión
			course.IdUserUpdatedBy = 1;
			course.CreatedAt = DateTime.Now;
			course.UpdatedAt = null;
			course.DeletedAt = null;
			course.IsDeleted = false;

			List<Category> categories = new(course.Categories);
			course.Categories.Clear();
			foreach (var category in categories)
			{
				Category? existingCategory = await _context.Categories.FindAsync(category.Id);
				if (existingCategory == null)
				{
					throw new CourseException($"Category with ID '{category.Id}' not found.");
				}

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
				courses = await _context.Courses
					.Select(course => course)
					.Where(course =>
						course.Level.Equals(courseLevel) &&
						course.Students != null &&
						course.Students.Count() > 0
					)
					.ToListAsync();
			}

			return courses;
		}

		public async Task<List<Course>> GetByCategory(int idCategory)
		{
			List<Course> courses = new();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Where(course =>
						course.Categories.Any(category => category.Id == idCategory)
					)
					.Select(course => course)
					.ToListAsync();
			}

			return courses;
		}

		public async Task<List<Course>> GetByLevelCategory(CourseLevel courseLevel, int idCategory)
		{
			List<Course> courses = new();

			if (_context.Courses != null)
			{
				courses = await _context.Courses
					.Select(course => course)
					.Where(course =>
						course.Level.Equals(courseLevel) &&
						course.Categories.Any(category => category.Id == idCategory)
					)
					.ToListAsync();
			}

			return courses;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
