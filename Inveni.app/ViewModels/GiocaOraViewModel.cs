using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Inveni.App.ViewModels
{
    public partial class GiocaOraViewModel : BaseViewModel
    {
        [ObservableProperty]
        private int _selectedTabIndex = 0; // Default: prima scheda (VICINO A ME)

        public GiocaOraViewModel()
        {
            Title = "GIOCA ORA";
        }

        [RelayCommand]
        private void TabChanged(string indexString)  // CAMBIA: int → string
        {
            // Converti la stringa in numero
            if (int.TryParse(indexString, out int index))
            {
                SelectedTabIndex = index;
                Console.WriteLine($"✅ Cambiata scheda a indice: {index}");
                // TODO: Qui cambieremo il contenuto principale in base alla tab
            }
            else
            {
                Console.WriteLine($"⚠️ Errore: parametro non valido: {indexString}");
            }
        }
    }
}