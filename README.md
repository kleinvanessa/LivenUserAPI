# LivenUserAPI

## Project Description

LivenUserAPI is an ASP.NET Core API designed to manage users and their addresses. 


## Requirements

- .NET Core SDK 8.0
- SQL Server
- Entity Framework Core


- ## Features

- JWT Authentication
- User CRUD
- Address CRUD
- Automated Tests with xUnit
- Swagger Documentation

## Quick Start

1. **Clone the repository:**

    ```bash
    git clone https://github.com/kleinvanessa/LivenUserAPI.git
    cd LivenUserAPI
    ```

2. **If necessary, Adjust your connection string in `appsettings.json`:**


3. **Install dependencies:**

    ```bash
    dotnet restore
    ```

4. **Apply EF Core migrations to create the database:**

    ```bash
    dotnet ef database update
    ```

        +-------------------+                +-----------------+
  |      User         |                |     Address     |
  +-------------------+                +-----------------+
  | Id (PK)           |<-------------->| Id (PK)         |
  | Name              |  1    N        | Street          |
  | Email             |                | City            |
  | Password          |                | Country         |
  |                   |                | PostalCode      | 
  +-------------------+                | UserId (FK)     |
                                       +-----------------+

5. **Run the application:**

    ```bash
    dotnet run
    ```

6. **Access Swagger documentation:**

    Open `https://localhost:5001/swagger` or `http://localhost:5000/swagger` in your browser.

7. **Run tests:**

    ```bash
    dotnet test
    ```

## Project Architecture

### Folder Structure


/LivenUserAPI
│
├── /Controllers
│   ├── UsersController.cs
│   ├── AddressesController.cs
│   ├── AuthController.cs
│
├── /Services
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IAddressService.cs
│   ├── AddressService.cs
│
├── /Repositories
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── IAddressRepository.cs
│   ├── AddressRepository.cs
│
├── /Domain
│   ├── Entities
│   │   ├── User.cs
│   │   ├── Address.cs
│
├── /Infrastructure
│   ├── Data
│   │   ├── ApplicationDbContext.cs
│   ├── Security
│   │   ├── JwtTokenService.cs
│   │   ├── IJwtTokenService.cs
│
├── /Mappings
│   ├── AddressMappings.cs
│   ├── UserMappings.cs
│
├── /Migrations
│   ├── (EF Core Migration Files)
│
├── /DTOs
│   ├── AddressDTO.cs
│   ├── UserDTO.cs
│   ├── LoginDTO.cs
│
├── /Properties
│   ├── launchSettings.json
│
├── appsettings.json
└── Program.cs

├── /LivenUserAPITests
