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
            Routing.RegisterRoute("GiocaOraPage", typeof(Views.GiocaOraPage));
            Routing.RegisterRoute("InProgrammaPage", typeof(Views.InProgrammaPage));
            Routing.RegisterRoute("StoricoPage", typeof(Views.StoricoPage));

            // Note: Le pagine dentro GiocaOra (Intorno, Città, etc.) 
            // saranno gestite dal TabView dentro GiocaOraPage, non dalla Shell
        }
    }
}