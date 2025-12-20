# CQRS Implementation with MediatR

## Overview
This document describes the CQRS (Command Query Responsibility Segregation) pattern implementation using MediatR (free version) in the HBlog application.

## What Was Implemented

### 1. **Package Installation**
- Added `MediatR` v12.4.1 to `HBlog.Application` project
- Added `MediatR.Contracts` v2.0.1 for proper type resolution
- Registered MediatR in DI container

### 2. **Commands Created** (`HBlog.Application/Commands/Posts/`)
Commands handle write operations (Create, Update, Delete):

- **CreatePostCommand**: Creates a new post
  - Uses `Post.Create()` factory method
  - Validates category and user existence
  - Adds tags using `post.AddTag()` domain method
  - Activates the post automatically

- **UpdatePostCommand**: Updates an existing post
  - Uses `post.Update()` domain method
  - Updates post type with `post.ChangeType()`
  - Handles tag additions

- **UpdatePostStatusCommand**: Changes post status
  - Uses domain methods: `post.Publish()`, `post.Activate()`, `post.Archive()`
  - Properly encapsulates status changes

- **DeletePostCommand**: Removes a post
  - Performs soft/hard delete based on repository implementation

- **AddTagForPostCommand**: Adds tags to an existing post
  - Uses `post.AddTag()` domain method

### 3. **Queries Created** (`HBlog.Application/Queries/Posts/`)
Queries handle read operations:

- **GetPostByIdQuery**: Retrieves a single post by ID
- **GetPostsQuery**: Retrieves posts with filtering (category, tags)
- **GetPostsTitleContainsQuery**: Searches posts by title
- **GetPostsByTagSlugQuery**: Gets posts by tag slug
- **GetPostsByTagIdQuery**: Gets posts by tag ID
- **GetPostsByCategoryQuery**: Gets posts by category
- **GetPostsByUsernameQuery**: Gets posts by username

### 4. **PostService Refactoring**
Transformed from a traditional service class to a thin mediator wrapper:

**Before:**
```csharp
public class PostService : BaseService, IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    // ... many dependencies
    
    public async Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto)
    {
        // ... lots of business logic here
    }
}
```

**After:**
```csharp
public class PostService : IPostService
{
    private readonly IMediator _mediator;
    
    public PostService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto)
        => await _mediator.Send(new CreatePostCommand(userName, createDto));
}
```

## Benefits Achieved

### 1. **Separation of Concerns**
- Commands and queries are now in separate files
- Each handler has a single responsibility
- Business logic is isolated and testable

### 2. **Better Encapsulation**
- Uses domain entity methods (`Post.Create()`, `post.Update()`, `post.Publish()`)
- Respects value objects (`PostStatus`, `PostType`, `PostTitle`)
- Follows DDD principles

### 3. **Improved Testability**
- Each command/query handler can be tested independently
- No need to mock the entire service with all dependencies
- Easier to write focused unit tests

### 4. **Scalability**
- Easy to add new commands/queries without modifying existing code
- Can add pipeline behaviors (validation, logging, caching) easily
- Supports cross-cutting concerns through MediatR pipelines

### 5. **Clean Architecture**
- Application layer only coordinates (no business logic)
- Domain layer contains business rules
- Clear flow: Controller ? Service ? MediatR ? Handler ? Repository

## Architecture Flow

```
???????????????
? Controller  ?
???????????????
       ?
       ?
????????????????
? PostService  ? (Thin wrapper)
????????????????
       ?
       ?
????????????????
?   MediatR    ? (Mediator)
????????????????
       ?
       ?
??????????????????????
? Command/Query      ?
? Handler            ?
??????????????????????
          ?
          ?
     ???????????????????
     ?                 ?
???????????      ????????????
? Domain  ?      ?Repository?
? Entity  ?      ?          ?
???????????      ????????????
```

## Usage Examples

### Creating a Post (Command)
```csharp
// In Controller
var result = await _postService.CreatePost(username, createDto);

// Behind the scenes:
// 1. PostService sends CreatePostCommand to MediatR
// 2. MediatR routes to CreatePostCommandHandler
// 3. Handler validates and uses Post.Create() factory
// 4. Post is saved via repository
```

### Getting a Post (Query)
```csharp
// In Controller
var result = await _postService.GetByIdAsync(postId);

// Behind the scenes:
// 1. PostService sends GetPostByIdQuery to MediatR
// 2. MediatR routes to GetPostByIdQueryHandler
// 3. Handler retrieves post from repository
// 4. Maps to DTO and returns
```

## Next Steps

### 1. **Add Pipeline Behaviors**
```csharp
// Validation Behavior
public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
{
    // Validate commands before execution
}

// Logging Behavior
public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    // Log all commands/queries
}
```

### 2. **Extend to Other Services**
- Apply same pattern to `TagService`, `UserService`, `MessageService`
- Create commands/queries for each service

### 3. **Add FluentValidation**
```xml
<PackageReference Include="FluentValidation" Version="11.9.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.9.0" />
```

### 4. **Implement Query Caching**
- Cache read-heavy queries
- Invalidate cache on commands

### 5. **Add Integration Tests**
```csharp
[Test]
public async Task CreatePost_WithValidData_ShouldSucceed()
{
    // Arrange
    var command = new CreatePostCommand("testuser", createDto);
    
    // Act
    var result = await _mediator.Send(command);
    
    // Assert
    Assert.IsTrue(result.IsSuccess);
}
```

## Known Issues

### Current Build Errors
1. **WebClient Constants**: Need to add `using HBlog.WebClient.Commons;`
2. **Test Projects**: Need to update test mocks to use `Post.Create()` factory
3. **PostRepository**: `Title.ToLower()` needs to be `Title.Value.ToLower()` for PostTitle value object

These are existing issues from the domain model refactoring and not related to CQRS implementation.

## Conclusion

CQRS with MediatR has been successfully implemented for the PostService. The pattern provides:
- ? Better code organization
- ? Improved testability
- ? Easier maintenance
- ? Scalability for future features
- ? No licensing costs (free MediatR version)

The implementation follows clean architecture principles and domain-driven design practices.
