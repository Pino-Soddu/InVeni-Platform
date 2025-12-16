using Microsoft.Maui.Controls;
using Inveni.App.ViewModels;
using Inveni.App.Servizi;

namespace Inveni.App.Pages
{
    public partial class OrganizzatoriPage : ContentPage
    {
        public OrganizzatoriPage()
        {
            InitializeComponent();
            BindingContext = new OrganizzatoriViewModel(new ApiServizio());
        }
    }
}