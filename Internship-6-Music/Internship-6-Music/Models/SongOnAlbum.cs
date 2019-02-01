namespace Internship_6_Music.Models
{
    public class SongOnAlbum
    {
        public int SongId { get; set; }
        public Song Song { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
