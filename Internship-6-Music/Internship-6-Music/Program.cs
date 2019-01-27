using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
            var connectionString = 
                "Data Source = (LocalDb)\\MSSQLLocalDB; Initial Catalog = Internship-6-Music; Integrated Security = true; MultipleActiveResultSets = true";
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
                Console.WriteLine("Input nationality that you want to search by:");
                var nationalityForSearch = Console.ReadLine();
                var artistsWithNationality = allArtists.Where(artist => artist.Nationality == nationalityForSearch).OrderBy(artist => artist.Name);
                foreach (var artist in artistsWithNationality)
                {
                    Console.WriteLine($"{artist.Name}, {artist.Nationality}");
                }
                
                Console.WriteLine();
                var albumArtistList =
                    from artist in allArtists
                    join album in allAlbums on artist.ArtistId equals album.ArtistId
                    select new {Artist = artist, Album = album};
                    
                albumArtistList = albumArtistList.OrderBy(album => album.Album.ReleaseYear);

                foreach (var albumArtistItem in albumArtistList)
                {
                    Console.WriteLine($"Album: {albumArtistItem.Album.Name}, {albumArtistItem.Album.ReleaseYear}");
                    Console.WriteLine($"Artist: {albumArtistItem.Artist.Name}");
                    Console.WriteLine();
                }
                
                Console.WriteLine("Search albums: ");
                
                var searchParameter = Console.ReadLine() ?? "";
                var parameter = searchParameter;
                var albumsByParameter = allAlbums.Where(album => album.Name.Contains(parameter));
                foreach (var album in albumsByParameter)
                {
                    Console.WriteLine($"{album.Name}, {album.ReleaseYear}");
                }
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
                Console.WriteLine("Search albums that contain song:");
                searchParameter = Console.ReadLine() ?? "";
                var matchParameterList =
                    songAlbumList.Where(songAlbumItem => songAlbumItem.Song.Name == searchParameter);
                foreach (var matchItem in matchParameterList)
                {
                    Console.WriteLine($"{matchItem.Album.Name}, {matchItem.Album.ReleaseYear}");
                }
                
                Console.WriteLine();
                var songArtistList =
                    from songAlbumItem in songAlbumList
                    join artist in allArtists on songAlbumItem.Album.ArtistId equals artist.ArtistId
                    select new {SongAlbumItem = songAlbumItem, Artist = artist};
                Console.WriteLine("Search songs by artist:");
                var artistParameter = Console.ReadLine() ?? "";
                searchParameter = "";
                var yearParameter = 0;
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
