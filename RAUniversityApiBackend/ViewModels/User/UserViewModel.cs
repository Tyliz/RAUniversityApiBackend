using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.User
{
	public class UserViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required, StringLength(50)]
		public string UserName { get; set; } = string.Empty;

		[Required, StringLength(50)]
		public string Name { get; set; } = string.Empty;

		[Required, StringLength(100)]
		public string Surname { get; set; } = string.Empty;

		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;


		public static UserViewModel Create(Models.DataModels.User user)
		{
			return new UserViewModel
			{
				Id = user.Id,
				UserName = user.UserName,
				Name = user.Name,
				Surname = user.Surname,
				Email = user.Email
			};
		}
	}
}
