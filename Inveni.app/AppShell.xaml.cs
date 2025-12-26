using Inveni.App.Pages;

namespace Inveni.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registra le route per la navigazione
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            // Registra tutte le pagine navigabili

            Routing.RegisterRoute(nameof(MainTabPage), typeof(MainTabPage));

            Routing.RegisterRoute(nameof(VicinoAMePage), typeof(VicinoAMePage));
            Routing.RegisterRoute(nameof(PerComunePage), typeof(PerComunePage));
            Routing.RegisterRoute(nameof(OrganizzatoriPage), typeof(OrganizzatoriPage));
            Routing.RegisterRoute(nameof(InEvidenzaPage), typeof(InEvidenzaPage));
            Routing.RegisterRoute(nameof(DettaglioComunePage), typeof(DettaglioComunePage));
            Routing.RegisterRoute(nameof(DettaglioOrganizzatorePage), typeof(DettaglioOrganizzatorePage));
        }
    }
}