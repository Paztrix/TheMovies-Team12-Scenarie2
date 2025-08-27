using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace MovieTest.Model.Repository{
    public class FileCinemaShowingRepository : ICinemaShowingRepository {
        private string _filePath = "Forestillinger.CSV";

        public FileCinemaShowingRepository(string filePath) {
            _filePath = filePath;
        }

        //Henter alle forestillinger fra CSV-filen
        public List<CinemaShowing> GetAllCinemaShowings(List<Movie> movies) {
            var cinemaShowings = new List<CinemaShowing>();
            //Hvis filen ikke findes så returneres en tom liste
            if(!File.Exists(_filePath)) return cinemaShowings;

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
                //Spring linjen over hvis den er ugyldig (Mere end 4 fields)
                if(parts == null || parts.Length < 4) continue;

                string movieTitle = parts[0].Trim();
                var movie = movies.FirstOrDefault(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
                if (movie == null) continue;

                var cinema = (Cinema.CinemaName)Enum.Parse(typeof(Cinema.CinemaName), parts[1].Trim());
                var showDate = DateTime.Parse(parts[2].Trim());
                var cinemaHall = parts[3].Trim();

                cinemaShowings.Add(new CinemaShowing(movie, cinema, showDate, cinemaHall));
            }
            return cinemaShowings;
        }

        // Tilføjer en ny forestilling til CSV-filen
        public void AddCinemaShowing(CinemaShowing cinemaShowing) {
            bool fileExists = File.Exists(_filePath);

            using (var writer = new StreamWriter(_filePath, append: true)) {
                // Hvis filen ikke findes, skriv header først
                if (!fileExists) {
                    writer.WriteLine("\"Title\",\"Cinema\",\"ShowDate\",\"CinemaHall\"");
                }

                // Brug komma-separator og anførselstegn omkring tekstfelter
                writer.WriteLine($"\"{cinemaShowing.Movie.Title}\",\"{cinemaShowing.Cinema}\",\"{cinemaShowing.ShowDate:dd-MM-yyyy hh\\:mm}\",\"{cinemaShowing.CinemaHall}\"");
            }
        }
    }
}
