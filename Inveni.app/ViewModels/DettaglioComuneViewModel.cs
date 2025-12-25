using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Modelli;
using Inveni.App.Servizi;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inveni.App.ViewModels
{
    public partial class DettaglioComuneViewModel : BaseViewModel
    {
        private readonly ApiServizio _apiServizio;
        //private string _nomeComune;  // NON readonly

        // ============================================
        // PROPRIETÀ OBSERVABLE
        // ============================================

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isCaricamento = true;

        [ObservableProperty]
        private bool isSuccesso;

        [ObservableProperty]
        private bool isVuoto;

        [ObservableProperty]
        private bool isErrore;

        [ObservableProperty]
        private string messaggioErrore = string.Empty;

        // STATI ACCORDION
        private bool _isGiocaOraEspanso = true;
        private bool _isInProgrammaEspanso = true;
        private bool _isStoricheEspanso = true;

        // PROPRIETÀ PUBBLICHE ACCORDION
        public bool IsGiocaOraEspanso
        {
            get => _isGiocaOraEspanso;
            set => SetProperty(ref _isGiocaOraEspanso, value);
        }

        public bool IsInProgrammaEspanso
        {
            get => _isInProgrammaEspanso;
            set => SetProperty(ref _isInProgrammaEspanso, value);
        }

        public bool IsStoricheEspanso
        {
            get => _isStoricheEspanso;
            set => SetProperty(ref _isStoricheEspanso, value);
        }

        // ============================================
        // PROPRIETÀ PUBBLICHE
        // ============================================

        private string _nomeComune;
        public string NomeComune
        {
            get => _nomeComune;
            private set => SetProperty(ref _nomeComune, value);
        }

        private int _totaleCacce;
        public int TotaleCacce
        {
            get => _totaleCacce;
            private set => SetProperty(ref _totaleCacce, value);
        }

        private string _immagineComune;
        public string ImmagineComune
        {
            get => _immagineComune;
            private set => SetProperty(ref _immagineComune, value);
        }

        public ObservableCollection<Gioco> CacceAttive { get; } = new();
        public ObservableCollection<Gioco> CacceProgrammate { get; } = new();
        public ObservableCollection<Gioco> CacceScaduteDisponibili { get; } = new();

        // ============================================
        // COSTRUTTORE
        // ============================================

        public DettaglioComuneViewModel(ApiServizio apiServizio)
        {
            _apiServizio = apiServizio;
        }

        // ============================================
        // INIZIALIZZAZIONE CON PARAMETRO
        // ============================================

        public void Initialize(string nomeComune)
        {
            Console.WriteLine($"=== INITIALIZE: {nomeComune} ===");

            // RESETTA TUTTI I DATI PRIMA
            _nomeComune = nomeComune;
            NomeComune = nomeComune;
            ImmagineComune = $"comune_{nomeComune.ToLower().Replace(" ", "").Replace("'", "")}.png";
            Title = $"COMUNE: {nomeComune}"; // ★★★ LASCIA SOLO QUESTA ★★★

            // SVUOTA LE COLLEZIONI
            CacceAttive.Clear();
            CacceProgrammate.Clear();
            CacceScaduteDisponibili.Clear();

            // RESETTA I CONTATORI
            TotaleCacce = 0;

            // FORZA RICALCOLO
            Task.Run(async () => await CaricaCacceComune());
        }

        // ============================================
        // COMANDO PRINCIPALE: CARICA CACCE DEL COMUNE
        // ============================================

        [RelayCommand]
        private async Task CaricaCacceComune()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                IsCaricamento = true;
                IsSuccesso = false;
                IsVuoto = false;
                IsErrore = false;

                Console.WriteLine($"=== CARICA CACCE PER COMUNE: {_nomeComune} ===");

                var tutteLeCacce = await _apiServizio.OttieniListaGiochiAsync();

                Console.WriteLine($"Totale cacce API: {tutteLeCacce?.Count ?? 0}");

                if (tutteLeCacce == null || tutteLeCacce.Count == 0)
                {
                    IsCaricamento = false;
                    IsVuoto = true;
                    return;
                }

                // FILTRA PER COMUNE
                var cacceDelComune = tutteLeCacce
                    .Where(c => c.comune?.ToUpper() == _nomeComune.ToUpper())
                    .ToList();

                Console.WriteLine($"Cacce trovate per '{_nomeComune}': {cacceDelComune.Count}");

                TotaleCacce = cacceDelComune.Count;

                // SEPARA PER STATO
                var now = DateTime.Now;

                // PULISCI LE COLLEZIONI PRIMA
                CacceAttive.Clear();
                CacceProgrammate.Clear();
                CacceScaduteDisponibili.Clear();

                foreach (var caccia in cacceDelComune)
                {
                    if (caccia.dataInizio == null || caccia.dataFine == null)
                        continue;

                    if (caccia.dataInizio <= now && caccia.dataFine >= now)
                        CacceAttive.Add(caccia);
                    else if (caccia.dataInizio > now)
                        CacceProgrammate.Add(caccia);
                    else // caccia.dataFine < now
                        CacceScaduteDisponibili.Add(caccia);
                }

                Console.WriteLine($"Risultati - Attive: {CacceAttive.Count}, Programmate: {CacceProgrammate.Count}, Scadute: {CacceScaduteDisponibili.Count}");

                // FORZA AGGIORNAMENTO UI
                OnPropertyChanged(nameof(TotaleCacce));
                OnPropertyChanged(nameof(NomeComune));
                OnPropertyChanged(nameof(ImmagineComune));

                // FORZA AGGIORNAMENTO DELLE COLLEZIONI
                OnPropertyChanged(nameof(CacceAttive));
                OnPropertyChanged(nameof(CacceProgrammate));
                OnPropertyChanged(nameof(CacceScaduteDisponibili));

                IsCaricamento = false;
                IsSuccesso = cacceDelComune.Count > 0;
                IsVuoto = cacceDelComune.Count == 0;
            }
            catch (Exception ex)
            {
                MessaggioErrore = $"Errore: {ex.Message}";
                IsCaricamento = false;
                IsErrore = true;
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ============================================
        // COMANDI SECONDARI
        // ============================================

        [RelayCommand]
        private async Task Refresh()
        {
            await CaricaCacceComune();
        }

        [RelayCommand]
        private async Task TornaIndietro()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void Riprova()
        {
            CaricaCacceComuneCommand.Execute(null);
        }

        // COMANDI TOGGLE ACCORDION
        [RelayCommand]
        private void ToggleGiocaOra()
        {
            IsGiocaOraEspanso = !IsGiocaOraEspanso;
        }

        [RelayCommand]
        private void ToggleInProgramma()
        {
            IsInProgrammaEspanso = !IsInProgrammaEspanso;
        }

        [RelayCommand]
        private void ToggleStoriche()
        {
            IsStoricheEspanso = !IsStoricheEspanso;
        }

        [RelayCommand]
        private async Task VaiADettaglioCaccia(Gioco caccia)
        {
            Console.WriteLine($"DEBUG: Navigazione a dettaglio caccia: {caccia?.name}");

            if (caccia != null)
            {
                await Shell.Current.GoToAsync($"//MainTab/DettaglioCacciaPage?cacciaId={caccia.IdGioco}");
            }
        }
    }
}
