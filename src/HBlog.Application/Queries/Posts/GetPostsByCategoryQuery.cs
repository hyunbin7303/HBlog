using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Repositories;
using MediatR;

namespace HBlog.Application.Queries.Posts;

public record GetPostsByCategoryQuery(int CategoryId) : IRequest<ServiceResult<IEnumerable<PostDisplayDto>>>;

public class GetPostsByCategoryQueryHandler : IRequestHandler<GetPostsByCategoryQuery, ServiceResult<IEnumerable<PostDisplayDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetPostsByCategoryQueryHandler(
        IPostRepository postRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _postRepository = postRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> Handle(GetPostsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetById(request.CategoryId);
        if (category is null)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "NotFound Category.");

        var posts = await _postRepository.GetAll(o => o.CategoryId == request.CategoryId);
        if (posts.Count() == 0)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "NotFound Posts.");

        return ServiceResult.Success(_mapper.Map<IEnumerable<PostDisplayDto>>(posts));
    }
}
