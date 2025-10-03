using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IDV.Application.DTOs;
using IDV.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IDV.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IDVDbContext _context;

        public ProductsController(IDVDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            try
            {
                var products = await _context.Products
                    .Where(p => p.IsActive)
                    .Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        ProductCode = p.ProductCode,
                        ProductName = p.ProductName,
                        Category = p.Category,
                        Description = p.Description,
                        PremiumAmount = p.PremiumAmount,
                        Currency = p.Currency,
                        IsActive = p.IsActive,
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving products", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            try
            {
                var product = await _context.Products
                    .Where(p => p.ProductId == id && p.IsActive)
                    .Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        ProductCode = p.ProductCode,
                        ProductName = p.ProductName,
                        Category = p.Category,
                        Description = p.Description,
                        PremiumAmount = p.PremiumAmount,
                        Currency = p.Currency,
                        IsActive = p.IsActive,
                        CreatedAt = p.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the product", error = ex.Message });
            }
        }
    }
}