using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Domain.ValueObjects;
using MediatR;

namespace HBlog.Application.Queries.Posts;

public record GetPostByIdQuery(int Id) : IRequest<ServiceResult<PostDisplayDetailsDto>>;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ServiceResult<PostDisplayDetailsDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetPostByIdQueryHandler(
        IPostRepository postRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PostDisplayDetailsDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        Post post = await _postRepository.GetPostDetails(request.Id);
        if (post is null || post.Status.IsRemoved)
            return ServiceResult.Fail<PostDisplayDetailsDto>(msg: "Post is not exist or status is removed.");

        var postDisplay = _mapper.Map<PostDisplayDetailsDto>(post);
        User user = await _userRepository.GetUserByIdAsync(post.UserId);
        postDisplay.UserName = user?.UserName ?? "Unknown";
      
        return ServiceResult.Success(postDisplay);
    }
}
