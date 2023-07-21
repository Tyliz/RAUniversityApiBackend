using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface IUsersService : IBaseService<User>
	{
		public Task<IEnumerable<User>> SearchByEmail(string email);
	}
}
