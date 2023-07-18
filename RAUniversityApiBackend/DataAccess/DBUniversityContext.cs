using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Extensions;


namespace RAUniversityApiBackend.DataAccess
{
	public class DBUniversityContext : DbContext
	{
		public DBUniversityContext(DbContextOptions<DBUniversityContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Original code from: https://www.red-gate.com/simple-talk/blogs/change-delete-behavior-and-more-on-ef-core/
			// Author: Dennes Torres (https://www.red-gate.com/simple-talk/author/dennes-torres/)
			// Copy by: Tyliz
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				// equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
				entityType.SetTableName(entityType.DisplayName());

				// equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
				entityType.GetForeignKeys()
					.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
					.ToList()
					.ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
			}


			base.OnModelCreating(modelBuilder);
		}

		// TODO: Add DBSets (Table of our database)
		public DbSet<User>? Users { get; set; }

		public DbSet<Role>? Roles { get; set; }

		public DbSet<Chapter>? Chapters { get; set; }

		public DbSet<Course>? Courses { get; set; }

		public DbSet<Category>? Categories { get; set; }

		public DbSet<Student>? Students { get; set; }
	}
}
