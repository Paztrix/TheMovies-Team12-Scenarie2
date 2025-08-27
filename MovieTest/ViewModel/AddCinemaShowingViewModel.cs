using MovieTest.Model;
using MovieTest.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTest.ViewModel {
    public class AddCinemaShowingViewModel : BaseViewModel {
        private Movie _selectedMovie;
        private Cinema.CinemaName? _cinema;
        private string _showDate;
        private string _cinemaHall;

        public ObservableCollection<Movie> Movies { get; }
        public ObservableCollection<Cinema.CinemaName> CinemaNames { get; }

        public Movie SelectedMovie {
            get => _selectedMovie;
            set {
                _selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public Cinema.CinemaName? Cinema {
            get => _cinema;
            set {
                _cinema = value;
                OnPropertyChanged(nameof(Cinema));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public string ShowDate {
            get => _showDate;
            set {
                _showDate = value;
                OnPropertyChanged(nameof(ShowDate));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public string CinemaHall {
            get => _cinemaHall;
            set {
                _cinemaHall = value;
                OnPropertyChanged(nameof(CinemaHall));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public bool IsConfirmed { get; private set; }


        //Action closeAction er en delegate/funktion der lukker vinduet når brugeren har trykket Ok eller Annuller
        public AddCinemaShowingViewModel(Action closeAction, IEnumerable<Movie> movies) {
            //Opretter ObservableCollection af eksisterende film
            Movies = new ObservableCollection<Movie>(movies);
            //Opretter ObservableCollection af biografnavne fra Cinema.CinameName enum
            CinemaNames = new ObservableCollection<Cinema.CinemaName>((Cinema.CinemaName[])Enum.GetValues(typeof(Cinema.CinemaName)));

            ConfirmCommand = new RelayCommand(execute => Confirm(closeAction), canExecute => CanConfirm());
            CancelCommand = new RelayCommand(execute => Cancel(closeAction));
        }

        private void Confirm(Action closeAction) {
            IsConfirmed = true;
            closeAction();
        }

        private void Cancel(Action closeAction) {
            IsConfirmed = false;
            closeAction();
        }

        private bool CanConfirm() {
            return SelectedMovie != null
                && Cinema.HasValue
                && DateTime.TryParse(ShowDate, out var showDate)
                && !string.IsNullOrWhiteSpace(CinemaHall);
        }
    }
}
