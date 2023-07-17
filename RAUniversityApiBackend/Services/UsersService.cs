using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.Student;
using RAUniversityApiBackend.Exceptions.User;
using RAUniversityApiBackend.Helpers;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using System.Data;

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
					.Include(user => user.Roles)
					.ToListAsync();
			}

			return users;
		}

		public async Task<User> Get(int id)
		{
			if (_context.Users != null)
			{
				User? user = await _context.Users
					.Include(user => user.Roles)
					.FirstOrDefaultAsync(user => !user.IsDeleted && user.Id == id);

				if (user != null) return user;
			}

			throw new UserNotExistException();
		}

		public async Task Update(User user)
		{
			if (_context.Roles == null)
				throw new StudentException("Entity set 'DBUniversityContext.Roles' is null.");

			User originalUser = await Get(user.Id);

			originalUser.UpdatedAt = DateTime.Now;
			originalUser.IdUserUpdatedBy = user.IdUserUpdatedBy; // TODO: Change that id for Sesion id User
			originalUser.UserName = user.UserName;
			originalUser.Name = user.Name;
			originalUser.Surname = user.Surname;
			originalUser.Email = user.Email;

			List<Role> Roles = user.Roles != null ? new(user.Roles) : new();

			originalUser.Roles.Clear();

			foreach (var Role in Roles)
			{
				Role existingRole = await _context.Roles.FindAsync(Role.Id) ??
					throw new StudentException($"Role with ID '{Role.Id}' not found.");

				originalUser.Roles.Add(existingRole);
			}


			_context.Entry(originalUser).State = EntityState.Modified;


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
					throw new UserException(ex.Message);
				}
			}
		}

		public async Task<User> Create(User user)
		{
			if (_context.Roles == null)
				throw new StudentException("Entity set 'DBUniversityContext.Roles' is null.");

			if (_context.Users == null)
				throw new UserException("Entity set 'DBUniversityContext.Users' is null.");

			user.CreatedAt = DateTime.Now;
			user.Password = PasswordHelper.HashPassword(user.Password);
			user.UpdatedAt = null;
			//user.IdUserCreatedBy = null; TODO: make automatic
			user.IdUserDeletedBy = null;
			user.IdUserUpdatedBy = null;
			user.DeletedAt = null;
			user.IsDeleted = false;

			List<Role> Roles = user.Roles != null ? new(user.Roles) : new();
			user.Roles = new List<Role>();

			foreach (var Role in Roles)
			{
				Role existingRole = await _context.Roles.FindAsync(Role.Id) ??
					throw new StudentException($"Role with ID '{Role.Id}' not found.");

				user.Roles.Add(existingRole);
			}

			_context.Users.Add(user);

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
					throw new UserException(ex.Message);
				}
			}

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
					.Include(user => user.Roles)
					.ToListAsync();
			}

			return users;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
