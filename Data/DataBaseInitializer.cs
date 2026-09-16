using System;
using System.IO;
using System.Data.SQLite;

namespace TransportDepartmentMVVM.Data
{
    public static class DataBaseInitializer
    {
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TransportDepartmentMVVM",
            "TransportInformation.db"
        );

        public static string GetConnectionString()
        {
            string folder = Path.GetDirectoryName(DbPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return $"Data Source={DbPath};Version=3;";
        }

        public static void EnsureDataBaseStructure()
        {
            using var connection = new SQLiteConnection(GetConnectionString());
            // при ошибке "System.DllNotFoundException: "Unable to load DLL 'e_sqlite3'
            // or one of its dependencies: Не найден указанный модуль. (0x8007007E)""
            // скопировать e_sqlite3.dll в папку рядом с .exe и пересобрать
            connection.Open();

            // 1. Таблица транспорта
            const string createTransportTable = @"
                CREATE TABLE IF NOT EXISTS TransportInformation (
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
                    FirstDriverFullName TEXT,
                    SecondDriverFullName TEXT,
                    Additions TEXT,
                    Region TEXT
                );";

            using (var cmd = new SQLiteCommand(createTransportTable, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // 2. Таблица журнала
            const string createAccountingTable = @"
                CREATE TABLE IF NOT EXISTS DemonstrationCard (
                    DayNumber INT NOT NULL,
                    WaySheet INT NOT NULL,
                    FirstDriver TEXT NOT NULL,
                    SecondDriver TEXT,
                    GetGas REAL, GetPetrol REAL, GetDiesel REAL,
                    MonthBeginningOdometerValue REAL NOT NULL,
                    MonthEndingOdometerValue REAL NOT NULL,
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

            using (var cmd = new SQLiteCommand(createAccountingTable, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // 3. Миграция TransportInformation
            string[] transportColumnsToAdd = new[]
            {
                "FirstDriverFullName TEXT",
                "SecondDriverFullName TEXT"
            };

            foreach (var colDef in transportColumnsToAdd)
            {
                string colName = colDef.Split(' ')[0];
                string checkSql = $"SELECT count(*) FROM pragma_table_info('TransportInformation') WHERE name='{colName}';";

                using var checkCmd = new SQLiteCommand(checkSql, connection);
                long count = (long)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    using var alterCmd = new SQLiteCommand(
                        $"ALTER TABLE TransportInformation ADD COLUMN {colDef};", connection);
                    alterCmd.ExecuteNonQuery();
                }
            }

            // 4. Миграция DemonstrationCard
            string[] accountingColumnsToAdd = new[]
            {
                "DieselConsumptionStandard REAL",
                "AdditionalGasValue REAL",
                "AdditionalPetrolValue REAL",
                "AdditionalDieselValue REAL",
                "Region TEXT"
            };

            foreach (var colDef in accountingColumnsToAdd)
            {
                string colName = colDef.Split(' ')[0];
                string checkSql = $"SELECT count(*) FROM pragma_table_info('DemonstrationCard') WHERE name='{colName}';";

                using var checkCmd = new SQLiteCommand(checkSql, connection);
                long count = (long)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    using var alterCmd = new SQLiteCommand(
                        $"ALTER TABLE DemonstrationCard ADD COLUMN {colDef};", connection);
                    alterCmd.ExecuteNonQuery();
                }
            }

           
        }
    }
}
