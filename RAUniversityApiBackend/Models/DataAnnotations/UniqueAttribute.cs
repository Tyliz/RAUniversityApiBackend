using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RAUniversityApiBackend.Models.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class UniqueAttribute : ValidationAttribute
	{
		public Type? ModelType { get; set; }

		public string? PropertyName { get; set; }


		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value == null)
				return ValidationResult.Success;

			if (validationContext == null)
				return new ValidationResult("Validation is not posible");


			if (validationContext.GetService(typeof(DBUniversityContext)) is not DBUniversityContext dbContext)
				return new ValidationResult("Validation is not posible");


			Type entityType = ModelType ?? validationContext.ObjectType;
			string? propertyName = PropertyName ?? validationContext.MemberName;

			if (propertyName == null)
				return new ValidationResult("Validation is not posible");


			string parameterName = $"@{propertyName}";

			string sql = $"SELECT [{propertyName}] FROM [{dbContext.GetTableName(entityType)}] WHERE [{propertyName}] = {parameterName}"; 

			int count = dbContext.Database.SqlQueryRaw<int>(sql, new SqlParameter(parameterName, value)).Count();


			if (count > 0)
				return new ValidationResult(ErrorMessage ?? $"{propertyName} should be unique.");

			return ValidationResult.Success;
		}
	}
}
