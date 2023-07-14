using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services
{
	public interface IStudentsService
	{
		IEnumerable<Student> GetOldersThan18();
		Task<IEnumerable<Student>> GetStudenWithAtLeastOneCourse();
		IEnumerable<Student> GetStudents();
		IEnumerable<Student> GetStudentsWithNoCourses();
	}
}
