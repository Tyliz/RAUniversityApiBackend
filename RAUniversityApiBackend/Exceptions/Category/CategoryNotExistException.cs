namespace RAUniversityApiBackend.Exceptions.Category
{
	public class CategoryNotExistException : CategoryException
	{
		public CategoryNotExistException() : base("The category doesn't exist.") { }
	}
}
