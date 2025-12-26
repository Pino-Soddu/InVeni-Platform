using Inveni.App.ViewModels;
using Inveni.App.Servizi;
using Microsoft.Maui.Controls;
using Inveni.App.Modelli;  

namespace Inveni.App.Pages
{
    public partial class DettaglioOrganizzatorePage : ContentPage
    {
        public DettaglioOrganizzatorePage()
        {
            InitializeComponent();
        }

        // ★★★ METODO PER IMPOSTARE ORGANIZZATORE (già esistente) ★★★
        public void SetOrganizzatore(string nomeOrganizzatore)
        {
            var apiServizio = new ApiServizio();
            var viewModel = new DettaglioOrganizzatoreViewModel(apiServizio, nomeOrganizzatore);
            BindingContext = viewModel;
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