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
using static TransportDepartment.DataBaseInitializer;


namespace TransportDepartment
{
    /// <summary>
    /// Логика взаимодействия для AccountGeorgievsk.xaml
    /// </summary>
    public partial class AccountGeorgievsk : Window
    {
        public AccountGeorgievsk()
        {
            InitializeComponent();
        }

        // Обработчик события Loaded
        private void AccountGeorgievsk_Loaded(object sender, RoutedEventArgs e)
        {
          //  WinApiHelper.HideCloseButton(this);
            DataBaseInitializer.InitializeDataBase();


            string targetRegion = "Георгиевск";

            try
            {
                var transports = DataBaseInitializer.GetTransportsByRegion(targetRegion);

                TransportButtonsPanel.Children.Clear();

                foreach (var item in transports)
                {
                    var btn = new Button
                    {
                        Content = $"{item.TransportBrand} \n({item.StateNumber})",
                        Style = (Style)FindResource("ModernButtonStyle"),
                        Padding = new Thickness(15, 8, 15, 8),
                        Margin = new Thickness(5),
                        Tag = item,
                    };
                    btn.Click += OnTransportButtonClick;
                    TransportButtonsPanel.Children.Add(btn);

                }
            }
            catch (Exception ex) 
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
                newWindow.Closed += (s, args) => this.Show();
                newWindow.Show();

            }
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        
        //public void OpenAccountingCard(object sender, RoutedEventArgs e)
        //{
        //    this.Hide();
        //    var newWindow = new AccountingCard(transport);
        //    newWindow.Show();
        //}
        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new MainWindow();
            newWindow.Show();
        }
    }
}
