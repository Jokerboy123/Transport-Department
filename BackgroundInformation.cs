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
