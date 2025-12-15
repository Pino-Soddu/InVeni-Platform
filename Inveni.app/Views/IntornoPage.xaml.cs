namespace Inveni.App.Views
{
    public partial class IntornoPage : ContentPage
    {
        public IntornoPage()
        {
            InitializeComponent();

            // Imposta BindingContext al ViewModel
            BindingContext = new ViewModels.IntornoViewModel();
        }
    }
}