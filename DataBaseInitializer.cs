using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace TransportDepartment
{
    public static class DataBaseInitializer
    {
        public static string GetConnectionString()
        {
           // string dbPath = Path.Combine(AppContext.BaseDirectory, "TransportInformation.db");
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(dbPath, "TransportDepartment");
            Debug.WriteLine(appFolder);

            if (!File.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }
            dbPath = Path.Combine(appFolder, "TransportInformation.db");
            return $"Data Source={dbPath};Version=3;";
        }

        // Старый метод (переименован логически, но имя сохранено для совместимости)
        public static void InitializeDataBase()
        {
            EnsureDatabaseStructure();
        }

        // Новый метод, который гарантирует структуру БД
        public static void EnsureDatabaseStructure()
        {
            string connStr = GetConnectionString();
            using var connection = new SQLiteConnection(connStr);
            connection.Open();

            // 1. Создаем таблицу авто (без изменений)
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

            using (var cmd = new SQLiteCommand(SQLScriptTransport, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // 2. Создаем таблицу журнала
            var SQLScriptAccounting = @"CREATE TABLE IF NOT EXISTS AccountingCard (
        DayNumber INT NOT NULL,
        WaySheet INT NOT NULL,
        FirstDriver TEXT NOT NULL,
        SecondDriver TEXT,
        GetGas REAL, GetPetrol REAL, GetDiesel REAL,
        MonthBeginningOdometerValue REAL NOT NULL,
        GasConsumptionStandard REAL NOT NULL,
        PetrolConsumptionStandard REAL NOT NULL,
        DieselConsumptionStandard REAL NOT NULL,
        UsedGasValue REAL, UsedPetrolValue REAL, UsedDieselValue REAL,
        AdditionalToolBrand TEXT,
        AdditionalGasValue REAL, AdditionalPetrolValue REAL, AdditionalDieselValue REAL,
        RemaindDayKilometrageValue INT,
        RemaindDayGasValue REAL, RemaindDayPetrolValue REAL, RemaindDayDieselValue REAL,
        Region TEXT,
        TransportStateNumber TEXT
    );";

            using (var cmd = new SQLiteCommand(SQLScriptAccounting, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // 3. Проверяем и добавляем ВСЕ недостающие колонки в существующую таблицу
            // Это спасает, если таблица была создана старой версией скрипта
            string[] requiredColumns = new[]
            {
        "TransportStateNumber TEXT",
        "DieselConsumptionStandard REAL",
        "AdditionalGasValue REAL",
        "AdditionalPetrolValue REAL",
        "AdditionalDieselValue REAL",
        "Region TEXT"
    };

            foreach (var colDef in requiredColumns)
            {
                string colName = colDef.Split(' ')[0];
                string checkSql = $"SELECT count(*) FROM pragma_table_info('AccountingCard') WHERE name='{colName}';";

                using (var checkCmd = new SQLiteCommand(checkSql, connection))
                {
                    long count = (long)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        // Колонки нет — добавляем
                        string alterSql = $"ALTER TABLE AccountingCard ADD COLUMN {colDef};";
                        using (var alterCmd = new SQLiteCommand(alterSql, connection))
                        {
                            alterCmd.ExecuteNonQuery();
                            Debug.WriteLine($"[DB] Добавлена колонка: {colName}");
                        }
                    }
                }
            }
        }

        // --- СТАРЫЕ МЕТОДЫ ---

        public static List<TransportProperties> GetTransportsByRegion(string region)
        {
            var result = new List<TransportProperties>();
            string connStr = GetConnectionString();

            using var connection = new SQLiteConnection(connStr);
            connection.Open();

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
            string connStr = GetConnectionString();

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
                if (!reader.IsDBNull(reader.GetOrdinal("TransportBrand")))
                    return reader.GetString(reader.GetOrdinal("TransportBrand"));
            }

            return null;
        }

        // --- НОВЫЕ МЕТОДЫ ДЛЯ ЖУРНАЛА ---

        public static List<AccountingCardProperties> GetRecordsByCar(string stateNumber)
        {
            var result = new List<AccountingCardProperties>();
            string connStr = GetConnectionString();

            using var connection = new SQLiteConnection(connStr);
            connection.Open();

            string SQL = @"SELECT * FROM AccountingCard WHERE TransportStateNumber = @stateNumber";

            using var cmd = new SQLiteCommand(SQL, connection);
            cmd.Parameters.AddWithValue("@stateNumber", stateNumber);

            using var reader = cmd.ExecuteReader();

            // Получаем индексы колонок один раз для производительности и безопасности
            int idxDayNumber = reader.GetOrdinal("DayNumber");
            int idxWaySheet = reader.GetOrdinal("WaySheet");
            int idxFirstDriver = reader.GetOrdinal("FirstDriver");
            int idxSecondDriver = reader.GetOrdinal("SecondDriver");
            int idxGetGas = reader.GetOrdinal("GetGas");
            int idxGetPetrol = reader.GetOrdinal("GetPetrol");
            int idxGetDiesel = reader.GetOrdinal("GetDiesel");
            int idxGasConsumptionStandard = reader.GetOrdinal("GasConsumptionStandard");
            int idxPetrolConsumptionStandard = reader.GetOrdinal("PetrolConsumptionStandard");
            int idxDieselConsumptionStandard = reader.GetOrdinal("DieselConsumptionStandard");
            int idxUsedGasValue = reader.GetOrdinal("UsedGasValue");
            int idxUsedPetrolValue = reader.GetOrdinal("UsedPetrolValue");
            int idxUsedDieselValue = reader.GetOrdinal("UsedDieselValue");
            int idxAdditionalToolBrand = reader.GetOrdinal("AdditionalToolBrand");
            int idxAdditionalGasValue = reader.GetOrdinal("AdditionalGasValue");
            int idxAdditionalPetrolValue = reader.GetOrdinal("AdditionalPetrolValue");
            int idxAdditionalDieselValue = reader.GetOrdinal("AdditionalDieselValue");
            int idxRemaindDayKilometrageValue = reader.GetOrdinal("RemaindDayKilometrageValue");
            int idxRemaindDayGasValue = reader.GetOrdinal("RemaindDayGasValue");
            int idxRemaindDayPetrolValue = reader.GetOrdinal("RemaindDayPetrolValue");
            int idxRemaindDayDieselValue = reader.GetOrdinal("RemaindDayDieselValue");
            int idxTransportStateNumber = reader.GetOrdinal("TransportStateNumber");

            while (reader.Read())
            {
                // Безопасное чтение INT (возвращаем 0, если NULL)
                int dayNumber = reader.IsDBNull(idxDayNumber) ? 0 : reader.GetInt32(idxDayNumber);
                int waySheet = reader.IsDBNull(idxWaySheet) ? 0 : reader.GetInt32(idxWaySheet);
                int remaindDayKilometrageValue = reader.IsDBNull(idxRemaindDayKilometrageValue) ? 0 : reader.GetInt32(idxRemaindDayKilometrageValue);

                // Безопасное чтение TEXT (возвращаем null, если NULL)
                string firstDriver = reader.IsDBNull(idxFirstDriver) ? null : reader.GetString(idxFirstDriver);
                string secondDriver = reader.IsDBNull(idxSecondDriver) ? null : reader.GetString(idxSecondDriver);
                string transportStateNumber = reader.IsDBNull(idxTransportStateNumber) ? null : reader.GetString(idxTransportStateNumber);
                string additionalToolBrand = reader.IsDBNull(idxAdditionalToolBrand) ? null : reader.GetString(idxAdditionalToolBrand);
                
                // Безопасное чтение REAL/DOUBLE (возвращаем 0.0, если NULL)
                double getGas = reader.IsDBNull(idxGetGas) ? 0.0 : reader.GetDouble(idxGetGas);
                double getPetrol = reader.IsDBNull(idxGetPetrol) ? 0.0 : reader.GetDouble(idxGetPetrol);
                double getDiesel = reader.IsDBNull(idxGetDiesel) ? 0.0 : reader.GetDouble(idxGetDiesel);

                double gasConsumptionStandard = reader.IsDBNull(idxGasConsumptionStandard) ? 0.0 : reader.GetDouble(idxGasConsumptionStandard);
                double petrolConsumptionStandard = reader.IsDBNull(idxPetrolConsumptionStandard) ? 0.0 : reader.GetDouble(idxPetrolConsumptionStandard);
                double dieselConsumptionStandard = reader.IsDBNull(idxDieselConsumptionStandard) ? 0.0 : reader.GetDouble(idxDieselConsumptionStandard);

                double usedGasValue = reader.IsDBNull(idxUsedGasValue) ? 0.0 : reader.GetDouble(idxUsedGasValue);
                double usedPetrolValue = reader.IsDBNull(idxUsedPetrolValue) ? 0.0 : reader.GetDouble(idxUsedPetrolValue);
                double usedDieselValue = reader.IsDBNull(idxUsedDieselValue) ? 0.0 : reader.GetDouble(idxUsedDieselValue);

                double additionalGasValue = reader.IsDBNull(idxAdditionalGasValue) ? 0.0 : reader.GetDouble(idxAdditionalGasValue);
                double additionalPetrolValue = reader.IsDBNull(idxAdditionalPetrolValue) ? 0.0 : reader.GetDouble(idxAdditionalPetrolValue);
                double additionalDieselValue = reader.IsDBNull(idxAdditionalDieselValue) ? 0.0 : reader.GetDouble(idxAdditionalDieselValue);



                double remaindDayGasValue = reader.IsDBNull(idxRemaindDayGasValue) ? 0.0 : reader.GetDouble(idxRemaindDayGasValue);
                double remaindDayPetrolValue = reader.IsDBNull(idxRemaindDayPetrolValue) ? 0.0 : reader.GetDouble(idxRemaindDayPetrolValue);
                double remaindDayDieselValue = reader.IsDBNull(idxRemaindDayDieselValue) ? 0.0 : reader.GetDouble(idxRemaindDayDieselValue);

                result.Add(new AccountingCardProperties
                {
                    DayNumber = dayNumber,
                    WaySheet = waySheet,
                    FirstDriver = firstDriver,
                    SecondDriver = secondDriver,
                    GetGas = getGas,
                    GetPetrol = getPetrol,
                    GetDiesel = getDiesel,
                    GasConsumptionStandard = gasConsumptionStandard,
                    PetrolConsumptionStandard = petrolConsumptionStandard,
                    DieselConsumptionStandard = dieselConsumptionStandard,
                    UsedGasValue = usedGasValue,
                    UsedPetrolValue = usedPetrolValue,
                    UsedDieselValue = usedDieselValue,
                    AdditionalDieselValue = additionalDieselValue,
                    AdditionalGasValue = additionalGasValue,
                    AdditionalPetrolValue = additionalPetrolValue,
                    AdditionalToolBrand = additionalToolBrand,
                    RemaindDayKilometrageValue = remaindDayKilometrageValue,
                    RemaindDayGasValue = remaindDayGasValue,
                    RemaindDayPetrolValue = remaindDayPetrolValue,
                    RemaindDayDieselValue = remaindDayDieselValue,
                    TransportStateNumber = transportStateNumber
                });
            }

            return result;
        }

        public static void InsertRecord(AccountingCardProperties record)
        {
            string connStr = GetConnectionString();
            using var connection = new SQLiteConnection(connStr);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            string SQL = @"INSERT INTO AccountingCard (
                DayNumber, WaySheet, FirstDriver, SecondDriver, GetGas, GetPetrol, GetDiesel,
                GasConsumptionStandard, PetrolConsumptionStandard, DieselConsumptionStandard,
                UsedGasValue, UsedPetrolValue, UsedDieselValue,
                AdditionalToolBrand, AdditionalGasValue, AdditionalPetrolValue, AdditionalDieselValue,
                RemaindDayKilometrageValue, RemaindDayGasValue, RemaindDayPetrolValue, RemaindDayDieselValue,
                TransportStateNumber
            ) VALUES (
                @DayNumber, @WaySheet, @FirstDriver, @SecondDriver, @GetGas, @GetPetrol, @GetDiesel,
                @GasConsumptionStandard, @PetrolConsumptionStandard, @DieselConsumptionStandard,
                @UsedGasValue, @UsedPetrolValue, @UsedDieselValue,
                @AdditionalToolBrand, @AdditionalGasValue, @AdditionalPetrolValue, @AdditionalDieselValue,
                @RemaindDayKilometrageValue, @RemaindDayGasValue, @RemaindDayPetrolValue, @RemaindDayDieselValue,
                @TransportStateNumber
            )";

            using var cmd = new SQLiteCommand(SQL, connection, transaction);

            cmd.Parameters.AddWithValue("@DayNumber", record.DayNumber);
            cmd.Parameters.AddWithValue("@WaySheet", record.WaySheet);
            cmd.Parameters.AddWithValue("@FirstDriver", record.FirstDriver ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SecondDriver", record.SecondDriver ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GetGas", record.GetGas);
            cmd.Parameters.AddWithValue("@GetPetrol", record.GetPetrol);
            cmd.Parameters.AddWithValue("@GetDiesel", record.GetDiesel);
            cmd.Parameters.AddWithValue("@GasConsumptionStandard", record.GasConsumptionStandard);
            cmd.Parameters.AddWithValue("@PetrolConsumptionStandard", record.PetrolConsumptionStandard);
            cmd.Parameters.AddWithValue("@DieselConsumptionStandard", record.DieselConsumptionStandard);

            cmd.Parameters.AddWithValue("@UsedGasValue", record.UsedGasValue);
            cmd.Parameters.AddWithValue("@UsedPetrolValue", record.UsedPetrolValue);
            cmd.Parameters.AddWithValue("@UsedDieselValue", record.UsedDieselValue);
            cmd.Parameters.AddWithValue("@AdditionalToolBrand", record.AdditionalToolBrand ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AdditionalGasValue", record.AdditionalGasValue);
            cmd.Parameters.AddWithValue("@AdditionalPetrolValue", record.AdditionalPetrolValue);
            cmd.Parameters.AddWithValue("@AdditionalDieselValue", record.AdditionalDieselValue);
            cmd.Parameters.AddWithValue("@RemaindDayKilometrageValue", record.RemaindDayKilometrageValue);
            cmd.Parameters.AddWithValue("@RemaindDayGasValue", record.RemaindDayGasValue);
            cmd.Parameters.AddWithValue("@RemaindDayPetrolValue", record.RemaindDayPetrolValue);
            cmd.Parameters.AddWithValue("@RemaindDayDieselValue", record.RemaindDayDieselValue);
            cmd.Parameters.AddWithValue("@TransportStateNumber", record.TransportStateNumber ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();

            transaction.Commit(); // Сохранение данных в БД!!!!
        }
    }
}
// убрать рамку если ест значение. рамка должна отображаться если значения нет

