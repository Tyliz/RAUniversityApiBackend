using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Category
{
	public class CategoryViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;
	}
}
