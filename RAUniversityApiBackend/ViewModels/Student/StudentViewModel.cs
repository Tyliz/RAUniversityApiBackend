using RAUniversityApiBackend.ViewModels.Course;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.Student
{
	public class StudentViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Surname { get; set; } = string.Empty;

		[Required]
		public DateTime DateOfBird { get; set; }


		public static StudentViewModel Create(Models.DataModels.Student student)
		{
			return new StudentViewModel
			{
				Id = student.Id,
				Name = student.Name,
				Surname = student.Surname,
				DateOfBird = student.DateOfBird,
			};
		}
	}
}
