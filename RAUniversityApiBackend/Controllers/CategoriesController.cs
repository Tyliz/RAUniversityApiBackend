using Microsoft.AspNetCore.Mvc;
using RAUniversityApiBackend.Exceptions.Category;
using RAUniversityApiBackend.Models.DataModels;
using RAUniversityApiBackend.Services.Interfaces;
using RAUniversityApiBackend.ViewModels.Category;
using System.ComponentModel;

namespace RAUniversityApiBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesService _service;

		public CategoriesController(ICategoriesService service)
		{
			_service = service;
		}

		// GET: api/Categories
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
		{
			IEnumerable<Category> categories = await _service.GetAll();
			IEnumerable<CategoryViewModel> categoriesResult = categories
				.Select(category => CategoryViewModel.Create(category)); 

			return Ok(categoriesResult);
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
				return Problem(ex.Message);
			}
		}

		// PUT: api/Categories/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
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
				return Problem(ex.Message);
			}
		}

		// POST: api/Categories
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
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
				return Problem(ex.Message);
			}
		}

		// DELETE: api/Categories/5
		[HttpDelete("{id}")]
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
				return Problem(ex.Message);
			}
		}
	}
}
