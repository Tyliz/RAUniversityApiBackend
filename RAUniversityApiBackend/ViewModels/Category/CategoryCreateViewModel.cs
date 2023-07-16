using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Category
{
	public class CategoryCreateViewModel
	{
		[Required]
		public string Name { get; set; } = string.Empty;
	}
}
