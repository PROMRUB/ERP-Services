using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.User;
using ERP.Services.API.Models.ResponseModels.User;

namespace ERP.Services.API.Configurations
{
    public class UserConfigurations : Profile
    {
        public UserConfigurations()
        {
            CreateMap<UserRequest, UserEntity>()
                .ForMember(dest => dest.UserCreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UserEntity, UserResponse>();

            CreateMap<AddUserToBusinessRequest, UserBusinessEntity>()
                .ForMember(dest => dest.UserBusinessId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Roles));
        }
    }
}
