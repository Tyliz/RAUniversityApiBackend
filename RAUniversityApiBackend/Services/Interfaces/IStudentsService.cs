using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface IStudentsService : IBaseService<Student>
	{
		public IEnumerable<Student> GetAllSync();
		public Task<IEnumerable<Student>> GetOldersThan18();
		public Task<IEnumerable<Student>> GetStudenWithAtLeastOneCourse();
		public Task<IEnumerable<Student>> GetStudentsWithNoCourses();
		public Task<IEnumerable<Student>> GetStudentsByCourse(int IdCourse);
	}
}
