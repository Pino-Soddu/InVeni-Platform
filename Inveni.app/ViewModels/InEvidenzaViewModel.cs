using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Modelli;
using Inveni.App.Servizi;
using System;
using System.Collections.ObjectModel;

namespace Inveni.App.ViewModels
{
    public partial class InEvidenzaViewModel : BaseViewModel
    {
        private readonly ApiServizio _apiServizio;

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

        // ============================================
        // PROPRIETÀ PUBBLICHE
        // ============================================

        public ObservableCollection<CacciaInEvidenza> CacceInEvidenza { get; } = new();

        // ============================================
        // COSTRUTTORE
        // ============================================

        public InEvidenzaViewModel(ApiServizio apiServizio)
        {
            _apiServizio = apiServizio;
            Title = "IN EVIDENZA";

            // Carica dati all'avvio
            Task.Run(async () => await CaricaCacceInEvidenza());
        }

        // ============================================
        // COMANDO PRINCIPALE: CARICA CACCE IN EVIDENZA
        // ============================================

        [RelayCommand]
        private async Task CaricaCacceInEvidenza()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                IsCaricamento = true;
                IsSuccesso = false;
                IsVuoto = false;
                IsErrore = false;

                // 1. CHIAMA L'API PER OTTENERE LE CACCE
                var giochi = await _apiServizio.OttieniListaGiochiAsync();

                if (giochi == null || giochi.Count == 0)
                {
                    // NESSUNA CACCIA TROVATA
                    IsCaricamento = false;
                    IsVuoto = true;
                    return;
                }

                Console.WriteLine($"✅ Ricevute {giochi.Count} cacce dal backend");

                // 2. FILTRA E TRASFORMA IN CACCE IN EVIDENZA
                var cacceFiltrate = FiltraCacceInEvidenza(giochi);

                // 3. AGGIORNA LA LISTA VISIBILE
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CacceInEvidenza.Clear();

                    foreach (var cacciaFiltrata in cacceFiltrate)
                    {
                        CacceInEvidenza.Add(cacciaFiltrata);
                    }

                    Console.WriteLine($"⭐ Trovate {CacceInEvidenza.Count} cacce in evidenza");

                    // 4. IMPOSTA STATO UI
                    IsCaricamento = false;
                    IsSuccesso = CacceInEvidenza.Count > 0;
                    IsVuoto = CacceInEvidenza.Count == 0;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Errore caricamento cacce in evidenza: {ex.Message}");
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

        // ============================================
        // METODO PRIVATO: FILTRA CACCE IN EVIDENZA
        // ============================================

        /// <summary>
        /// Filtra le cacce per evidenza (topCaccia == true) e stato (attive/programmate)
        /// </summary>
        private List<CacciaInEvidenza> FiltraCacceInEvidenza(List<Gioco> giochi)
        {
            var risultato = new List<CacciaInEvidenza>();
            var now = DateTime.Now;

            // 1. PRIMA LE ATTIVE, POI LE PROGRAMMATE
            var cacceTop = giochi
                .Where(g => g.topCaccia == true)  // Solo cacce TOP
                .Where(g => g.dataInizio != null && g.dataFine != null)  // Con date valide
                .ToList();

            Console.WriteLine($"🔍 Trovate {cacceTop.Count} cacce TOP totali");

            // 2. SEPARA ATTIVE E PROGRAMMATE
            var cacceAttive = cacceTop
                .Where(g => g.dataInizio <= now && g.dataFine >= now)
                .OrderBy(g => g.dataInizio)  // Ordina per data inizio
                .ToList();

            var cacceProgrammate = cacceTop
                .Where(g => g.dataInizio > now)
                .OrderBy(g => g.dataInizio)  // Ordina per data inizio (più vicine prima)
                .ToList();

            Console.WriteLine($"  • Attive: {cacceAttive.Count}");
            Console.WriteLine($"  • Programmate: {cacceProgrammate.Count}");

            // 3. CREA WRAPPER PER OGNI CACCIA
            foreach (var caccia in cacceAttive.Concat(cacceProgrammate))
            {
                risultato.Add(new CacciaInEvidenza(caccia));
            }

            return risultato;
        }

        // ============================================
        // COMANDI SECONDARI
        // ============================================

        /// <summary>
        /// Comando per riprovare il caricamento in caso di errore
        /// </summary>
        [RelayCommand]
        private void Riprova()
        {
            CaricaCacceInEvidenzaCommand.Execute(null);
        }

        /// <summary>
        /// Comando per aprire i dettagli di una caccia in evidenza
        /// </summary>
        [RelayCommand]
        private void ApriDettaglioCaccia(CacciaInEvidenza cacciaInEvidenza)
        {
            if (cacciaInEvidenza == null || cacciaInEvidenza.Caccia == null) return;

            var caccia = cacciaInEvidenza.Caccia;
            Console.WriteLine($"🎯 Caccia in evidenza selezionata:");
            Console.WriteLine($"  • Nome: {caccia.name}");
            Console.WriteLine($"  • Comune: {caccia.comune}");
            Console.WriteLine($"  • Stato: {cacciaInEvidenza.TestoStato}");
            Console.WriteLine($"  • Top: {caccia.topCaccia}");

            // TODO: Naviga a pagina dettaglio caccia
            // await Shell.Current.GoToAsync($"dettaglioCaccia?id={caccia.idGioco}");
        }
    }
}