using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface ICoursesService: IBaseService<Course>
	{
		public Task<IEnumerable<Course>> GetWithoutsStudents();
		public Task<IEnumerable<Course>> GetByLevelWithAtLeastOneStudent(CourseLevel courseLevel);
		public Task<IEnumerable<Course>> GetWithoutThemes();
		public Task<IEnumerable<Course>> GetByCategory(int IdCategory);
		public Task<IEnumerable<Course>> GetByLevelCategory(CourseLevel courseLevel, int idCategory);
		public Task<IEnumerable<Course>> GetByStudent(int IdStudent);
	}
}
