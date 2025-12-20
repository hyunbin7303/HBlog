using AutoMapper;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common.Params;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using MediatR;

namespace HBlog.Application.Queries.Posts;

public record GetPostsQuery(PostParams Query) : IRequest<IEnumerable<PostDisplayDto>>;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<PostDisplayDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostDisplayDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Post> posts = await _postRepository.GetPostsAsync(request.Query.Limit, request.Query.Offset);
        
        if (request.Query.CategoryId != 0)
            posts = posts.Where(p => p.CategoryId == request.Query.CategoryId);

        if (request.Query.TagId.Any())
            posts = posts.Where(p => p.Tags.Any(tag => request.Query.TagId.Contains(tag.Id)));

        return _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
    }
}
