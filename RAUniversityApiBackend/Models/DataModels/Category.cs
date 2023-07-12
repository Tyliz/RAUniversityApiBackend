using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class Category: BaseEntity
	{
		[Required]
		[Column(Order = 1)]
		public string Name { get; set; } = string.Empty;

		public ICollection<Course>? Courses { get; set; } = new List<Course>();
	}
}
