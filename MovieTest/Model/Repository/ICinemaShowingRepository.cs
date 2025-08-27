using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTest.Model.Repository {
    public interface ICinemaShowingRepository {
        List<CinemaShowing> GetAllCinemaShowings(List<Movie> movies);
        void AddCinemaShowing(CinemaShowing cinemaShowing);
    }
}
