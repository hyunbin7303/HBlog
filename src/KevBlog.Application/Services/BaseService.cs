using AutoMapper;

namespace KevBlog.Application.Services
{
    public abstract class BaseService
    {
        protected IMapper _mapper;
        public BaseService(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
