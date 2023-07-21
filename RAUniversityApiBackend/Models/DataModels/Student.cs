using RAUniversityApiBackend.Models.DataModels.Interfaces;
using RAUniversityApiBackend.ViewModels.Student;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class Student : EntityUserManagement
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Surname { get; set; } = string.Empty;

		[Required]
		public DateTime DateOfBird { get; set; }

		public virtual ICollection<Course> Courses { get; set; } = new List<Course>();


		public static Student Create(StudentViewModel studentViewModel)
		{
			return new Student
			{
				Id = studentViewModel.Id,
				Name = studentViewModel.Name,
				Surname = studentViewModel.Surname,
				DateOfBird = studentViewModel.DateOfBird,
			};
		}

		public static Student Create(StudentCreateViewModel studentViewModel)
		{
			ICollection<Course> Courses = studentViewModel.Courses
				.Select(IdCourse => new Course { Id = IdCourse })
				.ToList();

			return new Student
			{
				Name = studentViewModel.Name,
				Surname = studentViewModel.Surname,
				DateOfBird = studentViewModel.DateOfBird,
				Courses = Courses,
			};
		}

		public static Student Create(StudentUpdateViewModel studentViewModel)
		{
			ICollection<Course> Courses = studentViewModel.Courses
				.Select(IdCourse => new Course { Id = IdCourse })
				.ToList();

			return new Student
			{
				Id = studentViewModel.Id,
				Name = studentViewModel.Name,
				Surname = studentViewModel.Surname,
				DateOfBird = studentViewModel.DateOfBird,
				Courses = Courses,
			};
		}
	}
}
