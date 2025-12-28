using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Modelli;
using Inveni.App.Servizi;
using System.Collections.ObjectModel;

namespace Inveni.App.ViewModels
{
    public partial class DettaglioCacciaViewModel : BaseViewModel
    {
        private readonly ApiServizio _apiServizio;
        private readonly int _cacciaId;

        // ============================================
        // PROPRIETÀ OBSERVABLE
        // ============================================

        [ObservableProperty]
        private Gioco? _caccia; // Il modello Gioco completo

        [ObservableProperty]
        private bool _isCaricamento = true;

        [ObservableProperty]
        private bool isErrore;

        [ObservableProperty]
        private string messaggioErrore = string.Empty;

        // Stato accordion info
        private bool _isInfoEspanso = true;
        public bool IsInfoEspanso
        {
            get => _isInfoEspanso;
            set => SetProperty(ref _isInfoEspanso, value);
        }


        // Per il pulsante primario dinamico (per ora valori statici)
        public string TestoPulsantePrimario => "AVVIA LA CACCIA"; // Cambierà con logica API
        public Color ColorePulsantePrimario => Color.FromArgb("#FFD700"); // Giallo oro

        // ============================================
        // COSTRUTTORE
        // ============================================

        public DettaglioCacciaViewModel(ApiServizio apiServizio, int cacciaId)
        {
            _apiServizio = apiServizio;
            _cacciaId = cacciaId;
            Title = "Dettaglio Caccia";

            // Carica i dati all'avvio
            Task.Run(async () => await CaricaDettaglioCaccia());
        }

        // ============================================
        // COMANDI PRINCIPALI
        // ============================================

        [RelayCommand]
        private async Task CaricaDettaglioCaccia()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                IsCaricamento = true;
                IsErrore = false;
                MessaggioErrore = string.Empty;

                Console.WriteLine($"=== CARICA DETTAGLIO CACCIA ID: {_cacciaId} ===");

                // 1. Ottieni TUTTE le cacce (come fanno le altre pagine)
                var tutteLeCacce = await _apiServizio.OttieniListaGiochiAsync();

                Console.WriteLine($"Totale cacce API: {tutteLeCacce?.Count ?? 0}");

                if (tutteLeCacce == null || tutteLeCacce.Count == 0)
                {
                    MessaggioErrore = "Nessuna caccia disponibile";
                    IsErrore = true;
                    IsCaricamento = false;
                    return;
                }

                // 2. Filtra la caccia con l'ID che ci interessa
                Caccia = tutteLeCacce.FirstOrDefault(g => g.IdGioco == _cacciaId);

                if (Caccia == null)
                {
                    Console.WriteLine($"⚠️ Caccia con ID {_cacciaId} non trovata");
                    MessaggioErrore = $"Caccia non trovata (ID: {_cacciaId})";
                    IsErrore = true;
                }
                else
                {
                    Console.WriteLine($"✅ Caccia trovata: {Caccia.name}");
                    Console.WriteLine($"FotoSemplice: '{Caccia?.FotoSemplice ?? "NULL"}'");
                    Console.WriteLine($"   Testo: {Caccia.text?.Substring(0, Math.Min(50, Caccia.text.Length))}...");

                    // Imposta il titolo della pagina
                    Title = Caccia.name ?? "Dettaglio Caccia";

                    // Forza aggiornamento UI per le proprietà calcolate
                    OnPropertyChanged(nameof(TestoPulsantePrimario));
                    OnPropertyChanged(nameof(ColorePulsantePrimario));
                }

                IsCaricamento = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Errore caricamento dettaglio caccia: {ex.Message}");
                MessaggioErrore = $"Errore: {ex.Message}";
                IsErrore = true;
                IsCaricamento = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ============================================
        // COMANDI SECONDARI
        // ============================================

        [RelayCommand]
        private async Task TornaIndietro()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void ToggleInfo() => IsInfoEspanso = !IsInfoEspanso;

