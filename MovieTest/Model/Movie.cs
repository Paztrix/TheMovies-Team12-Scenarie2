using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MovieTest.Model {
    public class Movie {
        public string Title { get; set; }
        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }
        public string Instructor { get; set; }
        public DateOnly MoviePremiere { get; set; }

        public Movie(string title, string genre, TimeSpan duration, string instructor, DateOnly moviePremiere) {
            Title = title;
            Genre = genre;
            Duration = duration;
            Instructor = instructor;
            MoviePremiere = moviePremiere;
        }

        public override string ToString() {
            return $"{Title},{Genre},{Duration},{Instructor},{MoviePremiere}";
        }

        public static Movie FromString(string line) {
            var parts = line.Split(",");
            string title = parts[0].Trim().Trim('"');
            string genre = parts[1].Trim().Trim('"');
            TimeSpan duration = TimeSpan.ParseExact(parts[2].Trim(), @"hh\:mm", null);
            string instructor = parts[3].Trim().Trim('"');
            DateOnly moviePremiere = DateOnly.Parse(parts[4].Trim().Trim('"'));

            return new Movie(title, genre, duration, instructor, moviePremiere);
        }
    }
}
