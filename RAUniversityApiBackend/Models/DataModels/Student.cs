using RAUniversityApiBackend.Models.DataModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class Student : EntityUserManagement
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Surname { get; set; } = string.Empty;

		[Required]
		public DateTime DateOfBird { get; set; }

		public ICollection<Course>? Courses { get; set; } = new List<Course>();
	}
}
