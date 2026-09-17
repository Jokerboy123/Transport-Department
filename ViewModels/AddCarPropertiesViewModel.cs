using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Views;

namespace TransportDepartmentMVVM.ViewModels
{
    public class AddCarPropertiesViewModel : INotifyPropertyChanged
    {
        private readonly TransportProperties _transport;
        private readonly Action<DemonstrationCardProperties> _onSave;

        public event EventHandler CloseRequested;
        public RelayCommand GoToPreviousCommand { get; }

        public RelayCommand AddRecordCommand { get; }
        public RelayCommand CancelCommand { get; }

        // --- Поля формы ---

        private int? _dayNumber;
        public int? DayNumber
        {
            get => _dayNumber;
            set { _dayNumber = value; OnPropertyChanged(); }
        }

        private int? _waySheet;
        public int? WaySheet
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

        private double? _getGas;
        public double? GetGas
        {
            get => _getGas;
            set { _getGas = value; OnPropertyChanged(); }
        }

        private double? _getPetrol;
        public double? GetPetrol
        {
            get => _getPetrol;
            set { _getPetrol = value; OnPropertyChanged(); }
        }

        private double? _getDiesel;
        public double? GetDiesel
        {
            get => _getDiesel;
            set { _getDiesel = value; OnPropertyChanged(); }
        }

        private int? _remaindDayKilometrageValue;
        public int? RemaindDayKilometrageValue
        {
            get => _remaindDayKilometrageValue;
            set
            {
                if (_remaindDayKilometrageValue == value) return;
                _remaindDayKilometrageValue = value;
                OnPropertyChanged(nameof(RemaindDayKilometrageValue));
                RecalculateDependentValues();

            }
        }

        public double? GasConsumptionStandard => _transport.GasConsumptionStandard;
        public double? PetrolConsumptionStandard => _transport.PetrolConsumptionStandard;

        // Нормативные значения (read-only, берутся из транспорта)
        // рассчитываются по формуле
        //public double? ExpectedGasValue => _transport.GasConsumptionStandard;
        //public double? ExpectedPetrolValue => _transport.PetrolConsumptionStandard;

        private double? _expectedGasValue;
        private double? _expectedPetrolValue;

        public double? ExpectedPetrolValue
        {
            get => _expectedPetrolValue;
            set { _expectedPetrolValue = value; OnPropertyChanged(); } 
        
        }

        public double? ExpectedGasValue
        {
            get => _expectedGasValue;
            set { _expectedGasValue = value; OnPropertyChanged(); }

        }

        private double? _usedGasValue;
        public double? UsedGasValue
        {
            get => _usedGasValue;
            set { _usedGasValue = value; OnPropertyChanged(); }
        }

        private double? _usedPetrolValue;
        public double? UsedPetrolValue
        {
            get => _usedPetrolValue;
            set { _usedPetrolValue = value; OnPropertyChanged(); }
        }

        private double? _usedDieselValue;
        public double? UsedDieselValue
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

        private double? _additionalGasValue;
        public double? AdditionalGasValue
        {
            get => _additionalGasValue;
            set { _additionalGasValue = value; OnPropertyChanged(); }
        }

        private double? _additionalPetrolValue;
        public double? AdditionalPetrolValue
        {
            get => _additionalPetrolValue;
            set { _additionalPetrolValue = value; OnPropertyChanged(); }
        }

        private double? _additionalDieselValue;
        public double? AdditionalDieselValue
        {
            get => _additionalDieselValue;
            set { _additionalDieselValue = value; OnPropertyChanged(); }
        }

        public event Action OnCloseRequested;

        // --- Конструктор ---

        public AddCarPropertiesViewModel(
            TransportProperties transport,
            Action<DemonstrationCardProperties> onSave)
        {
            _transport = transport;
            _onSave = onSave;

            // Заполняем значения по умолчанию из транспорта
            FirstDriver = transport.FirstDriverFullName ?? "";
            SecondDriver = transport.SecondDriverFullName ?? "";
            AddRecordCommand = new RelayCommand(AddRecocd);
            CancelCommand = new RelayCommand(Cancel);
        }

        // --- Команды ---

        private void AddRecocd()
        {
            var newRecord = new DemonstrationCardProperties
            {
                DayNumber = DayNumber ?? 0,
                WaySheet = WaySheet ?? 0,
                FirstDriver = FirstDriver,
                SecondDriver = SecondDriver,
                GetGas = GetGas ?? 0,
                GetPetrol = GetPetrol ?? 0,
                GetDiesel = GetDiesel ??0,
                RemaindDayKilometrageValue = (int?)RemaindDayKilometrageValue??0,
                GasConsumptionStandard = GasConsumptionStandard??0,
                PetrolConsumptionStandard = PetrolConsumptionStandard??0,
                UsedGasValue = UsedGasValue??0,
                UsedPetrolValue = UsedPetrolValue ?? 0,
                UsedDieselValue = UsedDieselValue ?? 0,
                AdditionalToolBrand = AdditionalToolBrand,
                AdditionalGasValue = AdditionalGasValue ?? 0,
                AdditionalPetrolValue = AdditionalPetrolValue ?? 0,
                AdditionalDieselValue = AdditionalDieselValue ?? 0,
                ExpectedGasValue = ExpectedGasValue ?? 0, 
                ExpectedPetrolValue = ExpectedPetrolValue ?? 0, 
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

      private void RecalculateDependentValues()
        {
            //MessageBox.Show(GasConsumptionStandard.ToString()); // стандарт бензина для транспорта. На основе его производить расчет

            //MessageBox.Show(PetrolConsumptionStandard.ToString()); // стандарт бензина для транспорта. На основе его производить расчет

            if (RemaindDayKilometrageValue.HasValue && RemaindDayKilometrageValue.Value > 0)
            {
               ExpectedGasValue = Parameters.RoundUpTwoDecimals((double)(RemaindDayKilometrageValue.Value * GasConsumptionStandard / 100.0));
               ExpectedPetrolValue = Parameters.RoundUpTwoDecimals((double)(RemaindDayKilometrageValue.Value * PetrolConsumptionStandard / 100.0));
            }
            else
            {
                ExpectedGasValue = null;
                ExpectedPetrolValue = null;
            }

            //Расчеты верны !!!
            //MessageBox.Show("ExpectedPetrolValue: " + RemaindDayKilometrageValue.ToString() + " * " + PetrolConsumptionStandard + " / 100 = " + ExpectedPetrolValue.ToString());
            //MessageBox.Show("ExpectedGasValue: " + RemaindDayKilometrageValue.ToString() + " * " + GasConsumptionStandard + " / 100 = " + ExpectedGasValue.ToString());
        }


        public void GoToMain()
        {
            var main = new MainWindow();
            main.Show();
            OnCloseRequested?.Invoke();
        }
        // --- INotifyPropertyChanged ---

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
