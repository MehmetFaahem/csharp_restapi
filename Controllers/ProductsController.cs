using Microsoft.AspNetCore.Mvc;
using SimpleRestApi.Models;
using SimpleRestApi.Services;
using System.Text.Json;

namespace SimpleRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create(Product product)
        {
            var newProduct = _productService.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] object rawData)
        {
            try
            {
                _logger.LogInformation($"Update request received for id: {id}, body: {rawData}");
                
                var existingProduct = _productService.GetById(id);
                if (existingProduct == null)
                    return NotFound();
                
                // Convert the raw data to a Product
                Product updatedProduct;
                if (rawData is JsonElement jsonElement)
                {
                    updatedProduct = new Product
                    {
                        Id = id,
                        Name = jsonElement.TryGetProperty("Name", out var name) ? name.GetString() ?? existingProduct.Name : existingProduct.Name,
                        Description = jsonElement.TryGetProperty("Description", out var desc) ? desc.GetString() ?? existingProduct.Description : existingProduct.Description,
                        Price = jsonElement.TryGetProperty("Price", out var price) ? price.GetDecimal() : existingProduct.Price,
                        StockQuantity = jsonElement.TryGetProperty("StockQuantity", out var stock) ? stock.GetInt32() : existingProduct.StockQuantity
                    };
                }
                else
                {
                    // Try to deserialize raw data
                    var updatedValues = System.Text.Json.JsonSerializer.Deserialize<Product>(rawData.ToString() ?? "{}");
                    
                    // Create updated product with values from the request body or fallback to existing values
                    updatedProduct = new Product
                    {
                        Id = id,
                        Name = updatedValues?.Name ?? existingProduct.Name,
                        Description = updatedValues?.Description ?? existingProduct.Description,
                        Price = updatedValues?.Price ?? existingProduct.Price,
                        StockQuantity = updatedValues?.StockQuantity ?? existingProduct.StockQuantity
                    };
                }
                
                // Update the product
                var result = _productService.Update(updatedProduct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product: {ex.Message}");
                return StatusCode(500, $"Error updating product: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 