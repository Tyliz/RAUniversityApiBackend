using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface IStudentsService : IBaseService<Student>
    {
        Task<IEnumerable<Student>> GetOldersThan18();
        Task<IEnumerable<Student>> GetStudenWithAtLeastOneCourse();
        Task<IEnumerable<Student>> GetStudentsWithNoCourses();
    }
}
