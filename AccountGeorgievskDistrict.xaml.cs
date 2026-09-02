using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using TransportDepartment;
using static TransportDepartment.DataBaseInitializer;

namespace TransportDepartment
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

            DataBaseInitializer.InitializeDataBase();
            string targetRegion = "Георгиевский район";

            try
            {
                var transports = DataBaseInitializer.GetTransportsByRegion(targetRegion);

                TransportButtonsPanel.Children.Clear();

                foreach(var item in transports)
                {
                    var btn = new Button
                    {
                        Content = $"{item.TransportBrand} \n({item.StateNumber})",
                        Style = (Style)FindResource("ModernButtonStyle"),
                        Padding = new Thickness(15, 8, 15, 8),
                        Margin = new Thickness(5),
                        Tag = item
                    };
                    btn.Click += OnTransportButtonClick;

                    TransportButtonsPanel.Children.Add(btn);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить данные: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnTransportButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is TransportProperties transport)
            {
                this.Hide();

                // Создаем окно карточки. 
                // Если в AccountingCard есть конструктор, принимающий номер, используй его:

                var newWindow = new AccountingCard(transport);

                // Если нужно передать номер внутрь окна, сделай это через публичное свойство:
               // newWindow.Closed += (s, args) => this.Show();
                newWindow.Show();

            }
        }

        //public void OpenAccountingCard(object sender, RoutedEventArgs e)
        //{
        //    this.Hide();
        //    var newWindow = new AccountingCard(transport);
        //    newWindow.Show();
        //}


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
