using RAUniversityApiBackend.Models.DataModels.Interfaces;
using RAUniversityApiBackend.ViewModels.Category;
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


		public static Category Create(CategoryViewModel categoryVM)
		{
			Category category = new()
			{
				Id = categoryVM.Id,
				Name = categoryVM.Name,
			};

			return category;
		}

		public static Category Create(CategoryCreateViewModel categoryVM)
		{
			return new Category
			{
				Name = categoryVM.Name,
			};
		}

		public static Category Create(CategoryUpdateViewModel categoryVM)
		{
			return new Category
			{
				Id = categoryVM.Id,
				Name = categoryVM.Name,
			};
		}
	}
}
