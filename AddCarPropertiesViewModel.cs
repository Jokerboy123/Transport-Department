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
            //MessageBox.Show($"Нормы из транспорта: Gas={_transport.GasConsumptionStandard}, Petrol={_transport.PetrolConsumptionStandard}");
            //MessageBox.Show($"Транспорт: {transport.TransportBrand} {transport.StateNumber}, " +
            //                                $"Норма СУГ: {transport.GasConsumptionStandard}, " +
            //                                $"Норма бензин: {transport.PetrolConsumptionStandard}");
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
                _remaindDayKilometrageValue = value;
                OnPropertyChanged();

                if (!value.HasValue || value < 0)
                {
                    ExpectedGasValue = null;
                    ExpectedPetrolValue = null;
                    return;
                }

                var remaind = value.Value;
                // Округление строго вверх до 2 знаков (как Excel ОКРУГЛВВЕРХ)
                ExpectedGasValue = RoundUpTo2Decimals(
                    (decimal)(((decimal)remaind * (decimal)_transport.GasConsumptionStandard / 100m)));

                ExpectedPetrolValue = RoundUpTo2Decimals(
                    (decimal)((decimal)remaind * (decimal)_transport.PetrolConsumptionStandard / 100m));
            } 
        }

        private static decimal RoundUpTo2Decimals(decimal value)
        {
            const decimal factor = 100m;
            return Math.Ceiling(value * factor) / factor;
        }

    
        

        // --- Расчётные поля ---

        private decimal? _expectedGasValue;
        public decimal? ExpectedGasValue
        {
            get => _expectedGasValue;
            set { _expectedGasValue = value; OnPropertyChanged(); }
        }

        // Свойство специально для отображения
      


        private decimal? _expectedPetrolValue;
        public decimal? ExpectedPetrolValue
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

     




     
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
