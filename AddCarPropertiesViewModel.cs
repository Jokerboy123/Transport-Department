using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TransportDepartment
{
    public class AddCarPropertiesViewModel : INotifyPropertyChanged
    {
        private readonly TransportProperties _transport;

        public AddCarPropertiesViewModel(TransportProperties transport)
        {
            if (transport == null)
            {
                throw new ArgumentNullException(nameof(transport), "Транспорт не может быть null");
            }
            _transport = DataBaseInitializer.GetTransportByStateNumber(transport.StateNumber);
            MessageBox.Show($"Нормы из транспорта: Gas={_transport.GasConsumptionStandard}, Petrol={_transport.PetrolConsumptionStandard}");
            MessageBox.Show($"Транспорт: {transport.TransportBrand} {transport.StateNumber}, " +
                                            $"Норма СУГ: {transport.GasConsumptionStandard}, " +
                                            $"Норма бензин: {transport.PetrolConsumptionStandard}");
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

        private int? _remaindDayKilometrageValue;

        public int? RemaindDayKilometrageValue
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

        private double _expectedGasValue;
        private double _expectedPetrolValue;

        public double ExpectedGasValue
        {
            get => _expectedGasValue;
            set
            {
                if (_expectedGasValue != value)
                {
                    _expectedGasValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ExpectedPetrolValue
        {
            get => _expectedPetrolValue;
            set
            {
                if (_expectedPetrolValue != value)
                {
                    _expectedPetrolValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private void RecalculateDerivedValues()
        {
            MessageBox.Show(_remaindDayKilometrageValue.ToString());
            MessageBox.Show(_transport.GasConsumptionStandard.ToString());
            MessageBox.Show(_transport.PetrolConsumptionStandard.ToString());

            ExpectedGasValue = (double)(_remaindDayKilometrageValue * _transport.GasConsumptionStandard / 100.0); 
            ExpectedPetrolValue = (double)(_remaindDayKilometrageValue * _transport.PetrolConsumptionStandard / 100.0);
     
            // проверка корректна
            // MessageBox.Show(ExpectedGasValue.ToString());
            // MessageBox.Show(ExpectedPetrolValue.ToString());

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
