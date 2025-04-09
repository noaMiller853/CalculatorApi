# 📐 Calculator API
A fully documented .NET 8 REST API for performing arithmetic operations based on two numeric values and an HTTP header operation type. Built using OpenAPI (YAML) via SwaggerHub, secured with JWT Bearer authentication, and containerized using Docker.

---

## 🧩 Project Requirements & Status
| Requirement                                                                | Status |
|----------------------------------------------------------------------------|--------|
| Define API using SwaggerHub + YAML                                         | ✅     |
| Generate server stub using Swagger Codegen                                 | ✅     |
| Use JWT (Bearer) Authorization                                             | ✅     |
| Perform math operation based on HTTP header `X-Operation`                  | ✅     |
| Use POST method                                                            | ✅     |
| Include response codes (200, 400, 401, 500)                                | ✅     |
| Include unit tests                                                         | ✅     |
| Document code and architecture using Design Patterns                       | ✅     |
| Dockerize API using Dockerfile & docker-compose.yml                        | ✅     |

---

## 📄 OpenAPI & Swagger

- ✅ Defined with **SwaggerHub** + OpenAPI YAML.
- 🔗 SwaggerHub: [https://app.swaggerhub.com/apis/noamiller-8e1/CalculatorApi1/1.0.0]
- 📁 YAML is also included under `Docs/CalculatorApi.yaml`

---

## 🧾 How It Works

### 🔐 Authentication
All API requests require a JWT token in the header:
```http
Authorization: Bearer <token>
```
To generate a token:
```http
POST /api/v1/auth/token
```
**Request Body:**
```json
{
  "username": "admin",
  "password": "1234"
}
```
**Response Body:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}

### ➕ Arithmetic Endpoint
```http
POST /api/v1/calculator/calculate
```
**Required Header:**
```
X-Operation: add | subtract | multiply | divide
```
**Request Body:**
```json
{
  "firstNumber": 10,
  "secondNumber": 5
}
```
**Response:**
```json
{
  "result": 15,
  "operation": "add",
  "timestamp": "2025-04-09T12:00:00Z"
}
```

---

## 🧠 Technologies Used
- ASP.NET Core 8 Web API
- JWT Bearer Authentication
- Swagger + Swashbuckle
- SwaggerHub & Swagger Codegen
- Newtonsoft for JSON + Enum formatting
- Logging
- Exception Handling Middleware
- Unit Tests (xUnit)
- Docker + Docker Compose

---

## 📐 Design Patterns Used
| Pattern                          | Usage Description |
|----------------------------------|--------------------|
| **Controller Pattern**           | `AuthController`, `CalculatorController`  |
| **Strategy Pattern**             | Arithmetic operations (Add, Subtract...) via `IOperation` interface |
| **Factory Method Pattern**       | `OperationFactory` creates appropriate `IOperation` strategy |
| **Service Layer Pattern**        | Business logic encapsulated in `CalculatorService` |
| **Constructor Injection**        | Injecting services and configurations |
| **POCO + Immutable Pattern**     | `CalculationRequest`, `CalculationResult` models |
| **Exception Handling Middleware**| Global error handling layer |
| **JWT Handling**                 | Secure API calls using tokens |

All patterns are documented in code in **both English and Hebrew** as XML comments.

---

## 🚀 Running Locally
### Prerequisites:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Postman](https://www.postman.com/) (recommended for testing)

### Step-by-Step
#### 🔧 Environment
Update `appsettings.json` or use UserSecrets to define:
```json
"JwtSettings": {
  "Secret": "your_secret_key",
  "Issuer": "CalculatorApi",
  "Audience": "CalculatorApiUsers",
  "ExpirationMinutes": 60
}
```

#### ▶️ Run with .NET CLI:
```bash
dotnet run
```
Browse Swagger UI:
```bash
https://localhost:<port>/swagger
```

#### 🐳 Run with Docker:
```bash
cd path/to/CalculatorApi/CalculatorApi
docker build -t calculator-api .
docker run -p 5000:80 calculator-api
```

#### 🐳 Or with Docker Compose:
```bash
cd path/to/CalculatorApi/CalculatorApi
docker-compose up --build
```

---

## 🧪 Testing
- Unit tests for arithmetic logic using **xUnit**.
- Tests for authorization, invalid input, and edge cases.

---

