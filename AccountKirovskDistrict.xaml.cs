using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TransportDepartment;


namespace TransportDepartment
{
    /// <summary>
    /// Логика взаимодействия для AccountKirovskDistrict.xaml
    /// </summary>
    public partial class AccountKirovskDistrict : Window
    {
        public AccountKirovskDistrict()
        {
            InitializeComponent();
            HideCloseButton();
        }

        // Обработчик события Loaded
        private void AccountKirovskDistrict_Loaded(object sender, RoutedEventArgs e)
        {
            HideCloseButton();
        }

        public void OpenAccountingCard(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new AccountingCard();
            newWindow.Show();
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

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

        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new MainWindow();
            newWindow.Show();
        }
    }
}
