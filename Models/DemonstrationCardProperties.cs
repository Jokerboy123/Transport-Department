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
        public double GasConsumptionStandard { get; set; }
        public double PetrolConsumptionStandard { get; set; }
        public double DieselConsumptionStandard { get; set; }

        public double UsedGasValue { get; set; }
        public double UsedPetrolValue { get; set; }
        public double UsedDieselValue { get; set; }

        public string? AdditionalToolBrand { get; set; }
        public double AdditionalGasValue { get; set; }
        public double AdditionalPetrolValue { get; set; }
        public double AdditionalDieselValue { get; set; }

        public int RemaindDayKilometrageValue { get; set; }
        public double RemaindDayGasValue { get; set; }
        public double RemaindDayPetrolValue { get; set; }
        public double RemaindDayDieselValue { get; set; }
        public double ExpectedGasValue { get; set; }
        public double ExpectedPetrolValue { get; set; }
        public double ExpectedDieselValue { get; set; }

        public string? Region { get; set; }
        public string? TransportStateNumber { get; set; }

        // Эти поля мы считаем в репозитории, но они нужны в классе
        public double RemaindMonthGasValue { get; set; }
        public double RemaindMonthPetrolValue { get; set; }
        public double RemaindMonthDieselValue { get; set; }
        public double RemaindMonthKilometrageValue { get; set; }
    }
}
