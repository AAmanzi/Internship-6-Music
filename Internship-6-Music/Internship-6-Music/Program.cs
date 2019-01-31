using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Internship_6_Music.Models;

namespace Internship_6_Music
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding("x-mac-croatian");
            Console.InputEncoding = System.Text.Encoding.GetEncoding("x-mac-croatian");
            LoadDataBase();
        }

        public static void LoadDataBase()
        {
            const string connectionString = "Data Source = (LocalDb)\\MSSQLLocalDB; Initial Catalog = Internship-6-Music; Integrated Security = true; MultipleActiveResultSets = true";
            using (var connection = new SqlConnection(connectionString))
            {
                var allArtists = connection.Query<Artist>("SELECT * FROM Artists").ToList();
                var allSongs = connection.Query<Song>("SELECT * FROM Songs").ToList();
                var allAlbums = connection.Query<Album>("SELECT * FROM Albums").ToList();
                var allSongsOnAlbums = connection.Query<SongOnAlbum>("SELECT * FROM SongsOnAlbums").ToList();

                var artistsByName = allArtists.OrderBy(artist => artist.Name);
                foreach (var artist in artistsByName)
                {
                    Console.WriteLine($"{artist.Name}, {artist.Nationality}");
                }

                Console.WriteLine();
                Console.WriteLine("Input nationality that you want to search artists by:");
                var nationalityForSearch = Console.ReadLine();
                var artistsWithNationality = allArtists.Where(artist => artist.Nationality == nationalityForSearch).OrderBy(artist => artist.Name);
                foreach (var artist in artistsWithNationality)
                {
                    Console.WriteLine($"{artist.Name}, {artist.Nationality}");
                }
                Console.WriteLine();

                Console.WriteLine("All albums with their proprietary artist:");
                Console.WriteLine();
                var albumArtistList =
                    from artist in allArtists
                    join album in allAlbums on artist.ArtistId equals album.ArtistId
                    select new
                    {
                        ArtistName = artist.Name,
                        artist.ArtistId,
                        artist.Nationality,
                        album.AlbumId,
                        AlbumName = album.Name,
                        album.ReleaseYear
                    };
                    
                var albumArtistsByYear = 
                    from albumArtist in albumArtistList 
                    orderby albumArtist.ReleaseYear group albumArtist by albumArtist.ReleaseYear;

                foreach (var albumArtistItem in albumArtistsByYear)
                {
                    Console.WriteLine($"Albums released in: {albumArtistItem.Key}");

                    foreach (var album in albumArtistItem)
                    {
                        Console.WriteLine($"{album.AlbumName}");
                        Console.WriteLine($"Artist: {album.ArtistName}");
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                
                Console.WriteLine("Input part(s) of the album name that you want to search:");
                
                var searchParameter = Console.ReadLine() ?? "";
                var albumsByParameter = allAlbums.Where(album => album.Name.Contains(searchParameter)); //?
                foreach (var album in albumsByParameter)
                {
                    Console.WriteLine($"{album.Name}, {album.ReleaseYear}");
                }
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Albums with their full duration:");
                Console.WriteLine();
                var songAlbumList =
                    from album in allAlbums
                    join songOnAlbum in allSongsOnAlbums on album.AlbumId equals songOnAlbum.AlbumId
                    join song in allSongs on songOnAlbum.SongId equals song.SongId
                    orderby album.AlbumId
                    select new {Album = album, Song = song};
                songAlbumList = songAlbumList.ToList();
                foreach (var album in allAlbums)
                {
                    var albumSongs = songAlbumList.Where(songAlbumItem => songAlbumItem.Album.AlbumId == album.AlbumId);
                    var albumDuration = new TimeSpan();
                    albumDuration = albumSongs.Aggregate(albumDuration, (current, albumSongItem) => current + albumSongItem.Song.Duration);

                    Console.WriteLine($"Album: {album.Name}, {album.ReleaseYear}");
                    Console.WriteLine($"Duration: {albumDuration}");
                }
                
                Console.WriteLine();
                Console.WriteLine("Input song name that you want to be found on albums:");
                searchParameter = Console.ReadLine() ?? "";
                var matchParameterList =
                    songAlbumList.Where(songAlbumItem => songAlbumItem.Song.Name == searchParameter); //?
                foreach (var matchItem in matchParameterList)
                {
                    Console.WriteLine($"{matchItem.Album.Name}, {matchItem.Album.ReleaseYear}");
                }
                
                Console.WriteLine();
                var songArtistList =
                    from songAlbumItem in songAlbumList
                    join artist in allArtists on songAlbumItem.Album.ArtistId equals artist.ArtistId
                    select new {SongAlbumItem = songAlbumItem, Artist = artist};
                Console.WriteLine("Input artist name that you want to search songs by:");
                var artistParameter = Console.ReadLine() ?? "";
                searchParameter = "";
                int yearParameter;
                do
                {
                    Console.WriteLine("Released after:");
                    searchParameter = Console.ReadLine();
                } while (!int.TryParse(searchParameter, out yearParameter));

                var songsByParameter = songArtistList.Where(songArtistItem =>
                    songArtistItem.Artist.Name == artistParameter &&
                    int.Parse(songArtistItem.SongAlbumItem.Album.ReleaseYear) > yearParameter);
                foreach (var song in songsByParameter)
                {
                    Console.WriteLine($"{song.SongAlbumItem.Song.Name}, {song.SongAlbumItem.Song.Duration}");
                }
                Console.WriteLine();
            }
        }
    }
}
