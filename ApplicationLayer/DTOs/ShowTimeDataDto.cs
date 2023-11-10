using DomainLayer.Entities;
using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs
{
    public class ShowTimeDataDto
    {
        public List<MovieDataDto> MovieList { get; set; }
        public List<TheaterDataDto> TheaterList { get; set; }
    }

    public class MovieDataDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ImageBase64 { get; set; }
    }

    public class TheaterDataDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}