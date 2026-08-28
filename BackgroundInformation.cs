using System.Collections.ObjectModel;
using System.Linq;

namespace TransportDepartment
{
    public class BackgroundInformation
    {
        // ObservableCollection — таблица WPF сама обновляется при добавлении/удалении
        public ObservableCollection<TransportProperties> GeorgievskTransport { get; set; }
        private int _nextId;

        public BackgroundInformation()
        {
            GeorgievskTransport = new ObservableCollection<TransportProperties>
            {
                new TransportProperties()
                {
                    TransportBrand = "УАЗ-3909",
                    ID = "М 671 ЕМ",
                    GasConsumptionStandard = 26.0,
                    PetrolConsumptionStandard = 19.7,
                    MonthBeginningOdometerValue = 339214,
                    MonthEndingOdometerValue = 339214,
                    MonthBeginningGasState = 3.25,
                    MonthBeginningPetrolState = 0,
                    MonthBeginningDieselState = 0,
                    MonthEndingGasState = 3.25,
                    MonthEndingPetrolState = 0,
                    MonthEndingDieselState = 0,
                    DriverFullName = "Арушанян А.Э.",
                    Additions = "CAK"
                },
                new TransportProperties()
                {
                    TransportBrand = "УАЗ-3909",
                    ID = "М 092 ЕМ",
                    GasConsumptionStandard = 26.0,
                    PetrolConsumptionStandard = 19.7,
                    MonthBeginningOdometerValue = 56678,
                    MonthEndingOdometerValue = 56678,
                    MonthBeginningGasState = 4.1,
                    MonthBeginningPetrolState = 0,
                    MonthBeginningDieselState = 0,
                    MonthEndingGasState = 4.1,
                    MonthEndingPetrolState = 0,
                    MonthEndingDieselState = 0,
                    DriverFullName = "Арушанян Г.А.",
                    Additions = "Компрессор"
                },
                new TransportProperties()
                {
                    TransportBrand = "ГАЗ-3110",
                    ID = "М 093 ЕМ",
                    GasConsumptionStandard = 18.7,
                    PetrolConsumptionStandard = 14.20,
                    MonthBeginningOdometerValue = 562552,
                    MonthEndingOdometerValue = 562552,
                    MonthBeginningGasState = 9.0,
                    MonthBeginningPetrolState = 0,
                    MonthBeginningDieselState = 0,
                    MonthEndingGasState = 9,
                    MonthEndingPetrolState = 0,
                    MonthEndingDieselState = 0,
                    DriverFullName = "Аршинов В.Н.",
                    Additions = "Газонокосилка"
                }
            };
        }

        //// === ДОБАВИТЬ ===

        //public TransportProperties AddRoute(string routeNumber)
        //{
        //    var route = new TransportProperties(_nextId++, routeNumber);
        //    GeorgievskTransport.Add(route);
        //    return route;
        //}

        //// === УДАЛИТЬ ===

        //public bool DeleteRoute(TransportProperties route)
        //{
        //    return GeorgievskTransport.Remove(route);
        //}

        // === ПОИСК ===

       
    }
}
