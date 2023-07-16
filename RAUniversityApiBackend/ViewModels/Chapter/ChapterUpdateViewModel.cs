using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Chapter
{
	public class ChapterUpdateViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Themes { get; set; } = string.Empty;

		[Required]
		public int IdCourse { get; set; }
	}
}
