using CommunityToolkit.Mvvm.ComponentModel;
using Inveni.App.Servizi;

namespace Inveni.App.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        // ★ AGGIUNTA: ResourceService disponibile in tutti i ViewModels
        protected ResourceService ResourceService { get; }

        public BaseViewModel()
        {
            ResourceService = new ResourceService();
        }
    }
}