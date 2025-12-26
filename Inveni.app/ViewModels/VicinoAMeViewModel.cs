using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inveni.App.Modelli;
using Inveni.App.Servizi;
using Inveni.App.Controls;
using System.Collections.ObjectModel;

namespace Inveni.App.ViewModels
{
    public partial class VicinoAMeViewModel : BaseViewModel
    {

        private readonly ApiServizio _apiServizio;
        private ObservableCollection<TabItem> _tabItems;
        public ObservableCollection<TabItem> TabItems
        {
            get => _tabItems;
            set => SetProperty(ref _tabItems, value);
        }

        private TabItem _selectedTab;
        public TabItem SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (SetProperty(ref _selectedTab, value) && value != null)
                {
                    // Quando cambia tab, aggiorna tutti gli stati
                    UpdateTabStates();
                }
            }
        }

        [ObservableProperty]
        private bool _isRefreshing;

        // STATI ACCORDION - USIAMO BACKING FIELDS TEMPORANEI
        private bool _isGiocaOraEspanso = true;
        private bool _isInProgrammaEspanso = false;
        private bool _isStoricheEspanso = false;

        // PROPRIETÀ PUBBLICHE (non automatiche per ora)
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

        // Collezioni ESISTENTI
        public ObservableCollection<Gioco> CacceAttive { get; } = new();
        public ObservableCollection<Gioco> CacceProgrammate { get; } = new();
        public ObservableCollection<Gioco> CacceStoriche { get; } = new();

        public VicinoAMeViewModel(ApiServizio apiServizio)
        {
            _apiServizio = apiServizio;
            Title = "VICINO A ME";

            // --- AGGIUNGI QUESTA RIGA ---
            InitializeTabItems();

            Task.Run(async () => await CaricaDati());
        }

        // --- AGGIUNGI QUESTO METODO QUI ---
        private void InitializeTabItems()
        {
            TabItems = new ObservableCollection<TabItem>
    {
        new TabItem
        {
            Title = "VICINO A ME",
            IsActive = true,
            SelectCommand = new RelayCommand(() => SelectTab(0))
        },
        new TabItem
        {
            Title = "PER COMUNE",
            IsActive = false,
            SelectCommand = new RelayCommand(() => SelectTab(1))
        },
        new TabItem
        {
            Title = "ORGANIZZATORE",
            IsActive = false,
            SelectCommand = new RelayCommand(() => SelectTab(2))
        },
        new TabItem
        {
            Title = "IN EVIDENZA",
            IsActive = false,
            SelectCommand = new RelayCommand(() => SelectTab(3))
        }
    };

            SelectedTab = TabItems.FirstOrDefault();
        }

        // --- AGGIUNGI ANCHE QUESTO METODO ---
        private void SelectTab(int index)
        {
            if (index >= 0 && index < TabItems.Count)
            {
                SelectedTab = TabItems[index];
            }
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

        // METODO CaricaDati
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

                //Console.WriteLine($"✅ Ricevute {giochi.Count} cacce");

                var now = DateTime.Now;

                // USARE MainThread per modifiche UI
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Pulisci liste
                    CacceAttive.Clear();
                    CacceProgrammate.Clear();
                    CacceStoriche.Clear();

                    foreach (var gioco in giochi)
                    {
                        Console.WriteLine($"Caccia: '{gioco.name}', ID={gioco.IdGioco}, _id={gioco._id}, IdUtente={gioco.IdUtente}");

                        // Filtra per data
                        if (gioco.dataInizio == null || gioco.dataFine == null)
                        {
                            Console.WriteLine("   ❌ date null - salto");
                            continue;
                        }

                        if (gioco.dataInizio <= now && gioco.dataFine >= now)
                        {
                            CacceAttive.Add(gioco);
                            Console.WriteLine($"   ✅ Aggiunta a ATTIVE");
                        }
                        else if (gioco.dataInizio > now)
                        {
                            CacceProgrammate.Add(gioco);
                            Console.WriteLine($"   ✅ Aggiunta a PROGRAMMATE");
                        }
                        else // gioco.dataFine < now
                        {
                            CacceStoriche.Add(gioco);
                            Console.WriteLine($"   ✅ Aggiunta a STORICHE");
                        }
                    }

                    //Console.WriteLine($"📊 Statistiche: {CacceAttive.Count} attive, {CacceProgrammate.Count} programmate, {CacceStoriche.Count} storiche");
                });

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

        // METODO Refresh (ESISTENTE - nessuna modifica)
        [RelayCommand]
        private async Task Refresh()
        {
            await CaricaDati();
        }

        private void UpdateTabStates()
        {
            if (TabItems == null) return;

            foreach (var tab in TabItems)
            {
                tab.IsActive = (tab == SelectedTab);
            }
        }

    }
}