using System;

namespace TransportDepartmentMVVM.Models
{
    public class DemonstrationCardProperties
    {
        public int DayNumber { get; set; }
        public int WaySheet { get; set; }
        public string? FirstDriver { get; set; }
        public string? SecondDriver { get; set; }

        public double GetGas { get; set; }
        public double GetPetrol { get; set; }
        public double GetDiesel { get; set; }

        public double MonthBeginningOdometerValue { get; set; }
        public double GasConsumptionStandard { get; set; } // нормативы для формул, не  рассчитываются
        public double PetrolConsumptionStandard { get; set; } // нормативы для формул, не  рассчитываются
        public double DieselConsumptionStandard { get; set; } // нормативы для формул, не  рассчитываются

        public double UsedGasValue { get; set; }
        public double UsedPetrolValue { get; set; }
        public double UsedDieselValue { get; set; }

        public string? AdditionalToolBrand { get; set; }
        public double AdditionalGasValue { get; set; }
        public double AdditionalPetrolValue { get; set; }
        public double AdditionalDieselValue { get; set; }

        public int RemaindDayKilometrageValue { get; set; }
        public double RemaindDayGasValue { get; set; } // остаток на конец дня
        public double RemaindDayPetrolValue { get; set; } // остаток на конец дня
        public double RemaindDayDieselValue { get; set; } // остаток на конец дня
        public double ExpectedGasValue { get; set; } // рассчитываемые поля по пробегу
        public double ExpectedPetrolValue { get; set; } // рассчитываемые поля по пробегу
        public double ExpectedDieselValue { get; set; } // рассчитываемые поля по пробегу

        public string? Region { get; set; }
        public string? TransportStateNumber { get; set; }

        // Эти поля мы считаем в репозитории, но они нужны в классе
        public double RemaindMonthGasValue { get; set; }
        public double RemaindMonthPetrolValue { get; set; }
        public double RemaindMonthDieselValue { get; set; }
        public double RemaindMonthKilometrageValue { get; set; }
    }
}
