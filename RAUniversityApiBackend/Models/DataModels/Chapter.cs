using RAUniversityApiBackend.Models.DataModels.Interfaces;
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
	}
}
