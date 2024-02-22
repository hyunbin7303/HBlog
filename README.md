# HappYness Blog / KevBlog
This project is for building my own blog using Asp.net Web API and Blazor WebAssembly.

## Technology Stacks
* ASP.NET Web API .NET8
* Blazor Web Assembly 
* Postgres

# Functionality Overview
* User can see post posts without login.
* User is able to create an account.
* User can create a post and update.
* User can link their blog post from external site.

# Technology implementations
## Frontend(Blaor WASM) Side
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
