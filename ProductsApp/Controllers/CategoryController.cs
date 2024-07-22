using Microsoft.AspNetCore.Mvc;
using ProductsApp.DB.Models;
using ProductsApp.Model.Request;
using ProductsApp.Services;

namespace ProductsApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var categoryItem = await _categoryService.GetCategoryByIdAsync(id);
            if (categoryItem == null) { 
                return NotFound();
            }
            return Ok(categoryItem);
        }

        [HttpGet("CategoryProducts/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var categoryItem = await _categoryService.GetCategoryProducts(name);
            if (categoryItem == null)
            {
                return NotFound();
            }
            return Ok(categoryItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var products = await _categoryService.GetCategoriesAsync();
            return Ok(products);
        }

        [HttpGet("Subcategories/{id}")]  // Ne radi ako je parametar categoryId, pa mora ići samo id
        public async Task<IActionResult> GetSubcategories(int id)
        {
            var subcategoriesResult = await _categoryService.GetSubcategoriesOfParentCategory(id);
            // return subcategoriesResult != null ? Ok(subcategoriesResult) : NotFound();

            if (subcategoriesResult == null) {
                return NotFound();
            }
            else if (subcategoriesResult.Count() == 0)
            {
                return NoContent();
            }
            return Ok(subcategoriesResult);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest categoryItemRequest)
        {
            Category categoryItem = new Category()
            {
                Name = categoryItemRequest.Name,
            };
            categoryItem.ParentCategoryId = categoryItemRequest.ParentCategoryId ?? null;

            await _categoryService.AddCategoryAsync(categoryItem);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryItem.Id }, categoryItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequest categoryItemRequest)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null) {
                return NotFound();
            }

            existingCategory.Name = categoryItemRequest.Name;

            await _categoryService.UpdateCategoryAsync(existingCategory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var product = await _categoryService.GetCategoryByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
