using RAUniversityApiBackend.Models.DataModels.Interfaces;
using RAUniversityApiBackend.ViewModels.Course;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.Models.DataModels
{
	public enum CourseLevel
	{
		Basic = 1,
		Medium = 2,
		Advanced = 3,
		Expert = 4,
	}

	public class Course : EntityUserManagement
	{

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

		public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

		public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

		public virtual ICollection<Student> Students { get; set; } = new List<Student>();


		public static Course Create(CourseViewModel courseViewModel)
		{
			ICollection<Category> Categories = courseViewModel.Categories
				.Select(category => Category.Create(category))
				.ToList();

			return new Course
			{
				Id = courseViewModel.Id,
				Name = courseViewModel.Name,
				ShortDescription = courseViewModel.ShortDescription,
				LongDescription = courseViewModel.LongDescription,
				Requirements = courseViewModel.Requirements,
				Level = courseViewModel.Level,
				Categories = Categories,
			};
		}

		public static Course Create(CourseCreateViewModel courseViewModel)
		{
			ICollection<Category> Categories = courseViewModel.Categories
				.Select(IdCategory => new Category { Id = IdCategory })
				.ToList();

			return new Course
			{
				Name = courseViewModel.Name,
				ShortDescription = courseViewModel.ShortDescription,
				LongDescription = courseViewModel.LongDescription,
				Requirements = courseViewModel.Requirements,
				Level = courseViewModel.Level,
				Categories = Categories,
			};
		}

		public static Course Create(CourseUpdateViewModel courseViewModel)
		{
			ICollection<Category> Categories = courseViewModel.Categories
				.Select(IdCategory => new Category { Id = IdCategory })
				.ToList();

			return new Course
			{
				Id = courseViewModel.Id,
				Name = courseViewModel.Name,
				ShortDescription = courseViewModel.ShortDescription,
				LongDescription = courseViewModel.LongDescription,
				Requirements = courseViewModel.Requirements,
				Level = courseViewModel.Level,
				Categories = Categories,
			};
		}
	}
}
