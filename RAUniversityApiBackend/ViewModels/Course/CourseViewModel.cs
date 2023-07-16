using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.ViewModels.Category;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Course
{
	public class CourseViewModel
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

		public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();


		public static CourseViewModel Create(Models.DataModels.Course course)
		{
			IEnumerable<CategoryViewModel> Categories = course.Categories
				.Select(category => CategoryViewModel.Create(category));

			return new CourseViewModel
			{
				Id = course.Id,
				Name = course.Name,
				ShortDescription = course.ShortDescription,
				LongDescription = course.LongDescription,
				Requirements = course.Requirements,
				Level = course.Level,
				Categories = Categories,
			};
		}
	}
}
