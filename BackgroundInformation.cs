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
                new TransportProperties(1, "1")
                {
                    RouteName = "ул. Ермолова — Мясокомбинат",
                    TransportType = "Автобус",
                    Carrier = "ИП Акопян Г.З.",
                    StartPoint = "ул. Ермолова",
                    EndPoint = "Мясокомбинат",
                    StopsCount = 14,
                    FirstDeparture = "6:00",
                    LastDeparture = "19:00",
                    WeekendSchedule = false,
                    Fare = 30,
                    IntervalMinutes = 30,
                    Comment = "через ж.д. вокзал, автовокзал"
                },
                new TransportProperties(2, "2")
                {
                    RouteName = "Кожзавод — Мелькомбинат",
                    TransportType = "Автобус",
                    Carrier = "ИП Акопян Г.З.",
                    StartPoint = "Кожзавод",
                    EndPoint = "Мелькомбинат",
                    StopsCount = 13,
                    FirstDeparture = "5:49",
                    LastDeparture = "19:41",
                    WeekendSchedule = true,
                    Fare = 30,
                    IntervalMinutes = 30,
                    Comment = "через Площадь Победы, Университет"
                },
                new TransportProperties(3, "6")
                {
                    RouteName = "пер. Юго-Западный — Спиртзавод",
                    TransportType = "Автобус",
                    Carrier = "ИП Акопян Г.З.",
                    StartPoint = "пер. Юго-Западный",
                    EndPoint = "Спиртзавод",
                    StopsCount = 20,
                    FirstDeparture = "5:45",
                    LastDeparture = "19:00",
                    WeekendSchedule = false,
                    Fare = 30,
                    IntervalMinutes = 40,
                    Comment = "через автовокзал, АрЗиЛ"
                },
                new TransportProperties(4, "7")
                {
                    RouteName = "Объездная дорога — в/ч 41477",
                    TransportType = "Автобус",
                    Carrier = "ИП Акопян Г.З.",
                    StartPoint = "Объездная дорога",
                    EndPoint = "в/ч 41477",
                    StopsCount = 25,
                    FirstDeparture = "6:00",
                    LastDeparture = "18:30",
                    WeekendSchedule = false,
                    Fare = 30,
                    IntervalMinutes = 60,
                    Comment = "кольцевой, через Винзавод, Пятигорскую"
                },
                new TransportProperties(5, "8")
                {
                    RouteName = "Больница — Заготзерно",
                    TransportType = "Автобус",
                    Carrier = "ИП Акопян Г.З.",
                    StartPoint = "Больница",
                    EndPoint = "Заготзерно",
                    StopsCount = 18,
                    FirstDeparture = "4:49",
                    LastDeparture = "18:04",
                    WeekendSchedule = true,
                    Fare = 30,
                    IntervalMinutes = 30,
                    Comment = "через ж.д. вокзал, Молокозавод"
                },
                new TransportProperties(6, "10")
                {
                    RouteName = "Госпиталь — Водолечебница",
                    TransportType = "Автобус",
                    Carrier = "ИП Восканян М.Б.",
                    StartPoint = "Госпиталь",
                    EndPoint = "Водолечебница",
                    StopsCount = 11,
                    FirstDeparture = "5:51",
                    LastDeparture = "19:00",
                    WeekendSchedule = false,
                    Fare = 30,
                    IntervalMinutes = 40,
                    Comment = "через Площадь Победы, Озеро"
                }
            };
            _nextId = 7;
        }

        // === ДОБАВИТЬ ===

        public TransportProperties AddRoute(string routeNumber)
        {
            var route = new TransportProperties(_nextId++, routeNumber);
            GeorgievskTransport.Add(route);
            return route;
        }

        // === УДАЛИТЬ ===

        public bool DeleteRoute(TransportProperties route)
        {
            return GeorgievskTransport.Remove(route);
        }

        // === ПОИСК ===

        public TransportProperties GetById(int id)
        {
            return GeorgievskTransport.FirstOrDefault(x => x.Id == id);
        }
    }
}
