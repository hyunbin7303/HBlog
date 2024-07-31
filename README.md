# HBlog
This project is for building my own blog using Asp.net Web API and Blazor WebAssembly.

## Technology Stacks
* ASP.NET Web API .NET8
* EFCore
* Blazor WebAssembly
* Bootstrap
* Postgres


# Technology implementations
## Frontend(Blazor WASM) Side
* HTTP Client service layer to interact with Web API.
* Authentication using JWT Token.
* Authorization

## Backend(ASP.NET Web API) Side
* SOLID Principle
* Service layer  -> maybe will be replaced to CQRS.
* Repository Pattern for persisting data
* Authentication via JWT
* DDD (InProgress)
* Pagination for retrieving data.
* Infrastructure layer(Extensions, Helpers, Data migrations/seed)
* AutoMapper for Domain-DTO mapping.
* Unit Testing / Integration testing (InProgress)

## Contribution

If you have any suggestions for how HP could be improved, feel free to create a issue and do some works for me!
For more, checkout the [Contributing guidelines](https://github.com/hyunbin7303/HBlog/blob/main/.github/CONTRIBUTING.md).

