using Microsoft.Maui.Controls;
using Inveni.App.ViewModels;
using Inveni.App.Servizi;

namespace Inveni.App.Pages
{
    public partial class VicinoAMePage : ContentPage
    {
        public VicinoAMePage()
        {
            InitializeComponent();
            BindingContext = new VicinoAMeViewModel(new ApiServizio());
        }
    }
}