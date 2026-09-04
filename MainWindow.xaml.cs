using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TransportDepartment;

namespace TransportDepartment
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

        // Обработчик события Loaded
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataBaseInitializer.InitializeDataBase();
          //  Parameters.HideCloseButton(this);  // теперь корректно

            // MessageBox.Show("База данных успешно инициализирована или уже существует!", "Успех");
        }

        private void GeoAccountPage_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new AccountGeorgievsk();
            newWindow.Show();
       //     DataBaseInitializer.GetTransportsByRegion("Георгиевск");
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

        private void Parameters_Click(object sender, RoutedEventArgs e)
        {
            var paramWind = new ParametersWindow();
            if (paramWind.ShowDialog() == true)
            {
                string chosenMonth = paramWind.SelectedMonth ?? "Не выбран";
                string choosenYear = paramWind.SelectedYear ?? "Не выбран";
                MessageBox.Show("Выбран месяц " + chosenMonth.ToString());
            //    AppSettings.CurrentMonth = chosenMonth;

            }
        }
    }
}