using Microsoft.Maui.Controls;
using Inveni.App.ViewModels;
using Inveni.App.Servizi;

namespace Inveni.App.Pages
{
    public partial class PerComunePage : ContentPage
    {
        public PerComunePage()
        {
            InitializeComponent();
            BindingContext = new PerComuneViewModel(new ApiServizio());
        }
    }
}