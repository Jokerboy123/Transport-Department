using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Interop;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.ViewModels;

namespace TransportDepartmentMVVM
{
    public class Parameters : INotifyPropertyChanged
    {
        private static readonly Parameters _instance = new Parameters();
        public static Parameters Instance => _instance;
        private string _selectedMonth;
        private string _selectedYear;

        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();
            }
        }

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OpenDemonstrationCard(Window currentWindow, TransportProperties transport)
        {
            var viewModel = currentWindow.DataContext;
            if (viewModel == null)
            {
                MessageBox.Show("Ошибка! Контекст данных не установлен в данном окне!");
                return;
            }
            string regionIndex = "";
            object parentVM = null;

            if (viewModel is AccountGeorgievskViewModel geoVm)
            {
                regionIndex = geoVm.RegionIndex;
                parentVM = geoVm;
            }
            else if (viewModel is AccountGeorgievskDistrictViewModel distVm)
            {
                regionIndex = distVm.RegionIndex;
                parentVM = distVm;
            }
            else if (viewModel is AccountKirovskDistrictViewModel kirVm)
            {
                regionIndex = kirVm.RegionIndex;
                parentVM = kirVm;
            }
            else
            {
                MessageBox.Show($"Неизвестный тип ViewModel: {viewModel.GetType().Name}. Не могу открыть карточку.", "Ошибка типа");
                return;
            }

            var cardVM = new DemonstrationCardViewModel(transport,regionIndex, parentVM);
            var cardWindow = new DemonstrationCard(cardVM);

            // ВАЖНО: Сначала скрываем текущее окно, потом показываем новое
            currentWindow.Hide();
            cardWindow.Show();

        }

        public static double RoundUpTwoDecimals(double value)
        {
            return Math.Ceiling(value * 100) / 100;
        }
    }
}
