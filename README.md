# 🛠️ Customer Support Ticketing System

A full-stack web application serving as a basic support ticketing system. The project showcases **clean architecture**, proper use of **design patterns**, and **best practices** in backend and frontend development.

## 📋 Features

### 🎫 Ticketing Dashboard

- **Ticket Creation**: Create support tickets with subject and description.
- **Ticket List View**: View unresolved tickets with subject, username, user ID, and avatar.
- **Ticket Detail View**: View full ticket content and chronological reply thread.
- **Reply to Ticket**: Support agents can add replies to tickets.
- **Status Flow**:
  - Starts as **Open**.
  - Becomes **In Resolution** when an agent replies.
  - Manually marked as **Resolved** by the agent.

## 🧱 Architecture

The project follows **Clean Architecture** principles:

```
/TicketingSystem
│
├── Domain           // Business logic and entities
├── Application      // Interfaces, DTOs, services, orchestration
├── Infrastructure   // Persistence (EF Core + In-Memory database), Repositories
├── Api              // FastEndpoints REST API with Swagger
├── SharedKernel     // Reusable generic types and utilities shared across layers
├── Client           // Frontend (Angular 20)
└── Tests            // Unit tests for domain and application, Integration test
```

#### Backend Notes

- `internal` constructors are used in domain entities for clean **EF Core** mapping.
- **CQRS** is not implemented to keep complexity low but can be introduced later thanks to the clean layering.
- Uses a **Result pattern** for explicit handling of success/failure outcomes in service methods, improving error management and reducing exceptions.
- RESTful error handling follows the `ProblemDetails` standard.
- Manual mapping is used instead of **AutoMapper** to maintain full control and avoid extra licenses.
- **SharedKernel** holds reusable generics/utilities across layers, a small but practical deviation from pure Clean Architecture.

#### Frontend Notes

- Built with **Angular 20**, leveraging its latest features to handle state via injected services rather than relying heavily on complex RxJS patterns. This keeps state management simple and maintainable.

- Applies the **Smart and Dumb Components** pattern to clearly separate presentational components from those handling logic. This balance fosters reusability without adding development overhead.

- Uses **Angular Material** to quickly build a consistent and accessible UI, ideal for rapid prototyping within this assessment.

## 🤔 Assumptions & Scalability

- Authentication and authorization are out of scope and usually handled by an external identity provider.
- The system's current architecture supports scaling with the existing feature set and would be production-ready with added **monitoring**, **logging**, and a robust database.
- For more complex features such as advanced queries, filtering, or real-time notifications, consider adopting **CQRS** and an event-driven design.
- The design balances simplicity and maintainability by adhering to clear separation of concerns and domain-driven principles.

## 🧪 Tests

- Unit tests cover core business logic
- Integration tests cover a basic flow for the assessment
- Use `dotnet test` to run the test suite.

## 🚀 Technologies

### Backend

- **.NET 8**
- **FastEndpoints** (REST with ProblemDetails)
- **EF Core** with In-Memory Database
- **xUnit**
- Built-in Dependency Injection
- Clean Architecture

### Frontend

- **Angular 20**
- Material
- SCSS

## 🔧 Setup Instructions

### 1. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js + NPM](https://nodejs.org/)

### 2. Clone the Repository

```bash
git clone https://github.com/LimiteRoche/ticketing-system.git
cd ticketing-system
```

### 3. Run the Backend API

```bash
cd src/TicketingSystem.Api
dotnet run
```

The backend API will be available at `https://localhost:7028`.

### 4. Run the Frontend App

```bash
cd src/TicketingSystem.Client
npm install
npm run start
```

Frontend runs at `http://localhost:4200`.

### 5. Run the Tests

```bash
dotnet test
```

## 🔐 Security Notes

- No authentication/authorization implemented.
- In a real-world scenario, this should be handled via a reverse proxy (as YARP).

## 🧠 Future Improvements

- Implement refined exception handling for the API by carefully selecting which **exceptions** are **exposed** to the end user.
- Add **CQRS** and command/query separation.
- **Logging** and monitoring
- Introduce authentication.
- Replace In-Memory Database with a production-ready database.
- Implement tests for feature components in frontend using jest

---

## 🤝 Contributions

This project is for technical assessment purposes, but feel free to fork or open a PR with improvements =:D
