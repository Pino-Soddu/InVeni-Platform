using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Servizi;
using Inveni.App.Modelli;
using System.Collections.ObjectModel;

namespace Inveni.App.ViewModels
{
    public partial class OrganizzatoriViewModel : BaseViewModel
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

        [ObservableProperty]
        private string testoRicerca = string.Empty;

        // ============================================
        // PROPRIETÀ PUBBLICHE
        // ============================================

        public ObservableCollection<OrganizzatoreRaggruppato> OrganizzatoriRaggruppati { get; } = new();

        // ============================================
        // COSTRUTTORE
        // ============================================

        public OrganizzatoriViewModel(ApiServizio apiServizio)
        {
            _apiServizio = apiServizio;
            Title = "ORGANIZZATORI";

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

                Console.WriteLine($"✅ Ricevute {giochi.Count} cacce dal backend");

                // 2. RAGGRUPPA PER ORGANIZZATORE
                var organizzatoriRaggruppati = RaggruppaPerOrganizzatore(giochi);

                // 3. AGGIORNA LA LISTA VISIBILE
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OrganizzatoriRaggruppati.Clear();

                    foreach (var organizzatore in organizzatoriRaggruppati)
                    {
                        OrganizzatoriRaggruppati.Add(organizzatore);
                    }

                    Console.WriteLine($"📊 Creati {OrganizzatoriRaggruppati.Count} organizzatori raggruppati");

                    // 4. IMPOSTA STATO UI
                    IsCaricamento = false;
                    IsSuccesso = OrganizzatoriRaggruppati.Count > 0;
                    IsVuoto = OrganizzatoriRaggruppati.Count == 0;
                });
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
        private List<OrganizzatoreRaggruppato> RaggruppaPerOrganizzatore(List<Gioco> giochi)
        {
            var risultato = new List<OrganizzatoreRaggruppato>();
            var now = DateTime.Now;

            // 1. RAGGRUPPA PER NOME ORGANIZZATORE
            var gruppiPerOrganizzatore = giochi
                .Where(g => !string.IsNullOrEmpty(g.organizzatore))
                .GroupBy(g => g.organizzatore.Trim())
                .ToList();

            Console.WriteLine($"👤 Trovati {gruppiPerOrganizzatore.Count} organizzatori distinti");

            foreach (var gruppo in gruppiPerOrganizzatore)
            {
                var nomeOrganizzatore = gruppo.Key;
                var cacceDellOrganizzatore = gruppo.ToList();

                Console.WriteLine($"  • {nomeOrganizzatore}: {cacceDellOrganizzatore.Count} cacce");

                // 2. CALCOLA CONTEGGI PER STATO
                int attive = 0;
                int programmate = 0;
                int scaduteDisponibili = 0;

                foreach (var caccia in cacceDellOrganizzatore)
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
                    TotaleCacce = cacceDellOrganizzatore.Count,
                    CacceAttive = attive,
                    CacceProgrammate = programmate,
                    CacceScaduteDisponibili = scaduteDisponibili,
                    CacceDettaglio = cacceDellOrganizzatore
                };

                risultato.Add(organizzatoreRaggruppato);
            }

            // 4. ORDINA ALFABETICAMENTE
            return risultato.OrderBy(o => o.NomeOrganizzatore).ToList();
        }

        // ============================================
        // COMANDI SECONDARI
        // ============================================

        /// <summary>
        /// Comando per filtrare gli organizzatori in base al testo di ricerca
        /// </summary>
        [RelayCommand]
        private void Ricerca()
        {
            Console.WriteLine($"Ricerca organizzatori: {TestoRicerca}");
            // TODO: Implementare filtro
        }

        /// <summary>
        /// Comando per resettare la ricerca e mostrare tutti gli organizzatori
        /// </summary>
        [RelayCommand]
        private void MostraTutti()
        {
            TestoRicerca = string.Empty;
            // Ricarica tutti gli organizzatori
            CaricaOrganizzatoriCommand.Execute(null);
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
        private void VediCacce(OrganizzatoreRaggruppato organizzatore)
        {
            if (organizzatore == null) return;

            Console.WriteLine($"Selezionato organizzatore: {organizzatore.NomeOrganizzatore}");
            Console.WriteLine($"  • Totale cacce: {organizzatore.TotaleCacce}");
            Console.WriteLine($"  • Attive: {organizzatore.CacceAttive}");
            Console.WriteLine($"  • Programmate: {organizzatore.CacceProgrammate}");
            Console.WriteLine($"  • Scadute/Disponibili: {organizzatore.CacceScaduteDisponibili}");

            // TODO: Naviga a pagina dettaglio organizzatore
        }
    }
}