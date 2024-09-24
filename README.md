# HBlog
This project is for building my own blog using Asp.net Web API and Blazor WebAssembly.

## Technology Stacks
* ASP.NET Web API .NET8
* EFCore
* Blazor WebAssembly
* Bootstrap
* Postgres


# Technology implementations
## Frontend(Blaor WASM) Side
* HTTP Client service layer to interact with Web API.
* Authentication using JWT Token.
* Authorization

## Backend(ASP.NET Web API) Side
* SOLID Principle
* A RESTful API design
* Service layer for business logic
* Repository Pattern for persisting data
* Authentication via JWT
* Pagination for handling large data
* Infrastructure layer(Extensions, Helpers, Data migrations/seed)
* AutoMapper for Domain-DTO mapping.
* Unit Testing / Integration testing (InProgress)

## Getting started
1. Clone the git repository
2. Turn on Docker desktop
3. Within the Hblog folder, create containers for Postgres and Web API. `docker compose up -d`
4. Access to `http://localhost:8090/swagger/index.html` to check swagger page(Web API).
5. Get authentication token using `/account/login` endpoint. Json Body : `{ "username" : "testuser", "password" : "Testing#1234!"}`
6. In Visual studio, change the startup project to `HBlog.WebClient`
7. Run the project.


## Contribution

If you have any suggestions for how HP could be improved, feel free to create a issue and do some works for me!
For more, checkout the [Contributing guidelines](https://github.com/hyunbin7303/HBlog/blob/main/.github/CONTRIBUTING.md).

