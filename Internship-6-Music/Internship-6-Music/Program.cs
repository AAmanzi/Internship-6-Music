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

                foreach (var songOnAlbum in allSongsOnAlbums)
                {
                    songOnAlbum.Album = allAlbums.FirstOrDefault(album => album.AlbumId == songOnAlbum.AlbumId);
                    songOnAlbum.Song = allSongs.FirstOrDefault(song => song.SongId == songOnAlbum.SongId);
                }

                foreach (var album in allAlbums)
                {
                    album.Artist = allArtists.FirstOrDefault(artist => artist.ArtistId == album.ArtistId);
                    album.SongsAlbums = allSongsOnAlbums.Where(songOnAlbum => songOnAlbum.AlbumId == album.AlbumId).ToList();
                }

                foreach (var song in allSongs)
                {
                    song.SongsAlbums = allSongsOnAlbums.Where(songOnAlbum => songOnAlbum.SongId == song.SongId).ToList();
                }

                foreach (var artist in allArtists)
                {
                    artist.Albums = allAlbums.Where(album => album.ArtistId == artist.ArtistId).ToList();
                }

                //Svi glazbenici poredani po imenu (u insert SQL-u možete prvo navesti prezimena da bude lakše)

                var orderedArtists = allArtists.OrderBy(artist => artist.Name);
                Console.WriteLine("ARTISTS ORDERED BY NAME");
                Console.WriteLine();
                foreach (var artist in orderedArtists)
                {
                    Console.WriteLine($"Name: {artist.Name}");
                    Console.WriteLine($"Nationality: {artist.Nationality}");
                    Console.WriteLine();
                }

                //Svi glazbenici koji su određene nacionalnosti
                Console.WriteLine("Input nationality you want to search artists by");
                var searchParameter = Console.ReadLine() ?? "";
                var artistsWithNationality = allArtists.Where(artist => artist.Nationality == searchParameter);
                foreach (var artist in artistsWithNationality)
                {
                    Console.WriteLine($"Name: {artist.Name}");
                    Console.WriteLine($"Nationality: {artist.Nationality}");
                    Console.WriteLine();
                }
            }
        }
    }
}
