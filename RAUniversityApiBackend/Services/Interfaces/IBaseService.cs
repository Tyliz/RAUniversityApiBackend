using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Services.Interfaces
{
	public interface IBaseService<T> where T : BaseEntity
	{
		public Task<IEnumerable<T>> GetAll();
		public Task<T> Get(int id);
		public Task Update(T entity);
		public Task<T> Create(T entity);
		public Task Delete(int id);
	}
}
