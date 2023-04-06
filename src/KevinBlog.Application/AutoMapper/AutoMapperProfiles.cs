using AutoMapper;
using KevBlog.Domain.Common.Extensions;
using KevBlog.Domain.Entities;
using KevBlog.Application.DTOs;

namespace KevBlog.Application.Automapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x=> x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<Post, PostDisplayDto>();
            CreateMap<MemberUpdateDto, User>();
        }
    }
}