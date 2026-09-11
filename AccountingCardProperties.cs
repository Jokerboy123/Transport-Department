using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TransportDepartment
{
    public class AccountingCardProperties : INotifyPropertyChanged
    {
        // Обязательные поля (NOT NULL)
        public int? DayNumber { get; set; }           // Число месяца
        public int? WaySheet { get; set; }            // № путевого листа
        public string FirstDriver { get; set; }     // Водитель №1

        // Опциональные поля
        public string SecondDriver { get; set; }    // Водитель №2

        // Выдано (литры)
        public double? GetGas { get; set; }
        public double? GetPetrol { get; set; }
        public double? GetDiesel { get; set; }

        // Нормы расхода (копируем из карточки авто при создании)
        public double? GasConsumptionStandard { get; set; }
        public double? PetrolConsumptionStandard { get; set; }
        public double? DieselConsumptionStandard { get; set; }

        // Расход (фактический)
        public double? UsedGasValue { get; set; }
        public double? UsedPetrolValue { get; set; }
        public double? UsedDieselValue { get; set; }

        // Вспомогательное оборудование
        public string AdditionalToolBrand { get; set; }
        public double? AdditionalGasValue { get; set; }
        public double? AdditionalPetrolValue { get; set; }
        public double? AdditionalDieselValue { get; set; }

        // Пробег и остатки на конец дня
        public int? RemaindDayKilometrageValue { get; set; } // Спидометр (км)
        public double? RemaindDayGasValue { get; set; }
        public double? RemaindDayPetrolValue { get; set; }
        public double? RemaindDayDieselValue { get; set; }

        // Пробег и остатки на конец месяца
        public int? RemaindMonthKilometrageValue { get; set; } // Спидометр (км)
        public double? RemaindMonthGasValue { get; set; }
        public double? RemaindMonthPetrolValue { get; set; }
        public double? RemaindMonthDieselValue { get; set; }


        // Связь с машиной (ОБЯЗАТЕЛЬНО для выборки данных)
        public string TransportStateNumber { get; set; }
        private bool _hasValidationError;
        public bool HasValidationError
        {
            get => _hasValidationError;
            set
            {
                if (_hasValidationError = value)
                    return;
                _hasValidationError = value;
                OnPropertyChanged();
            }
        }

        public decimal? ExpectedPetrolValue { get; internal set; }
        public decimal? ExpectedGasValue { get; internal set; }

        public bool Validate()
        {
           bool IsDayNumberEmpty = !DayNumber.HasValue || DayNumber <= 0 || DayNumber > 31;
            bool IsFirstDriverEmpty = string.IsNullOrWhiteSpace(FirstDriver);

            bool  HasValidationError = IsDayNumberEmpty || IsFirstDriverEmpty;

            // Временная отладка — покажет, что именно не заполнено
            //if (HasValidationError)
            //{
            //    var errors = new List<string>();
            //    if (IsDayNumberEmpty) errors.Add("DayNumber пустой или невалидный");
            //    if (IsFirstDriverEmpty) errors.Add("FirstDriver пустой");

            //    MessageBox.Show(string.Join("\n", errors), "Отладка валидации");
            //}

            return !HasValidationError;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
