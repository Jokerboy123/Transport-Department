using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Transport_Department
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
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void HideCloseButton()
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            // GWL_STYLE = -16
            // WS_SYSMENU = 0x80000
            int currentStyle = GetWindowLong(hwnd, -16);
            SetWindowLong(hwnd, -16, currentStyle & ~0x80000);
        }

        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new MainWindow();
            newWindow.Show();
        }
    }
}
