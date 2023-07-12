using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public abstract class BaseEntity
	{
		[Required]
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public DateTime? UpdatedAt { get; set; }

		public int IdUserCreatedBy { get; set; }
		[ForeignKey(nameof(IdUserCreatedBy))]
		public User? UserCreatedBy { get; set; }
		
		public int? IdUserUpdatedBy { get; set; }
		[ForeignKey(nameof(IdUserUpdatedBy))]
		public User? UserUpdatedBy { get; set; }

		public int? IdUserDeletedBy { get; set; }
		[ForeignKey(nameof(IdUserDeletedBy))]
		public User? UserDeletedBy { get; set; }

		public DateTime? DeletedAt { get; set; }

		public bool IsDeleted { get; set; } = false;
	}
}
