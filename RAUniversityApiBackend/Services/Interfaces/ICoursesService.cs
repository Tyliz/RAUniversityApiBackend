using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface ICoursesService: IBaseService<Course>
	{
		public Task<List<Course>> GetWithoutsStudents();
		public Task<List<Course>> GetByLevelWithAtLeastOneStudent(CourseLevel courseLevel);
		public Task<List<Course>> GetByCategory(int idCategory);
		public Task<List<Course>> GetByLevelCategory(CourseLevel courseLevel, int idCategory);
	}
}
