using System;
using System.Collections.Generic;
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
                Console.WriteLine();

                //Svi albumi grupirani po godini izdavanja, kraj imena albuma piše tko je autor (glazbenik)
                Console.WriteLine("ALBUMS GROUPED BY YEAR");
                var groupedAlbums = allAlbums.OrderBy(album => album.ReleaseYear).GroupBy(album => album.ReleaseYear);
                foreach (var albumItem in groupedAlbums)
                {
                    Console.WriteLine($"Year: {albumItem.Key}");
                    Console.WriteLine();
                    foreach (var album in albumItem)
                    {
                        Console.WriteLine($"Album: {album.Name}");
                        Console.WriteLine($"Artist: {album.Artist.Name}");
                        Console.WriteLine();
                    }
                }

                //Svi albumi koji sadrže u imenu zadani tekst
                Console.WriteLine("Input part of the album name you want to search albums by:");
                searchParameter = Console.ReadLine() ?? "";
                var albumsByParameter = allAlbums.Where(album => album.Name.Contains(searchParameter));
                foreach (var album in albumsByParameter)
                {
                    Console.WriteLine($"{album.Name} {album.ReleaseYear}");
                }
                Console.WriteLine();

                //Svi albumi skupa sa ukupnim trajanjem - ukupno trajanje je zbroj trajanja svih pjesama na albumu
                Console.WriteLine("ALBUMS WITH TOTAL DURATION");
                foreach (var album in allAlbums)
                {
                    var duration = new TimeSpan(00,00,00);
                    duration = album.SongsAlbums.Aggregate(duration, (current, songOnAlbum) => current + songOnAlbum.Song.Duration);

                    Console.WriteLine($"{album.Name} {duration}");
                }
                Console.WriteLine();
                //Svi albumi na kojima se pojavljuje zadana pjesma
                Console.WriteLine("What albums have the song you input:");
                searchParameter = Console.ReadLine() ?? "";
                var songByParameter = allSongs.FirstOrDefault(song => song.Name == searchParameter);
                var albumsBySong = allAlbums.Where(album =>
                    songByParameter != null && album.SongsAlbums.Contains
                        (allSongsOnAlbums.FirstOrDefault(songAlbum => songAlbum.Album == album && songAlbum.Song == songByParameter)));

                foreach (var album in albumsBySong)
                {
                    Console.WriteLine($"{album.Name} {album.ReleaseYear}");
                }

                //Sve pjesme zadanog glazbenika, koje su na albumima izdanim iza određene godine
                Console.WriteLine("Input artist name you want to get songs by:");
                searchParameter = Console.ReadLine() ?? "";
                var songsByArtist = allSongs.Where(song => song.SongsAlbums.Contains
                (allSongsOnAlbums.FirstOrDefault(songAlbum => songAlbum.Song == song &&
                                                              songAlbum.Album.Artist.Name == searchParameter)));

                searchParameter = "";
                int yearParameter;
                do
                {
                    Console.WriteLine("Released after:");
                    searchParameter = Console.ReadLine();
                } while (!int.TryParse(searchParameter, out yearParameter));

                var songsAfterYear = songsByArtist.Where(song => song.SongsAlbums.Contains(
                    allSongsOnAlbums.FirstOrDefault(songAlbum => songAlbum.Song == song &&
                                                                 int.Parse(songAlbum.Album.ReleaseYear) > yearParameter)));

                foreach (var song in songsAfterYear)
                {
                    Console.WriteLine(song.Name);
                }
            }
        }
    }
}
