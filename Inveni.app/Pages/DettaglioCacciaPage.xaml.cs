using Inveni.App.ViewModels;
using Inveni.App.Servizi;
using Microsoft.Maui.Controls;

namespace Inveni.App.Pages
{
    public partial class DettaglioCacciaPage : ContentPage
    {
        public DettaglioCacciaPage(int cacciaId)
        {
            InitializeComponent();

            var apiServizio = new ApiServizio();
            BindingContext = new DettaglioCacciaViewModel(apiServizio, cacciaId);
        }
    }
}