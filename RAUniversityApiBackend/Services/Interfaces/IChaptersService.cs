using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface IChaptersService : IBaseService<Chapter>
	{
		Task<IEnumerable<Chapter>> GetByCourse(int IdCourse);
	}
}
