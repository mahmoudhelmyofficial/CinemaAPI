﻿using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }
    }
}
