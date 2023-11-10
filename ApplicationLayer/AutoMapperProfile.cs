using ApplicationLayer.DTOs;
using AutoMapper;
using DomainLayer.Entities;

namespace ApplicationLayer
{
    //SP: Custom class with map dto with actual Entity
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieDto, Movie>(); 
            CreateMap<TheaterDto, Theater>();
            CreateMap<ShowTimeDto, ShowTime>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Movie))
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.Theater, opt => opt.Ignore())
                .ForMember(dest => dest.TheaterId, opt => opt.MapFrom(src => src.Theater)).ReverseMap();
            CreateMap<BookingDto, Booking>();
            CreateMap<ReviewDto, Reviews>();

            CreateMap<Movie, MovieDataDto>();
            CreateMap<Theater, TheaterDataDto>();
        }
    }
}
