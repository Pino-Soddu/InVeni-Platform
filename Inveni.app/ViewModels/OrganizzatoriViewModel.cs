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
    public partial class OrganizzatoriViewModel : BaseViewModel
    {
        private readonly ApiServizio _apiServizio;
        private List<OrganizzatoreRaggruppato> _tuttiOrganizzatori = new();

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
        [NotifyPropertyChangedFor(nameof(OrganizzatoriFiltrati))]
        private string testoRicerca = string.Empty;

        // ============================================
        // PROPRIETÀ PUBBLICHE
        // ============================================

        public ObservableCollection<OrganizzatoreRaggruppato> OrganizzatoriRaggruppati { get; } = new();
        public ObservableCollection<OrganizzatoreRaggruppato> OrganizzatoriFiltrati { get; } = new();

        // ============================================
        // COSTRUTTORE
        // ============================================

        public OrganizzatoriViewModel(ApiServizio apiServizio)
        {
            _apiServizio = apiServizio;
            Title = "PER ORGANIZZATORE";

            // Carica dati all'avvio
            Task.Run(async () => await CaricaOrganizzatori());
        }

        // ============================================
        // COMANDO PRINCIPALE: CARICA ORGANIZZATORI
        // ============================================

        [RelayCommand]
        private async Task CaricaOrganizzatori()
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

                // 2. RAGGRUPPA PER ORGANIZZATORE
                var organizzatoriRaggruppati = RaggruppaOrganizzatori(giochi);
                _tuttiOrganizzatori = organizzatoriRaggruppati;  // SALVA LISTA COMPLETA

                // 3. AGGIORNA LE LISTE VISIBILI
                OrganizzatoriRaggruppati.Clear();
                OrganizzatoriFiltrati.Clear();  // AGGIUNGI

                foreach (var organizzatore in organizzatoriRaggruppati)
                {
                    OrganizzatoriRaggruppati.Add(organizzatore);
                    OrganizzatoriFiltrati.Add(organizzatore);  // AGGIUNGI
                    //Console.WriteLine($"  • Aggiunto: {organizzatore.NomeOrganizzatore}");
                }

                // 4. IMPOSTA STATO UI
                IsCaricamento = false;
                IsSuccesso = OrganizzatoriRaggruppati.Count > 0;
                IsVuoto = OrganizzatoriRaggruppati.Count == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Errore caricamento organizzatori: {ex.Message}");
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
        // METODO PRIVATO: RAGGRUPPA PER ORGANIZZATORE
        // ============================================

        /// <summary>
        /// Raggruppa la lista di giochi per organizzatore e calcola i conteggi per stato
        /// </summary>
        private List<OrganizzatoreRaggruppato> RaggruppaOrganizzatori(List<Gioco> giochi)
        {
            var risultato = new List<OrganizzatoreRaggruppato>();
            var now = DateTime.Now;

            // 1. RAGGRUPPA PER NOME ORGANIZZATORE
            var gruppiOrganizzatori = giochi
                .Where(g => !string.IsNullOrEmpty(g.organizzatore))
                .GroupBy(g => g.organizzatore.Trim().ToUpper())
                .ToList();

            //Console.WriteLine($"🏙️ Trovati {gruppiOrganizzatori.Count} organizzatori distinti");

            foreach (var gruppo in gruppiOrganizzatori)
            {
                var nomeOrganizzatore = gruppo.Key;
                var cacceDelOrganizzatore = gruppo.ToList();

                //Console.WriteLine($"  • {nomeOrganizzatore}: {cacceDelOrganizzatore.Count} cacce");

                // 2. CALCOLA CONTEGGI PER STATO
                int attive = 0;
                int programmate = 0;
                int scaduteDisponibili = 0;

                foreach (var caccia in cacceDelOrganizzatore)
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
                var organizzatoreRaggruppato = new OrganizzatoreRaggruppato(nomeOrganizzatore)
                {
                    TotaleCacce = cacceDelOrganizzatore.Count,
                    CacceAttive = attive,
                    CacceProgrammate = programmate,
                    CacceScaduteDisponibili = scaduteDisponibili,
                    CacceDettaglio = cacceDelOrganizzatore
                };

                risultato.Add(organizzatoreRaggruppato);
            }

            // 4. ORDINA ALFABETICAMENTE
            return risultato.OrderBy(c => c.NomeOrganizzatore).ToList();
        }


        // ============================================
        // FILTRO RICERCA AUTOMATICO
        // ============================================

        /// <summary>
        /// Metodo chiamato automaticamente quando cambia il testo di ricerca
        /// Gestisce il filtro in tempo reale dei organizzatori visualizzati
        /// </summary>
        /// <param name="value">Nuovo valore della ricerca</param>
        partial void OnTestoRicercaChanged(string value)
        {
            FiltraOrganizzatori();
        }

        /// <summary>
        /// Filtra la lista dei organizzatori in base al testo di ricerca
        /// Mostra solo i organizzatori il cui nome contiene il testo cercato (case-insensitive)
        /// Se la ricerca è vuota, mostra tutti i organizzatori
        /// </summary>
        private void FiltraOrganizzatori()
        {
            if (string.IsNullOrWhiteSpace(TestoRicerca))
            {
                // MOSTRA TUTTI I ORGANIZZATORI
                OrganizzatoriFiltrati.Clear();
                foreach (var organizzatore in _tuttiOrganizzatori)
                {
                    OrganizzatoriFiltrati.Add(organizzatore);
                }
            }
            else
            {
                // FILTRA PER NOME ORGANIZZATORE
                var testo = TestoRicerca.Trim().ToUpper();
                OrganizzatoriFiltrati.Clear();

                foreach (var organizzatore in _tuttiOrganizzatori)
                {
                    if (organizzatore.NomeOrganizzatore.ToUpper().Contains(testo))
                    {
                        OrganizzatoriFiltrati.Add(organizzatore);
                    }
                }
            }

            // AGGIORNA STATO DELLA UI
            IsSuccesso = OrganizzatoriFiltrati.Count > 0;
            IsVuoto = OrganizzatoriFiltrati.Count == 0;
        }

        // ============================================
        // COMANDI SECONDARI
        // ============================================

        /// <summary>
        /// Comando per resettare la ricerca e mostrare tutti i organizzatori
        /// </summary>
        [RelayCommand]
        private void MostraTutti()
        {
            TestoRicerca = string.Empty;
            // Ricarica tutti i organizzatori
            //CaricaOrganizzatoriCommand.Execute(null);
        }

        /// <summary>
        /// Comando per riprovare il caricamento in caso di errore
        /// </summary>
        [RelayCommand]
        private void Riprova()
        {
            CaricaOrganizzatoriCommand.Execute(null);
        }

        /// <summary>
        /// Comando per vedere le cacce dettaglio di un organizzatore
        /// </summary>
        [RelayCommand]
        private async Task VediCacce(OrganizzatoreRaggruppato organizzatore)
        {
            if (organizzatore == null) return;

            Console.WriteLine($"DEBUG: Navigazione a dettaglio per {organizzatore.NomeOrganizzatore}");

            // ★★★ CREA NUOVA PAGINA E USA SetOrganizzatore ★★★
            var dettaglioPage = new DettaglioOrganizzatorePage();
            dettaglioPage.SetOrganizzatore(organizzatore.NomeOrganizzatore);

            // Naviga
            await Shell.Current.Navigation.PushAsync(dettaglioPage);
        }
    }
}







