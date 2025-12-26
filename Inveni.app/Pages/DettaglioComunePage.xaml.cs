using Inveni.App.ViewModels;
using Inveni.App.Servizi;
using Microsoft.Maui.Controls;
using Inveni.App.Modelli;

namespace Inveni.App.Pages
{
    public partial class DettaglioComunePage : ContentPage
    {
        public DettaglioComunePage()
        {
            InitializeComponent();
        }

        // ★★★ METODO PER IMPOSTARE IL COMUNE ★★★
        public void SetComune(string nomeComune)
        {
            // Crea ViewModel e imposta BindingContext
            var apiServizio = new ApiServizio();
            var viewModel = new DettaglioComuneViewModel(apiServizio);
            BindingContext = viewModel;

            // Inizializza
            viewModel.Initialize(nomeComune);

            Console.WriteLine($"★★★ SetComune: {nomeComune}");
        }

        // ★★★★ METODO NUOVO PER GESTIRE IL TAP ★★★★
        private async void OnDettagliTapped(object sender, EventArgs e)
        {
            // 1. Trova la card cliccata
            if (sender is Border border && border.BindingContext is Gioco caccia)
            {
                // 2. Prendi l'ID
                int cacciaId = caccia.IdGioco;

                // 3. Apri la scheda (Navigation è disponibile perché siamo in una Page)
                await Navigation.PushAsync(new DettaglioCacciaPage(cacciaId));
            }
        }
    }
}