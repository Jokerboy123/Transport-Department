using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Reflection;
using System.Windows;

namespace TransportDepartment
{
    class DataBaseInitializer
    {
        public static string GetConnectionString()
        {

            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? ".";
            var dbPath = Path.Combine(exePath, "TransportInformation.db");
            return $"Data Source={dbPath}";
        }

        public static void InitializeDataBase()
        {
            string connStr = GetConnectionString();

            try {

                using var connection = new SqliteConnection(GetConnectionString());
                connection.Open();

                // Cоздание таблицы БД
                // Создаю две таблицы - одна таблица - информация о транспортных средствах,
                // вторая таблица - для вывода карточки учета автомобиля на экран
                var sqlScriptTransportInformation = @"
                    CREATE TABLE IF NOT EXISTS TransportInformation (
                        TransportBrand TEXT NOT NULL,
                        TransportStateNumber TEXT PRIMARY KEY NOT NULL,
                        GasConsumptionStandard REAL NOT NULL,
                        PetrolConsumptionStandart REAL NOT NULL,
                        MonthBeginningOdometerValue REAL,
                        MonthEndingOdometerValue REAL,
                        MonthBeginningGasState REAL,
                        MonthBeginningPetrolState REAL,
                        MonthBeginningDieselState REAL,
                        MonthEndingGasState REAL,
                        MonthEndingPetrolState REAL,
                        MonthEndingDieselState REAL,
                        DriverFullName TEXT,
                        Additions TEXT
                    );";

                var sqlScriptAccountingCard = @"                    
                    CREATE TABLE IF NOT EXISTS AccountingCard (
                        DayNumber INT NOT NULL,
                        WaySheet INT NOT NULL,
                        FirstDriver TEXT NOT NULL,
                        SecondDriver TEXT,
                        GetGas REAL, 
                        GetPetrol REAL,
                        GetDiesel REAL,
                        MonthBeginningOdometerValue REAL,
                        GasConsumptionStandard REAL NOT NULL,
                        PetrolConsumptionStandart REAL NOT NULL,
                        UsedGasValue REAL,
                        UsedPetrolValue REAL,
                        UsedDieselValue REAL,
                        AdditionalToolBrand TEXT,
                        AdditionalGasValue REAL,
                        AdditionalPetrolValue REAL,
                        AdditionalDieselValue REAL,
                        RemaindDayKilometrageValue INT,
                        RemaindDayGasValue REAL,
                        RemaindDayPetrolValue REAL,
                        RemaindDayDieselValue REAL
);";

                using var cmd = connection.CreateCommand();
                cmd.CommandText = sqlScriptTransportInformation;
                cmd.ExecuteNonQuery();

                cmd.CommandText = sqlScriptAccountingCard;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // КРИТИЧЕСКИ ВАЖНО: Теперь здесь будет настоящая ошибка SQL, а не ошибка подключения
                MessageBox.Show($"Ошибка БД: {ex.Message}");
                MessageBox.Show($"Не удалось инициализировать БД:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public static string GetTransportBrandByStateNumber(string stateNumber)
        {
            string connStr = DataBaseInitializer.GetConnectionString();

            using var connection = new SqliteConnection(connStr);
            connection.Open();

            string sql = @"SELECT TransportBrand 
                    FROM TransportInformation 
                    WHERE TransportStateNumber = @stateNumber";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("@stateNumber", stateNumber);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // Проверяем, что значение не NULL
                if (!reader.IsDBNull(reader.GetOrdinal("TransportBrand")))
                    return reader.GetString(reader.GetOrdinal("TransportBrand"));
            }

            return null; // или пустая строка, или выбросить исключение — на твой выбор
        }


    }
}
