using Inveni.App.ViewModels;

namespace Inveni.App.Pages;

public partial class GiochiPage : ContentPage
{
    /// <summary>
    /// Costruttore: inizializza pagina e ViewModel
    /// </summary>
    public GiochiPage()
    {
        InitializeComponent();

        // Imposta BindingContext al ViewModel
        BindingContext = new GiochiViewModel();
    }

    /// <summary>
    /// Chiamato quando la pagina appare
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Forza aggiornamento se necessario
        var viewModel = BindingContext as GiochiViewModel;
        if (viewModel != null && !viewModel.Giochi.Any())
        {
            // Potrebbe essere necessario ricaricare
        }
    }
}