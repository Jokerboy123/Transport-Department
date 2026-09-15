using TransportDepartmentMVVM.Data;

namespace TransportDepartmentMVVM
{
    public partial class App : System.Windows.Application
    {
        public App()
        {
            // Создаём структуру БД, если её ещё нет
            DataBaseInitializer.EnsureDataBaseStructure();
        }
    }
}
