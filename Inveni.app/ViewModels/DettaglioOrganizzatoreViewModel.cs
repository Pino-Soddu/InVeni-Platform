using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Modelli;
using Inveni.App.Servizi;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inveni.App.ViewModels
{
    public partial class DettaglioOrganizzatoreViewModel : BaseViewModel
    {

        private readonly ApiServizio _apiServizio;
        private string _nomeOrganizzatore;

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

        public string NomeOrganizzatore
        {
            get => _nomeOrganizzatore;
            private set => SetProperty(ref _nomeOrganizzatore, value);
        }

        private int _totaleCacce;
        public int TotaleCacce
        {
            get => _totaleCacce;
            private set => SetProperty(ref _totaleCacce, value);
        }

        // ★★★★ PROPRIETÀ CALCOLATA SEMPLICE E FUNZIONANTE ★★★★
        public string ImmagineOrganizzatore
        {
            get
            {
                if (string.IsNullOrEmpty(NomeOrganizzatore))
                    return "org_default.jpg";

                // ★★★ CREA UN MODELLO TEMPORANEO PER USARE LA SUA LOGICA ★★★
                var modelloTemp = new OrganizzatoreRaggruppato(NomeOrganizzatore);
                return modelloTemp.ImmagineOrganizzatore;
            }
        }

        public ObservableCollection<Gioco> CacceAttive { get; } = new();
        public ObservableCollection<Gioco> CacceProgrammate { get; } = new();
        public ObservableCollection<Gioco> CacceScaduteDisponibili { get; } = new();

        // ============================================
        // ★★★★ NUOVO COSTRUTTORE CON PARAMETRO ★★★★
        // ============================================

        public DettaglioOrganizzatoreViewModel(ApiServizio apiServizio, string nomeOrganizzatore)
        {
            _apiServizio = apiServizio;

            // ★★★ IMPOSTA SUBITO IL NOME AL COSTRUTTORE ★★★
            NomeOrganizzatore = nomeOrganizzatore;
            Title = $"ORGANIZZATORE: {nomeOrganizzatore}";

            // ★★★ CARICA I DATI SUBITO ★★★
            Task.Run(async () => await CaricaCacceOrganizzatore());
        }


        // ============================================
        // COMANDO PRINCIPALE: CARICA CACCE DEL ORGANIZZATORE
        // ============================================

        [RelayCommand]
        private async Task CaricaCacceOrganizzatore()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                IsCaricamento = true;
                IsSuccesso = false;
                IsVuoto = false;
                IsErrore = false;

                Console.WriteLine($"=== CARICA CACCE PER ORGANIZZATORE: {NomeOrganizzatore} ===");

                var tutteLeCacce = await _apiServizio.OttieniListaGiochiAsync();

                Console.WriteLine($"Totale cacce API: {tutteLeCacce?.Count ?? 0}");

                if (tutteLeCacce == null || tutteLeCacce.Count == 0)
                {
                    IsCaricamento = false;
                    IsVuoto = true;
                    return;
                }

                // FILTRA PER ORGANIZZATORE
                var cacceDelOrganizzatore = tutteLeCacce
                    .Where(c => c.organizzatore?.ToUpper() == NomeOrganizzatore.ToUpper())
                    .ToList();

                Console.WriteLine($"Cacce trovate per '{NomeOrganizzatore}': {cacceDelOrganizzatore.Count}");

                TotaleCacce = cacceDelOrganizzatore.Count;

                // SEPARA PER STATO
                var now = DateTime.Now;

                // PULISCI LE COLLEZIONI
                CacceAttive.Clear();
                CacceProgrammate.Clear();
                CacceScaduteDisponibili.Clear();

                foreach (var caccia in cacceDelOrganizzatore)
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
                OnPropertyChanged(nameof(NomeOrganizzatore));
                OnPropertyChanged(nameof(ImmagineOrganizzatore));

                // FORZA AGGIORNAMENTO DELLE COLLEZIONI
                OnPropertyChanged(nameof(CacceAttive));
                OnPropertyChanged(nameof(CacceProgrammate));
                OnPropertyChanged(nameof(CacceScaduteDisponibili));

                IsCaricamento = false;
                IsSuccesso = cacceDelOrganizzatore.Count > 0;
                IsVuoto = cacceDelOrganizzatore.Count == 0;
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
            await CaricaCacceOrganizzatore();
        }

        [RelayCommand]
        private async Task TornaIndietro()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void Riprova()
        {
            CaricaCacceOrganizzatoreCommand.Execute(null);
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

    }
}