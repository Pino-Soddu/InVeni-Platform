using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Servizi;
using Inveni.App.Modelli;
using Inveni.App.Pages;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;

namespace Inveni.App.ViewModels
{
    // CLASSE PARIAL PER ELIMINARE AVVISI MVVMTK0045
    public partial class PerComuneViewModel : BaseViewModel
    {
        private readonly ApiServizio _apiServizio;
        private List<ComuneRaggruppato> _tuttiComuni = new();

        // ============================================
        // PROPRIETÀ OBSERVABLE (SENZA UNDERSCORE PER AVVISI)
        // ============================================

        [ObservableProperty] private bool isRefreshing;
        [ObservableProperty] private bool isCaricamento = true;
        [ObservableProperty] private bool isSuccesso;
        [ObservableProperty] private bool isVuoto;
        [ObservableProperty] private bool isErrore;
        [ObservableProperty] private string messaggioErrore = string.Empty;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ComuniFiltrati))]
        private string testoRicerca = string.Empty;

        // ============================================
        // PROPRIETÀ PUBBLICHE
        // ============================================

        public ObservableCollection<ComuneRaggruppato> ComuniRaggruppati { get; } = new();
        public ObservableCollection<ComuneRaggruppato> ComuniFiltrati { get; } = new();

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

                // 2. RAGGRUPPA PER COMUNE
                var comuniRaggruppati = RaggruppaPerComune(giochi);
                _tuttiComuni = comuniRaggruppati;  // SALVA LISTA COMPLETA

                // 3. AGGIORNA LE LISTE VISIBILI
                ComuniRaggruppati.Clear();
                ComuniFiltrati.Clear();  // AGGIUNGI

                foreach (var comune in comuniRaggruppati)
                {
                    ComuniRaggruppati.Add(comune);
                    ComuniFiltrati.Add(comune);  // AGGIUNGI
                    //Console.WriteLine($"  • Aggiunto: {comune.NomeComune}");
                }

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

            //Console.WriteLine($"🏙️ Trovati {gruppiPerComune.Count} comuni distinti");

            foreach (var gruppo in gruppiPerComune)
            {
                var nomeComune = gruppo.Key;
                var cacceDelComune = gruppo.ToList();

                //Console.WriteLine($"  • {nomeComune}: {cacceDelComune.Count} cacce");

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
        // FILTRO RICERCA AUTOMATICO
        // ============================================

        /// <summary>
        /// Metodo chiamato automaticamente quando cambia il testo di ricerca
        /// Gestisce il filtro in tempo reale dei comuni visualizzati
        /// </summary>
        /// <param name="value">Nuovo valore della ricerca</param>
        partial void OnTestoRicercaChanged(string value)
        {
            FiltraComuni();
        }

        /// <summary>
        /// Filtra la lista dei comuni in base al testo di ricerca
        /// Mostra solo i comuni il cui nome contiene il testo cercato (case-insensitive)
        /// Se la ricerca è vuota, mostra tutti i comuni
        /// </summary>
        private void FiltraComuni()
        {
            if (string.IsNullOrWhiteSpace(TestoRicerca))
            {
                // MOSTRA TUTTI I COMUNI
                ComuniFiltrati.Clear();
                foreach (var comune in _tuttiComuni)
                {
                    ComuniFiltrati.Add(comune);
                }
            }
            else
            {
                // FILTRA PER NOME COMUNE
                var testo = TestoRicerca.Trim().ToUpper();
                ComuniFiltrati.Clear();

                foreach (var comune in _tuttiComuni)
                {
                    if (comune.NomeComune.ToUpper().Contains(testo))
                    {
                        ComuniFiltrati.Add(comune);
                    }
                }
            }

            // AGGIORNA STATO DELLA UI
            IsSuccesso = ComuniFiltrati.Count > 0;
            IsVuoto = ComuniFiltrati.Count == 0;
        }

        // ============================================
        // COMANDI SECONDARI
        // ============================================

        /// <summary>
        /// Comando per resettare la ricerca e mostrare tutti i comuni
        /// </summary>
        [RelayCommand]
        private void MostraTutti()
        {
            TestoRicerca = string.Empty;
            // Ricarica tutti i comuni
            //CaricaComuniCommand.Execute(null);
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
        private async Task VediCacce(ComuneRaggruppato comune)
        {
            if (comune == null) return;

            Console.WriteLine($"DEBUG: Navigazione a dettaglio per {comune.NomeComune}");

            // ★★★ CREA NUOVA PAGINA E USA SetComune ★★★
            var dettaglioPage = new DettaglioComunePage();
            dettaglioPage.SetComune(comune.NomeComune);

            // Naviga
            await Shell.Current.Navigation.PushAsync(dettaglioPage);
        }
    }
}







