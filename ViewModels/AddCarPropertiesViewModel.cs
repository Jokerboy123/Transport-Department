using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TransportDepartmentMVVM.Models;

namespace TransportDepartmentMVVM.ViewModels
{
    public class AddCarPropertiesViewModel : INotifyPropertyChanged
    {
        private readonly TransportProperties _transport;
        private readonly Action<AccountingCardProperties> _onSave;

        public event EventHandler CloseRequested;

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        // --- Поля формы ---

        private int _dayNumber;
        public int DayNumber
        {
            get => _dayNumber;
            set { _dayNumber = value; OnPropertyChanged(); }
        }

        private int _waySheet;
        public int WaySheet
        {
            get => _waySheet;
            set { _waySheet = value; OnPropertyChanged(); }
        }

        private string _firstDriver;
        public string FirstDriver
        {
            get => _firstDriver;
            set { _firstDriver = value; OnPropertyChanged(); }
        }

        private string _secondDriver;
        public string SecondDriver
        {
            get => _secondDriver;
            set { _secondDriver = value; OnPropertyChanged(); }
        }

        private double _getGas;
        public double GetGas
        {
            get => _getGas;
            set { _getGas = value; OnPropertyChanged(); }
        }

        private double _getPetrol;
        public double GetPetrol
        {
            get => _getPetrol;
            set { _getPetrol = value; OnPropertyChanged(); }
        }

        private double _getDiesel;
        public double GetDiesel
        {
            get => _getDiesel;
            set { _getDiesel = value; OnPropertyChanged(); }
        }

        private int _remaindDayKilometrageValue;
        public int RemaindDayKilometrageValue
        {
            get => _remaindDayKilometrageValue;
            set { _remaindDayKilometrageValue = value; OnPropertyChanged(); }
        }

        public double GasConsumptionStandard => _transport.GasConsumptionStandard;
        public double PetrolConsumptionStandard => _transport.PetrolConsumptionStandard;

        // Нормативные значения (read-only, берутся из транспорта)
        public double ExpectedGasValue => _transport.GasConsumptionStandard;
        public double ExpectedPetrolValue => _transport.PetrolConsumptionStandard;

        private double _usedGasValue;
        public double UsedGasValue
        {
            get => _usedGasValue;
            set { _usedGasValue = value; OnPropertyChanged(); }
        }

        private double _usedPetrolValue;
        public double UsedPetrolValue
        {
            get => _usedPetrolValue;
            set { _usedPetrolValue = value; OnPropertyChanged(); }
        }

        private double _usedDieselValue;
        public double UsedDieselValue
        {
            get => _usedDieselValue;
            set { _usedDieselValue = value; OnPropertyChanged(); }
        }

        private string _additionalToolBrand;
        public string AdditionalToolBrand
        {
            get => _additionalToolBrand;
            set { _additionalToolBrand = value; OnPropertyChanged(); }
        }

        private double _additionalGasValue;
        public double AdditionalGasValue
        {
            get => _additionalGasValue;
            set { _additionalGasValue = value; OnPropertyChanged(); }
        }

        private double _additionalPetrolValue;
        public double AdditionalPetrolValue
        {
            get => _additionalPetrolValue;
            set { _additionalPetrolValue = value; OnPropertyChanged(); }
        }

        private double _additionalDieselValue;
        public double AdditionalDieselValue
        {
            get => _additionalDieselValue;
            set { _additionalDieselValue = value; OnPropertyChanged(); }
        }

        // --- Конструктор ---

        public AddCarPropertiesViewModel(
            TransportProperties transport,
            Action<AccountingCardProperties> onSave)
        {
            _transport = transport;
            _onSave = onSave;

            // Заполняем значения по умолчанию из транспорта
            FirstDriver = transport.FirstDriverFullName ?? "";
            SecondDriver = transport.SecondDriverFullName ?? "";

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        // --- Команды ---

        private void Save()
        {
            var newRecord = new AccountingCardProperties
            {
                DayNumber = DayNumber,
                WaySheet = WaySheet,
                FirstDriver = FirstDriver,
                SecondDriver = SecondDriver,
                GetGas = GetGas,
                GetPetrol = GetPetrol,
                GetDiesel = GetDiesel,
                RemaindDayKilometrageValue = RemaindDayKilometrageValue,
                GasConsumptionStandard = GasConsumptionStandard,
                PetrolConsumptionStandard = PetrolConsumptionStandard,
                UsedGasValue = UsedGasValue,
                UsedPetrolValue = UsedPetrolValue,
                UsedDieselValue = UsedDieselValue,
                AdditionalToolBrand = AdditionalToolBrand,
                AdditionalGasValue = AdditionalGasValue,
                AdditionalPetrolValue = AdditionalPetrolValue,
                AdditionalDieselValue = AdditionalDieselValue,
                ExpectedGasValue = ExpectedGasValue,
                ExpectedPetrolValue = ExpectedPetrolValue,
                Region = _transport.Region,
                TransportStateNumber = _transport.StateNumber
            };

            //if (!newRecord.Validate())
            //{
            //    MessageBox.Show(
            //        "Проверьте правильность введённых данных.",
            //        "Ошибка",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Warning);
            //    return;
            //}

            _onSave?.Invoke(newRecord);
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        // --- INotifyPropertyChanged ---

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
