using RAUniversityApiBackend.Models.DataModels;

namespace RAUniversityApiBackend.Models.JwtModels
{
	public class UserToken
	{
		public int Id { get; set; }
		public string Token { get; set; }
		public string UserName { get; set; }
		public TimeSpan Validity { get; set; }
		public string RefreshToken { get; set; }
		public string EmailId { get; set; }
		public Guid GuidId { get; set; }
		public DateTime ExpiredTime { get; set; }
		public IEnumerable<string> Roles { get; set; } = new List<string>();
	}
}
