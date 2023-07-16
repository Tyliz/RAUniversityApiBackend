using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.Chapter;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Services
{
	public class ChaptersService : IChaptersService
	{
		#region Properties

		private DBUniversityContext _context;

		#endregion

		#region Constructor

		public ChaptersService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<IEnumerable<Chapter>> GetAll()
		{
			IEnumerable<Chapter> chapters = new List<Chapter>();

			if (_context.Chapters != null)
			{
				chapters = await _context.Chapters
					.Where(chapter => !chapter.IsDeleted)
					.ToListAsync();
			}

			return chapters;
		}

		public async Task<Chapter> Get(int id)
		{
			if (_context.Chapters != null)
			{
				Chapter? chapter = await _context.Chapters.FindAsync(id);

				if (chapter != null) return chapter;
			}

			throw new ChapterNotExistException();
		}

		public async Task Update(Chapter chapter)
		{
			Chapter originalChapter = await Get(chapter.Id);

			try
			{
				originalChapter.IdUserUpdatedBy = 1; // TODO: take from session
				originalChapter.UpdatedAt = DateTime.Now;
				originalChapter.Themes = chapter.Themes;
				originalChapter.IdCourse = chapter.IdCourse;

				_context.Entry(originalChapter).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new ChapterException("An error occurred while updating the chapter.");
			}
		}

		public async Task<Chapter> Create(Chapter chapter)
		{
			if (_context.Chapters == null)
			{
				throw new ChapterException("Entity set 'DBUniversityContext.Chapters' is null.");
			}

			chapter.IdUserUpdatedBy = 1; // TODO: take from session
			chapter.CreatedAt = DateTime.Now;
			chapter.UpdatedAt = null;
			chapter.DeletedAt = null;
			chapter.IsDeleted = false;

			_context.Chapters.Add(chapter);
			await _context.SaveChangesAsync();

			return chapter;
		}

		public async Task Delete(int id)
		{
			if (_context.Chapters == null)
			{
				throw new ChapterNotExistException();
			}
			Chapter chapter = await _context.Chapters.FindAsync(id) ?? throw new ChapterNotExistException();

			if (chapter.IsDeleted) throw new ChapterNotExistException();

			// Physical deletion
			//_context.Chapters.Remove(chapter);

			// Logical deletion
			try
			{
				chapter.UpdatedAt = DateTime.Now;
				chapter.DeletedAt = DateTime.Now;
				chapter.IsDeleted = true;
				_context.Entry(chapter).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new ChapterException("An error occurred while updating the chapter.");
			} // End Logical deletion

			await _context.SaveChangesAsync();
		}

		#endregion
	}
}
