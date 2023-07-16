using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface IUsersService : IBaseService<User>
	{
		public Task<List<User>> SearchByEmail(string email);
	}
}
