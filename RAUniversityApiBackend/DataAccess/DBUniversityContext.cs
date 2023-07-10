using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.DataAccess
{
	public class DBUniversityContext : DbContext
	{
        public DBUniversityContext(DbContextOptions<DBUniversityContext> options): base(options) { }

		// TODO: Add DBSets (Table of our database)
		public DbSet<User?> User { get; set; }

		public DbSet<Course?> Course { get; set; }
    }
}
