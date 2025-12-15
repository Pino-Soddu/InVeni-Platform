using Inveni.App.Models;
using Inveni.App.Servizi;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inveni.App.ViewModels
{
    public class IntornoViewModel : INotifyPropertyChanged
    {
        private readonly ApiServizio _apiServizio;
        private ObservableCollection<Caccia> _cacce = new();
        private bool _isLoading;
        private string _title = "VICINO A ME";

        public ObservableCollection<Caccia> Cacce
        {
            get => _cacce;
            set
            {
                _cacce = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public Command CaricaCacceCommand { get; }

        public IntornoViewModel()
        {
            _apiServizio = new ApiServizio();
            CaricaCacceCommand = new Command(async () => await CaricaCacceAsync());

            // Carica all'avvio
            Task.Run(async () => await CaricaCacceAsync());
        }

        public async Task CaricaCacceAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                Console.WriteLine("🔄 Inizio caricamento cacce...");

                // 1. Chiama API backend
                var giochi = await _apiServizio.OttieniListaGiochiAsync();
                Console.WriteLine($"📦 Ricevuti {giochi.Count} giochi dall'API");

                // Pulisci lista corrente
                Cacce.Clear();

                // 2. Processa ogni gioco
                var cacceTemp = new List<Caccia>();

                foreach (var gioco in giochi)
                {
                    try
                    {
                        // Verifica se è attivo
                        if (IsAttiva(gioco))
                        {
                            var caccia = new Caccia
                            {
                                IdGioco = gioco.IdGioco,  // CAMBIATO: IdGioco con maiuscola
                                Name = gioco.name,
                                Organizzatore = gioco.organizzatore,
                                Comune = gioco.comune,
                                Photo1 = gioco.photo1,
                                Latitudine = gioco.lat,
                                Longitudine = gioco.lon,
                                TopCaccia = gioco.topCaccia ?? false,
                                LocalitaCaccia = gioco.localitaCaccia,
                                LunghezzaCaccia = gioco.lunghezzaCaccia,
                                NumTappeCaccia = gioco.numTappeCaccia,
                                DataInizio = gioco.dataInizio,
                                DataFine = gioco.dataFine
                            };

                            // Calcola distanza da Roma
                            caccia.DistanzaKm = App.PosizioneSimulata.CalcolaDistanzaKm(
                                gioco.lat,
                                gioco.lon);

                            // Determina stato
                            caccia.Stato = DeterminaStato(gioco);

                            cacceTemp.Add(caccia);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Errore processamento gioco: {ex.Message}");
                    }
                }

                Console.WriteLine($"✅ Trovate {cacceTemp.Count} cacce attive");

                // 3. Ordina per distanza
                var cacceOrdinate = cacceTemp
                    .OrderBy(c => c.DistanzaKm)
                    .ToList();

                // 4. Aggiorna UI
                Cacce.Clear();
                foreach (var caccia in cacceOrdinate)
                {
                    Cacce.Add(caccia);
                }

                Console.WriteLine($"🎯 Caricate {cacceOrdinate.Count} cacce ordinate per distanza");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Errore caricamento: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool IsAttiva(Modelli.Gioco gioco)
        {
            if (!gioco.dataInizio.HasValue || !gioco.dataFine.HasValue)
                return true;

            var now = DateTime.Now;
            return now >= gioco.dataInizio.Value && now <= gioco.dataFine.Value;
        }

        private string DeterminaStato(Modelli.Gioco gioco)
        {
            if (!gioco.dataInizio.HasValue || !gioco.dataFine.HasValue)
                return "attiva";

            var now = DateTime.Now;
            if (now < gioco.dataInizio.Value)
                return "programmata";
            if (now > gioco.dataFine.Value)
                return "scaduta";
            return "attiva";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}