        [RelayCommand]
        private async Task Ascolta()
        {
            if (Caccia == null || string.IsNullOrEmpty(Caccia.audio))
            {
                await Shell.Current.DisplayAlert("Info", "Nessun audio disponibile per questa caccia", "OK");
                return;
            }

            Console.WriteLine($"Avvia audio: {Caccia.audio}");
            // TODO: Implementare riproduzione audio
            await Shell.Current.DisplayAlert("Audio", "Riproduzione audio non ancora implementata", "OK");
        }

        [RelayCommand]
        private async Task VediMappa()
        {
            if (Caccia == null)
                return;

            Console.WriteLine($"Mostra mappa per: {Caccia.name} ({Caccia.lat}, {Caccia.lon})");
            // TODO: Implementare visualizzazione mappa
            await Shell.Current.DisplayAlert("Mappa", "Visualizzazione mappa non ancora implementata", "OK");
        }

        [RelayCommand]
        private async Task AvviaCaccia()
        {
            if (Caccia == null) return;

            Console.WriteLine($"Avvia caccia {Caccia.IdGioco}: {Caccia.name}");

            // TODO: Implementare la logica di avvio/continua
            // 1. Chiamare API per avviare/riprendere caccia
            // 2. Chiudere questa pagina
            // 3. Aprire la mappa di gioco

            await Shell.Current.DisplayAlert("Avvio Caccia",
                $"Pronto per iniziare: {Caccia.name}\nQuesta funzione sarà implementata nella prossima fase.",
                "OK");
        }

        [RelayCommand]
        private async Task Riprova()
        {
            await CaricaDettaglioCaccia();
        }



        // ============================================
        // PROPRIETÀ CALCOLATE PER LA UI
        // ============================================

        /// <summary>
        /// Restituisce il testo formattato per il periodo
        /// </summary>
        public string TestoPeriodoFormattato => Caccia?.TestoPeriodoMesi ?? "N/D";

        /// <summary>
        /// Restituisce la lunghezza formattata
        /// </summary>
        public string LunghezzaFormattata => Caccia?.TestoLunghezzaSenzaIcona ?? "N/D";

        /// <summary>
        /// Restituisce il numero di tappe formattato
        /// </summary>
        public string TappeFormattate => Caccia?.numTappeCaccia ?? "N/D";

        /// <summary>
        /// Restituisce il testo dello stato basato sul colore del bordo
        /// </summary>
        public string TestoStato
        {
            get
            {
                if (Caccia == null)
                    return "N/D";

                // Verifica se le date sono valide
                if (!Caccia.dataInizio.HasValue || !Caccia.dataFine.HasValue)
                    return "DATE NON DEFINITE";

                DateTime oggi = DateTime.Now;
                DateTime inizio = Caccia.dataInizio.Value;
                DateTime fine = Caccia.dataFine.Value;

                if (oggi < inizio)
                {
                    // Non ancora iniziata
                    return "IN PROGRAMMA";
                }
                else if (oggi >= inizio && oggi <= fine)
                {
                    // In corso
                    return "ATTIVA";
                }
                else // oggi > fine
                {
                    // Scaduta
                    return "SCADUTA (SOLO DIVERTIMENTO)";
                }
            }
        }

        /// <summary>
        /// Restituisce la categoria formattata (basata su catId e serviceId)
        /// </summary>
        public string CategoriaTesto
        {
            get
            {
                if (Caccia == null) return "N/D";

                // Logica semplificata per il momento
                if (Caccia.catId == 20)
                {
                    return Caccia.serviceId switch
                    {
                        1 => "Caccia Base",
                        2 => "Enigma Indizio",
                        3 => "Enigma Complesso",
                        4 => "Intermedia",
                        5 => "Avanzata",
                        6 => "Esperto",
                        _ => "Caccia Speciale"
                    };
                }
                return "Caccia Standard";
            }
        }

