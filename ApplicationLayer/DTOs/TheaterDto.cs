﻿using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs
{
    public class TheaterDto 
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }
    }
}
