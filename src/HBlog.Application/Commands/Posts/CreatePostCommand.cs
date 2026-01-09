using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Domain.ValueObjects;
using MediatR;

namespace HBlog.Application.Commands.Posts;

public record CreatePostCommand(string UserName, PostCreateDto CreateDto) : IRequest<ServiceResult>;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ServiceResult>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;

    public CreatePostCommandHandler(
        IPostRepository postRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
    }

    public async Task<ServiceResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.CreateDto.Title))
            return ServiceResult.Fail(msg: "Title cannot be empty.");

        var category = await _categoryRepository.GetById(request.CreateDto.CategoryId);
        if (category is null)
            return ServiceResult.NotFound(msg: "Cannot find category.");

        var user = await _userRepository.GetUserByUsernameAsync(request.UserName);
        if (user is null)
            return ServiceResult.NotFound(msg: "Cannot find user.");

        var postType = string.IsNullOrEmpty(request.CreateDto.Type) 
            ? PostType.Normal 
            : PostType.FromString(request.CreateDto.Type);

        var post = Post.Create(
            title: request.CreateDto.Title,
            description: request.CreateDto.Desc ?? string.Empty,
            content: request.CreateDto.Content ?? string.Empty,
            userId: user.Id,
            categoryId: request.CreateDto.CategoryId,
            type: postType
        );

        post.Activate(); // or post.Publish() if you want it published immediately
        
        // Add tags if provided
        if (request.CreateDto.TagIds.Length > 0)
        {
            foreach (var tagId in request.CreateDto.TagIds)
            {
                var tag = await _tagRepository.GetById(tagId);
                if (tag is not null)
                {
                    post.AddTag(tag);
                }
            }
        }
        
        _postRepository.Add(post);
        await _postRepository.SaveChangesAsync();
        return ServiceResult.Success(msg: $"Post Id:{post.Id}");
    }
}
