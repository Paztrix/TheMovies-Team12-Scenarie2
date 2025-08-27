using MovieTest.Model;
using MovieTest.Model.Repository;
using MovieTest.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTest.ViewModel {
    public class AddMovieViewModel : BaseViewModel {
        private string _title;
        private string _genre;
        private string _duration;
        private string _instructor;
        private string _moviePremiere;

        public string Title {
            get => _title;
            set {
                _title = value;
                OnPropertyChanged(nameof(Title));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public string Genre {
            get => _genre;
            set {
                _genre = value;
                OnPropertyChanged(nameof(Genre));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public string Duration {
            get => _duration;
            set {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public string Instructor {
            get => _instructor;
            set {
                _instructor = value;
                OnPropertyChanged(nameof(Instructor));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public string MoviePremiere {
            get => _moviePremiere;
            set {
                _moviePremiere = value;
                OnPropertyChanged(nameof(MoviePremiere));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public bool IsConfirmed { get; private set; }

        /*
         * Action closeAction er en delegate der lukker vinduet når brugeren har trykket Ok eller Annuller
         */
        public AddMovieViewModel(Action closeAction) {
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
            return !string.IsNullOrWhiteSpace(Title)
                && !string.IsNullOrWhiteSpace(Genre)
                && TimeSpan.TryParse(Duration, out var duration) && duration > TimeSpan.Zero 
                && !string.IsNullOrWhiteSpace(Instructor) 
                && DateOnly.TryParse(MoviePremiere, out _);
        }
    }
}
