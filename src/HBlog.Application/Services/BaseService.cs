using AutoMapper;

namespace HBlog.Application.Services
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
