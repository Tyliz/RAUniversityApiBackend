namespace RAUniversityApiBackend.Exceptions.Chapter
{
	public class ChapterNotExistException : ChapterException
	{
		public ChapterNotExistException() : base("The chapter doesn't exist.") { }
	}
}
