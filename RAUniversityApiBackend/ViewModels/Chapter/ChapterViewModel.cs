using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Chapter
{
	public class ChapterViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Themes { get; set; } = string.Empty;

		[Required]
		public int IdCourse { get; set; }


		public static ChapterViewModel Create(Models.DataModels.Chapter chapter)
		{
			ChapterViewModel chapterViewModel = new()
			{
				Id = chapter.Id,
				Themes = chapter.Themes,
				IdCourse = chapter.IdCourse,
			};

			return chapterViewModel;
		}
	}
}
