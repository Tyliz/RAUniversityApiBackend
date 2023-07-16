using RAUniversityApiBackend.Models.DataModels.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class Category : EntityUserManagement
	{
		[Required]
		[Column(Order = 1)]
		public string Name { get; set; } = string.Empty;

		public ICollection<Course>? Courses { get; set; } = new List<Course>();
	}
}
