using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.Category;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Services
{
	public class CategoriesService : ICategoriesService
	{
		#region Properties

		private DBUniversityContext _context;

		#endregion

		#region Constructor

		public CategoriesService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<IEnumerable<Category>> GetAll()
		{
			IEnumerable<Category> categories = new List<Category>();

			if (_context.Categories != null)
			{
				categories = await _context.Categories
					.Where(category => !category.IsDeleted)
					.Select(category => new Category() { Id = category.Id, Name = category.Name })
					.ToListAsync();
			}

			return categories;
		}

		public async Task<Category> Get(int id)
		{
			if (_context.Categories != null)
			{
				Category? category = await _context.Categories.FindAsync(id);

				if (category != null) return category;
			}

			throw new CategoryNotExistException();
		}

		public async Task Update(Category category)
		{
			Category originalCategory = await Get(category.Id);

			try
			{
				originalCategory.IdUserUpdatedBy = 1; // TODO: take from session
				originalCategory.UpdatedAt = DateTime.Now;
				originalCategory.Name = category.Name;

				_context.Entry(category).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new CategoryException("An error occurred while updating the category.");
			}
		}

		public async Task<Category> Create(Category category)
		{
			if (_context.Categories == null)
			{
				throw new CategoryException("Entity set 'DBUniversityContext.Categories' is null.");
			}

			category.IdUserUpdatedBy = 1; // TODO: take from session
			category.CreatedAt = DateTime.Now;
			category.UpdatedAt = null;
			category.DeletedAt = null;
			category.IsDeleted = false;

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return category;
		}

		public async Task Delete(int id)
		{
			if (_context.Categories == null)
			{
				throw new CategoryNotExistException();
			}
			Category category = await _context.Categories.FindAsync(id) ?? throw new CategoryNotExistException();

			if (category.IsDeleted) throw new CategoryNotExistException();

			// Physical deletion
			//_context.Categories.Remove(category);

			// Logical deletion
			try
			{
				category.UpdatedAt = DateTime.Now;
				category.DeletedAt = DateTime.Now;
				category.IsDeleted = true;
				_context.Entry(category).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new CategoryException("An error occurred while updating the category.");
			} // End Logical deletion

			await _context.SaveChangesAsync();
		}

		#endregion
	}
}
