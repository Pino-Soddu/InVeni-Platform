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
            Routing.RegisterRoute("GiocaOraPage", typeof(Pages.GiocaOraPage));
            Routing.RegisterRoute("InProgrammaPage", typeof(Pages.InProgrammaPage));
            Routing.RegisterRoute("StoricoPage", typeof(Pages.StoricoPage));

            Routing.RegisterRoute(nameof(VicinoAMePage), typeof(VicinoAMePage));
            Routing.RegisterRoute(nameof(PerComunePage), typeof(PerComunePage));
            Routing.RegisterRoute(nameof(OrganizzatoriPage), typeof(OrganizzatoriPage));
            Routing.RegisterRoute(nameof(InEvidenzaPage), typeof(InEvidenzaPage));
        }
    }
}