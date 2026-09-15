using System.Windows;
using TransportDepartmentMVVM.Models;

namespace TransportDepartmentMVVM.ViewModels
{
    public class MainWindowViewModel
    {
        public RelayCommand OpenGeorgievskCommand { get; }
        public RelayCommand OpenGeorgievskDistrictCommand { get; }
        public RelayCommand OpenKirovskCommand { get; }
        public RelayCommand OpenParametersCommand { get; }
        public RelayCommand CloseApplicationCommand { get; }

        public MainWindowViewModel()
        {
            // Заглушки: просто показываем сообщение, чтобы проверить работу кнопок
            OpenGeorgievskCommand = new RelayCommand(() =>
            {
                var w = new AccountGeorgievsk();
                w.Show();
            });

            OpenGeorgievskDistrictCommand = new RelayCommand(() =>
            {
                var w = new AccountGeorgievskDistrict();
                w.Show();
            });

            OpenKirovskCommand = new RelayCommand(() =>
            {
                var w = new AccountKirovskDistrict();
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
