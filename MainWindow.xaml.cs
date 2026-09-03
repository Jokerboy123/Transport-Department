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
            HideCloseButton();
        }

        // Обработчик события Loaded
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HideCloseButton();
            DataBaseInitializer.InitializeDataBase();
            //   MessageBox.Show("База данных успешно инициализирована или уже существует!", "Успех");
           

        }
        private void HideCloseButton()
        {
            // Теперь Handle гарантированно существует
            var hwnd = new WindowInteropHelper(this).Handle;

            const int GWL_STYLE = -16;
            const int WS_SYSMENU = 0x80000;

            int currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            // Убираем флаг WS_SYSMENU (системное меню), который отвечает за крестик
            SetWindowLong(hwnd, GWL_STYLE, currentStyle & ~WS_SYSMENU);
        }
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void GeoAccountPage_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new AccountGeorgievsk();
            newWindow.Show();
            DataBaseInitializer.GetTransportsByRegion("Георгиевск");
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
            MessageBox.Show("Открыть новое окно с параметрами, где задать месяц и год (пока минимально)");
        }
    }
}