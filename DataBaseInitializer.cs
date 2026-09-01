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
                using var connection = new SqliteConnection(connStr);
                connection.Open();

                // Скрипт создания таблиц (без изменений)
                var sqlScriptTransport = @"CREATE TABLE IF NOT EXISTS TransportInformation (
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

                var sqlScriptAccounting = @"CREATE TABLE IF NOT EXISTS AccountingCard (
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

                cmd.CommandText = sqlScriptTransport;
                cmd.ExecuteNonQuery();
                cmd.CommandText = sqlScriptAccounting;
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

        // ИСПРАВЛЕННЫЙ метод получения транспорта по региону
        // Сначала создай простой класс-модель (можно добавить в конец файла DataBaseInitializer.cs или в отдельный файл)
        public class TransportItem
        {
            public string Brand { get; set; }
            public string StateNumber { get; set; }
        }

        // Замени свой старый метод GetTransportsByRegion на этот:
        public static List<TransportItem> GetTransportsByRegion(string region)
        {
            var result = new List<TransportItem>();
            string connStr = GetConnectionString();

            using var connection = new SqliteConnection(connStr);
            connection.Open();

            // ИСПРАВЛЕНО: Выбираем и марку, и госномер, и фильтруем по колонке Region
            string sql = @"SELECT TransportBrand, TransportStateNumber 
                   FROM TransportInformation 
                   WHERE Region = @region";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("@region", region);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new TransportItem
                {
                    Brand = reader.GetString(reader.GetOrdinal("TransportBrand")),
                    StateNumber = reader.GetString(reader.GetOrdinal("TransportStateNumber"))
                });
            }

            return result;
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
