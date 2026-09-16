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

            // 5. Тестовые данные (удали после проверки!)
            using var checkData = new SQLiteCommand(
                "SELECT COUNT(*) FROM TransportInformation", connection);
            long rows = (long)checkData.ExecuteScalar();
            if (rows == 0)
            {
                string[] inserts = new[]
                {
                    "INSERT INTO TransportInformation (TransportBrand, TransportStateNumber, GasConsumptionStandard, PetrolConsumptionStandard, Region) VALUES ('ГАЗ-3307', 'А123ВС26', 25.0, 18.0, 'Георгиевск')",
                    "INSERT INTO TransportInformation (TransportBrand, TransportStateNumber, GasConsumptionStandard, PetrolConsumptionStandard, Region) VALUES ('УАЗ-3909', 'В456ОР26', 20.0, 15.0, 'Георгиевск')",
                    "INSERT INTO TransportInformation (TransportBrand, TransportStateNumber, GasConsumptionStandard, PetrolConsumptionStandard, Region) VALUES ('ГАЗ-53', 'Е787КХ26', 30.0, 22.0, 'Георгиевский район')",
                    "INSERT INTO TransportInformation (TransportBrand, TransportStateNumber, GasConsumptionStandard, PetrolConsumptionStandard, Region) VALUES ('ЗИЛ-130', 'Р999ТТ26', 35.0, 28.0, 'Кировский район')"
                };

                foreach (var sql in inserts)
                {
                    using var insertCmd = new SQLiteCommand(sql, connection);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
