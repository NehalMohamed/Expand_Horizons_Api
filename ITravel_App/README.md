# ITravel APP

## Description

API is the backend service for the Expand Horizons Website & POS App

It provides REST APIs for:

- Booking Management
- Trip Management
- Destination Management
- POS Integration
- Currency Exchange Rates
- Authentication
- images Uploads
- Admin Management
---

## Technology

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- POSTGres Databse
- JWT Authentication
- Serilog
- Swagger
- QuestPDF

---

## Project Structure

Controllers/
Services/
Models/
Resources/
wwwroot/
Views/

---

## Requirements

- Windows 10/11
- .NET 8 Runtime (if not self-contained)

---

## Configuration

Update **appsettings.json**

```json
"ConnectionStrings": {
    "DBConnection": "Server=localhost:5432;Database=horizon_client_db;User ID=postgres;Password=Berlin2020;"
  },
  "Jwt": {
    "Key": "8F8D1546C2C1A23FECE7FCEE13E542DCA4F4B6613A072DE63B7F7F9C1F13263F"
  },
  "Urls": {
    "BaseUrl": "https://expand-horizons.de/api/"
  },
  "MailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "DefaultCredentials": false,
    "Name": "expandredsea@gmail.com",
    "EmailId": "expandredsea@gmail.com",
    "UserName": "expandredsea@gmail.com",
    "Password": "iofe zxmd redt gdzt",
    "UseSSL": true
  }
  }
  ```
---

## Swagger

Swagger UI

```
https://expand-horizons.de/api/swagger/index.html

```

All endpoint documentation is available through Swagger.

---

## Authentication

The API uses JWT Bearer Authentication.

Authorization Header

```
Authorization: Bearer {token}

```

---


## Logging

Logs are generated using Serilog.

Default location

```
Logs/
```

---

## Deployment

Publish

```bash
dotnet publish -c Release
```

Deploy to

- IIS
- Windows Server

---

