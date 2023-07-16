using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Category
{
	public class CategoryViewModel
	{
		private CategoryViewModel() { }

		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;


		public static CategoryViewModel Create(Models.DataModels.Category category)
		{
			CategoryViewModel categoryViewModel = new()
			{
				Id = category.Id,
				Name = category.Name,
			};

			return categoryViewModel;
		}
	}
}
