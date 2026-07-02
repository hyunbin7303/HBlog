# HBlog
This project is for building my own blog using ASP.NET Web API and Blazor WebAssembly.

## Technology Stacks
* ASP.NET Web API on .NET 10
* EF Core
* Blazor WebAssembly
* Bootstrap
* PostgreSQL
* .NET Aspire (`HBlog.AppHost`) for local orchestration


# Technology implementations
## Frontend (Blazor WASM) Side
* HTTP Client service layer to interact with Web API.
* Authentication using JWT Token.
* Authorization

## Backend (ASP.NET Web API) Side
* SOLID Principle
* A RESTful API design
* Service layer for business logic
* Repository Pattern for persisting data
* CQRS (see [CQRS_IMPLEMENTATION.md](src/CQRS_IMPLEMENTATION.md))
* Authentication via JWT with refresh tokens
* External sign-in via OAuth (Google, Apple) with a short-lived ticket flow for first-time profile completion
* Pagination for handling large data
* Infrastructure layer (Extensions, Helpers, Data migrations/seed)
* AutoMapper for Domain–DTO mapping
* Global exception handling via `GlobalExceptionHandler` + RFC 7807 ProblemDetails
* Request correlation via `CorrelationMiddleware`
* OpenAPI documentation via both Swagger UI and [Scalar](https://scalar.com/)
* Unit tests and Integration tests

## Getting started
1. Clone the git repository.
2. Turn on Docker Desktop.
3. From the `src/` folder, start the Postgres and Web API containers: `docker compose up -d`.
4. Open the API docs:
   * Swagger UI: `http://localhost:8090/swagger/index.html`
   * Scalar: `http://localhost:8090/scalar/v1`
5. Get an authentication token by calling the `/account/login` endpoint with your seeded test user credentials.
6. In Visual Studio, change the startup project to `HBlog.WebClient`.
7. Run the project.

### OAuth configuration
The `/account/oauth/{provider}` endpoints validate ID tokens against a configured audience list. Set these before running the OAuth flows:

* `GoogleAuth:AllowedAudiences` — Google OAuth client IDs (one per platform).
* `AppleAuth:AllowedAudiences` — Apple client IDs.
* `OAuthTicket:SigningKey` — signing key for the short-lived ticket returned when a first-time OAuth user needs to complete their profile. In hosted environments this can be supplied via the `OAUTH_TICKET_SIGNING_KEY` environment variable.


## Contribution

If you have any suggestions for how HP could be improved, feel free to create a issue and do some works for me!
For more, checkout the [Contributing guidelines](https://github.com/hyunbin7303/HBlog/blob/main/.github/CONTRIBUTING.md).

