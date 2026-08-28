using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TransportDepartment
{
    public class TransportProperties : INotifyPropertyChanged
    {
        // Приватные поля — просто хранилище
        private string _stateNumber;
        private string _transportBrand;
        private double _gasConsumptionStandard;
        private double _petrolConsumptionStandard; 
        private double _dieselConsumptionStandard; 
        private double _monthBeginningOdometerValue;
        private double _monthEndingOdometerValue;
        private double _monthBeginningGasState;
        private double _monthEndingGasState;
        private double _monthBeginningPetrolState;
        private double _monthEndingPetrolState;
        private double _monthBeginningDieselState;
        private double _monthEndingDieselState;
        private string _driverFullName;
        private string _additions;

        // Публичные свойства — всё с public set для быстрой проверки
        public string ID
        {
            get => _stateNumber;
            set { _stateNumber = value; OnPropertyChanged(); }
        }

        public string TransportBrand
        {
            get => _transportBrand;
            set { _transportBrand = value; OnPropertyChanged(); }
        }

        public double GasConsumptionStandard
        {
            get => _gasConsumptionStandard;
            set { _gasConsumptionStandard = value; OnPropertyChanged(); }
        }

        public double PetrolConsumptionStandard
        {
            get => _petrolConsumptionStandard;
            set { _petrolConsumptionStandard = value; OnPropertyChanged(); }
        }

        public double DieselConsumptionStandard
        {
            get => _dieselConsumptionStandard;
            set { _dieselConsumptionStandard = value; OnPropertyChanged(); }
        }

        public double MonthBeginningOdometerValue
        {
            get => _monthBeginningOdometerValue;
            set { _monthBeginningOdometerValue = value; OnPropertyChanged(); }
        }

        public double MonthEndingOdometerValue
        {
            get => _monthEndingOdometerValue;
            set { _monthEndingOdometerValue = value; OnPropertyChanged(); }
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
            set { _monthBeginningPetrolState = value; OnPropertyChanged(); }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
