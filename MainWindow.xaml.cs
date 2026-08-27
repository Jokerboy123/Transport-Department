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

namespace Transport_Department
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GeoAccountPage_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new AccountGeorgievsk();
            newWindow.Show();
        }
        private void KirovskAccountPage_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new AccountKirovskDistrict();
            newWindow.Show();
        }
        private void GeoDistrictAccountPage_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new AccountGeorgievskDistrict();
            newWindow.Show();
        }

        private void CloseProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}