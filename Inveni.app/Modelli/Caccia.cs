using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inveni.App.Models
{
    public class Caccia : INotifyPropertyChanged
    {
        private string? _name;
        private DateTime? _dataInizio;
        private DateTime? _dataFine;
        private string? _organizzatore;
        private bool _topCaccia;
        private string? _comune;
        private string? _photo1;
        private string? _stato;
        private double _distanzaKm;
        private string? _localitaCaccia;
        private string? _numTappeCaccia;
        private string? _lunghezzaCaccia;
        private double _latitudine;
        private double _longitudine;

        public int IdGioco { get; set; }

        public string? Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public string? Organizzatore
        {
            get => _organizzatore;
            set => SetField(ref _organizzatore, value);
        }
        public bool TopCaccia
        {
            get => _topCaccia;
            set => SetField(ref _topCaccia, value);
        }

        public string? Comune
        {
            get => _comune;
            set => SetField(ref _comune, value);
        }

        public string? Photo1
        {
            get => _photo1;
            set => SetField(ref _photo1, value);
        }

        public string? Stato
        {
            get => _stato;
            set => SetField(ref _stato, value);
        }

        public double DistanzaKm
        {
            get => _distanzaKm;
            set => SetField(ref _distanzaKm, value);
        }

        public string? LocalitaCaccia
        {
            get => _localitaCaccia;
            set => SetField(ref _localitaCaccia, value);
        }

        public string? NumTappeCaccia
        {
            get => _numTappeCaccia;
            set => SetField(ref _numTappeCaccia, value);
        }

        public string? LunghezzaCaccia
        {
            get => _lunghezzaCaccia;
            set => SetField(ref _lunghezzaCaccia, value);
        }

        public double Latitudine
        {
            get => _latitudine;
            set => SetField(ref _latitudine, value);
        }
        public double Longitudine
        {
            get => _longitudine;
            set => SetField(ref _longitudine, value);
        }

        public DateTime? DataInizio
        {
            get => _dataInizio;
            set => SetField(ref _dataInizio, value);
        }

        public DateTime? DataFine
        {
            get => _dataFine;
            set => SetField(ref _dataFine, value);
        }

        // Per UI - proprietà calcolate
        public string DistanzaFormattata => DistanzaKm > 0 ? $"{DistanzaKm:F1} km" : "";
        public bool MostraDistanza => DistanzaKm > 0;
        public string IconaStato => Stato switch
        {
            "attiva" => "📍",
            "programmata" => "🗓️",
            "scaduta" => "🔴",
            _ => "❓"
        };
        public string ColoreStato => Stato switch
        {
            "attiva" => "#4CAF50", // Verde
            "programmata" => "#2196F3", // Blu
            "scaduta" => "#9E9E9E", // Grigio
            _ => "#757575"
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // UN SOLO METODO - funziona per tutti i tipi
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string StatoCalcolato
        {
            get
            {
                if (!DataInizio.HasValue || !DataFine.HasValue)
                    return "sconosciuto";

                var now = DateTime.Now;
                if (now < DataInizio.Value)
                    return "programmata";
                if (now > DataFine.Value)
                    return "scaduta";
                return "attiva";
            }
        }
    }
}
