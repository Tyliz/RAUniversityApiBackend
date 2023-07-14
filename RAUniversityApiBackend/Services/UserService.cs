using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services
{
	public class UserService
	{
		#region Properties

		private readonly DBUniversityContext _context;

		#endregion

		#region Constructors

		public UserService(DBUniversityContext context)
		{
			_context = context;
		}

		#endregion

		#region Public Methods

		public async Task<List<User>> SearchByEmail(string email)
		{
			List<User> users = new();

			if (_context.Users != null)
			{
				users = await _context.Users.ToListAsync();

				users = users
					.Select(user => user)
					.Where(x => x.Email == email)
					.ToList();
			}

			return users;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
