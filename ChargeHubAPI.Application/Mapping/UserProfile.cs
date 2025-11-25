using AutoMapper;
using ChargeHubAPI.Application.Contracts.Responses;
using ChargeHubAPI.Application.Dtos;
using ChargeHubAPI.Domain.Entities;

namespace ChargeHubAPI.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserInfoResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Identecation, opt => opt.MapFrom(src => src.Identecation))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CarCharge, opt => opt.MapFrom(src => src.CarCharge));

        CreateMap<Esp32Device, Esp32Dto>();
        CreateMap<StatusPosition, StatusPositionDto>();
    }
}




