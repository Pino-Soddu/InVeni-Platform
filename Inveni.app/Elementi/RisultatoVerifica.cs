using Microsoft.Maui.Devices.Sensors;

namespace Inveni.App.Elementi
{
    public class RisultatoVerifica
    {
        public Location? PosizioneUtente { get; set; }
        public Location? PosizioneTesoro { get; set; }
        public double Angolo { get; set; }
        public double Direzione { get; set; }
        public int? PrecisioneCaccia { get; set; }
        public float Inclinazione { get; set; }
        public double? InclinazioneDa { get; set; }
        public double? InclinazioneA { get; set; }
        public string? Messaggio { get; set; }
        public bool Successo { get; set; }
    }
}