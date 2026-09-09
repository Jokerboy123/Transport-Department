using System.Windows;

namespace TransportDepartment
{
    public partial class AccountingCard : Window
    {
        private readonly AccountingCardViewModel _viewModel;

        public AccountingCard(TransportProperties transport)
        {
            InitializeComponent();

            // Создаем ViewModel -> она сама загрузит данные из БД
            _viewModel = new AccountingCardViewModel(transport);

            // Привязываем всё окно к ViewModel
            this.DataContext = _viewModel;
        }

        private void btnAddData_Click(object sender, RoutedEventArgs e)
        {
            // Передаем ViewModel в окно добавления, чтобы оно могло сохранить данные
            var addWindow = new AddCarProperties(_viewModel);
            addWindow.ShowDialog();
        }

        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new MainWindow();
            newWindow.Show();
        }
    }
}
