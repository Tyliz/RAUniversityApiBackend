using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.User;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Services
{
	public class UsersService : IUsersService
	{
		#region Properties

		private readonly DBUniversityContext _context;

		#endregion

		#region Constructors

		public UsersService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<IEnumerable<User>> GetAll()
		{
			IEnumerable<User> users = new List<User>();

			if (_context.Users != null)
			{
				users = await _context.Users
					.Where(user => !user.IsDeleted)
					.ToListAsync();
			}

			return users;
		}

		public async Task<User> Get(int id)
		{
			if (_context.Users != null)
			{
				User? user = await _context.Users
					.FirstOrDefaultAsync(user => !user.IsDeleted && user.Id == id);

				if (user != null) return user;
			}

			throw new UserNotExistException();
		}

		public async Task Update(User user)
		{
			User originalUser = await Get(user.Id);

			try
			{
				originalUser.UpdatedAt = DateTime.Now;
				originalUser.IdUserUpdatedBy = user.IdUserUpdatedBy; // TODO: Change that id for Sesion id User
				originalUser.UserName = user.UserName;
				originalUser.Name = user.Name;
				originalUser.Surname = user.Surname;
				originalUser.Email = user.Email;

				_context.Entry(originalUser).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new UserException("An error occurred while updating the user.");
			}
		}

		public async Task<User> Create(User user)
		{
			if (_context.Users == null)
			{
				throw new UserException("Entity set 'DBUniversityContext.Users' is null.");
			}

			user.CreatedAt = DateTime.Now;
			user.Password = PasswordService.HashPassword(user.Password);
			user.UpdatedAt = null;
			//user.IdUserCreatedBy = null; TODO: make automatic
			user.IdUserDeletedBy = null;
			user.IdUserUpdatedBy = null;
			user.DeletedAt = null;
			user.IsDeleted = false;

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return user;
		}

		public async Task Delete(int id)
		{
			if (_context.Users == null)
			{
				throw new UserNotExistException();
			}
			User user = await _context.Users.FindAsync(id) ?? throw new UserNotExistException();

			if (user.IsDeleted) throw new UserNotExistException();

			// Physical deletion
			//_context.Users.Remove(user);

			// Logical deletion
			try
			{
				user.UpdatedAt = DateTime.Now;
				user.DeletedAt = DateTime.Now;
				user.IsDeleted = true;
				_context.Entry(user).State = EntityState.Modified;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new UserException("An error occurred while updating the user.");
			} // End Logical deletion

			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<User>> SearchByEmail(string email)
		{
			IEnumerable<User> users = new List<User>();

			if (_context.Users != null)
			{
				users = await _context.Users
					.Where(user => !user.IsDeleted && user.Email == email)
					.ToListAsync();
			}

			return users;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
