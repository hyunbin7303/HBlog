using AutoMapper;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Common.Extensions;
using KevBlog.Domain.Entities;

namespace KevBlog.Application.Automapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(s => s.Id));

            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x=> x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>();
            CreateMap<Post, PostDisplayDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Post, PostDisplayDetailsDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags.Select(x => x.Tag)));

            CreateMap<MemberUpdateDto, User>();
            CreateMap<RegisterDto, User>();
            CreateMap<PostUpdateDto, Post>();
            CreateMap<PostCreateDto, Post>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x=> x.IsMain).Url));
        }
    }
}