using System.Windows;
using System.Windows.Controls;

namespace TransportDepartment
{
    public partial class AccountingCard : Window
    {
        private readonly AccountingCardViewModel _viewModel;
        private readonly TransportProperties _transport; // <-- сохраняем транспорт

        public AccountingCard(TransportProperties transport)
        {
            InitializeComponent();

            _transport = transport; // <-- запоминаем
            _viewModel = new AccountingCardViewModel(transport);
            this.DataContext = _viewModel;
        }

        private void btnAddData_Click(object sender, RoutedEventArgs e)
        {
            // <-- передаём и ViewModel, и TransportProperties
            var addWindow = new AddCarProperties(_viewModel, _transport);
            addWindow.ShowDialog();
        }

        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new MainWindow();
            newWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("выводить на печать нужно страницу полностью и полностью печатать таблицу");
            var pd = new PrintDialog();
            pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
            pd.PrintVisual(this, "");
        }
    }
}
