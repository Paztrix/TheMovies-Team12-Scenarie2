using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTest.Model {
    public class CinemaShowing {
        public Movie Movie { get; set; }
        public Cinema.CinemaName Cinema { get; set; }
        public DateTime ShowDate { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public string CinemaHall { get; set; }

        public CinemaShowing(Movie movie, Cinema.CinemaName cinema, DateTime showDate, string cinemaHall) {
            Movie = movie;
            Cinema = cinema;
            ShowDate = showDate;
            TotalDuration = movie.Duration + TimeSpan.FromMinutes(30);
            CinemaHall = cinemaHall;
        }

        public override string ToString() {
            return $"{Movie.Title},{Cinema},{ShowDate:yyyy-MM-dd HH:mm},{CinemaHall}";
        }

        public static CinemaShowing FromString(string line, List<Movie> movies) {
            var parts = line.Split(",");
            string movieTitle = parts[0].Trim().Trim('"');
            var movie = movies.FirstOrDefault(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
            var cinema = (Cinema.CinemaName)Enum.Parse(typeof(Cinema.CinemaName), parts[1].Trim().Trim('"'));
            DateTime showDate = DateTime.Parse(parts[2].Trim().Trim('"'));
            string cinemaHall = parts[3].Trim().Trim('"');

            return new CinemaShowing(movie, cinema, showDate, cinemaHall);
        }
    }
}