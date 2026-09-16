using System.Windows;
using TransportDepartmentMVVM.Models;

namespace TransportDepartmentMVVM.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly Window _mainWindow;
        public string regionIndex = "";

        public RelayCommand OpenGeorgievskCommand { get; }
        public RelayCommand OpenGeorgievskDistrictCommand { get; }
        public RelayCommand OpenKirovskCommand { get; }
        public RelayCommand OpenParametersCommand { get; }
        public RelayCommand CloseApplicationCommand { get; }

        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;

            OpenGeorgievskCommand = new RelayCommand(() =>
            {
                var w = new AccountGeorgievsk(_mainWindow, "GEO");
                _mainWindow.Hide();
                w.Show();
            });

            OpenGeorgievskDistrictCommand = new RelayCommand(() =>
            {
                var w = new AccountGeorgievskDistrict(_mainWindow, "GEODSTRCT");
                _mainWindow.Hide();
                w.Show();
            });

            OpenKirovskCommand = new RelayCommand(() =>
            {
                var w = new AccountKirovskDistrict(_mainWindow, "KIR");
                _mainWindow.Hide();
                w.Show();
            });

            OpenParametersCommand = new RelayCommand(() =>
            {
                var w = new ParametersWindow();
                w.Show();
            });

            CloseApplicationCommand = new RelayCommand(() => Application.Current.Shutdown());
        }
    }
}
