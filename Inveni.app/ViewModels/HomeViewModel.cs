using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Inveni.App.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private ObservableCollection<Caccia> _cacceAttive = new();

        // Comandi
        public ICommand RefreshCommand { get; }
        public ICommand SelezionaCacciaCommand { get; }

        public HomeViewModel()
        {
            Title = "Cacce al Tesoro";

            // Inizializza comandi
            RefreshCommand = new Command(async () => await LoadCacceAsync());
            SelezionaCacciaCommand = new Command<Caccia>(OnCacciaSelezionata);

            // Carica dati mock per test
            LoadDatiMock();
        }

        public async Task LoadCacceAsync()
        {
            try
            {
                IsRefreshing = true;
                await Task.Delay(1000); // Simula caricamento
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void LoadDatiMock()
        {
            CacceAttive.Clear();

            CacceAttive.Add(new Caccia
            {
                IdGioco = 180269,
                Name = "Caccia a Antium",
                Organizzatore = "PROLOCO DI ANZIO",
                Comune = "ANZIO",
                Photo1 = "https://via.placeholder.com/150",
                Stato = "attiva",
                DistanzaKm = 2.5,
                LocalitaCaccia = "Centro storico",
                LunghezzaCaccia = "2,5 km",
                NumTappeCaccia = "8"
            });
        }

        private void OnCacciaSelezionata(Caccia? caccia)
        {
            if (caccia == null) return;
            // Naviga alla pagina dettaglio caccia
        }

        public void OnAppearing()
        {
            // Chiamato quando la pagina appare
        }
    }
}