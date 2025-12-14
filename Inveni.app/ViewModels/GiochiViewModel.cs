using System.Collections.ObjectModel;
using System.ComponentModel;
using Inveni.App.Servizi;

namespace Inveni.App.ViewModels;

/// <summary>
/// ViewModel per la pagina che mostra la lista delle cacce
/// Gestisce caricamento dati e stato dell'interfaccia
/// </summary>
public class GiochiViewModel : INotifyPropertyChanged
{
    private readonly ApiServizio _apiServizio;
    private bool _isLoading;
    private string _titolo = "Cacce Disponibili";

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Indica se i dati stanno caricando
    /// </summary>
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
    }

    /// <summary>
    /// Titolo della pagina
    /// </summary>
    public string Titolo
    {
        get => _titolo;
        set
        {
            if (_titolo != value)
            {
                _titolo = value;
                OnPropertyChanged(nameof(Titolo));
            }
        }
    }

    /// <summary>
    /// Lista delle cacce da mostrare
    /// </summary>
    public ObservableCollection<Modelli.Gioco> Giochi { get; } = new();

    /// <summary>
    /// Costruttore: inizializza servizio API
    /// </summary>
    public GiochiViewModel()
    {
        _apiServizio = new ApiServizio();
        CaricaGiochi();
    }

    /// <summary>
    /// Carica la lista delle cacce dal backend
    /// </summary>
    private async void CaricaGiochi()
    {
        IsLoading = true;

        try
        {
            Giochi.Clear();
            var giochi = await _apiServizio.OttieniListaGiochiAsync();

            foreach (var gioco in giochi)
            {
                Giochi.Add(gioco);
            }

            Titolo = $"Cacce ({Giochi.Count})";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore caricamento giochi: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Metodo per notificare cambiamenti alle proprietà
    /// </summary>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}