using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTest.Model.Repository {
    public class FileMovieRepository : IMovieRepository {
        private readonly string _filePath = "Filmliste.CSV";
        public FileMovieRepository(string filePath) {
            _filePath = filePath;
        }

        //Henter alle film fra CSV-filen
        public List<Movie> GetAllMovies() {
            var movies = new List<Movie>();
            //Hvis filen ikke findes så returneres en tom liste
            if(!File.Exists(_filePath)) return movies;

            //Parser til af læse CSV-filen
            using var parser = new TextFieldParser(_filePath) {
                TextFieldType = FieldType.Delimited //Angiver at filen er kommasepareret
            };
            parser.SetDelimiters(","); //Field-separator er komma
            parser.HasFieldsEnclosedInQuotes = true; //Fields kan være omsluttet af ""

            parser.ReadFields(); //Læser første linje (Header) og ignorerer den

            //Læser restenm af CSV-filen linje for linje
            while(!parser.EndOfData) {
                var parts = parser.ReadFields();
                //Spring linjen over hvis den er ugyldig (Mere end 5 fields)
                if(parts == null || parts.Length < 5) continue;

                var title = parts[0].Trim(); //Titel
                var genre = parts[1].Trim(); //Genre
                //Forsøger at parse filmens varighed i formatet hh:mm
                if(TimeSpan.TryParseExact(parts[2].Trim(), @"hh\:mm", null, out var duration) &&
                        DateOnly.TryParse(parts[4].Trim(), out var moviePremiere)) {
                    var instructor = parts[3].Trim();
                    //Tilføjer filmen til listen
                    movies.Add(new Movie(title, genre, duration, instructor, moviePremiere));
                }
            }
            return movies;
        }

        // Tilføjer en ny film til CSV-filen
        public void AddMovie(Movie movie) {
            bool fileExists = File.Exists(_filePath);

            using (var writer = new StreamWriter(_filePath, append: true)) {
                // Hvis filen ikke findes, skriv header først
                if (!fileExists) {
                    writer.WriteLine("\"Title\",\"Genre\",\"Duration\",\"Instructor\",\"MoviePremiere\"");
                }

                // Brug komma-separator og anførselstegn omkring tekstfelter
                writer.WriteLine($"\"{ movie.Title}\",\"{movie.Genre}\",\"{movie.Duration:hh\\:mm}\",\"{movie.Instructor}\",\"{movie.MoviePremiere}\"");
            }
        }
    }
}
