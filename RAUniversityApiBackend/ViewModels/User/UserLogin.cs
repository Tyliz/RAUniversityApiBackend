using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.User
{
	public class UserLogin
	{
		[Required(ErrorMessage = "RequiredUserName")]
		public string UserName { get; set; } = string.Empty;

		[Required(ErrorMessage = "RequiredPassword")]
		public string Password { get; set; } = string.Empty;
	}
}
