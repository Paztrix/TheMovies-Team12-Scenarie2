using MovieTest.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        /*
         * Property ViewModel af type MovieViewModel
         * => Når ViewModel læses udfør udtryk på højre side af pilen og returnér resultat
         * as Forsøger at cast DataContext til type MovieViewModel (Hvis DataContext er MovieViewModel object returneres det, hvis ikke returneres null)
         * kan læses som public MovieViewModel ViewModel { get { return DataContext as MovieViewModel; } }
         */
        private MovieViewModel ViewModel => DataContext as MovieViewModel;
        public MainWindow() {
            // Initialiserer alle XAML-komponenter
            InitializeComponent();

            // Sæt ViewModel som DataContext
            var vm = new MovieViewModel();
            DataContext = vm;

            //Reagere på ViewModel ændringer og lav kolonner, opdatere datagrid når der skiftes mellem film og forestillinger
            vm.PropertyChanged += ViewModel_PropertyChanged;
            //Initialisere kolonner i datagrid baseret på ViewModel nuværende tilstand
            UpdateDataGridColumns();
        }

        //Metode til at håndtere PropertyChange-event fra ViewModel
        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            //Tjekker om IsShowingMovies ændrer sig og opdatere kolonnerne i datagrid hvis ja
            if(e.PropertyName == nameof(MovieViewModel.IsShowingMovies)) {
                UpdateDataGridColumns();
            }
        }

        //Metode til at definere og opdatere kolonnerne
        private void UpdateDataGridColumns() {
            //Rydder de eksisterende kolonner
            MainDataGrid.Columns.Clear();

            if(ViewModel.IsShowingMovies) {
                //Kolonner for Film
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Titel", Binding = new Binding("Title") });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Genre", Binding = new Binding("Genre") });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Varighed", Binding = new Binding("Duration") });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Instruktør", Binding = new Binding("Instructor") });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Premiere", Binding = new Binding("MoviePremiere") });
            } else {
                //Kolonner for Forestillinger
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Film", Binding = new Binding("Movie.Title") });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Biograf", Binding = new Binding("Cinema") });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Dato", Binding = new Binding("ShowDate") { StringFormat = "dd-MM-yyyy HH:mm" } });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Varighed", Binding = new Binding("TotalDuration") });
                MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Sal", Binding = new Binding("CinemaHall") });
            }
        }
    }
}



/*
namespace MovieTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            MovieViewModel mw = new MovieViewModel();
            DataContext = mw;
        }
    }
}
*/