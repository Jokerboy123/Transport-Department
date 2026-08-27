using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using TransportDepartment;

namespace Transport_Department
{
    public partial class AccountGeorgievskDistrict : Window
    {
        public AccountGeorgievskDistrict()
        {
            InitializeComponent();
            HideCloseButton();
        }

        // Обработчик события Loaded
        private void AccountGeorgievskDistrict_Loaded(object sender, RoutedEventArgs e)
        {
            HideCloseButton();
        }
        
      

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public void OpenAccountingCard(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new AccountingCard();
            newWindow.Show();
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

        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new MainWindow();
            newWindow.Show();
        }

      
    }
}
