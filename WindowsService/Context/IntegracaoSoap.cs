using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passaredo.Integracao.Context
{
    /// <summary>
    /// Classe contendo os campos com o resultado esperado.
    /// </summary>
    [Table("TableName")]
    public class IntegracaoSoap
    {
        [Key]
        public int IdIntegracaoAirSupport { get; set; }

        [Required]
        public DateTime DataHoraCadastro { get; set; }
        public string Toa { get; set; }
        public string ACFTAIL { get; set; }
        public string FlightLogID { get; set; }
        public string Disp { get; set; }
        public string Dep { get; set; }
        public string Dest { get; set; }
        public string Alt1 { get; set; }
        public string Alt2 { get; set; }
        public string Toalt { get; set; }
        public DateTime? STD { get; set; }
        public DateTime? ETA { get; set; }
        public int? PAX { get; set; }
        public double? TrafficLoad { get; set; }
        public int? EmptyWeight { get; set; }
        public double? ZFM { get; set; }
        public double? TripFuel { get; set; }
        public int? DestTime { get; set; }
        public double? AltFuel { get; set; }
        public int? AltTime { get; set; }
        public double? Alt2Fuel { get; set; }
        public int? Alt2Time { get; set; }
        public double? HoldFuel { get; set; }
        public int? HoldTime { get; set; }
        public double? FuelMin { get; set; }
        public string TimeMin { get; set; }
        public double? FuelExtra { get; set; }
        public string TimeExtra { get; set; }
        public string FuelTaxi { get; set; }
        public double? Fuel { get; set; }
        public int TotalDistance { get; set; }
        public double? ActTOW { get; set; }
        public double? MaxTOM { get; set; }
        public double? Elw { get; set; }
        public double? MaxLM { get; set; }
        public string MelItems { get; set; }
        public int? FL { get; set; }
        public string GCD { get; set; }
        public string ESAD { get; set; }
        public string WindComponent { get; set; }
        public string Climb { get; set; }
        public string Descend { get; set; }
        public double? AddFuel { get; set; }
        public double? FinalReserveFuel { get; set; }
        public string DESTSTDALT { get; set; }
        public string TIMETAXI { get; set; }
        public string FUELLDG { get; set; }
        public string TIMELDG { get; set; }
        public string Crew { get; set; }
        public string FUELBIAS { get; set; }
        public int? SCHBLOCKTIME { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string RouteName { get; set; }
        public int? AltDist { get; set; }
        public string FuelPolicy { get; set; }
        public double? MaxZFM { get; set; }
        public string FuelPL { get; set; }
        public string StepClimbProfile { get; set; }
        public string ATCData { get; set; }
        public string RouteStrings { get; set; }
        public string SIDPlanned { get; set; }
        public string SIDAlternatives { get; set; }
        public int? FinalReserveMinutes { get; set; }
        public int? AddFuelMinutes { get; set; }
        public string FreeTextItems { get; set; }
        public string SidAndStarProcedures { get; set; }
        public string Messages { get; set; }
        public string FuelAltDef { get; set; }
        public DateTime? WeatherObsTime { get; set; }
        public DateTime? WeatherPlanTime { get; set; }
    }
}
