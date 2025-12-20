using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Repositories;
using MediatR;

namespace HBlog.Application.Queries.Posts;

public record GetPostsByTagSlugQuery(string TagSlug) : IRequest<ServiceResult<IEnumerable<PostDisplayDto>>>;

public class GetPostsByTagSlugQueryHandler : IRequestHandler<GetPostsByTagSlugQuery, ServiceResult<IEnumerable<PostDisplayDto>>>
{
    private readonly ITagRepository _tagRepository;
    private readonly IRepository<Domain.Entities.PostTags> _postTagRepository;
    private readonly IMapper _mapper;

    public GetPostsByTagSlugQueryHandler(
        ITagRepository tagRepository,
        IRepository<Domain.Entities.PostTags> postTagRepository,
        IMapper mapper)
    {
        _tagRepository = tagRepository;
        _postTagRepository = postTagRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> Handle(GetPostsByTagSlugQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.FindbySlug(request.TagSlug);
        if (tags is null)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "Tag does not exist.");
        
        var tagPosts = await _postTagRepository.GetAll(o => o.TagId == tags.Id);
        var posts = tagPosts.Select(o => o.Post);
        var result = _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
        return ServiceResult.Success(result);
    }
}
