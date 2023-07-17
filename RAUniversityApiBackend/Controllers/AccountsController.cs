using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Helpers;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.ViewModels.User;
using System.IdentityModel.Tokens.Jwt;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly JwtSettings _jwtSettings;

		public AccountsController(JwtSettings jwtSettings)
		{
			_jwtSettings = jwtSettings;
		}

		// TODO: Chage by Users DB
		private IEnumerable<User> Logins = new List<User>()
		{
			new User() {
				Id = 1,
				Email = "tyliz@gmail.com",
				UserName = "Admin",
				Password = "Admin",
			},
			new User() {
				Id = 2,
				Email = "lunes@gmail.com",
				UserName = "User1",
				Password = "Lunes",
			},
		};

		[HttpPost]
		public async Task<IActionResult> GetToken(UserLogin userLogin)
		{
			try
			{
				var token = new UserToken();
				var valid = Logins.Any(user =>
					user.UserName.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase)
				);

				if (valid)
				{
					var user = Logins.FirstOrDefault(user =>
						user.UserName.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase)
					);

					if (user == null)
					{
						throw new Exception();
					}

					token = JwtHelper.GetTokenKey(
						new UserToken
						{
							UserName = user.UserName,
							EmailId = user.Email,
							Id = user.Id,
							GuidId = Guid.NewGuid(),
						},
						_jwtSettings
					);

				}
				else
					return BadRequest("Wrong username or password");

				return Ok(token);
			}
			catch (Exception ex)
			{
				throw new Exception("Get Token", ex);
			}
		}

		//RBAC Role based access control
		[HttpGet]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public IActionResult GetUserList()
		{
			return Ok(Logins);
		}
	}
}
