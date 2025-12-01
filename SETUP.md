# Quick Setup Guide for Order Management API

## Step-by-Step Instructions

### 1. Prerequisites

- [ ] Install .NET 10 SDK
- [ ] Install SQL Server (Express/LocalDB/Full)
- [ ] Install dotnet-ef tools globally

### 2. Database Configuration

**Choose ONE of the following connection strings and update `appsettings.json`:**

#### For SQL Server Express (Windows):

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=OrderManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

#### For SQL Server LocalDB (Windows with Visual Studio):

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

#### For SQL Server with SQL Authentication:

```json
"DefaultConnection": "Server=localhost;Database=OrderManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
```

#### For macOS/Linux with Docker SQL Server:

```json
"DefaultConnection": "Server=localhost,1433;Database=OrderManagementDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;"
```

### 3. Setup Commands

Run these commands in order:

```bash
# Navigate to project directory
cd /Users/knives/Work/OrderManagementAPI/OrderManagementAPI

# Restore packages
dotnet restore

# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# For macOS/Linux - Add to PATH
export PATH="$PATH:$HOME/.dotnet/tools"

# Create database and tables
dotnet ef database update

# Run the application
dotnet run
```

### 4. Test the API

Once running, open your browser to:

- **Swagger UI**: http://localhost:5041/swagger

### 5. Quick API Test Flow

**Step 1: Register a User**

```http
POST http://localhost:5041/api/auth/register
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@example.com",
  "password": "Test123",
  "fullName": "Test User"
}
```

**Step 2: Login**

```http
POST http://localhost:5041/api/auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "Test123"
}
```

**Response will include a token - copy it!**

```json
{
  "token": "eyJhbGci...",
  "username": "testuser",
  "email": "test@example.com"
}
```

**Step 3: Create an Order**

```http
POST http://localhost:5041/api/orders
Authorization: Bearer {paste-your-token-here}
Content-Type: application/json

{
  "productName": "Laptop",
  "quantity": 1,
  "unitPrice": 1500.00
}
```

**Step 4: Get All Orders**

```http
GET http://localhost:5041/api/orders
Authorization: Bearer {paste-your-token-here}
```

### 6. Using Swagger UI (Easiest Method)

1. Run `dotnet run`
2. Open http://localhost:5041/swagger
3. Click on `/api/auth/register` → Try it out → Fill in details → Execute
4. Click on `/api/auth/login` → Try it out → Fill in credentials → Execute
5. **Copy the token from the response**
6. Click the **"Authorize"** button at the top
7. Paste: `Bearer {your-token}` and click Authorize
8. Now you can test all Order endpoints!

## Troubleshooting

### Cannot connect to SQL Server

1. Check SQL Server is running
2. Verify server name in connection string
3. Try using LocalDB: `(localdb)\mssqllocaldb`

### Migration Error

```bash
# Drop and recreate
dotnet ef database drop
dotnet ef database update
```

### Port Already in Use

```bash
dotnet run --urls="http://localhost:5050"
```

### Token Errors

- Make sure to include "Bearer " before the token
- Token expires in 60 minutes by default
- Login again to get a new token

## Running SQL Server in Docker (macOS/Linux)

If you don't have SQL Server installed:

```bash
# Pull SQL Server image
docker pull mcr.microsoft.com/mssql/server:2022-latest

# Run SQL Server container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest

# Update connection string in appsettings.json
"DefaultConnection": "Server=localhost,1433;Database=OrderManagementDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;"

# Then run migrations
dotnet ef database update
```

## Next Steps

After successful setup:

1. Explore all CRUD operations in Swagger
2. Create multiple users to test user isolation
3. Review the code in Controllers, Models, and Data folders
4. Customize JWT settings in appsettings.json
5. Add more features as needed

## Support

For issues:

1. Check README.md for detailed documentation
2. Verify all prerequisites are installed
3. Review error messages in terminal
4. Ensure connection string is correct for your SQL Server setup
