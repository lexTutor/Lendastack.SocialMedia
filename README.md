# Lendastack.SocialMedia

## Overview

This project is built using .NET 6 and follows a clean architecture pattern. It leverages various technologies and methodologies to ensure robustness, security, and performance.

## Highlights

- **Clean Architecture:** Built using a clean architecture pattern for enhanced maintainability and scalability.
- **Security Middleware:** Implements robust security measures with a dedicated middleware for authentication and authorization.
- **Global Exception Handler Middleware:** Centralizes error handling with a global exception handler middleware for seamless error management.
- **EF Core and SQL Server:** Utilizes Entity Framework Core and SQL Server for efficient data storage and retrieval.
- **Fluent Validation:** Integrates Fluent Validation for streamlined and composable input validation rules.
- **MediatR:** Facilitates decoupled communication between components through the Mediator pattern with Mediatr integration.
- **Automapper:** Simplifies object-to-object mapping with Automapper, reducing manual mapping code and enhancing code readability.
- **Views for Quicker Queries:** Optimizes query performance by utilizing database views for faster data retrieval.
- **Serilog for Logging:** Implements Serilog for comprehensive logging capabilities, with logs stored in files for easy tracking and analysis.
- **Dummy Data Seeding:** Seeds dummy data for quick testing and development, including sample user credentials for ease of use. Dummy data includes the following login information:
  - Email: john@lendastack.io, Password: password
  - Email: jane@lendastack.io, Password: password
- **Unit Tests:** Includes a suite of unit tests to ensure code correctness and reliability.
- **Performance Middleware:** Monitors and alerts on slow-running executions with a performance behavior middleware for improved performance monitoring.

## Getting Started

1. Clone the repository.
2. Set up the database and update the connection string in the appsettings.json file.
3. Build and run the application.

## Requirement Scope

### Take Home Assessment

**What to Build:**

- A mini social media feed that allows users to see posts from people they are following.
- No frontend UI is required.

**User Stories (User is an API consumer)**

- As a user, I want to be able to create posts with text.
- As a user, I want to be able to follow another user.
- As a user, I want to be able to get a post feed containing recent posts from me and people I am following.

**Functional Requirements:**

- Restful Web API (JSON)
- Maximum text character limit = 140
- Posts should be sorted by likes in descending order.
- Posts should be efficiently paginated.

**Non-Functional Requirements:**

- Maximum response time for any API call should be 100ms.
- Minimum throughput handled by the system should be 50 RPS.

**Usage Forecast:**

- 50k posts per hour
