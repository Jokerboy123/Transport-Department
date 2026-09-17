using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using TransportDepartmentMVVM.Data;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Services;
using TransportDepartmentMVVM.ViewModels;
using TransportDepartmentMVVM.Views;

namespace TransportDepartmentMVVM
{
    public partial class AccountGeorgievskDistrict : Window
    {
        private readonly string _regionIndex;
        public Window _mainWindow;
        public AccountGeorgievskDistrict(Window mainWindow, string regionIndex)
        {
        
            InitializeComponent();

            var vm = new AccountGeorgievskDistrictViewModel(mainWindow, regionIndex);
            DataContext = vm;

            vm.OnHideRequested += () => this.Hide();
            vm.OnShowRequested += () => this.Show();
            vm.OnCloseRequested += () => this.Close();
        
        }

        // Обработчик события Loaded
        public void AccountGeorgievskDistrict_Loaded(object sender, RoutedEventArgs e)

        {
            // 1. Инициализируем структуру БД (создаем таблицы, если нет)
            DataBaseInitializer.EnsureDataBaseStructure();

            // 2. Получаем данные 
            string targetRegion = "Георгиевский район";

            try
            {
                // ВАЖНО: Этот метод должен возвращать List<TransportItem> (см. пояснение ниже)
                //  var transports = DataBaseInitializer.GetTransportsByRegion(targetRegion);


                // 3. Динамически создаем кнопки и добавляем их в WrapPanel из XAML
                TransportButtonsPanel.Children.Clear(); // Очищаем на случай повторного открытия

                var transports = new TransportRepository().GetTransportsByRegion(targetRegion);

                TransportButtonsPanel.Children.Clear();

                foreach (var item in transports)
                {
                    var btn = new Button
                    {
                        // Красивый текст: "Марка (Госномер)"
                        Content = $"{item.TransportBrand} \n({item.StateNumber})",
                        // Применяем твой стиль из ресурсов окна
                        Style = (Style)FindResource("ModernButtonStyle"),
                        Padding = new Thickness(15, 8, 15, 8),
                        Margin = new Thickness(5),
                        // Сохраняем госномер в Tag, чтобы знать, какую карточку открывать
                        Tag = item
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
                        Text = "Транспорт для Георгиевского района не найден.",
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
            if (sender is Button btn && btn.Tag is TransportProperties transport)
            {
                Parameters.Instance.OpenDemonstrationCard(this, transport);
            }
        }


        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

       

      

       

      
    }
}
