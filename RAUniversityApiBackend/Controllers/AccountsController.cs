using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.User;
using RAUniversityApiBackend.Helpers;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Models.JwtModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.User;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountsService _service;
		private readonly JwtSettings _jwtSettings;

		public AccountsController(IAccountsService service, JwtSettings jwtSettings)
		{
			_jwtSettings = jwtSettings;
			_service = service;
		}


		[HttpPost]
		public async Task<IActionResult> GetToken(UserLogin userLogin)
		{
			try
			{
				UserToken token = new UserToken();
				User user = await _service.ValidateCredential(userLogin.UserName, userLogin.Password);

				token = JwtHelper.GetTokenKey(
					new UserToken
					{
						UserName = user.UserName,
						EmailId = user.Email,
						Id = user.Id,
						Roles = user.Roles.Select(role => role.Name),
						GuidId = Guid.NewGuid(),
					},
					_jwtSettings
				);

				return Ok(token);
			}
			catch (UserNotExistException)
			{
				return BadRequest("Wrong username or password");
			}
			catch (Exception ex)
			{
				throw new Exception("Get Token", ex);
			}
		}

		//RBAC Role based access control

		//[HttpGet]
		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		//public IActionResult GetUserList()
		//{
		//	return Ok(Logins);
		//}
	}
}
