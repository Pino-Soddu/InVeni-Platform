using CommunityToolkit.Mvvm.ComponentModel;

namespace Inveni.App.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private bool _isBusy;
    }
}