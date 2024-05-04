using Application.Core.Entities;
using Application.Services.Services.CreatePost;
using Application.Services.Services.GetPostFeed;
using Application.Services.Services.Signup;
using AutoMapper;

namespace Application.Services.Common;

public class AutoMapperInitializer : Profile
{
    public AutoMapperInitializer()
    {
        CreateMap<SignupCommand, ApplicationUser>()
            .ForMember(dest => dest.UserName, src => src.MapFrom(y => y.Email))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<CreatePostCommand, Post>();

        CreateMap<PostFeedView, PostFeedResponse>();
    }
}
