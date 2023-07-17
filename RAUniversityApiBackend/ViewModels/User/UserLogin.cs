using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.User
{
	public class UserLogin
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
