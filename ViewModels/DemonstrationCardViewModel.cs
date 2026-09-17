using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Services;
using TransportDepartmentMVVM.Views;

namespace TransportDepartmentMVVM.ViewModels
{
    public class DemonstrationCardViewModel : INotifyPropertyChanged
    {
        private readonly string _regionIndex;
        private readonly TransportProperties _transport;
        private readonly DemonstrationCardProperties _acp;
        private readonly TransportRepository _repo;

        // ДОБАВИТЬ ЭТО:
        public object PreviousViewModel { get; private set; }

        public RelayCommand AddRecordCommand { get; }

        // Команда "Вернуться на главный"
        public RelayCommand GoToMainCommand { get; }
        public RelayCommand GoToPreviousCommand { get; }

        // Команда "Печать"
        public RelayCommand PrintCommand { get; }


        public TransportProperties CurrentTransport => _transport;
        public DemonstrationCardProperties DemonstrationCardProperties => _acp;

        public ObservableCollection<DemonstrationCardProperties> Records { get; set; }

        public event Action OnCloseRequested;

        public DemonstrationCardViewModel(TransportProperties transport, string regionIndex, object previousViewModel)
        {
            _transport = transport;
            _regionIndex = regionIndex;
            _acp = new DemonstrationCardProperties();
            _repo = new TransportRepository();

            // ДОБАВИТЬ ЭТО:
            PreviousViewModel = previousViewModel;


            _transport.MonthBeginningOdometerValue = 10000;

            Records = new ObservableCollection<DemonstrationCardProperties>();


            // Команды
            AddRecordCommand = new RelayCommand(() => AddRecord());
            GoToMainCommand = new RelayCommand(() => GoToMain());
            GoToPreviousCommand = new RelayCommand(() => GoToPrevious());
            PrintCommand = new RelayCommand(() => { /* пока пусто */ });
        
            LoadData();
        }
        private void GoToMain()
        {
            var main = new MainWindow();
            main.Show();
            main.Activate();
            OnCloseRequested?.Invoke(); // закрывает текущее окно

        }


        private void GoToPrevious()
        {
            OnCloseRequested?.Invoke();

            var type = PreviousViewModel?.GetType().Name ?? "NULL";
            MessageBox.Show($"PreviousViewModel = {type}");

            Window win = null;
            switch (PreviousViewModel)
            {
                case AccountGeorgievskViewModel geoVM:
                    win = new AccountGeorgievsk(geoVM.MainWindow, geoVM.RegionIndex);
                    win.DataContext = geoVM;
                    break;

                case AccountGeorgievskDistrictViewModel geoDistVM:
                    win = new AccountGeorgievskDistrict(geoDistVM.MainWindow, geoDistVM.RegionIndex);
                    win.DataContext = geoDistVM;
                    break;

                case AccountKirovskDistrictViewModel kirVM:
                    win = new AccountKirovskDistrict(kirVM.MainWindow, kirVM.RegionIndex);
                    win.DataContext = kirVM;
                    break;

                default:
                    // Если тип не распознан — ничего не делаем или можно показать ошибку
                    return;
            }

            if (win != null)
                win.Show();
        }




        private void LoadData()
        {
            // Было: DataBaseInitializer.GetRecordsByCar(...)
            // Стало:
            var recordsFromDb = _repo.GetRecordsByCar(_transport.StateNumber);

            foreach (var record in recordsFromDb)
            {
                if (record.GasConsumptionStandard == 0)
                    record.GasConsumptionStandard = _transport.GasConsumptionStandard;
                if (record.PetrolConsumptionStandard == 0)
                    record.PetrolConsumptionStandard = _transport.PetrolConsumptionStandard;
                if (record.DieselConsumptionStandard == 0)
                    record.DieselConsumptionStandard = _transport.DieselConsumptionStandard;

                Records.Add(record);
            }
        }

        private void AddRecord()
        {
            // 1. Создаём ViewModel для окна добавления
            // Важно: передаём AddNewRecord как колбэк — он сработает, когда нажмут «Сохранить» в окне
            var addVm = new AddCarPropertiesViewModel(_transport, AddNewRecord);

            // 2. Создаём окно и передаём туда ViewModel
            var window = new AddCarProperties(addVm);

            // 3. Показываем как модальное окно
            window.ShowDialog();
            // После закрытия окна выполнение кода продолжится здесь
        }

        // Этот метод должен быть приватным и принимать тот тип, который возвращает окно
        private void AddNewRecord(DemonstrationCardProperties record)
        {
            if (record == null) return;

            record.TransportStateNumber = _transport.StateNumber;

            // Сначала добавляем в коллекцию UI
            Records.Add(record);

            try
            {
                // Потом сохраняем в БД
                _repo.InsertRecord(record);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в БД:\n{ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                // Если ошибка БД — удаляем из UI, чтобы не было «фантомной» записи
                Records.Remove(record);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
