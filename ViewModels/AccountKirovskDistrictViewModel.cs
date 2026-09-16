using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TransportDepartmentMVVM.Data;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Services;
using TransportDepartmentMVVM.Views;

namespace TransportDepartmentMVVM.ViewModels
{
    public class AccountKirovskDistrictViewModel : INotifyPropertyChanged
    {
        private readonly TransportRepository _repo;
        private readonly string _regionIndex;


        public ObservableCollection<TransportProperties> Transports { get; set; }

        public ICommand OpenDemonstrationCardCommand { get; }
        public RelayCommand GoToMainCommand { get; }

        public event Action OnCloseRequested;
        public event Action OnHideRequested;
        public event Action OnShowRequested;
        public Window _mainWindow;

        public AccountKirovskDistrictViewModel(Window mainWindow, string regionIndex)
        {
            _mainWindow = mainWindow;
            _repo = new TransportRepository();
            _regionIndex = regionIndex; // теперь регион доступен в этой VM
                                        // Команда БЕЗ <T> — просто передаём метод
            OpenDemonstrationCardCommand = new RelayCommand(OpenDemonstrationCard);
            GoToMainCommand = new RelayCommand(GoToMain);

            LoadTransports();
        }
        public void LoadTransports()

        {
            try
            {
                DataBaseInitializer.EnsureDataBaseStructure();
                string targetRegion = "Кировский район";


                var list = _repo.GetTransportsByRegion(targetRegion);
                Transports = new ObservableCollection<TransportProperties>(list);
                OnPropertyChanged(nameof(Transports));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить данные: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод принимает object — RelayCommand передаст параметр из XAML
        private void OpenDemonstrationCard(object parameter)
        {
            if (parameter is TransportProperties transport)
            {
                OnHideRequested?.Invoke();

                var vm = new DemonstrationCardViewModel(transport);
                var win = new DemonstrationCard(vm);
                win.Show();
            }
        }

        private void GoToMain()
        {
            var main = new MainWindow();
            main.Show();
            OnCloseRequested?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}