using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Inveni.App.Controls
{
    public partial class TabNavigationView : ContentView
    {
        // Proprietà bindable per le tab
        public static readonly BindableProperty TabItemsProperty =
            BindableProperty.Create(
                nameof(TabItems),
                typeof(ObservableCollection<TabItem>),
                typeof(TabNavigationView),
                defaultValueCreator: _ => new ObservableCollection<TabItem>(),
                propertyChanged: OnTabItemsChanged);

        public ObservableCollection<TabItem> TabItems
        {
            get => (ObservableCollection<TabItem>)GetValue(TabItemsProperty);
            set => SetValue(TabItemsProperty, value);
        }

        // Proprietà bindable per la tab selezionata
        public static readonly BindableProperty SelectedTabProperty =
            BindableProperty.Create(
                nameof(SelectedTab),
                typeof(TabItem),
                typeof(TabNavigationView),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnSelectedTabChanged);

        public TabItem SelectedTab
        {
            get => (TabItem)GetValue(SelectedTabProperty);
            set => SetValue(SelectedTabProperty, value);
        }

        public TabNavigationView()
        {
            InitializeComponent();

            // NESSUN BindingContext qui - lascia che sia ereditato dalla pagina
            // NON creare un ViewModel interno
        }

        // Metodo chiamato quando TabItems cambia
        private static void OnTabItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TabNavigationView control && newValue is ObservableCollection<TabItem> newItems)
            {
                // Collega il BindableLayout.ItemsSource
                BindableLayout.SetItemsSource(control.TabContainer, newItems);

                // Imposta i comandi per ogni tab
                foreach (var tab in newItems)
                {
                    tab.SelectCommand = new RelayCommand(() =>
                    {
                        control.SelectedTab = tab;
                    });
                }
            }
        }

        // Metodo chiamato quando SelectedTab cambia
        private static void OnSelectedTabChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TabNavigationView control && newValue is TabItem newTab)
            {
                // Aggiorna stati delle tab
                foreach (var tab in control.TabItems)
                {
                    tab.IsActive = (tab == newTab);
                }
            }
        }
    }

    // Modello per una singola tab (MANTENUTO COSÌ COME ERA)
    // Modello per una singola tab
    public partial class TabItem : ObservableObject
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Title { get; set; }
        public object? Data { get; set; } // Dati aggiuntivi se necessario

        [ObservableProperty]
        private bool _isActive;

        // AGGIUNGI QUESTA PROPRIETÀ:
        public Color TextColor => IsActive ? Colors.White : Color.FromArgb("#CCCCCC");

        public IRelayCommand SelectCommand { get; set; }

        // AGGIUNGI QUESTO METODO:
        partial void OnIsActiveChanged(bool value)
        {
            OnPropertyChanged(nameof(TextColor));
        }
    }
}

// Extension method per fire-and-forget (MANTENUTO)
public static class TaskExtensions
{
    public static void SafeFireAndForget(this Task task, bool continueOnCapturedContext = false, Action<Exception> onException = null)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted && onException != null)
                onException(t.Exception);
        },
        continueOnCapturedContext ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Default);
    }
}