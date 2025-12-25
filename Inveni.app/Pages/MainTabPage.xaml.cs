using CommunityToolkit.Mvvm.Input;
using Inveni.App.Controls;
using Inveni.App.Servizi;
using Inveni.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Inveni.App.Pages
{
    public partial class MainTabPage : ContentPage
    {
        // CACHE DELLE PAGINE GIÀ CARICATE
        private ContentView _vicinoAMeContent;
        private ContentView _perComuneContent;
        private ContentView _organizzatoriContent;
        private ContentView _inEvidenzaContent;

        public MainTabPage()
        {
            InitializeComponent();
            LoadTabs();

            // Carica il contenuto iniziale (VICINO A ME)
            ShowTabContent(0);
        }

        private void LoadTabs()
        {
            var tabs = new ObservableCollection<TabItem>
            {
                new TabItem
                {
                    Title = "VICINO A ME",
                    IsActive = true,
                    SelectCommand = new RelayCommand(() => ShowTabContent(0))
                },
                new TabItem
                {
                    Title = "PER COMUNE",
                    IsActive = false,
                    SelectCommand = new RelayCommand(() => ShowTabContent(1))
                },
                new TabItem
                {
                    Title = "ORGANIZZATORE",
                    IsActive = false,
                    SelectCommand = new RelayCommand(() => ShowTabContent(2))
                },
                new TabItem
                {
                    Title = "IN EVIDENZA",
                    IsActive = false,
                    SelectCommand = new RelayCommand(() => ShowTabContent(3))
                }
            };

            TabNavigation.TabItems = tabs;
            TabNavigation.SelectedTab = tabs[0];
        }

        private void ShowTabContent(int tabIndex)
        {
            // Aggiorna tab attiva
            UpdateActiveTab(tabIndex);

            switch (tabIndex)
            {
                case 0: // VICINO A ME
                    ShowVicinoAMeContent();
                    break;

                case 1: // PER COMUNE
                    ShowPerComuneContent();
                    break;

                case 2: // ORGANIZZATORE
                    ShowOrganizzatoriContent();
                    break;

                case 3: // IN EVIDENZA
                    ShowInEvidenzaContent();
                    break;
            }
        }

        private void ShowVicinoAMeContent()
        {
            // SE ESISTE GIÀ, RIUSA
            if (_vicinoAMeContent != null)
            {
                PageContainer.Content = _vicinoAMeContent;
                return;
            }

            // ALTRIMENTI CREA
            try
            {
                var apiService = App.Current?.Handler?.MauiContext?.Services?.GetService<ApiServizio>();

                if (apiService == null)
                {
                    ShowError("ApiServizio non disponibile");
                    return;
                }

                var viewModel = new VicinoAMeViewModel(apiService);
                var page = new VicinoAMePage();
                page.BindingContext = viewModel;

                _vicinoAMeContent = new ContentView
                {
                    Content = page.Content,
                    BindingContext = viewModel
                };

                PageContainer.Content = _vicinoAMeContent;
            }
            catch (Exception ex)
            {
                ShowError($"Errore: {ex.Message}");
            }
        }

        private void ShowPerComuneContent()
        {
            // SE ESISTE GIÀ, RIUSA
            if (_perComuneContent != null)
            {
                PageContainer.Content = _perComuneContent;
                return;
            }

            // ALTRIMENTI CREA
            try
            {
                var apiService = App.Current?.Handler?.MauiContext?.Services?.GetService<ApiServizio>();

                if (apiService == null)
                {
                    ShowError("ApiServizio non disponibile");
                    return;
                }

                var viewModel = new PerComuneViewModel(apiService);
                var page = new PerComunePage();
                page.BindingContext = viewModel;

                _perComuneContent = new ContentView
                {
                    Content = page.Content,
                    BindingContext = viewModel
                };

                PageContainer.Content = _perComuneContent;
            }
            catch (Exception ex)
            {
                ShowError($"Errore PER COMUNE: {ex.Message}");
            }
        }

        private void ShowOrganizzatoriContent()
        {
            if (_organizzatoriContent == null)
            {
                // SE ESISTE GIÀ, RIUSA
                if (_organizzatoriContent != null)
                {
                    PageContainer.Content = _organizzatoriContent;
                    return;
                }


                // ALTRIMENTI CREA
                try
                {
                    var apiService = App.Current?.Handler?.MauiContext?.Services?.GetService<ApiServizio>();

                    if (apiService == null)
                    {
                        ShowError("ApiServizio non disponibile");
                        return;
                    }

                    var viewModel = new OrganizzatoriViewModel(apiService);
                    var page = new OrganizzatoriPage();
                    page.BindingContext = viewModel;

                    _organizzatoriContent = new ContentView
                    {
                        Content = page.Content,
                        BindingContext = viewModel
                    };

                    PageContainer.Content = _organizzatoriContent;
                }
                catch (Exception ex)
                {
                    ShowError($"Errore PER ORGANIZZATORE: {ex.Message}");
                }
            }

            PageContainer.Content = _organizzatoriContent;
        }

        private void ShowInEvidenzaContent()
        {
            if (_inEvidenzaContent == null)
            {
                _inEvidenzaContent = new ContentView
                {
                    Content = new Label
                    {
                        Text = "IN EVIDENZA (in sviluppo)",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 20
                    }
                };
            }

            PageContainer.Content = _inEvidenzaContent;
        }

        private void UpdateActiveTab(int activeIndex)
        {
            if (TabNavigation.TabItems != null)
            {
                for (int i = 0; i < TabNavigation.TabItems.Count; i++)
                {
                    TabNavigation.TabItems[i].IsActive = (i == activeIndex);
                }
            }
        }

        private void ShowError(string message)
        {
            PageContainer.Content = new Label
            {
                Text = message,
                TextColor = Colors.Red,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
        }
    }
}