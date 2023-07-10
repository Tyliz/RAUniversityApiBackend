using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class User: BaseEntity
	{
		[Required, StringLength(50)]
		public string UserName { get; set; } = string.Empty;

		[Required, StringLength(50)]
		public string Name { get; set; } = string.Empty;

		[Required, StringLength(100)]
		public string Surname { get; set; } = string.Empty;

		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
}
