using RAUniversityApiBackend.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Student
{
	public class StudentUpdateViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Surname { get; set; } = string.Empty;

		[Required]
		public DateTime DateOfBird { get; set; }

		public ICollection<int> Courses { get; set; } = new List<int>();
	}
}
