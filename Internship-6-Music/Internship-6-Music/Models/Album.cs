using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_6_Music.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string ReleaseYear { get; set; }
        public int ArtistId { get; set; }
    }
}
