using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_6_Music.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
