﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTest.Model.Repository {
    public interface IMovieRepository {
        List<Movie> GetAllMovies();
        void AddMovie(Movie movie);
    }
}
