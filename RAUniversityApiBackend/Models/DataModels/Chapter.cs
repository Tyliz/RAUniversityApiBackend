using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class Chapter : BaseEntity
	{
		public int IdCourse { get; set; }
		public virtual Course Course { get; set; } = new Course();

		[Required]
		public string Themes { get; set; } = string.Empty;
	}
}
