using System.Windows;

namespace TransportDepartment
{
    public partial class AddCarProperties : Window
    {
        private readonly AccountingCardViewModel _viewModel;

        // Конструктор принимает ViewModel для сохранения данных
        public AddCarProperties(AccountingCardViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            // Создаем пустой объект для формы
            this.DataContext = new AccountingCardProperties();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newRecord = this.DataContext as AccountingCardProperties;

            if (newRecord == null)
            {
                MessageBox.Show("Не удалось получить данные формы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // ВАЛИДАЦИЯ
            if (!newRecord.Validate())
            {
                MessageBox.Show("Валидация не пройдена!");
                return; // AddNewRecord не вызывается, ничего не сохраняется
            }


            // Сохраняем и добавляем в таблицу главного окна
            _viewModel.AddNewRecord(newRecord);

            this.Close();
        }

    }

}
