namespace Inveni.App.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void OnTestClicked(object sender, EventArgs e)
        {
            // Test semplice
            DisplayAlert("Test", "HomePage funziona!", "OK");
        }
    }
}