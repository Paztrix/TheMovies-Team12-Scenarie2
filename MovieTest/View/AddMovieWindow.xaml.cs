using MovieTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MovieTest {
    /// <summary>
    /// Interaction logic for AddMovieWindow.xaml
    /// </summary>
    public partial class AddMovieWindow : Window {
        //Henter logik fra ViewModel
        public AddMovieViewModel ViewModel { get; }

        public AddMovieWindow() {
            // Initialiserer alle XAML-komponenter
            InitializeComponent();
            //ViewModel der med funktion til at lukke vinduet, samt liste med film
            ViewModel = new AddMovieViewModel(() => Close());
            //Binder DataContext til ViewModel så UI kan opdatere automatisk
            DataContext = ViewModel;
        }
    }
}
