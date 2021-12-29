using AutoMapper;
using Domain.Core.Identity;
using Domain.Core.Models.News;
using WebAPI.Models.Account;
using WebAPI.Models.News;

namespace WebAPI.MapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<UserEntity, UserViewModel>().ReverseMap();
            CreateMap<CreateUserModel, UserEntity>()
                .ForMember(m=>m.UserName, opt=>opt.MapFrom(m=>m.Email))
                .ForMember(m => m.NormalizedUserName, opt => opt.MapFrom(m => m.Email.ToUpper()));
            CreateMap<NewsEntity, NewsFullDto>();
            CreateMap<CreateNewsItemDto, NewsEntity>();
            CreateMap<NewsEntity, NewsDto>();
            CreateMap<UpdateNewsItemDto, NewsEntity>();
        }
    }
}
