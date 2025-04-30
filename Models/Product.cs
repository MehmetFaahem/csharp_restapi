namespace SimpleRestApi.Models
{
    /// <summary>
    /// Represents a product in the system
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The unique identifier for the product
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// The description of the product
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// The price of the product
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// The available quantity in stock
        /// </summary>
        public int StockQuantity { get; set; }
    }
} 