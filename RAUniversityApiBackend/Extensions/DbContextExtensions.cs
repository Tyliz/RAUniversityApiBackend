using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Extensions
{
	public static class DbContextExtensions
	{
		public static string? GetTableName(this DbContext context, Type TEntity)
		{
			IEntityType entityType = context.Model.FindEntityType(TEntity) ??
				throw new InvalidOperationException($"Entity type '{TEntity.FullName}' not found in the current context's model.");

			var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");

			return tableNameAnnotation.Value.ToString();
		}

		public static string? GetTableName<TEntity>(this DbContext context)
		{
			IEntityType entityType = context.Model.FindEntityType(typeof(TEntity)) ??
				throw new InvalidOperationException($"Entity type '{typeof(TEntity).FullName}' not found in the current context's model.");

			var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");

			return tableNameAnnotation.Value.ToString();
		}
	}
}
