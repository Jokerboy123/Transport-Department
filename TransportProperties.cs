using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TransportDepartment
{
    public class TransportProperties : INotifyPropertyChanged
    {
        // === НЕИЗМЕНЯЕМЫЕ ПОЛЯ  ОБОЗНАЧАЮТСЯ private set ===
        // === ИЗМЕНЯЕМЫЕ ПОЛЯ ОБОЗНАЧАЮТСЯ public set ===
        private string _id { get; set; } // Госномер
        private string _transportBrand { get; set; } // МАРКА АВТОМОБИЛЯ
        private double _gasConsumptionStandard { get; set; } // Норматив расхода газа (неизменяемое)
        private double _petrolConsumptionStandart { get; set; } // Норматив расхода бензина  (неизменяемое)
        private double _dieselConsumptionStandart { get; set; } // Норматив расхода дизеля  (неизменяемое)
        private double _monthBeginningOdometerValue { get; set; } // Показания одометра на начало месяца
        private double _monthEndingOdometerValue { get; set; } // Показания одометра на конец месяца
        private double _monthBeginningGasState { get; set; } // Остаток газа на начало месяца
        private double _monthEndingGasState { get; set; } // Остаток газа на конец месяца
        private double _monthBeginningPetrolState { get; set; } // Остаток бензина на начало месяца
        private double _monthEndingPetrolState { get; set; } // Остаток бензина на конец месяца
        private double _monthBeginningDieselState { get; set; } // Остаток дизеля на начало месяца
        private double _monthEndingDieselState { get; set; } // Остаток дизеля на конец месяца
        private string _driverFullName { get; set; } // Водитель
        private string _additions { get; set; } // Вспомогательное оборудование

        public string ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }
        public string TransportBrand
        {
            get => _transportBrand;
             set { _transportBrand = value; OnPropertyChanged(); }
        }
        public double GasConsumptionStandard
        {
            get => _gasConsumptionStandard;
            set {  _gasConsumptionStandard = value; OnPropertyChanged(); }
        }
        public double PetrolConsumptionStandart
        {
            get => _petrolConsumptionStandart;
             set { _petrolConsumptionStandart = value; OnPropertyChanged(); }
        }
        public double DieselConsumptionStandart
        {
            get => _dieselConsumptionStandart;
             set { _dieselConsumptionStandart = value; OnPropertyChanged(); }
        }
        public double MonthBeginningOdometerValue
        {
            get => _monthBeginningOdometerValue;
            set { _monthBeginningOdometerValue = value; OnPropertyChanged(); }
        }
        public double MonthEndingOdometerValue
        {
            get => _monthEndingOdometerValue;
             set { _monthEndingOdometerValue = value;OnPropertyChanged(); }
        }
        public double MonthBeginningGasState
        {
            get => _monthBeginningGasState;
            set { _monthBeginningGasState = value; OnPropertyChanged(); }
        }
        public double MonthEndingGasState
        {
            get => _monthEndingGasState;
             set { _monthEndingGasState = value; OnPropertyChanged(); }
        }
        public double MonthBeginningPetrolState
        {
            get => _monthBeginningPetrolState;
            set { _monthBeginningPetrolState = value; OnPropertyChanged();  }
        }
        public double MonthEndingPetrolState
        {
            get => _monthEndingPetrolState;
             set { _monthEndingPetrolState = value; OnPropertyChanged(); }
        }
        public double MonthBeginningDieselState
        {
            get => _monthBeginningDieselState;
            set { _monthBeginningDieselState = value; OnPropertyChanged(); }
        }
        public double MonthEndingDieselState
        {
            get => _monthEndingDieselState;
             set { _monthEndingDieselState = value; OnPropertyChanged(); }
        }
        public string DriverFullName
        {
            get => _driverFullName;
            set { _driverFullName = value; OnPropertyChanged(); }
        }
        public string Additions
        {
            get => _additions;
            set { _additions = value; OnPropertyChanged(); }
        }

        // === INotifyPropertyChanged ===

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
