using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RAUniversityApiBackend.Exceptions.User;
using RAUniversityApiBackend.Global;
using RAUniversityApiBackend.Helpers;
using RAUniversityApiBackend.Models;
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
		private readonly IStringLocalizer<AccountsController> _localaizer;
		private readonly IStringLocalizer<SharedResources> _sharedResourcesLocalizer;
		private readonly ILogger<AccountsController> _logger;

		public AccountsController(
			IAccountsService service,
			JwtSettings jwtSettings,
			IStringLocalizer<AccountsController> localaizer,
			IStringLocalizer<SharedResources> sharedResourcesLocalizer,
			ILogger<AccountsController> logger
		)
		{
			_jwtSettings = jwtSettings;
			_service = service;
			_localaizer = localaizer;
			_sharedResourcesLocalizer = sharedResourcesLocalizer;
			_logger = logger;
		}


		[HttpPost]
		public async Task<IActionResult> GetToken(UserLogin userLogin)
		{
			try
			{
				if (ModelState.IsValid)
				{
					UserToken token = new();
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

					string Welcome = _localaizer.GetString("Welcome").Value ?? string.Empty;
					Welcome = string.Format(Welcome, userLogin.UserName);;

					return Ok(new
					{
						Data = token,
						Welcome,
					});
				}

				IEnumerable<string> errors = ModelState.Values
					.SelectMany(x => x.Errors)
					.Select(x => _sharedResourcesLocalizer.GetString(x.ErrorMessage).Value ?? string.Empty)
					.Where(x => !string.IsNullOrEmpty(x));

				return BadRequest(errors);
			}
			catch (UserNotExistException)
			{
				return BadRequest(_localaizer.GetString("WrongDataGetToken").Value ?? string.Empty);
			}
			catch (Exception ex)
			{
				string message = $"{nameof(AccountsController)} - {nameof(GetToken)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.AccountsControllerGetToken), ex, message);

				throw new Exception("Get Token", ex);
			}
		}
	}
}
