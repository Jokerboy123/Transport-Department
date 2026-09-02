using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Windows;

namespace TransportDepartment
{
    public class DataBaseInitializer
    {
        public static string GetConnectionString()
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "TransportInformation.db");

            // Временно добавь это для диагностики
            if (!File.Exists(dbPath))
            {
                System.Diagnostics.Debug.WriteLine($"!!! Файл БД не найден по пути: {dbPath}");
                MessageBox.Show($"Файл БД не найден по пути: {dbPath}",
                    "Диагностика", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return $"Data Source={dbPath}";
        }


        public static void InitializeDataBase()
        {
            string connStr = GetConnectionString();
            // Проверка: сколько строк в таблице
           

            try
            {
                using var connection = new SQLiteConnection(connStr);
                connection.Open();

                // Скрипт создания таблиц (без изменений)
                var SQLScriptTransport = @"CREATE TABLE IF NOT EXISTS TransportInformation (
            TransportBrand TEXT NOT NULL,
            TransportStateNumber TEXT PRIMARY KEY NOT NULL,
            GasConsumptionStandard REAL NOT NULL,
            PetrolConsumptionStandard REAL NOT NULL,
            MonthBeginningOdometerValue REAL,
            MonthEndingOdometerValue REAL,
            MonthBeginningGasState REAL,
            MonthBeginningPetrolState REAL,
            MonthBeginningDieselState REAL,
            MonthEndingGasState REAL,
            MonthEndingPetrolState REAL,
            MonthEndingDieselState REAL,
            DriverFullName TEXT,
            Additions TEXT,
            Region TEXT
        );";

                var SQLScriptAccounting = @"CREATE TABLE IF NOT EXISTS AccountingCard (
            DayNumber INT NOT NULL,
            WaySheet INT NOT NULL,
            FirstDriver TEXT NOT NULL,
            SecondDriver TEXT,
            GetGas REAL, GetPetrol REAL, GetDiesel REAL,
            MonthBeginningOdometerValue REAL,
            GasConsumptionStandard REAL NOT NULL,
            PetrolConsumptionStandard REAL NOT NULL,
            UsedGasValue REAL, UsedPetrolValue REAL, UsedDieselValue REAL,
            AdditionalToolBrand TEXT,
            AdditionalGasValue REAL, AdditionalPetrolValue REAL, AdditionalDieselValue REAL,
            RemaindDayKilometrageValue INT,
            RemaindDayGasValue REAL, RemaindDayPetrolValue REAL, RemaindDayDieselValue REAL,
            Region TEXT
        );";

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM TransportInformation";
                long count = (long)cmd.ExecuteScalar();

                cmd.CommandText = SQLScriptTransport;
                cmd.ExecuteNonQuery();
                cmd.CommandText = SQLScriptAccounting;
                cmd.ExecuteNonQuery();

                // УБРАЛИ MessageBox! Используем Debug для отладки
                System.Diagnostics.Debug.WriteLine("База данных инициализирована.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка БД: {ex.Message}");
                throw;
            }
        }

        //public class TransportItem
        //{
        //    public string Brand { get; set; }
        //    public string StateNumber { get; set; }
        //}

        public static List<TransportProperties> GetTransportsByRegion(string region)
        {
            var result = new List<TransportProperties>();
            string connStr = GetConnectionString();

            using var connection = new SQLiteConnection(connStr);
            connection.Open();

            // ИСПРАВЛЕНО: Выбираем и марку, и госномер, и фильтруем по колонке Region
            string SQL = @"SELECT TransportBrand, TransportStateNumber 
                   FROM TransportInformation 
                   WHERE Region = @region";

            using var cmd = new SQLiteCommand(SQL, connection);
            cmd.Parameters.AddWithValue("@region", region);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new TransportProperties
                {
                    TransportBrand = reader.GetString(reader.GetOrdinal("TransportBrand")),
                    StateNumber = reader.GetString(reader.GetOrdinal("TransportStateNumber"))
                });
            }

            return result;
        }

        public static string GetTransportBrandByStateNumber(string stateNumber)
        {
            string connStr = DataBaseInitializer.GetConnectionString();

            using var connection = new SQLiteConnection(connStr);
            connection.Open();

            string SQL = @"SELECT TransportBrand 
                    FROM TransportInformation 
                    WHERE TransportStateNumber = @stateNumber";

            using var cmd = new SQLiteCommand(SQL, connection);
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
