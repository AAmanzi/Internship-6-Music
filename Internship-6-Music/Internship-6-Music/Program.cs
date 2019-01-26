using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                var nationalityForSearch = Console.ReadLine(); //TODO normalize strings
                var artistsWithNationality = allArtists.Where(artist => artist.Nationality == nationalityForSearch).OrderBy(artist => artist.Name);
                foreach (var artist in artistsWithNationality)
                {
                    Console.WriteLine($"{artist.Name}, {artist.Nationality}");
                }

                //Svi albumi grupirani po godini izdavanja, kraj imena albuma piše
                //tko je autor (glazbenik)
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

                //Svi albumi koji sadrže u imenu zadani tekst
                Console.WriteLine("Search albums: ");
                var searchParameter = Console.ReadLine();
                var albumsByParameter = allAlbums.Where(album => album.Name.Contains(searchParameter));
                foreach (var album in albumsByParameter)
                {
                    Console.WriteLine($"{album.Name}, {album.ReleaseYear}");
                }
                Console.WriteLine();
            }
        }
    }
}
