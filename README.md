# Order Management API

ASP.NET Core Web API with JWT authentication and PostgreSQL. [Part of C# Learning]

## Setup

### Prerequisites

- .NET 10 SDK
- PostgreSQL

### Installation

```bash
# 1. Navigate to project
cd OrderManagementAPI

# 2. Update database connection in appsettings.json
# "DefaultConnection": "Host=localhost;Database=OrderManagementDB;Username=postgres;Password=your_password"

# 3. Install EF Core tools
dotnet tool install --global dotnet-ef

# 4. Apply migrations
dotnet ef database update

# 5. Run the application
dotnet run
```

Access Swagger UI at: `http://localhost:5041/swagger`

## API Endpoints

### Authentication

- **POST** `/api/auth/register` - Register new user
- **POST** `/api/auth/login` - Login and get JWT token

### Orders (Requires Authentication)

Include JWT token in header: `Authorization: Bearer {token}`

- **GET** `/api/orders` - Get all orders for current user
- **GET** `/api/orders/{id}` - Get specific order
- **POST** `/api/orders` - Create new order
- **PUT** `/api/orders/{id}` - Update order
- **DELETE** `/api/orders/{id}` - Delete order
