using Microsoft.Maui.Controls;
using Inveni.App.ViewModels;
using Inveni.App.Servizi;

namespace Inveni.App.Pages
{
    public partial class InEvidenzaPage : ContentPage
    {
        public InEvidenzaPage()
        {
            InitializeComponent();

            // Inizializza il ViewModel con il servizio API
            BindingContext = new InEvidenzaViewModel(new ApiServizio());
        }
    }
}