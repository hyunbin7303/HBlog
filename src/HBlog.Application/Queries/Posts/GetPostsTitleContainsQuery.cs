using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Repositories;
using MediatR;

namespace HBlog.Application.Queries.Posts;

public record GetPostsTitleContainsQuery(string Title) : IRequest<ServiceResult<IEnumerable<PostDisplayDto>>>;

public class GetPostsTitleContainsQueryHandler : IRequestHandler<GetPostsTitleContainsQuery, ServiceResult<IEnumerable<PostDisplayDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostsTitleContainsQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> Handle(GetPostsTitleContainsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetPostsTitleContainsAsync(request.Title.ToLower());
        return ServiceResult.Success(_mapper.Map<IEnumerable<PostDisplayDto>>(posts));
    }
}
