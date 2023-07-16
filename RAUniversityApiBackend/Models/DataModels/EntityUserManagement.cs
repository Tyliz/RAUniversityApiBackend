using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels.Interfaces
{
	public abstract class EntityUserManagement : BaseEntity
	{
		public int IdUserCreatedBy { get; set; }
		[ForeignKey(nameof(IdUserCreatedBy))]
		public User UserCreatedBy { get; set; } = new User();

		public int? IdUserUpdatedBy { get; set; }
		[ForeignKey(nameof(IdUserUpdatedBy))]
		public User? UserUpdatedBy { get; set; }

		public int? IdUserDeletedBy { get; set; }
		[ForeignKey(nameof(IdUserDeletedBy))]
		public User? UserDeletedBy { get; set; }
	}
}
