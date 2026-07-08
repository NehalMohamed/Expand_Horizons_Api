# Authentication API

## Overview

Authentication API is responsible for user authentication and authorization for the Expand horizon website & POS.

It provides secure authentication using **ASP.NET Core Identity** and **JWT Bearer Tokens**.

---

# Features

- ASP.NET Core Identity
- JWT Authentication
- Refresh Token Support 
- User Registration
- User Login
- Change Password
- Role Management
- Email Confirmation 
- Account Lockout
- Claims & Roles
- Entity Framework Core
- SQL Server

---

# Technology Stack

- .NET 8
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core
- POSTGRES
- JWT Bearer Authentication
- Swagger
- Serilog

---



# Requirements

- Visual Studio 2022
- .NET 8 SDK
- SQL Server

---

# Configuration

Update **appsettings.json**

```json
{
  "ConnectionStrings": {
    "AuthConnection": "Server=localhost:5432;Database=horizon_auth_db;User ID=postgres;Password=Berlin2020;"
  },
  "Jwt": {
    "Key": "8F8D1546C2C1A23FECE7FCEE13E542DCA4F4B6613A072DE63B7F7F9C1F13263F"
  },
}
```

---


# Entity Framework Migrations

## Create Migration

```
1) create dbContext extend from IdentityDbContext
2)Update Startp file
   services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
            })
             .AddEntityFrameworkStores<AuthenticationDBContext>()
             .AddDefaultTokenProviders();

3) run in command

  Add-Migration AddingIdentity
4) run command => Update-Database
```

---



# Identity Tables

The following tables are created automatically by ASP.NET Core Identity:

- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetRoleClaims
- AspNetUserClaims
- AspNetUserLogins
- AspNetUserTokens

---

# Running the Project

Run using Visual Studio

or

```bash
dotnet run
```

---

# Swagger


Swagger provides complete API documentation.

---

# Authentication

The API uses **JWT Bearer Authentication**.

Add the token to the Authorization header.

Example

```
Authorization: Bearer {access_token}
```

---


# Roles

Example Roles

- Admin
- USER
- POS


---

# Default Admin User

Example

```
Email:
Admin@horizon.de

Password:
123456@Admin
```

---

# Password Policy

Example

- Minimum Length: 8
- Uppercase Required
- Lowercase Required
- Special Character Required



---

# Authorization

Use the Authorize attribute.

Example

```csharp
[Authorize]
```

Role-based authorization

```csharp
[Authorize(Roles = "Admin")]
```



---


---

# Logging

Serilog is used for application logging.

Log location

```
Logs/
```

---

# Deployment

Publish

```bash
dotnet publish -c Release
```

Deploy to

- IIS
- Windows Server

---


# Common Issues

## Invalid JWT Token

- Verify JWT Secret Key
- Verify Token Expiration
- Verify Issuer
- Verify Audience

---



## 401 Unauthorized

Verify

- Authorization header
- JWT Token
- User Roles
- Token Expiration

---
