# HappYness Blog / KevBlog
This project is for demo single page application using angular and asp.net, but eventually I am going to use it for blog posting.

## Technology Stacks
* ASP.NET Web API .NET7
* Angular 15
* SQL server

# Getting Started
1. Install .NET and Angular setup globally. `npm install -g @angular/cli`
2. Run web api from src/KevBlog.API, `dotnet run`
3. Run client side from src/client, `ng serve`

# Functionality Overview
* User can see post posts without login.
* User is able to create an account.
* User can create a post and update.
* User can link their blog post from external site.

# Technology implementations
## Frontend(Angular) Side
* Service layers and Dependency injection
* Interceptors for JWT token handling, error handling and loading screen.
* Gurads for Authentication process.
* Separating Forms(e.g. Date Pickers and text inputs)
* Pagination implementation 

## Backend(ASP.NET Web API) Side
* SOLID Principle
* CRUD 
* Authentication via JWT
* DDD (InProgress)
* Repository Pattern for persisting data.
* Pagination for retrieving data.
* Infrastructure layer(Extensions, Helpers, Data migrations/seed)
* AutoMapper for Domain-DTO mapping.
* Unit Testing / Integration testing (InProgress)

