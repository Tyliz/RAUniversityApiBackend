namespace RAUniversityApiBackend.Exceptions.Course
{
	public class CourseNotExistException: CourseException
	{
		public CourseNotExistException() : base("The course doesn't exist.") { }
	}
}
