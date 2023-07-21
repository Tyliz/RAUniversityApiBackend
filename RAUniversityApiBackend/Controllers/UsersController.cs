using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.User;
using RAUniversityApiBackend.Goblal;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.Student;
using RAUniversityApiBackend.ViewModels.User;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _service;
		private readonly ILogger<UsersController> _logger;
		private string Name
		{
			get
			{
				return nameof(StudentsController);
			}
		}

		public UsersController(IUsersService service, ILogger<UsersController> logger)
		{
			_service = service;
			_logger = logger;
		}

		// GET: api/Users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers()
		{
			try
			{
				IEnumerable<User> users = await _service.GetAll();
				IEnumerable<UserViewModel> usersViewModel = users
					.Select(user => UserViewModel.Create(user));

				return Ok(usersViewModel);
			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetUsers)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.UsersControllerGetUsers), ex, message);

				return Ok(Array.Empty<StudentViewModel>());
			}
		}

		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<ActionResult<UserViewModel>> GetUser(int id)
		{
			try
			{
				User user = await _service.Get(id);
				return UserViewModel.Create(user);
			}
			catch (UserNotExistException)
			{
				return NotFound();
			}
			catch (UserException ex)
			{
				string message = $"{Name} - {nameof(GetUser)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.UsersControllerGetUser), ex, message);

				return Problem(ex.Message);
			}
		}

		// PUT: api/Users/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> PutUser(int id, UserUpdateViewModel user)
		{
			if (id != user.Id) return BadRequest();

			try
			{
				await _service.Update(Models.DataModels.User.Create(user));
				return NoContent();
			}
			catch (UserNotExistException)
			{
				return NotFound();
			}
			catch (UserException ex)
			{
				string message = $"{Name} - {nameof(PutUser)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.UsersControllerPutUser), ex, message);

				return Problem(ex.Message);
			}
		}

		// POST: api/Users
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<ActionResult<UserViewModel>> PostUser(UserCreateViewModel user)
		{
			try
			{
				if (ModelState.IsValid)
				{
					User newUser = await _service.Create(Models.DataModels.User.Create(user));

					return CreatedAtAction(
						"GetUser",
						new { id = newUser.Id },
						UserViewModel.Create(newUser)
					);
				}

				return BadRequest(ModelState.Values.Select(x => x.Errors));
			}
			catch (UserException ex)
			{
				string message = $"{Name} - {nameof(PostUser)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.UsersControllerPostUser), ex, message);

				return Problem(ex.Message);
			}
		}

		// DELETE: api/Users/5
		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			try
			{
				await _service.Delete(id);
				return NoContent();
			}
			catch (UserNotExistException)
			{
				return NotFound();
			}
			catch (UserException ex)
			{
				string message = $"{Name} - {nameof(DeleteUser)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.UsersControllerDeleteUser), ex, message);

				return Problem(ex.Message);
			}
		}
	}
}
