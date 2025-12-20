using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Application.Queries.Posts;

public record GetPostsByTagIdQuery(int TagId) : IRequest<ServiceResult<IEnumerable<PostDisplayDto>>>;

public class GetPostsByTagIdQueryHandler : IRequestHandler<GetPostsByTagIdQuery, ServiceResult<IEnumerable<PostDisplayDto>>>
{
    private readonly ITagRepository _tagRepository;
    private readonly IRepository<Domain.Entities.PostTags> _postTagRepository;
    private readonly IMapper _mapper;

    public GetPostsByTagIdQueryHandler(
        ITagRepository tagRepository,
        IRepository<Domain.Entities.PostTags> postTagRepository,
        IMapper mapper)
    {
        _tagRepository = tagRepository;
        _postTagRepository = postTagRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> Handle(GetPostsByTagIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetById(request.TagId);
        if (tag is null)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "NotFound Tag.");

        var postTags = _postTagRepository.GetAll();
        var posts = postTags.Include(o => o.Post).Where(t => t.TagId == request.TagId).Select(x => x.Post);
        return ServiceResult.Success(_mapper.Map<IEnumerable<PostDisplayDto>>(posts));
    }
}
