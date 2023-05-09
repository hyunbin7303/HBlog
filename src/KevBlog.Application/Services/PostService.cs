using AutoMapper;

namespace KevBlog.Application.Services
{
    public class PostService : BaseService, IPostService
    {
        public PostService(IMapper mapper) : base(mapper) { }


    }
}
