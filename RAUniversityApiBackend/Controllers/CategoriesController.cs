using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.Category;
using RAUniversityApiBackend.Global;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.Category;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesService _service;
		private readonly ILogger<CategoriesController> _logger;
		private string Name
		{
			get
			{
				return nameof(CategoriesController);
			}
		}


		public CategoriesController(ICategoriesService service, ILogger<CategoriesController> logger)
		{
			_service = service;
			_logger = logger;
		}

		// GET: api/Categories
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
		{
			IEnumerable<Category> categories = new List<Category>();
			try
			{
				categories = await _service.GetAll();
				IEnumerable<CategoryViewModel> categoriesResult = categories
					.Select(category => CategoryViewModel.Create(category)); 

				return Ok(categoriesResult);
			}
			catch (Exception ex)
			{
				string message = $"{Name} - {nameof(GetCategories)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CategoriesControllerGetCategories), ex, message);

				return Ok(categories);
			}
		}

		// GET: api/Categories/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryViewModel>> GetCategory(int id)
		{
			try
			{
				Category category = await _service.Get(id);

				return CategoryViewModel.Create(category);
			}
			catch (CategoryNotExistException)
			{
				return NotFound();
			}
			catch (CategoryException ex)
			{
				string message = $"{Name} - {nameof(GetCategory)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CategoriesControllerGetGetCategory), ex, message);

				return Problem(ex.Message);
			}
		}

		// PUT: api/Categories/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> PutCategory(int id, CategoryUpdateViewModel category)
		{
			if (id != category.Id) return BadRequest();

			try
			{
				await _service.Update(Category.Create(category));
				return NoContent();
			}
			catch (CategoryNotExistException)
			{
				return NotFound();
			}
			catch (CategoryException ex)
			{
				string message = $"{Name} - {nameof(PutCategory)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CategoriesControllerPutCategory), ex, message);

				return Problem(ex.Message);
			}
		}

		// POST: api/Categories
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<ActionResult<CategoryViewModel>> PostCategory(CategoryCreateViewModel category)
		{
			try
			{
				Category newCategory = await _service.Create(Category.Create(category));

				return CreatedAtAction(
					"GetCategory", 
					new { id = newCategory.Id },
					CategoryViewModel.Create(newCategory)
				);
			}
			catch (CategoryException ex)
			{
				string message = $"{Name} - {nameof(PostCategory)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CategoriesControllerPostCategory), ex, message);

				return Problem(ex.Message);
			}
		}

		// DELETE: api/Categories/5
		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			try
			{
				await _service.Delete(id);
				return NoContent();
			}
			catch (CategoryNotExistException)
			{
				return NotFound();
			}
			catch (CategoryException ex)
			{
				string message = $"{Name} - {nameof(DeleteCategory)} - {ex.Message}";
				_logger.LogCritical(new EventId((int)EventIds.CategoriesControllerDeleteCategory), ex, message);

				return Problem(ex.Message);
			}
		}
	}
}
