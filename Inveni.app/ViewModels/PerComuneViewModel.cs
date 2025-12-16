using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Servizi;
using Inveni.App.Modelli;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;

namespace Inveni.App.ViewModels
{
    // CLASSE PARIAL PER ELIMINARE AVVISI MVVMTK0045
    public partial class PerComuneViewModel : BaseViewModel
    {
        private readonly ApiServizio _apiServizio;

        // ============================================
        // PROPRIETÀ OBSERVABLE (SENZA UNDERSCORE PER AVVISI)
        // ============================================

        [ObservableProperty] private bool isRefreshing;
        [ObservableProperty] private bool isCaricamento = true;
        [ObservableProperty] private bool isSuccesso;
        [ObservableProperty] private bool isVuoto;
        [ObservableProperty] private bool isErrore;
        [ObservableProperty] private string messaggioErrore = string.Empty;
        [ObservableProperty] private string testoRicerca = string.Empty;

        // ============================================
        // PROPRIETÀ PUBBLICHE
        // ============================================

        public ObservableCollection<ComuneRaggruppato> ComuniRaggruppati { get; } = new();

        // ============================================
        // COSTRUTTORE
        // ============================================

        public PerComuneViewModel(ApiServizio apiServizio)
        {
            _apiServizio = apiServizio;
            Title = "PER COMUNE";

            // Carica dati all'avvio
            Task.Run(async () => await CaricaComuni());
        }

        // ============================================
        // COMANDO PRINCIPALE: CARICA COMUNI
        // ============================================

        [RelayCommand]
        private async Task CaricaComuni()
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

                // 2. RAGGRUPPA PER COMUNE
                var comuniRaggruppati = RaggruppaPerComune(giochi);

                // 3. AGGIORNA LA LISTA VISIBILE - SENZA DISPATCH
                Console.WriteLine($"🔍 Aggiorno lista con {comuniRaggruppati.Count} comuni");

                ComuniRaggruppati.Clear();

                foreach (var comune in comuniRaggruppati)
                {
                    ComuniRaggruppati.Add(comune);
                    Console.WriteLine($"  • Aggiunto: {comune.NomeComune}");
                }

                Console.WriteLine($"📊 FINITO: {ComuniRaggruppati.Count} comuni nella lista");

                // 4. IMPOSTA STATO UI
                IsCaricamento = false;
                IsSuccesso = ComuniRaggruppati.Count > 0;
                IsVuoto = ComuniRaggruppati.Count == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Errore caricamento comuni: {ex.Message}");
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
        // METODO PRIVATO: RAGGRUPPA PER COMUNE
        // ============================================

        /// <summary>
        /// Raggruppa la lista di giochi per comune e calcola i conteggi per stato
        /// </summary>
        private List<ComuneRaggruppato> RaggruppaPerComune(List<Gioco> giochi)
        {
            var risultato = new List<ComuneRaggruppato>();
            var now = DateTime.Now;

            // 1. RAGGRUPPA PER NOME COMUNE
            var gruppiPerComune = giochi
                .Where(g => !string.IsNullOrEmpty(g.comune))
                .GroupBy(g => g.comune.Trim().ToUpper())
                .ToList();

            Console.WriteLine($"🏙️ Trovati {gruppiPerComune.Count} comuni distinti");

            foreach (var gruppo in gruppiPerComune)
            {
                var nomeComune = gruppo.Key;
                var cacceDelComune = gruppo.ToList();

                Console.WriteLine($"  • {nomeComune}: {cacceDelComune.Count} cacce");

                // 2. CALCOLA CONTEGGI PER STATO
                int attive = 0;
                int programmate = 0;
                int scaduteDisponibili = 0;

                foreach (var caccia in cacceDelComune)
                {
                    if (caccia.dataInizio == null || caccia.dataFine == null)
                        continue;

                    if (caccia.dataInizio <= now && caccia.dataFine >= now)
                        attive++;
                    else if (caccia.dataInizio > now)
                        programmate++;
                    else // caccia.dataFine < now
                        scaduteDisponibili++;
                }

                // 3. CREA OGGETTO RAGGRUPPATO
                var comuneRaggruppato = new ComuneRaggruppato(nomeComune)
                {
                    TotaleCacce = cacceDelComune.Count,
                    CacceAttive = attive,
                    CacceProgrammate = programmate,
                    CacceScaduteDisponibili = scaduteDisponibili,
                    CacceDettaglio = cacceDelComune
                };

                risultato.Add(comuneRaggruppato);
            }

            // 4. ORDINA ALFABETICAMENTE
            return risultato.OrderBy(c => c.NomeComune).ToList();
        }

        // ============================================
        // COMANDI SECONDARI
        // ============================================

        /// <summary>
        /// Comando per filtrare i comuni in base al testo di ricerca
        /// </summary>
        [RelayCommand]
        private void Ricerca()
        {
            Console.WriteLine($"Ricerca comuni: {TestoRicerca}");

            if (string.IsNullOrWhiteSpace(TestoRicerca))
            {
                // Se ricerca vuota, mostra tutti
                foreach (var comune in ComuniRaggruppati)
                {
                    // TODO: Implementare filtro
                }
            }
            else
            {
                // TODO: Implementare filtro per nome comune
                var testo = TestoRicerca.Trim().ToUpper();
                // Logica filtro da implementare
            }
        }

        /// <summary>
        /// Comando per resettare la ricerca e mostrare tutti i comuni
        /// </summary>
        [RelayCommand]
        private void MostraTutti()
        {
            TestoRicerca = string.Empty;
            // Ricarica tutti i comuni
            CaricaComuniCommand.Execute(null);
        }

        /// <summary>
        /// Comando per riprovare il caricamento in caso di errore
        /// </summary>
        [RelayCommand]
        private void Riprova()
        {
            CaricaComuniCommand.Execute(null);
        }

        /// <summary>
        /// Comando per vedere le cacce dettaglio di un comune
        /// </summary>
        [RelayCommand]
        private void VediCacce(ComuneRaggruppato comune)
        {
            if (comune == null) return;

            Console.WriteLine($"Selezionato comune: {comune.NomeComune}");
            Console.WriteLine($"  • Totale cacce: {comune.TotaleCacce}");
            Console.WriteLine($"  • Attive: {comune.CacceAttive}");
            Console.WriteLine($"  • Programmate: {comune.CacceProgrammate}");
            Console.WriteLine($"  • Scadute/Disponibili: {comune.CacceScaduteDisponibili}");

            // TODO: Naviga a pagina dettaglio comune
            // await Shell.Current.GoToAsync($"dettaglioComune?nome={comune.NomeComune}");
        }
    }
}