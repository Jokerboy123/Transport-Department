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

            DataBaseInitializer.InitializeDataBase();

            var mainWindow = new MainWindow();
            mainWindow.DataContext = this;
            
        }
    }

}
