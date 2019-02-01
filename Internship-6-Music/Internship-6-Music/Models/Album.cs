using System.Collections.Generic;
using System.Security.AccessControl;

namespace Internship_6_Music.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string ReleaseYear { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public ICollection<SongOnAlbum> SongsAlbums { get; set; }
    }
}
