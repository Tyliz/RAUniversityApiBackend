using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class User : BaseEntity
	{

		[Required, StringLength(50)]
		[Column(Order = 1)]
		public string UserName { get; set; } = string.Empty;

		[Required, StringLength(50)]
		[Column(Order = 2)]
		public string Name { get; set; } = string.Empty;

		[Required, StringLength(100)]
		[Column(Order = 3)]
		public string Surname { get; set; } = string.Empty;

		[Required, EmailAddress]
		[Column(Order = 4)]
		public string Email { get; set; } = string.Empty;

		[Required]
		[Column(Order = 5)]
		public string Password { get; set; } = string.Empty;

		[Column(Order = 6)]
		public int IdUserCreatedBy { get; set; }

		[Column(Order = 7)]
		public int? IdUserUpdatedBy { get; set; }

		[Column(Order = 8)]
		public int? IdUserDeletedBy { get; set; }
	}
}
