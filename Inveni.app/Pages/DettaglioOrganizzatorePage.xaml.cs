using Inveni.App.ViewModels;
using Inveni.App.Servizi;
using Microsoft.Maui.Controls;

namespace Inveni.App.Pages
{
    public partial class DettaglioOrganizzatorePage : ContentPage
    {
        public DettaglioOrganizzatorePage()
        {
            InitializeComponent();
        }

        // ★★★★ METODO SEMPLIFICATO ★★★★
        public void SetOrganizzatore(string nomeOrganizzatore)
        {
            // ★★★ CREA VIEWMODEL CON IL NOME GIÀ NEL COSTRUTTORE ★★★
            var apiServizio = new ApiServizio();
            var viewModel = new DettaglioOrganizzatoreViewModel(apiServizio, nomeOrganizzatore);

            BindingContext = viewModel;

            Console.WriteLine($"★★★ SetOrganizzatore: {nomeOrganizzatore}");
        }
    }
}