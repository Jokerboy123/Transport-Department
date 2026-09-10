using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TransportDepartment
{
    public class AddCarPropertiesViewModel : INotifyPropertyChanged
    {
        private readonly TransportProperties _transport;

        public AddCarPropertiesViewModel(TransportProperties transport)
        {
            _transport = transport;
        }

        // --- Поля формы ---

        public int? DayNumber { get; set; }
        public int? WaySheet { get; set; }
        public string FirstDriver { get; set; }
        public string SecondDriver { get; set; }

        public double GetGas { get; set; }
        public double GetPetrol { get; set; }
        public double GetDiesel { get; set; }

        public double UsedGasValue { get; set; }
        public double UsedPetrolValue { get; set; }
        public double UsedDieselValue { get; set; }

        public string AdditionalToolBrand { get; set; }
        public double AdditionalGasValue { get; set; }
        public double AdditionalPetrolValue { get; set; }
        public double AdditionalDieselValue { get; set; }

        // --- Нормы из транспорта (только для чтения) ---

        public double GasConsumptionStandard => _transport.GasConsumptionStandard;
        public double PetrolConsumptionStandard => _transport.PetrolConsumptionStandard;
        public double DieselConsumptionStandard => _transport.DieselConsumptionStandard;

        // --- Пробег (вводит пользователь) ---

        private double _remaindDayKilometrageValue;

        public double RemaindDayKilometrageValue
        {
            get => _remaindDayKilometrageValue;
            set
            {
                if (_remaindDayKilometrageValue != value)
                {
                    _remaindDayKilometrageValue = value;
                    OnPropertyChanged();
                    RecalculateDerivedValues();
                }
            }
        }

        // --- Расчётные поля ---

        private double _expectedGasVolume;
        private double _expectedPetrolVolume;

        public double ExpectedGasVolume
        {
            get => _expectedGasVolume;
            set
            {
                if (_expectedGasVolume != value)
                {
                    _expectedGasVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ExpectedPetrolVolume
        {
            get => _expectedPetrolVolume;
            set
            {
                if (_expectedPetrolVolume != value)
                {
                    _expectedPetrolVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        private void RecalculateDerivedValues()
        {
            ExpectedGasVolume = _remaindDayKilometrageValue * _transport.GasConsumptionStandard / 100.0;
            ExpectedPetrolVolume = _remaindDayKilometrageValue * _transport.PetrolConsumptionStandard / 100.0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
