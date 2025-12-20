using System.Reflection;

namespace Inveni.App.Servizi;

public class ResourceService
{
    /// <summary>
    /// Ottiene l'immagine di una città dalle risorse MAUI
    /// </summary>
    /// <param name="nomeCitta">Nome della città (es: "Roma")</param>
    /// <param name="nomeFoto">Nome del file senza estensione (es: "colosseo"). Se null, cerca "default.jpg" o prima immagine</param>
    /// <returns>ImageSource per il binding XAML o null se non trovata</returns>
    public ImageSource? GetCityImage(string nomeCitta, string? nomeFoto = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nomeCitta))
                return null;

            // Normalizza il nome città
            string cittaNormalizzata = NormalizeCityName(nomeCitta);

            // Se nomeFoto non specificato, cerca "default.jpg" o prima immagine
            string fotoCercata = nomeFoto ?? "default";

            // Lista di estensioni da provare
            string[] estensioni = { ".jpg", ".png", ".jpeg" };

            // Cerca con il nome specificato
            foreach (var estensione in estensioni)
            {
                var percorso = $"Inveni.App.Resources.Raw.Citta.{cittaNormalizzata}.Foto.{fotoCercata}{estensione}";
                if (ResourceExists(percorso))
                {
                    return ImageSource.FromResource(percorso);
                }
            }

            // Se non trova con nome specificato, cerca "default"
            if (nomeFoto != null)
            {
                foreach (var estensione in estensioni)
                {
                    var percorso = $"Inveni.App.Resources.Raw.Citta.{cittaNormalizzata}.Foto.default{estensione}";
                    if (ResourceExists(percorso))
                    {
                        return ImageSource.FromResource(percorso);
                    }
                }
            }

            // Cerca qualsiasi immagine nella cartella della città
            var assembly = Assembly.GetExecutingAssembly();
            var risorse = assembly.GetManifestResourceNames();

            var risorsaCitta = risorse.FirstOrDefault(r =>
                r.Contains($"Inveni.App.Resources.Raw.Citta.{cittaNormalizzata}.Foto", StringComparison.OrdinalIgnoreCase));

            if (risorsaCitta != null)
            {
                return ImageSource.FromResource(risorsaCitta);
            }

            // Log per debug
            Console.WriteLine($"⚠️ Immagine non trovata per città: {nomeCitta}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Errore caricamento immagine per {nomeCitta}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Ottiene il testo descrittivo di una città
    /// </summary>
    public async Task<string?> GetCityDescription(string nomeCitta)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nomeCitta))
                return null;

            string cittaNormalizzata = NormalizeCityName(nomeCitta);

            // Lista di possibili nomi file di testo
            string[] nomiTesti = { "descrizione.txt", "info.txt", "testo.txt", $"{cittaNormalizzata}.txt" };

            foreach (var nomeTesto in nomiTesti)
            {
                var percorso = $"Raw/Citta/{cittaNormalizzata}/Testi/{nomeTesto}";

                try
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync(percorso);
                    if (stream != null)
                    {
                        using var reader = new StreamReader(stream);
                        return await reader.ReadToEndAsync();
                    }
                }
                catch (FileNotFoundException)
                {
                    // Continua con il prossimo nome
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Errore lettura testo {percorso}: {ex.Message}");
                }
            }

            Console.WriteLine($"⚠️ Testo non trovato per città: {nomeCitta}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Errore caricamento testo per {nomeCitta}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Ottiene TUTTE le immagini di una città
    /// </summary>
    public List<ImageSource> GetCityAllImages(string nomeCitta)
    {
        var immagini = new List<ImageSource>();

        try
        {
            if (string.IsNullOrWhiteSpace(nomeCitta))
                return immagini;

            string cittaNormalizzata = NormalizeCityName(nomeCitta);
            var assembly = Assembly.GetExecutingAssembly();
            var risorse = assembly.GetManifestResourceNames();

            var risorseCitta = risorse.Where(r =>
                r.Contains($"Inveni.App.Resources.Raw.Citta.{cittaNormalizzata}.Foto", StringComparison.OrdinalIgnoreCase));

            foreach (var risorsa in risorseCitta)
            {
                immagini.Add(ImageSource.FromResource(risorsa));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Errore caricamento immagini per {nomeCitta}: {ex.Message}");
        }

        return immagini;
    }

    /// <summary>
    /// Verifica se una risorsa esiste nell'assembly
    /// </summary>
    private bool ResourceExists(string resourceName)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceNames()
                .Any(name => name.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Normalizza il nome della città per il percorso risorse
    /// </summary>
    private string NormalizeCityName(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return string.Empty;

        // Trim e uppercase
        var normalized = cityName.Trim().ToUpper();

        // Rimuovi accenti
        normalized = normalized
            .Replace("À", "A").Replace("È", "E").Replace("É", "E").Replace("Ì", "I")
            .Replace("Ò", "O").Replace("Ù", "U")
            .Replace("à", "A").Replace("è", "E").Replace("é", "E").Replace("ì", "I")
            .Replace("ò", "O").Replace("ù", "U");

        // Rimuovi caratteri speciali (mantieni solo lettere, numeri e underscore)
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"[^A-Z0-9_]", "");

        return normalized;
    }

    /// <summary>
    /// Elenca tutte le città disponibili nelle risorse
    /// </summary>
    public List<string> GetAvailableCities()
    {
        var cities = new List<string>();

        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var risorse = assembly.GetManifestResourceNames();

            foreach (var risorsa in risorse)
            {
                if (risorsa.Contains("Inveni.App.Resources.Raw.Citta.") &&
                    risorsa.Contains(".Foto."))
                {
                    // Estrai il nome della città dal percorso
                    var parts = risorsa.Split('.');
                    var cittaIndex = Array.IndexOf(parts, "Citta") + 1;
                    if (cittaIndex > 0 && cittaIndex < parts.Length)
                    {
                        var citta = parts[cittaIndex];
                        if (!cities.Contains(citta))
                            cities.Add(citta);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Errore elenco città: {ex.Message}");
        }

        return cities;
    }

    /// <summary>
    /// Verifica se una città ha risorse immagini
    /// </summary>
    public bool HasCityResources(string nomeCitta)
    {
        if (string.IsNullOrWhiteSpace(nomeCitta))
            return false;

        string cittaNormalizzata = NormalizeCityName(nomeCitta);
        var assembly = Assembly.GetExecutingAssembly();
        var risorse = assembly.GetManifestResourceNames();

        return risorse.Any(r =>
            r.Contains($"Inveni.App.Resources.Raw.Citta.{cittaNormalizzata}.Foto", StringComparison.OrdinalIgnoreCase));
    }
}