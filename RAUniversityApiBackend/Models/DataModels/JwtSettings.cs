namespace RAUniversityApiBackend.Models.DataModels
{
	public class JwtSettings
	{
		public bool ValidateIssuerSingingKey { get; set; }
		public string IssuerSingingKey { get; set; } = string.Empty;

		public bool ValidateIssuer { get; set; } = true;
		public string ValidIssuer { get; set; } = string.Empty;

		public bool ValidateAudience { get; set; }
		public string ValidAudience { get; set; } = string.Empty;

		public bool RequireExpirationTime { get; set; }
		public bool ValidateLifetime { get; set; }
	}
}
