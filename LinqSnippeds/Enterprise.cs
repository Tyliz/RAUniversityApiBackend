namespace LinqSnippeds
{
	public class Enterprise
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public ICollection<Employee> Employees { get; set; } = new List<Employee>();
	}
}
