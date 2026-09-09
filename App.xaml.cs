using System.Configuration;
using System.Data;
using System.Windows;
using TransportDepartment;

namespace TransportDepartment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // ЭТА СТРОКА СОЗДАЕТ БАЗУ И ТАБЛИЦЫ, ЕСЛИ ИХ НЕТ
            DataBaseInitializer.EnsureDatabaseStructure();
        

        var mainWindow = new MainWindow();
            mainWindow.DataContext = this;
            
        }
    }

}
