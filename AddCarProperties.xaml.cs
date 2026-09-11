using System.Windows;
using TransportDepartment;

namespace TransportDepartment  // <-- должно совпадать с namespace в .xaml
{
    public partial class AddCarProperties : Window  // <-- обязательно partial
    {
        private readonly AccountingCardViewModel _viewModel;
        private readonly TransportProperties _transport;

        public AddCarProperties(AccountingCardViewModel viewModel, TransportProperties transport)
        {
            InitializeComponent();  // теперь этот метод будет найден
            _viewModel = viewModel;
            _transport = transport;

            this.DataContext = new AddCarPropertiesViewModel(_transport);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as AddCarPropertiesViewModel;
            if (viewModel == null)
            {
                MessageBox.Show("Не удалось получить данные формы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newRecord = new AccountingCardProperties
            {
                DayNumber = viewModel.DayNumber,
                WaySheet = viewModel.WaySheet,
                FirstDriver = viewModel.FirstDriver,
                SecondDriver = viewModel.SecondDriver,
                GetGas = viewModel.GetGas,
                GetPetrol = viewModel.GetPetrol,
                GetDiesel = viewModel.GetDiesel,
                RemaindDayKilometrageValue = viewModel.RemaindDayKilometrageValue,
                GasConsumptionStandard = viewModel.GasConsumptionStandard,
                PetrolConsumptionStandard = viewModel.PetrolConsumptionStandard,
                UsedGasValue = viewModel.UsedGasValue,
                UsedPetrolValue = viewModel.UsedPetrolValue,
                UsedDieselValue = viewModel.UsedDieselValue,
                AdditionalToolBrand = viewModel.AdditionalToolBrand,
                AdditionalGasValue = viewModel.AdditionalGasValue,
                AdditionalPetrolValue = viewModel.AdditionalPetrolValue,
                AdditionalDieselValue = viewModel.AdditionalDieselValue,
                ExpectedGasValue = viewModel.ExpectedGasValue,
                ExpectedPetrolValue = viewModel.ExpectedPetrolValue
            };

            if (!newRecord.Validate())
            {
                MessageBox.Show("Валидация не пройдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _viewModel.AddNewRecord(newRecord);
            this.Close();
        }

       
    }
}
