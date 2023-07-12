using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class User
	{
		[Required]
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

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

		[Column(Order = 9)]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		[Column(Order = 10)]
		public DateTime? UpdatedAt { get; set; }

		[Column(Order = 11)]
		public DateTime? DeletedAt { get; set; }

		[Column(Order = 12)]
		public bool IsDeleted { get; set; } = false;
	}
}
