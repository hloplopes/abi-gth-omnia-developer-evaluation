# Developer Evaluation Project

`READ CAREFULLY`

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories.

See [Project Structure](/.doc/project-structure.md)

---

## How to Run

### Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop/) installed and running
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed
- [Entity Framework Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (optional, for migrations)

### 1. Start the Database (PostgreSQL)

Open a terminal and run:

```bash
docker run -d --name pg_ambev \
  -e POSTGRES_DB=developer_evaluation \
  -e POSTGRES_USER=developer \
  -e "POSTGRES_PASSWORD=ev@luAt10n" \
  -p 5432:5432 \
  postgres:13
```

This will download the PostgreSQL 13 image (if not already present) and start a container with the database ready to use.

### 2. Apply Database Migrations

Navigate to the backend folder and run the migrations:

```bash
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi

dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM
```

This creates the `Users`, `Sales`, and `SaleItems` tables in the database.

> **Note:** If you don't have `dotnet-ef` installed, run:
> ```bash
> dotnet tool install --global dotnet-ef --ignore-failed-sources
> ```

### 3. Run the API

From the WebApi folder:

```bash
dotnet run
```

The API will start at: **http://localhost:5119**

### 4. Access Swagger

Open your browser and go to:

**http://localhost:5119/swagger**

---

## How to Test

### Step 1: Create a User

**POST** `/api/Users`

```json
{
  "username": "admin",
  "password": "Admin@123",
  "phone": "+5511999999999",
  "email": "admin@teste.com",
  "status": 1,
  "role": 1
}
```

### Step 2: Authenticate

**POST** `/api/Auth`

```json
{
  "email": "admin@teste.com",
  "password": "Admin@123"
}
```

Copy the `token` from the response.

### Step 3: Authorize in Swagger

Click the **"Authorize"** button (padlock icon at the top) and enter:

```
Bearer <your_token_here>
```

### Step 4: Create a Sale

**POST** `/api/Sales`

```json
{
  "saleNumber": "SALE-001",
  "saleDate": "2026-01-28T12:00:00Z",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "John Doe",
  "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "branchName": "Main Branch",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "productName": "Brahma Beer 350ml",
      "quantity": 5,
      "unitPrice": 4.50
    },
    {
      "productId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
      "productName": "Skol Beer 350ml",
      "quantity": 10,
      "unitPrice": 4.00
    }
  ]
}
```

### Expected Discounts

| Quantity | Discount |
|----------|----------|
| 1-3 | 0% |
| 4-9 | 10% |
| 10-20 | 20% |
| > 20 | Not allowed |

In the example above:
- Item with qty 5 gets 10% discount
- Item with qty 10 gets 20% discount

---

## Running Unit Tests

```bash
cd template/backend
dotnet test
```

---

## Stopping the Database

To stop and remove the PostgreSQL container:

```bash
docker stop pg_ambev
docker rm pg_ambev
```