using Microsoft.AspNetCore.Mvc;
using SimpleRestApi.Models;
using SimpleRestApi.Services;
using System.Text.Json;

namespace SimpleRestApi.Controllers
{
    /// <summary>
    /// API controller for managing products
    /// </summary>
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

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>A collection of all products</returns>
        /// <response code="200">Returns the list of products</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(_productService.GetAll());
        }

        /// <summary>
        /// Gets a product by ID
        /// </summary>
        /// <param name="id">The ID of the product to retrieve</param>
        /// <returns>The product with the specified ID</returns>
        /// <response code="200">Returns the product with the specified ID</response>
        /// <response code="404">If the product doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the product is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> Create(Product product)
        {
            var newProduct = _productService.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The ID of the product to update</param>
        /// <param name="rawData">The updated product data</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the product was successfully updated</response>
        /// <response code="404">If the product doesn't exist</response>
        /// <response code="500">If there was an error updating the product</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The ID of the product to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the product was successfully deleted</response>
        /// <response code="404">If the product doesn't exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 