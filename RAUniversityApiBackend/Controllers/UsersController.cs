using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.User;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _service;

		public UsersController(IUsersService service)
		{
			_service = service;
		}

		// GET: api/Users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			IEnumerable<User> users = await _service.GetAll();
			return Ok(users);
		}

		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			try
			{
				User user = await _service.Get(id);
				return user;
			}
			catch (UserNotExistException)
			{
				return NotFound();
			}
			catch (UserException ex)
			{
				return Problem(ex.Message);
			}
		}

		// PUT: api/Users/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser(int id, User user)
		{
			if (id != user.Id) return BadRequest();

			try
			{
				await _service.Update(user);
				return NoContent();
			}
			catch (UserNotExistException)
			{
				return NotFound();
			}
			catch (UserException ex)
			{
				return Problem(ex.Message);
			}
		}

		// POST: api/Users
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			try
			{
				User newUser = await _service.Create(user);

				return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
			}
			catch (UserException ex)
			{
				return Problem(ex.Message);
			}
		}

		// DELETE: api/Users/5
		[HttpDelete("{id}")]
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
				return Problem(ex.Message);
			}
		}
	}
}
