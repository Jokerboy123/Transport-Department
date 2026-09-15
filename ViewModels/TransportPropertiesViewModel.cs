using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using TransportDepartmentMVVM.Models;

namespace TransportDepartmentMVVM.ViewModels
{
    public class TransportPropertiesViewModel : INotifyPropertyChanged
    {
        private TransportProperties _model;
        public TransportPropertiesViewModel(TransportProperties model = null)
        {
            _model = model ?? new TransportProperties();
        }

        public string StateNumber
        {
            get => _model.StateNumber;
            set
            {
                _model.StateNumber = value;
                OnPropertyChanged();
            }
        }

        public string TransportBrand
        {
            get => _model.TransportBrand;
            set
            {
                _model.TransportBrand = value;
                OnPropertyChanged();
            }
        }

        public string FirstDriverFullName
        {
            get => _model.FirstDriverFullName;
            set
            {
                _model.FirstDriverFullName = value;
                OnPropertyChanged();
            }
        }

        public string SecondDriverFullName
        {
            get => _model.SecondDriverFullName;
            set
            {
                _model.SecondDriverFullName = value;
                OnPropertyChanged();
            }
        }

        public string Region
        {
            get => _model.Region;
            set
            {
                _model.Region = value;
                OnPropertyChanged();
            }
        }

        public string Additions
        {
            get => _model.Additions;
            set
            {
                _model.Additions = value;
                OnPropertyChanged();
            }
        }

        public string Month
        {
            get => _model.Month;
            set
            {
                _model.Month = value;
                OnPropertyChanged();
            }
        }

        public int Year
        {
            get => _model.Year;
            set
            {
                _model.Year = value;
                OnPropertyChanged();
            }
        }

        public double GasConsumptionStandard
        {
            get => _model.GasConsumptionStandard;
            set
            {
                _model.GasConsumptionStandard = value;
                OnPropertyChanged();
            }
        }

        public double PetrolConsumptionStandard
        {
            get=>_model.PetrolConsumptionStandard;
            set
            {
                _model.PetrolConsumptionStandard = value;
                OnPropertyChanged();
            }
        }

        public double DieselConsumptionStandard
        {
            get => _model.DieselConsumptionStandard;
            set
            {
                _model.DieselConsumptionStandard = value;
                OnPropertyChanged() ;
            }
        }

        public double MonthBeginningOdometerValue
        {
            get => _model.MonthBeginningOdometerValue;
            set
            {
                _model.MonthBeginningOdometerValue = value;
                OnPropertyChanged();
                UpdateDerived();
            }
        }

        public double MonthEndingOdometerValue
        {
            get=>_model.MonthEndingOdometerValue;
            set
            {
                _model.MonthEndingOdometerValue = value;
                OnPropertyChanged();
                UpdateDerived();
            }
        }

        public double MonthBeginningGasState
        {
            get => _model.MonthBeginningGasState;
            set
            {
                _model.MonthBeginningGasState = value;
                OnPropertyChanged();
                UpdateDerived();
            }
        }

        public double MonthEndingGasState
        {
            get => _model.MonthEndingGasState;
            set
            {
                _model.MonthEndingGasState = value;
                OnPropertyChanged();
                UpdateDerived();
            }
        }

        public double MonthBeginningPetrolState
        {
            get=>_model.MonthBeginningPetrolState;
            set
            {
                _model.MonthBeginningPetrolState = value;
                OnPropertyChanged();
                UpdateDerived();
            }
        }

        public double MonthEndingPetrolState
        {
            get => _model.MonthEndingPetrolState;
            set
            {
                _model.MonthEndingPetrolState = value;
                OnPropertyChanged();
                UpdateDerived();
            }
        }

        public double MonthBeginningDieselState
        {
            get => _model.MonthBeginningDieselState;
            set
            {
                _model.MonthBeginningDieselState = value;
                OnPropertyChanged();
                UpdateDerived();
            }
        }

        public double MonthKilometrage =>
            MonthEndingOdometerValue - MonthBeginningOdometerValue;
        private void UpdateDerived()
        {
            OnPropertyChanged(nameof(MonthKilometrage));
            //OnPropertyChanged(nameof(GasActualConsumption));
            //OnPropertyChanged(nameof(PetrolActualConsumption));
            //OnPropertyChanged(nameof(DieselActualConsumption));
        }

        // Доступ к исходной модели (для сохранения в БД)
        public TransportProperties GetModel() => _model;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
