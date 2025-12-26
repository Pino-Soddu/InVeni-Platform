using Microsoft.Maui.Controls;
using Inveni.App.Modelli;
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

        // ★★★★ METODO NUOVO PER GESTIRE IL TAP ★★★★
        private async void OnDettagliTapped(object sender, EventArgs e)
        {
            //Console.WriteLine($"=== DEBUG OnDettagliTapped ===");
            //Console.WriteLine($"Sender: {sender?.GetType().Name}");

            if (sender is Border border)
            {
                //Console.WriteLine($"Border BindingContext type: {border.BindingContext?.GetType().Name ?? "NULL"}");
                //Console.WriteLine($"Border BindingContext value: {border.BindingContext}");

                // Controlla se è Gioco
                if (border.BindingContext is Gioco caccia)
                {
                    //Console.WriteLine($"✅ Trovato Gioco! ID: {caccia.IdGioco}, Name: {caccia.name}");
                    await Shell.Current.Navigation.PushAsync(new DettaglioCacciaPage(caccia.IdGioco));
                }
                else
                {
                    Console.WriteLine($"❌ BindingContext NON è Gioco");
                }
            }
        }
    }
}