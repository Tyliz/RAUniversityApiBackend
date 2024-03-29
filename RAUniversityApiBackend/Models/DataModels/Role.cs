﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAUniversityApiBackend.Models.DataModels
{
	public class Role : BaseEntity
	{
		[Required, Column(Order = 1)]
		public string Name { get; set; } = string.Empty;

		public ICollection<User> Users { get; set; } = new List<User>();
	}
}
