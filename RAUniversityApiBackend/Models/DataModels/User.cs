﻿using RAUniversityApiBackend.Models.DataAnnotations;
using RAUniversityApiBackend.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class User : BaseEntity
	{
		[Required, StringLength(50)]
		[Column(Order = 1)]
		[Unique(ErrorMessage = "The username already exist")]
		public string UserName { get; set; } = string.Empty;

		[Required, StringLength(50)]
		[Column(Order = 2)]
		public string Name { get; set; } = string.Empty;

		[Required, StringLength(100)]
		[Column(Order = 3)]
		public string Surname { get; set; } = string.Empty;

		[Required, EmailAddress]
		[Column(Order = 4)]
		[Unique(ErrorMessage = "The email already exist")]
		public string Email { get; set; } = string.Empty;

		[Required]
		[Column(Order = 5)]
		public string Password { get; set; } = string.Empty;

		[Column(Order = 6)]
		public int IdUserCreatedBy { get; set; }

		[Column(Order = 7)]
		public int? IdUserUpdatedBy { get; set; }

		[Column(Order = 8)]
		public int? IdUserDeletedBy { get; set; }

		public ICollection<Role> Roles { get; set; } = new List<Role>();


		public static User Create(UserViewModel model)
		{
			return new User()
			{
				Id = model.Id,
				UserName = model.UserName,
				Name = model.Name,
				Surname = model.Surname,
				Email = model.Email,
				Roles = model.Roles.Select(role => new Role { Id = role }).ToList(),
			};
		}

		public static User Create(UserCreateViewModel model)
		{
			return new User()
			{
				UserName = model.UserName,
				Name = model.Name,
				Surname = model.Surname,
				Email = model.Email,
				Password = model.Password,
				Roles = model.Roles.Select(role => new Role { Id = role }).ToList(),
			};
		}

		public static User Create(UserUpdateViewModel model)
		{
			return new User()
			{
				Id = model.Id,
				UserName = model.UserName,
				Name = model.Name,
				Surname = model.Surname,
				Email = model.Email,
				Roles = model.Roles.Select(role => new Role { Id = role }).ToList(),
			};
		}
	}
}
