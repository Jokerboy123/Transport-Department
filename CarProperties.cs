using System;

public class CarProperties // общий класс для описания автомобилей во всех районах
{
    public string ID { get; set; } // Госномер
    public string TransportBrand { get; set; } // МАРКА АВТОМОБИЛЯ
    public double GasConsumptionStandard {  get; set; } // Норматив расхода газа
    public double PetrolConsumptionStandart { get; set; } // Норматив расхода бензина
    public double DieselConsumptionStandart { get; set; } // Норматив расхода дизеля
    public double MonthBeginningOdometerValue { get; set; } // Показания одометра на начало месяца
    public double MonthEndingOdometerValue { get; set; } // Показания одометра на конец месяца
    public double MonthBeginningGasState {  get; set; } // Остаток газа на начало месяца
    public double MothBeginningPetrolState { get; set; } // Остаток бензина на начало месяца
    public double MonthBeginningDieselState { get; set; } // Остаток дизеля на начало месяца
    public double MonthEndingGasState { get; set; } // Остаток газа на конец месяца
    public double MothEndingPetrolState { get; set; } // Остаток бензина на конец месяца
    public double MonthEndingDieselState { get; set; } // Остаток дизеля на конец месяца
    public string DriverFullName { get; set; } // Водитель
    public string Additions {  get; set; } // Вспомогательное оборудование




}
