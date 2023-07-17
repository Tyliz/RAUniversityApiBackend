using Microsoft.IdentityModel.Tokens;
using RAUniversityApiBackend.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RAUniversityApiBackend.Helpers
{
    public static class JwtHelper
	{
		public static IEnumerable<Claim> GetClaims(this UserToken userAccount, Guid Id)
		{
			List<Claim> claims = new()
			{
				new Claim("Id", userAccount.Id.ToString()),
				new Claim(ClaimTypes.Name, userAccount.UserName),
				new Claim(ClaimTypes.Email, userAccount.EmailId),
				new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
				new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt")),
			};

			foreach (string role in userAccount.Roles)
				claims.Add(new Claim(ClaimTypes.Role, role));

			//claims.Add(new Claim("UserOnly", "User1"));

			return claims;
		}

		public static IEnumerable<Claim> GetClaims(this UserToken userAccounts, out Guid Id)
		{
			Id = Guid.NewGuid();

			return GetClaims(userAccounts, Id);
		}

		public static UserToken GetTokenKey(UserToken model, JwtSettings jwtSettings)
		{
			try
			{
				var userToken = new UserToken();

				if (model == null)
				{
					throw new ArgumentNullException(nameof(model));
				}

				// Obtain SECRET KEY
				var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSingingKey);

				Guid Id;

				// Expires in 1 Day
				DateTime expiredTime = DateTime.UtcNow.AddDays(1);

				// Validity of our token
				userToken.Validity = expiredTime.TimeOfDay;

				// GENERATE OUR JWT
				var jsonWebToken = new JwtSecurityToken(
					issuer: jwtSettings.ValidIssuer,
					audience: jwtSettings.ValidAudience,
					claims: GetClaims(model, out Id),
					notBefore: new DateTimeOffset(DateTime.UtcNow).DateTime,
					expires: new DateTimeOffset(expiredTime).DateTime,
					signingCredentials: new SigningCredentials(
						new SymmetricSecurityKey(key),
						SecurityAlgorithms.HmacSha256
					)
				);

				userToken.Token = new JwtSecurityTokenHandler().WriteToken(jsonWebToken);
				userToken.UserName = model.UserName;
				userToken.Id = model.Id;
				userToken.GuidId = Id;

				return userToken;
			}
			catch (Exception ex)
			{
				throw new Exception("Error Generating the JWT", ex);
			}
		}
	}
}
