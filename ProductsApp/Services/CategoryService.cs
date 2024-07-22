using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ProductsApp.DB.Models;
using ProductsApp.Model.Response;

namespace ProductsApp.Services
{
    public class CategoryService
    {
        private readonly ProductDBContext _dbContext;
        private readonly RedisCacheService _redisCacheService;

        public CategoryService(ProductDBContext dbContext, RedisCacheService redisCacheService)
        {
            _dbContext = dbContext;
            _redisCacheService = redisCacheService;
        }

        public async Task<List<Category>> GetCategoriesWithProductsAsync()
        {
            return await _dbContext.Categories
                                   .Include(el => el.Products)
                                   .ToListAsync();
        }

        public async Task<List<CategoryResponse>> GetCategoriesAsync()
        {
            return await _dbContext.Categories
                                   .Select(el => new CategoryResponse()
                                   {
                                       Id = el.Id,
                                       Name = el.Name
                                   })
                                   .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task<CategoryResponse> GetCategoryProducts(string categoryName)
        {
            var result = await _dbContext.Categories
                                         .Include(el => el.Products)
                                         .FirstOrDefaultAsync(el => el.Name.Contains(categoryName));
            CategoryResponse categoryResponse = new CategoryResponse()
            {
                Id = result.Id,
                Name = result.Name,
                Products = result.Products.Select(el => new ProductResponse()
                {
                    Id = (int)el.Id,
                    Name = el.Name,
                    CategoryId = el.CategoryId
                })
                .ToList()
            };
            return categoryResponse;
        }

        public async Task<List<CategoryResponse>> GetSubcategoriesOfParentCategory(int categoryId)
        {
            var result = await _dbContext.Categories
                                         .Where(el => el.ParentCategoryId == categoryId)
                                         .Select(el => new CategoryResponse()
                                         {
                                             Id = el.Id,
                                             ParentCategoryId = el.ParentCategoryId,
                                             Name = el.Name,
                                             ParentCategoryName = el.ParentCategory.Name
                                         })
                                         .ToListAsync();
            return result ?? new List<CategoryResponse>();
        }

        public async Task AddCategoryAsync(Category categoryItem)
        {
            await _dbContext.Categories.AddAsync(categoryItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category categoryItem)
        {
            _dbContext.Categories.Update(categoryItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category != null) 
            { 
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
