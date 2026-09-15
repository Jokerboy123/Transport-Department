using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using TransportDepartmentMVVM.Data;
using TransportDepartmentMVVM.Models;

namespace TransportDepartmentMVVM.Services
{
    // этот класс будет выполнять операции с БД
    public class TransportRepository
    {
        private readonly string _connectionString;

        public TransportRepository()
        {
            _connectionString = DataBaseInitializer.GetConnectionString();
        }

        // 1. Получение транспорта по госномеру
        public TransportProperties GetTransportByStateNumber(string stateNumber)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string SQL = @"SELECT * FROM TransportInformation WHERE TransportStateNumber = @stateNumber";

            using var cmd = new SQLiteCommand(SQL, connection);
            cmd.Parameters.AddWithValue("@stateNumber", stateNumber);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapTransportFromReader(reader);
            }
            return null;
        }

        // Список записей журнала авто
        public List<AccountingCardProperties> GetRecordsByCar(string stateNumber)
        {
            var result = new List<AccountingCardProperties>();

            // Сначала получаем базовые данные авто (нормы, остатки), чтобы считать логику
            var transport = GetTransportByStateNumber(stateNumber);
            if (transport == null) return result;

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = @"SELECT * FROM AccountingCard 
                           WHERE TransportStateNumber = @stateNumber";

            using var cmd = new SQLiteCommand(sql, connection);
            cmd.Parameters.AddWithValue("@stateNumber", stateNumber);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // Вспомогательные функции для безопасного чтения
                int GetInt(string col) => reader.IsDBNull(reader.GetOrdinal(col)) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal(col)));
                double GetDbl(string col) => reader.IsDBNull(reader.GetOrdinal(col)) ? 0.0 : Convert.ToDouble(reader.GetValue(reader.GetOrdinal(col)));
                string GetStr(string col) => reader.IsDBNull(reader.GetOrdinal(col)) ? null : reader.GetString(reader.GetOrdinal(col));

                var record = new AccountingCardProperties
                {
                    DayNumber = GetInt("DayNumber"),
                    WaySheet = GetInt("WaySheet"),
                    FirstDriver = GetStr("FirstDriver"),
                    SecondDriver = GetStr("SecondDriver"),
                    GetGas = GetDbl("GetGas"),
                    GetPetrol = GetDbl("GetPetrol"),
                    GetDiesel = GetDbl("GetDiesel"),
                    GasConsumptionStandard = GetDbl("GasConsumptionStandard"),
                    PetrolConsumptionStandard = GetDbl("PetrolConsumptionStandard"),
                    DieselConsumptionStandard = GetDbl("DieselConsumptionStandard"),
                    UsedGasValue = GetDbl("UsedGasValue"),
                    UsedPetrolValue = GetDbl("UsedPetrolValue"),
                    UsedDieselValue = GetDbl("UsedDieselValue"),
                    AdditionalToolBrand = GetStr("AdditionalToolBrand"),
                    AdditionalGasValue = GetDbl("AdditionalGasValue"),
                    AdditionalPetrolValue = GetDbl("AdditionalPetrolValue"),
                    AdditionalDieselValue = GetDbl("AdditionalDieselValue"),
                    RemaindDayKilometrageValue = GetInt("RemaindDayKilometrageValue"),
                    RemaindDayGasValue = GetDbl("RemaindDayGasValue"),
                    RemaindDayPetrolValue = GetDbl("RemaindDayPetrolValue"),
                    RemaindDayDieselValue = GetDbl("RemaindDayDieselValue"),
                    Region = GetStr("Region"),
                    TransportStateNumber = GetStr("TransportStateNumber")
                };

                // --- логика расчётов ---
                record.RemaindDayGasValue = transport.MonthBeginningGasState - record.UsedGasValue + record.GetGas;
                record.RemaindMonthGasValue = transport.MonthBeginningGasState;
                record.RemaindMonthPetrolValue = transport.MonthBeginningPetrolState;
                record.RemaindMonthDieselValue = transport.MonthBeginningDieselState;
                record.RemaindMonthKilometrageValue = transport.MonthBeginningOdometerValue;

                result.Add(record);
            }


            // --- ЛОГИКА РАСЧЁТОВ (была внутри GetRecordsByCar в старом коде) ---

            // Остаток газа на конец дня: (Остаток на начало месяца) - (Потрачено) + (Получено)
          

            return result;
        }

        // 3. Сохраняем запись журнала
        public void InsertRecord(AccountingCardProperties record)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            string sql = @"INSERT INTO AccountingCard (
                DayNumber, WaySheet, FirstDriver, SecondDriver, GetGas, GetPetrol, GetDiesel,
                GasConsumptionStandard, PetrolConsumptionStandard, DieselConsumptionStandard,
                UsedGasValue, UsedPetrolValue, UsedDieselValue,
                AdditionalToolBrand, AdditionalGasValue, AdditionalPetrolValue, AdditionalDieselValue,
                RemaindDayKilometrageValue, RemaindDayGasValue, RemaindDayPetrolValue, RemaindDayDieselValue,
                Region, TransportStateNumber
            ) VALUES (
                @DayNumber, @WaySheet, @FirstDriver, @SecondDriver, @GetGas, @GetPetrol, @GetDiesel,
                @GasConsumptionStandard, @PetrolConsumptionStandard, @DieselConsumptionStandard,
                @UsedGasValue, @UsedPetrolValue, @UsedDieselValue,
                @AdditionalToolBrand, @AdditionalGasValue, @AdditionalPetrolValue, @AdditionalDieselValue,
                @RemaindDayKilometrageValue, @RemaindDayGasValue, @RemaindDayPetrolValue, @RemaindDayDieselValue,
                @Region, @TransportStateNumber
            )";

            using var cmd = new SQLiteCommand(sql, connection, transaction);

            cmd.Parameters.AddWithValue("@DayNumber", record.DayNumber);
            cmd.Parameters.AddWithValue("@WaySheet", record.WaySheet);
            cmd.Parameters.AddWithValue("@FirstDriver", (object)record.FirstDriver ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SecondDriver", (object)record.SecondDriver ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GetGas", record.GetGas);
            cmd.Parameters.AddWithValue("@GetPetrol", record.GetPetrol);
            cmd.Parameters.AddWithValue("@GetDiesel", record.GetDiesel);
            cmd.Parameters.AddWithValue("@GasConsumptionStandard", record.GasConsumptionStandard);
            cmd.Parameters.AddWithValue("@PetrolConsumptionStandard", record.PetrolConsumptionStandard);
            cmd.Parameters.AddWithValue("@DieselConsumptionStandard", record.DieselConsumptionStandard);
            cmd.Parameters.AddWithValue("@UsedGasValue", record.UsedGasValue);
            cmd.Parameters.AddWithValue("@UsedPetrolValue", record.UsedPetrolValue);
            cmd.Parameters.AddWithValue("@UsedDieselValue", record.UsedDieselValue);
            cmd.Parameters.AddWithValue("@AdditionalToolBrand", (object)record.AdditionalToolBrand ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AdditionalGasValue", record.AdditionalGasValue);
            cmd.Parameters.AddWithValue("@AdditionalPetrolValue", record.AdditionalPetrolValue);
            cmd.Parameters.AddWithValue("@AdditionalDieselValue", record.AdditionalDieselValue);
            cmd.Parameters.AddWithValue("@RemaindDayKilometrageValue", record.RemaindDayKilometrageValue);
            cmd.Parameters.AddWithValue("@RemaindDayGasValue", record.RemaindDayGasValue);
            cmd.Parameters.AddWithValue("@RemaindDayPetrolValue", record.RemaindDayPetrolValue);
            cmd.Parameters.AddWithValue("@RemaindDayDieselValue", record.RemaindDayDieselValue);
            cmd.Parameters.AddWithValue("@Region", (object)record.Region ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TransportStateNumber", record.TransportStateNumber);

            cmd.ExecuteNonQuery();
            transaction.Commit();
        }

        // Вспомогательный метод для маппинга (чтобы не дублировать код)
        private TransportProperties MapTransportFromReader(SQLiteDataReader reader)
        {
            var result = new TransportProperties();

            // Вспомогательная функция для безопасного получения имени колонки
            int GetOrdinalSafe(string columnName)
            {
                int ord = reader.GetOrdinal(columnName);
                if (ord == -1)
                    throw new Exception($"Колонка не найдена в БД: '{columnName}'. Проверьте CREATE TABLE.");
                return ord;
            }

            result.TransportBrand = reader.GetString(GetOrdinalSafe("TransportBrand"));
            result.StateNumber = reader.GetString(GetOrdinalSafe("TransportStateNumber"));
            result.GasConsumptionStandard = reader.GetDouble(GetOrdinalSafe("GasConsumptionStandard"));
            result.PetrolConsumptionStandard = reader.GetDouble(GetOrdinalSafe("PetrolConsumptionStandard"));

            // Пробег
            if (!reader.IsDBNull(GetOrdinalSafe("MonthBeginningOdometerValue")))
                result.MonthBeginningOdometerValue = reader.GetDouble(GetOrdinalSafe("MonthBeginningOdometerValue"));
            if (!reader.IsDBNull(GetOrdinalSafe("MonthEndingOdometerValue")))
                result.MonthEndingOdometerValue = reader.GetDouble(GetOrdinalSafe("MonthEndingOdometerValue"));

            // Топливо (начало месяца)
            if (!reader.IsDBNull(GetOrdinalSafe("MonthBeginningGasState")))
                result.MonthBeginningGasState = reader.GetDouble(GetOrdinalSafe("MonthBeginningGasState"));
            if (!reader.IsDBNull(GetOrdinalSafe("MonthBeginningPetrolState")))
                result.MonthBeginningPetrolState = reader.GetDouble(GetOrdinalSafe("MonthBeginningPetrolState"));
            if (!reader.IsDBNull(GetOrdinalSafe("MonthBeginningDieselState")))
                result.MonthBeginningDieselState = reader.GetDouble(GetOrdinalSafe("MonthBeginningDieselState"));

            // Топливо (конец дня)
            if (!reader.IsDBNull(GetOrdinalSafe("MonthEndingGasState")))
                result.MonthEndingGasState = reader.GetDouble(GetOrdinalSafe("MonthEndingGasState"));
            if (!reader.IsDBNull(GetOrdinalSafe("MonthEndingPetrolState")))
                result.MonthEndingPetrolState = reader.GetDouble(GetOrdinalSafe("MonthEndingPetrolState"));
            if (!reader.IsDBNull(GetOrdinalSafe("MonthEndingDieselState")))
                result.MonthEndingDieselState = reader.GetDouble(GetOrdinalSafe("MonthEndingDieselState"));

            // Водители
            if (!reader.IsDBNull(GetOrdinalSafe("FirstDriverFullName")))
                result.FirstDriverFullName = reader.GetString(GetOrdinalSafe("FirstDriverFullName"));
            if (!reader.IsDBNull(GetOrdinalSafe("SecondDriverFullName")))
                result.SecondDriverFullName = reader.GetString(GetOrdinalSafe("SecondDriverFullName"));

            if (!reader.IsDBNull(GetOrdinalSafe("Additions")))
                result.Additions = reader.GetString(GetOrdinalSafe("Additions"));

            if (!reader.IsDBNull(GetOrdinalSafe("Region")))
                result.Region = reader.GetString(GetOrdinalSafe("Region"));

            return result;
        }
        // Получение списка транспорта по району
        public List<TransportProperties> GetTransportsByRegion(string region)
        {
            var result = new List<TransportProperties>();

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = @"SELECT * FROM TransportInformation WHERE Region = @region";

            using var cmd = new SQLiteCommand(sql, connection);
            cmd.Parameters.AddWithValue("@region", region);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(MapTransportFromReader(reader));
            }

            return result;
        }

    }
}