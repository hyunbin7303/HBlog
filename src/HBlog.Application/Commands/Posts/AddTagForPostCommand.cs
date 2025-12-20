using HBlog.Contract.Common;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using MediatR;

namespace HBlog.Application.Commands.Posts;

public record AddTagForPostCommand(int PostId, int[] TagIds) : IRequest<ServiceResult>;

public class AddTagForPostCommandHandler : IRequestHandler<AddTagForPostCommand, ServiceResult>
{
    private readonly IPostRepository _postRepository;
    private readonly ITagRepository _tagRepository;

    public AddTagForPostCommandHandler(
        IPostRepository postRepository,
        ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }

    public async Task<ServiceResult> Handle(AddTagForPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetById(request.PostId);
        if (post is null)
            return ServiceResult.NotFound(msg: "Cannot find post.");

        foreach (var tagId in request.TagIds)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag is null)
                return ServiceResult.NotFound(msg: $"Cannot find tag with ID {tagId}.");

            post.AddTag(tag);
        }

        await _postRepository.UpdateAsync(post);
        return ServiceResult.Success($"Successfully added tags to post.");
    }
}
