﻿using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.ViewModels.User
{
	public class UserCreateViewModel
	{
		[Required, StringLength(50)]
		public string UserName { get; set; } = string.Empty;

		[Required, StringLength(50)]
		public string Name { get; set; } = string.Empty;

		[Required, StringLength(100)]
		public string Surname { get; set; } = string.Empty;

		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		[Required]
		public ICollection<int> Roles { get; set; } = new List<int>();
	}
}
