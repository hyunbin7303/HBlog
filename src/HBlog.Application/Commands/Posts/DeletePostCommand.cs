using HBlog.Contract.Common;
using HBlog.Domain.Repositories;
using MediatR;

namespace HBlog.Application.Commands.Posts;

public record DeletePostCommand(int Id) : IRequest<ServiceResult>;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ServiceResult>
{
    private readonly IPostRepository _postRepository;

    public DeletePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<ServiceResult> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetById(request.Id);
        if (post is null)
            return ServiceResult.Fail(msg: "NotFound");

        _postRepository.Remove(request.Id);
        await _postRepository.SaveChangesAsync();
        return ServiceResult.Success(msg: $"Removed Post Id: {request.Id}");
    }
}
