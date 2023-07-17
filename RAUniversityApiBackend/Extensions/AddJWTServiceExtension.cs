using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RAUniversityApiBackend.Models.DataModels;
using System.Text;

namespace RAUniversityApiBackend.Extensions
{
	public static class AddJWTServiceExtension
	{
		public static void AddJWTServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Add JWT Settings
			var bindJWTSettings = new JwtSettings();
			configuration.Bind("JsonWebTokenKeys", bindJWTSettings);

			// add singleton of JWT Settings
			services.AddSingleton(bindJWTSettings);

			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = bindJWTSettings.ValidateIssuerSingingKey,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bindJWTSettings.IssuerSingingKey)),
						ValidateIssuer = bindJWTSettings.ValidateIssuer,
						ValidIssuer = bindJWTSettings.ValidIssuer,
						ValidateAudience = bindJWTSettings.ValidateAudience,
						ValidAudience = bindJWTSettings.ValidAudience,
						RequireExpirationTime = bindJWTSettings.RequireExpirationTime,
						ValidateLifetime = bindJWTSettings.ValidateLifetime,
						ClockSkew = TimeSpan.FromDays(1),
					};
				});
		}
	}
}
