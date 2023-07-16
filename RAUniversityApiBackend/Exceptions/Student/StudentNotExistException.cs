namespace RAUniversityApiBackend.Exceptions.Student
{
	public class StudentNotExistException : Exception
	{
		public StudentNotExistException(): base("The student doesn't exist.") { }
	}
}
