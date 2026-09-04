using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TransportDepartment
{
    public class AccountingCardProperties : INotifyPropertyChanged
    {
        // объявить поля класса
        // === НЕИЗМЕНЯЕМЫЕ ПОЛЯ  ОБОЗНАЧАЮТСЯ private set ===
        // === ИЗМЕНЯЕМЫЕ ПОЛЯ ОБОЗНАЧАЮТСЯ public set ===
        public int dayOfMonth { get; set; }
        public int waySheet { get; set; }
        public string firstDriver {  get; set; }
        public string secondDriver { get; set; }
        public double getGas {  get; set; }
        public double getPetrol { get; set; }
        public double getDiesel {  get; set; }
        public double monthBeginningOdometerValue {  get; set; }
        public double gasConsumptionStandard {  get; set; }
        public double petrolConsumptionStandard { get; set; }
        public double dieselConsumptionStandard { get; set; }


        // Поля для шапки
        private string _month { get; set; }
        private int _year { get; set; }

        public string Month
        {
            get => _month;
            set {  _month = value; }
        }

        public int Year
        {
            get => _year;
            set { _year = value; }
        }




        /*
CREATE TABLE IF NOT EXISTS AccountingCard (
                        DayOfMonth INT NOT NULL,
                        WaySheet INT NOT NULL,
                        FirstDriver TEXT NOT NULL,
                        SecondDriver TEXT,
                        GetGas REAL, 
                        GetPetrol REAL,
                        GetDiesel REAL,
                        MonthBeginningOdometerValue REAL,
                        GasConsumptionStandard REAL NOT NULL,
                        PetrolConsumptionStandard REAL NOT NULL,
                        UsedGasValue REAL,
                        UsedPetrolValue REAL,
                        UsedDieselValue REAL,
                        AdditionalToolBrand TEXT,
                        AdditionalGasValue REAL,
                        AdditionalPetrolValue REAL,
                        AdditionalDieselValue REAL,
                        RemaindDayKilometrageValue INT,
                        RemaindDayGasValue REAL,
                        RemaindDayPetrolValue REAL,
                        RemaindDayDieselValue REAL
);";
        пример: 
          private string _additions { get; set; } // Вспомогательное оборудование

        public string ID
        {
            get => _stateNumber;
            set { _stateNumber = value; OnPropertyChanged(); }
        }

        */
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
    