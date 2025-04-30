using SimpleRestApi.Models;

namespace SimpleRestApi.Services
{
    public class ProductService
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 1200.00m, StockQuantity = 10 },
            new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone", Price = 800.00m, StockQuantity = 15 },
            new Product { Id = 3, Name = "Headphones", Description = "Noise cancelling headphones", Price = 300.00m, StockQuantity = 20 }
        };

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Create(Product product)
        {
            // Auto-increment ID
            if (_products.Count > 0)
                product.Id = _products.Max(p => p.Id) + 1;
            else
                product.Id = 1;

            _products.Add(product);
            return product;
        }

        public bool Update(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                return false;

            var index = _products.IndexOf(existingProduct);
            _products[index] = product;
            return true;
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return false;

            _products.Remove(product);
            return true;
        }
    }
} 