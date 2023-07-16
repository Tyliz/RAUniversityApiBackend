namespace RAUniversityApiBackend.Exceptions.User
{
	public class UserNotExistException : UserException
	{
		public UserNotExistException() : base("The user doesn't exist.") { }
	}
}
