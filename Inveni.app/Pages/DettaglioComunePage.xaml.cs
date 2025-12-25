using Inveni.App.ViewModels;
using Inveni.App.Servizi;
using Microsoft.Maui.Controls;

namespace Inveni.App.Pages
{
    public partial class DettaglioComunePage : ContentPage
    {
        public DettaglioComunePage()
        {
            InitializeComponent();
            // ★★★ LASCIALO COMPLETAMENTE VUOTO ★★★
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
    }
}