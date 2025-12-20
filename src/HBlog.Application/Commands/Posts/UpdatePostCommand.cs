using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Constants;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Domain.ValueObjects;
using MediatR;

namespace HBlog.Application.Commands.Posts;

public record UpdatePostCommand(PostUpdateDto UpdateDto) : IRequest<ServiceResult>;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ServiceResult>
{
    private readonly IPostRepository _postRepository;
    private readonly ITagRepository _tagRepository;

    public UpdatePostCommandHandler(IPostRepository postRepository, ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }

    public async Task<ServiceResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        Post post = await _postRepository.GetById(request.UpdateDto.Id);
        if (post == null || post.Status.IsRemoved)
            return ServiceResult.Fail(msg: "Post does not exist.");

        // Use the domain method to update the post
        post.Update(
            title: request.UpdateDto.Title,
            description: request.UpdateDto.Desc ?? string.Empty,
            content: request.UpdateDto.Content ?? string.Empty,
            categoryId: request.UpdateDto.CategoryId
        );

        // Update type if changed
        if (!string.IsNullOrEmpty(request.UpdateDto.Type))
        {
            post.ChangeType(PostType.FromString(request.UpdateDto.Type));
        }
        
        // Add tags if provided
        if (request.UpdateDto.TagIds?.Length > 0)
        {
            foreach (var tagId in request.UpdateDto.TagIds)
            {
                var tag = await _tagRepository.GetById(tagId);
                if (tag is not null)
                {
                    post.AddTag(tag);
                }
            }
        }

        await _postRepository.UpdateAsync(post);
        return ServiceResult.Success();
    }
}