        /// <summary>
        /// Restituisce il testo formattato per il luogo: "Caccia a Comune - Località"
        /// </summary>
        public string TestoLuogoCompleto
        {
            get
            {
                if (Caccia == null)
                    return "";

                var luogo = "";

                if (!string.IsNullOrWhiteSpace(Caccia.comune))
                    luogo += Caccia.comune;

                // Aggiungi località SOLO se diversa dal comune e non vuota
                if (!string.IsNullOrWhiteSpace(Caccia.localitaCaccia) &&
                    !Caccia.localitaCaccia.Equals(Caccia.comune, StringComparison.OrdinalIgnoreCase))
                    luogo += " - " + Caccia.localitaCaccia;

                return luogo;
            }
        }

        /// <summary>
        /// Restituisce il periodo formattato completo della caccia
        /// </summary>
        public string PeriodoCompleto
        {
            get
            {
                if (Caccia == null || !Caccia.dataInizio.HasValue || !Caccia.dataFine.HasValue)
                    return "N/D";

                return $"dal {Caccia.dataInizio.Value:dd/MM/yyyy} al {Caccia.dataFine.Value:dd/MM/yyyy}";
            }
        }

        // ============================================
        // PROPRIETÀ CALCOLATE PER I NUOVI CAMPI
        // ============================================

        /// <summary>
        /// Indica se la caccia ha informazioni sul premio
        /// </summary>
        public bool HaPremio => !string.IsNullOrEmpty(Caccia?.premio);

        /// <summary>
        /// Indica se la caccia ha regole del gioco
        /// </summary>
        public bool HaRegole => !string.IsNullOrEmpty(Caccia?.comesigioca);

        /// <summary>
        /// Indica se la caccia ha una descrizione/storia
        /// </summary>
        public bool HaDescrizione => !string.IsNullOrEmpty(Caccia?.text);

        // ============================================
        // PROPRIETÀ PER GESTIRE GLI ACCORDION
        // ============================================

        private bool _isDescrizioneEspansa = true;
        public bool IsDescrizioneEspansa
        {
            get => _isDescrizioneEspansa;
            set => SetProperty(ref _isDescrizioneEspansa, value);
        }

        private bool _isPremioEspanso = false;
        public bool IsPremioEspanso
        {
            get => _isPremioEspanso;
            set => SetProperty(ref _isPremioEspanso, value);
        }

        private bool _isRegoleEspanso = false;
        public bool IsRegoleEspanso
        {
            get => _isRegoleEspanso;
            set => SetProperty(ref _isRegoleEspanso, value);
        }

        // ============================================
        // COMANDI PER GLI ACCORDION
        // ============================================

        [RelayCommand]
        private void ToggleDescrizione() => IsDescrizioneEspansa = !IsDescrizioneEspansa;

        [RelayCommand]
        private void TogglePremio() => IsPremioEspanso = !IsPremioEspanso;

        [RelayCommand]
        private void ToggleRegole() => IsRegoleEspanso = !IsRegoleEspanso;

        // ============================================
        // AGGIORNAMENTO OnCacciaChanged
        // ============================================

        partial void OnCacciaChanged(Gioco? value)
        {
            // Forza il ricalcolo delle proprietà dipendenti
            OnPropertyChanged(nameof(TestoLuogoCompleto));
            OnPropertyChanged(nameof(TestoStato));
            OnPropertyChanged(nameof(CategoriaTesto));
            OnPropertyChanged(nameof(TestoPeriodoFormattato));
            OnPropertyChanged(nameof(LunghezzaFormattata));
            OnPropertyChanged(nameof(TappeFormattate));

            // NUOVE proprietà
            OnPropertyChanged(nameof(HaDescrizione));
            OnPropertyChanged(nameof(HaPremio));
            OnPropertyChanged(nameof(HaRegole));
        }
    }
}