using AutoMapper;
using sports_reservation_system.Business.DTOs.BranchDtos;
using sports_reservation_system.Business.DTOs.UserDtos;
using sports_reservation_system.Business.DTOs.SessionDtos;
using sports_reservation_system.Business.DTOs.ReservationDtos;
using sports_reservation_system.Data.Entities;

namespace sports_reservation_system.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ========== BRANCH MAPPINGS ==========
        CreateMap<Branch, BranchDto>().ReverseMap();
        CreateMap<CreateBranchDto, Branch>();
        CreateMap<UpdateBranchDto, Branch>();

        // ========== USER MAPPINGS ==========
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password hash'lenecek, mapping'de değil
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Role)));

        // ========== SESSION MAPPINGS ==========
        CreateMap<Session, SessionDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));
        CreateMap<CreateSessionDto, Session>();
        CreateMap<UpdateSessionDto, Session>();

        // ========== RESERVATION MAPPINGS ==========
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.SessionStartTime, opt => opt.MapFrom(src => src.Session.StartTime))
            .ForMember(dest => dest.SessionDurationMinutes, opt => opt.MapFrom(src => src.Session.DurationMinutes))
            .ForMember(dest => dest.SessionPrice, opt => opt.MapFrom(src => src.Session.Price))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Session.Branch.Name));
        CreateMap<CreateReservationDto, Reservation>();
        CreateMap<UpdateReservationDto, Reservation>();
    }
}