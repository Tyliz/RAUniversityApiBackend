using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.ViewModels.Category;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Course
{
	public class CourseUpdateViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required, StringLength(50)]
		public string Name { get; set; } = string.Empty;

		[Required, StringLength(280)]
		public string ShortDescription { get; set; } = string.Empty;

		[Required]
		public string LongDescription { get; set; } = string.Empty;

		[Required]
		public string Requirements { get; set; } = string.Empty;

		[Required]
		public CourseLevel Level { get; set; } = CourseLevel.Basic;

		public ICollection<int> Categories { get; set; } = new List<int>();
	}
}
