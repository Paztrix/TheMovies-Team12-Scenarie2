using MovieTest.Model;
using MovieTest.Model.Repository;
using MovieTest.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static MovieTest.Model.Cinema;

namespace MovieTest.ViewModel {
    public class MovieViewModel : BaseViewModel {
        private readonly IMovieRepository _movieRepository;
        private readonly ICinemaShowingRepository _cinemaShowingRepository;
        private string _title;
        private string _genre;
        private TimeSpan _duration;
        private string _instructor;
        private DateOnly _moviePremiere;
        private IEnumerable _currentData;
        private bool _isShowingMovies = true;

        //ObservableCollection af film som UI kan binde til
        public ObservableCollection<Movie> Movies { get; }
        public ObservableCollection<CinemaShowing> CinemaShowings { get; }
        public ObservableCollection<Cinema.CinemaName> CinemaNames { get; }

        public string Title {
            get => _title;
            set {
                if (_title != value) {
                    _title = value;
                    OnPropertyChanged(nameof(Title)); //Opdatere UI
                }
            }
        }

        public string Genre {
            get => _genre;
            set {
                if (_genre != value) {
                    _genre = value;
                    OnPropertyChanged(nameof(Genre));
                }
            }
        }

        public TimeSpan Duration {
            get => _duration;
            set {
                if (_duration != value) {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        public string Instructor {
            get => _instructor;
            set {
                if (_instructor != value) {
                    _instructor = value;
                    OnPropertyChanged(nameof(Instructor));
                }
            }
        }

        public DateOnly MoviePremiere {
            get => _moviePremiere;
            set {
                if (_moviePremiere != value) {
                    _moviePremiere = value;
                    OnPropertyChanged(nameof(MoviePremiere));
                }
            }
        }

        public IEnumerable CurrentData {
            get => _currentData;
            set {
                _currentData = value;
                OnPropertyChanged(nameof(CurrentData));
            }
        }

        public bool IsShowingMovies {
            get => _isShowingMovies;
            set {
                _isShowingMovies = value;
                OnPropertyChanged(nameof(IsShowingMovies));
            }
        }

        //Relay command til at tilføje film
        public RelayCommand AddMovieCommand { get; }
        public RelayCommand ShowMoviesCommand { get; }
        public RelayCommand ShowCinemaShowingsCommand { get; }
        public RelayCommand AddCinemaShowingCommand { get; }

        //Constructor med repository som parameter
        public MovieViewModel(IMovieRepository movieRepository, ICinemaShowingRepository cinemaShowingRepository) {
            _movieRepository = movieRepository;
            _cinemaShowingRepository = cinemaShowingRepository;
            //Henter film of tilføjer dem til ObservableCollection
            Movies = new ObservableCollection<Movie>(_movieRepository.GetAllMovies());
            CinemaShowings = new ObservableCollection<CinemaShowing>(_cinemaShowingRepository.GetAllCinemaShowings(Movies.ToList()));
            //Initialisere Command med metode til AddMovie og tjekke om CanAddMovie
            AddMovieCommand = new RelayCommand(execute => AddMovie(), canExecute => CanAddMovie());
            AddCinemaShowingCommand = new RelayCommand(execute => AddCinemaShowing(), canExecute => CanAddCinemaShowing());
            
            ShowMoviesCommand = new RelayCommand(_ => { CurrentData = Movies; IsShowingMovies = true; });
            ShowCinemaShowingsCommand = new RelayCommand(_ => { CurrentData = CinemaShowings; IsShowingMovies = false; });

            CurrentData = Movies;

            CinemaNames = new ObservableCollection<Cinema.CinemaName>(
                (Cinema.CinemaName[])Enum.GetValues(typeof(Cinema.CinemaName))
            );
        }

        //Standard constructor med fil-baseret repossitory
        public MovieViewModel() : this(new FileMovieRepository("Filmliste.CSV"), new FileCinemaShowingRepository("Forestillinger.CSV")) { }


        //Logik - Film
        public void AddMovie() {
            //Opretter et nyt vindue til at tilføje en film
            var addMovieWindow = new AddMovieWindow {
                Owner = Application.Current.MainWindow //Sætter hovedvinduet som ejer
            };

            //Viser vinduet som modal dialog (Vindue der vises ovenpå eksisterende vindue, som brugeren skal interagere med før de kan vende tilbage)
            var result = addMovieWindow.ShowDialog();

            //Tjekker om brugeren trykkede OK eller blev bekræftet gennem ViewModel
            if(result == true || addMovieWindow.ViewModel.IsConfirmed) {
                //Forsøger at parse duration fra string til TimeSpan
                if(TimeSpan.TryParse(addMovieWindow.ViewModel.Duration, out var duration) 
                    && DateOnly.TryParse(addMovieWindow.ViewModel.MoviePremiere, out var moviePremiere)) {
                    var newMovie = new Movie(
                        addMovieWindow.ViewModel.Title,
                        addMovieWindow.ViewModel.Genre,
                        duration,
                        addMovieWindow.ViewModel.Instructor,
                        moviePremiere
                    );

                    _movieRepository.AddMovie(newMovie);
                    Movies.Add(newMovie);
                } else {
                    MessageBox.Show("Ugyldig varighed", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Tjekker om en film kan tilføjes, ved at se om alle felter er udfyldt
        public bool CanAddMovie() {
            return true;
        }

        //Logik - Forestillinger
        private void AddCinemaShowing() {
            //Opretter nyt vindue til at tilføje forestilling
            var addShowingWindow = new AddCinemaShowingWindow(Movies);
            //Opretter ny ViewModel til vinduet. 1 parameter er funktion til at lukke vinduet og 2 parameter sender listen af nuværende film til combobox
            var viewModel = new AddCinemaShowingViewModel(
                () => addShowingWindow.Close(),
                Movies
            );

            //Sætter DataContext til ViewModel så UI kan binde til properties
            addShowingWindow.DataContext = viewModel;
            //Viser vinduet som en dialog og venter på bruger lukker det
            var result = addShowingWindow.ShowDialog();

            //Hvis bruger trykker OK
            if (result == true || viewModel.IsConfirmed) {
                //Tjekker felterne er udfyldt korrekt
                if (viewModel.SelectedMovie != null &&
                    viewModel.Cinema.HasValue &&
                    DateTime.TryParse(viewModel.ShowDate, out var showDate) &&
                    !string.IsNullOrWhiteSpace(viewModel.CinemaHall)) {

                    //Opretter ny instans af CinemaShowing med givne værdier
                    var newShowing = new CinemaShowing(
                        viewModel.SelectedMovie,
                        viewModel.Cinema.Value,
                        showDate,
                        viewModel.CinemaHall
                    );

                    _cinemaShowingRepository.AddCinemaShowing(newShowing);
                    CinemaShowings.Add(newShowing);
                } else {
                    MessageBox.Show("Ugyldige felter i forestillingen", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Tjekker om en film kan tilføjes, ved at se om alle felter er udfyldt
        public bool CanAddCinemaShowing() {
            return true;
        }
    }
}
