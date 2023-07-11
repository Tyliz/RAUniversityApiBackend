using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.DataAccess
{
	public class DBUniversityContext : DbContext
	{
        public DBUniversityContext(DbContextOptions<DBUniversityContext> options): base(options) { }

		// TODO: Add DBSets (Table of our database)
		public DbSet<User>? Users { get; set; }

		public DbSet<Chapter>? Chapters { get; set; }

		public DbSet<Course>? Courses { get; set; }

		public DbSet<Category>? Categories { get; set; }

		public DbSet<Student>? Students { get; set; }
	}
}
