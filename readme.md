# Simple REST API

A basic REST API for product management built with ASP.NET Core.

## Overview

This project demonstrates a simple RESTful API implementation with the following components:

- **Models**: Data structures used by the application
- **Controllers**: API endpoints for handling HTTP requests
- **Services**: Business logic for data manipulation

## Project Structure

### Models

The application uses a `Product` model with the following properties:

- `Id`: Unique identifier for the product
- `Name`: Product name
- `Description`: Product description
- `Price`: Product price
- `StockQuantity`: Available stock quantity

### Controllers

The `ProductsController` provides the following endpoints:

| Method | Endpoint             | Description                |
| ------ | -------------------- | -------------------------- |
| GET    | `/api/products`      | Get all products           |
| GET    | `/api/products/{id}` | Get a product by ID        |
| POST   | `/api/products`      | Create a new product       |
| PUT    | `/api/products/{id}` | Update an existing product |
| DELETE | `/api/products/{id}` | Delete a product           |

### Services

The `ProductService` handles the business logic:

- In-memory storage of product data
- CRUD operations for products
- Data validation and manipulation

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the application: `dotnet run`
4. Access the API at: `https://localhost:[port]/api/products`

## API Usage Examples

### Get all products

```
GET /api/products
```

### Get a product by ID

```
GET /api/products/1
```

### Create a new product

```
POST /api/products
{
  "name": "Product 1",
  "description": "Description for product 1",
  "price": 10.99,
  "stockQuantity": 10
}
```

### Update an existing product

```
PUT /api/products/1
{
  "name": "Product 1",
  "description": "Updated description for product 1",
  "price": 10.99,
  "stockQuantity": 10
}
```

### Delete a product

```
DELETE /api/products/1
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request if you have any suggestions or improvements.
