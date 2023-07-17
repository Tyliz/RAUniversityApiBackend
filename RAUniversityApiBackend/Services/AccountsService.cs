using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Exceptions.User;
using RAUniversityApiBackend.Helpers;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Services
{
	public class AccountsService : IAccountsService
	{
		#region Properties

		private readonly DBUniversityContext _context;

		#endregion

		#region Constructor

		public AccountsService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<User> ValidateCredential(string userName, string password)
		{
			if (_context.Users == null)
				throw new UserNotExistException();

			User user = await _context.Users
				.Include(user => user.Roles)
				.FirstOrDefaultAsync(user => 
					user.UserName == userName ||
					user.Email == userName
				)
				?? throw new UserNotExistException();

			if (PasswordHelper.VerifyPassword(password, user.Password))
				return user;

			throw new UserNotExistException();
		}

		#endregion
	}
}
