using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Domain.ValueObjects;
using MediatR;

namespace HBlog.Application.Commands.Posts;

public record UpdatePostStatusCommand(int Id, PostChangeStatusDto UpdateStatusDto) : IRequest<ServiceResult>;

public class UpdatePostStatusCommandHandler : IRequestHandler<UpdatePostStatusCommand, ServiceResult>
{
    private readonly IPostRepository _postRepository;

    public UpdatePostStatusCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<ServiceResult> Handle(UpdatePostStatusCommand request, CancellationToken cancellationToken)
    {
        Post post = await _postRepository.GetById(request.Id);
        if (post == null || post.Status.IsRemoved)
            return ServiceResult.Fail(msg: "Post does not exist.");

        // Use domain methods to change status
        var newStatus = PostStatus.FromString(request.UpdateStatusDto.Status);
        
        if (newStatus.Equals(PostStatus.Published))
            post.Publish();
        else if (newStatus.Equals(PostStatus.Active))
            post.Activate();
        else if (newStatus.Equals(PostStatus.Removed))
            post.Archive();

        await _postRepository.UpdateAsync(post);
        return ServiceResult.Success();
    }
}
