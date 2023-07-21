using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface IAccountsService
	{
		public Task<User> ValidateCredential(string userName, string password);
	}
}
