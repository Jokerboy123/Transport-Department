using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace TransportDepartmentMVVM.Models
{
    public class TransportProperties
    {
        // Общая информация
        public string StateNumber { get; set; }
        public string TransportBrand { get; set; }
        public string FirstDriverFullName { get; set; }
        public string SecondDriverFullName { get; set; }
        public string Region { get; set; }
        public string Additions { get; set; }
        public string Month {  get; set; }
        public int Year {  get; set; }

        // Стандарты расходов
        public double GasConsumptionStandard {  get; set; }
        public double PetrolConsumptionStandard { get; set; }
        public double DieselConsumptionStandard { get; set; }

        // Показания одометра
        public double MonthBeginningOdometerValue {  get; set; }
        public double MonthEndingOdometerValue { get; set; }

        // Состояния показателей газа
        public double MonthBeginningGasState {  get; set; }
        public double MonthEndingGasState {  get; set; }

        // Cостояние показателей бензина
        public double MonthBeginningPetrolState {  get; set; }
        public double MonthEndingPetrolState { get; set; }

        // Cостояние показателей дизеля
        public double MonthBeginningDieselState { get; set; }
        public double MonthEndingDieselState { get; set; }
    }
}
