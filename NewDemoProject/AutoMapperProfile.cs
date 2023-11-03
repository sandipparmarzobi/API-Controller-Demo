using AutoMapper;
using DomainLayer.Entities;
using NewDemoProject.Model;
using System;

namespace API_Controller_Demo
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieModel, Movie>(); // Map PersonDto to Person
        }
    }
}
