using RAUniversityApiBackend.Models.DataModels.Interfaces;
using RAUniversityApiBackend.ViewModels.Chapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class Chapter : EntityUserManagement
	{
		[Required]
		[Column(Order = 1)]
		public string Themes { get; set; } = string.Empty;

		[Column(Order = 2)]
		public int IdCourse { get; set; }

		public Course? Course { get; set; } = new Course();


		public static Chapter Create(ChapterViewModel chapterViewModel)
		{
			return new Chapter
			{
				Id = chapterViewModel.Id,
				Themes = chapterViewModel.Themes,
				IdCourse = chapterViewModel.IdCourse,
			};
		}

		public static Chapter Create(ChapterCreateViewModel chapterViewModel)
		{
			return new Chapter
			{
				Themes = chapterViewModel.Themes,
				IdCourse = chapterViewModel.IdCourse,
			};
		}

		public static Chapter Create(ChapterUpdateViewModel chapterViewModel)
		{
			return new Chapter
			{
				Id = chapterViewModel.Id,
				Themes = chapterViewModel.Themes,
				IdCourse = chapterViewModel.IdCourse,
			};
		}
	}
}
