using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TransportDepartment
{
    public class AccountingCardViewModel : INotifyPropertyChanged
    {
        private readonly TransportProperties _transport;

        // Данные для шапки (не меняются в таблице)
        public TransportProperties CurrentTransport => _transport;

        // Коллекция для DataGrid (таблица строк)
        public ObservableCollection<AccountingCardProperties> Records { get; set; }

        public AccountingCardViewModel(TransportProperties transport)
        {
            _transport = transport;
            Records = new ObservableCollection<AccountingCardProperties>();
            LoadData();
        }

        private void LoadData()
        {
            // Загружаем строки из БД, привязанные к этому авто
            var recordsFromDb = DataBaseInitializer.GetRecordsByCar(_transport.StateNumber);

            foreach (var record in recordsFromDb)
            {
                // Если в БД нет норм, подставляем из карточки авто
                if (record.GasConsumptionStandard == 0)
                    record.GasConsumptionStandard = _transport.GasConsumptionStandard;
                if (record.PetrolConsumptionStandard == 0)
                    record.PetrolConsumptionStandard = _transport.PetrolConsumptionStandard;
                if (record.DieselConsumptionStandard == 0)
                    record.DieselConsumptionStandard = _transport.DieselConsumptionStandard;

                Records.Add(record);
            }
        }

        public void AddNewRecord(AccountingCardProperties newRecord)
        {
            // Устанавливаем связь с машиной
            newRecord.TransportStateNumber = _transport.StateNumber;

            // Добавляем в UI
            Records.Add(newRecord);

            try
            {
                DataBaseInitializer.InsertRecord(newRecord);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в БД:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Records.Remove(newRecord); // убираем из UI, раз не сохранилось
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
