﻿using ApplicationLayer.DTOs;
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
            CreateMap<ShowTimeDto, ShowTime>();
            CreateMap<BookingDto, Booking>();
            CreateMap<ReviewDto, Reviews>();
        }
    }
}