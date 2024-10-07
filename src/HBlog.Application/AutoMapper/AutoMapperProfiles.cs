using AutoMapper;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common.Extensions;
using HBlog.Domain.Entities;

namespace HBlog.Application.Automapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(s => s.Id));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>();
            CreateMap<Post, PostDisplayDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags.Select(x => x.Tag)));
            CreateMap<Post, PostDisplayDetailsDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags.Select(x => x.Tag)));

            CreateMap<UserUpdateDto, User>();
            CreateMap<RegisterDto, User>();
            CreateMap<PostUpdateDto, Post>();
            CreateMap<PostCreateDto, Post>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}