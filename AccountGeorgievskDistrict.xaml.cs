using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using TransportDepartmentMVVM.Data;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Services;
using TransportDepartmentMVVM.Views;

namespace TransportDepartmentMVVM
{
    public partial class AccountGeorgievskDistrict : Window
    {
        private readonly Window _mainWindow;

        public AccountGeorgievskDistrict(Window mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        // Обработчик события Loaded
        private void AccountGeorgievskDistrict_Loaded(object sender, RoutedEventArgs e)
        {
         //   WinApiHelper.HideCloseButton(this);

            DataBaseInitializer.EnsureDataBaseStructure();
            string targetRegion = "Георгиевский район";

            try
            {
                List <TransportProperties> transports = new TransportRepository().GetTransportsByRegion(targetRegion);

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

       

      

       

      
    }
}
