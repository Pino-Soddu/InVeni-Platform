using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Modelli;
using Inveni.App.Servizi;
using System.Collections.ObjectModel;

namespace Inveni.App.ViewModels
{
    public partial class VicinoAMeViewModel : BaseViewModel
    {
        private readonly ApiServizio _apiServizio;

        [ObservableProperty]        private bool _isRefreshing;

        // USA EstrazioneGioco (o Gioco) DIRETTAMENTE
        public ObservableCollection<Gioco> CacceAttive { get; } = new();
        public ObservableCollection<Gioco> CacceProgrammate { get; } = new();
        public ObservableCollection<Gioco> CacceStoriche { get; } = new();

        public VicinoAMeViewModel(ApiServizio apiServizio)
        {
            _apiServizio = apiServizio;
            Title = "VICINO A ME";

            Task.Run(async () => await CaricaDati());
        }

        [RelayCommand]
        private async Task CaricaDati()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var giochi = await _apiServizio.OttieniListaGiochiAsync();

                if (giochi == null || giochi.Count == 0)
                {
                    Console.WriteLine("⚠️ Nessuna caccia ricevuta");
                    return;
                }

                Console.WriteLine($"✅ Ricevute {giochi.Count} cacce");

                // Pulisci liste
                CacceAttive.Clear();
                CacceProgrammate.Clear();
                CacceStoriche.Clear();

                var now = DateTime.Now;

                foreach (var gioco in giochi)
                {
                    // CALCOLA DISTANZA PER OGNI GIOCO
                    var distanza = CalcolaDistanza(gioco.lat, gioco.lon);

                    // AGGIUNGI PROPRIETÀ DISTANZA (se serve)
                    // Potremmo creare una classe wrapper o aggiungere property

                    // FILTRA PER DATA
                    if (gioco.dataInizio == null || gioco.dataFine == null)
                        continue;

                    if (gioco.dataInizio <= now && gioco.dataFine >= now)
                    {
                        CacceAttive.Add(gioco);
                        Console.WriteLine($"🎯 ATTIVA: {gioco.name} ({gioco.comune}) - {distanza:F1}km");
                    }
                    else if (gioco.dataInizio > now)
                    {
                        CacceProgrammate.Add(gioco);
                        Console.WriteLine($"📅 PROGRAMMATA: {gioco.name}");
                    }
                    else // gioco.dataFine < now
                    {
                        CacceStoriche.Add(gioco);
                        Console.WriteLine($"📚 STORICA: {gioco.name}");
                    }
                }

                Console.WriteLine($"📊 Statistiche: {CacceAttive.Count} attive, {CacceProgrammate.Count} programmate, {CacceStoriche.Count} storiche");

                // Ordina per distanza
                OrdinaPerDistanza(CacceAttive);
                OrdinaPerDistanza(CacceProgrammate);
                OrdinaPerDistanza(CacceStoriche);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Errore: {ex.Message}");
                await Shell.Current.DisplayAlert("Errore", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private void OrdinaPerDistanza(ObservableCollection<Gioco> lista)
        {
            // Ordina per distanza calcolata
            var ordinata = lista.OrderBy(g => CalcolaDistanza(g.lat, g.lon)).ToList();
            lista.Clear();
            foreach (var item in ordinata)
            {
                lista.Add(item);
            }
        }

        private double CalcolaDistanza(double lat, double lon)
        {
            // Simulazione: distanza da Roma (41.9028, 12.4964)
            var romaLat = 41.9028;
            var RomaLon = 12.4964;

            // Formula semplificata
            var diffLat = lat - romaLat;
            var diffLon = lon - RomaLon;
            return Math.Sqrt(diffLat * diffLat + diffLon * diffLon) * 111; // 1 grado ≈ 111km
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await CaricaDati();
        }
    }
}