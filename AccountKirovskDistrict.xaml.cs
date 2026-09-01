using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace TransportDepartment
{
    public partial class AccountKirovskDistrict : Window
    {
        public AccountKirovskDistrict()
        {
            InitializeComponent();
            // HideCloseButton вызываем здесь, так как Handle уже существует после InitializeComponent
            HideCloseButton();
        }

        private void AccountKirovskDistrict_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Инициализируем структуру БД (создаем таблицы, если нет)
            DataBaseInitializer.InitializeDataBase();

            // 2. Получаем данные для Кировского района
            string targetRegion = "Кировский район";

            try
            {
                // ВАЖНО: Этот метод должен возвращать List<TransportItem> (см. пояснение ниже)
                var transports = DataBaseInitializer.GetTransportsByRegion(targetRegion);


                // 3. Динамически создаем кнопки и добавляем их в WrapPanel из XAML
                TransportButtonsPanel.Children.Clear(); // Очищаем на случай повторного открытия

                foreach (var item in transports)
                {
                    var btn = new Button
                    {
                        // Красивый текст: "Марка (Госномер)"
                        Content = $"{item.Brand} \n({item.StateNumber})",
                        // Применяем твой стиль из ресурсов окна
                        Style = (Style)FindResource("ModernButtonStyle"),
                        Padding = new Thickness(15, 8, 15, 8),
                        Margin = new Thickness(5),
                        // Сохраняем госномер в Tag, чтобы знать, какую карточку открывать
                        Tag = item.StateNumber
                    };

                    // Подписываемся на клик
                    btn.Click += OnTransportButtonClick;

                    // Добавляем кнопку на форму
                    TransportButtonsPanel.Children.Add(btn);
                }

                if (transports.Count == 0)
                {
                    var infoLabel = new TextBlock
                    {
                        Text = "Транспорт для Кировского района не найден.",
                        Foreground = System.Windows.Media.Brushes.Gray,
                        FontSize = 14,
                        Margin = new Thickness(10)
                    };
                    TransportButtonsPanel.Children.Add(infoLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить данные: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnTransportButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string stateNumber)
            {
                this.Hide();

                // Создаем окно карточки. 
                // Если в AccountingCard есть конструктор, принимающий номер, используй его:
                // var newWindow = new AccountingCard(stateNumber); 

                var newWindow = new AccountingCard();

                // Если нужно передать номер внутрь окна, сделай это через публичное свойство:
                // newWindow.CurrentTransportNumber = stateNumber; 

                newWindow.Show();
            }
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
            var hwnd = new WindowInteropHelper(this).Handle;
            const int GWL_STYLE = -16;
            const int WS_SYSMENU = 0x80000;

            int currentStyle = GetWindowLong(hwnd, GWL_STYLE);
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
