using System;

namespace Internship_6_Music.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
