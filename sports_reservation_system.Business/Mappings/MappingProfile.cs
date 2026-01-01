using AutoMapper;
using sports_reservation_system.Business.DTOs.BranchDtos;
using sports_reservation_system.Business.DTOs.SessionDtos;
using sports_reservation_system.Business.DTOs.ReservationDtos;
using sports_reservation_system.Business.DTOs.UserDtos;
using sports_reservation_system.Business.DTOs.AuthDtos;
using sports_reservation_system.Business.DTOs.SportDtos;
using sports_reservation_system.Data.Entities;

namespace sports_reservation_system.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // --- Branch Mappings ---
        CreateMap<Branch, BranchDto>().ReverseMap();
        CreateMap<CreateBranchDto, Branch>();
        CreateMap<UpdateBranchDto, Branch>();

        // --- Sport Mappings ---
        CreateMap<Sport, SportDto>().ReverseMap();
        CreateMap<CreateSportDto, Sport>();
        CreateMap<UpdateSportDto, Sport>();

        // --- Session Mappings ---
        CreateMap<Session, SessionDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.SportName, opt => opt.MapFrom(src => src.Sport.Name));
        CreateMap<CreateSessionDto, Session>();
        CreateMap<UpdateSessionDto, Session>();

        // --- Reservation Mappings ---
        CreateMap<Reservation, ReservationDto>().ReverseMap();
        CreateMap<CreateReservationDto, Reservation>();
        CreateMap<UpdateReservationDto, Reservation>();

        // --- User Mappings ---
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();

        // --- Auth (User) Mappings ---
        CreateMap<RegisterDto, User>();
    }
}