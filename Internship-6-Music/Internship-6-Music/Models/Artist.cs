﻿using System.Collections.Generic;

namespace Internship_6_Music.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
