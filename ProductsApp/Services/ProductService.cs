using Microsoft.EntityFrameworkCore;
using ProductsApp.DB;

namespace ProductsApp.Services
{
    public class ProductService
    {
        private readonly ProductDbContext _dbContext;
        private readonly RedisCacheService _redisCacheService;

        public ProductService(ProductDbContext dbContext, RedisCacheService redisCacheService)
        {
            _dbContext = dbContext;
            _redisCacheService = redisCacheService;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var cachedProduct = await _redisCacheService.GetCacheValueAsync<Product>($"product:{id}");
            if (cachedProduct != null) 
            {
                return cachedProduct;   
            }

            var product = await _dbContext.Products.FindAsync(id);
            if (product != null) 
            {
                await _redisCacheService.SetCacheValueAsync($"product:{id}", product);
            }
            return product;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            await _redisCacheService.SetCacheValueAsync($"product:{product.Id}", product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            await _redisCacheService.SetCacheValueAsync($"product:{product.Id}", product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = _dbContext.Products.FindAsync(id);
            if(product != null)
            {
                _dbContext.Products.Remove(product.Result);
                await _dbContext.SaveChangesAsync();
                await _redisCacheService.RemoveCacheValueAsync($"product:{id}");
            }
        }
    }
}
