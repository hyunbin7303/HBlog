using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Repositories;
using MediatR;

namespace HBlog.Application.Queries.Posts;

public record GetPostsByUsernameQuery(string UserName) : IRequest<ServiceResult<IEnumerable<PostDisplayDto>>>;

public class GetPostsByUsernameQueryHandler : IRequestHandler<GetPostsByUsernameQuery, ServiceResult<IEnumerable<PostDisplayDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostsByUsernameQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> Handle(GetPostsByUsernameQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetPostsByUserName(request.UserName);
        return ServiceResult.Success(_mapper.Map<IEnumerable<PostDisplayDto>>(posts));
    }
}
