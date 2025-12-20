using System.Text.Json;
using Inveni.App.Modelli;

namespace Inveni.App.Servizi;

/// <summary>
/// Servizio per chiamare il backend Inveni.Api
/// </summary>
public class ApiServizio
{
    private const string UrlBase = "http://10.64.254.197:5000";  // RETE ILIAD
    //private const string UrlBase = "http://192.168.137.1:5000";  // RETE VODAFONE
    
    //private const string UrlBase = "https://10.0.2.2:7124";  // EMULATORE

    /// <summary>
    /// Ottiene la lista delle cacce disponibili dal backend
    /// </summary>
    public async Task<List<Gioco>> OttieniListaGiochiAsync()
    {
        try
        {
            // URL ESATTAMENTE come Swagger
            string url = "/Api/ListaGiochi/ListaGiochi?IdUtente=1&lingua=ITA&stato=tutte";

            // Configura HttpClient per accettare certificati self-signed
            var handler = new HttpClientHandler();

#if DEBUG
            // Disabilita controllo SSL solo in sviluppo
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#endif

            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(UrlBase),
                Timeout = TimeSpan.FromSeconds(15)
            };

            Console.WriteLine($"🌐 Chiamando API: {UrlBase}{url}");

            HttpResponseMessage risposta = await client.GetAsync(url);
            Console.WriteLine($"📡 Status Code: {risposta.StatusCode}");

            if (!risposta.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Errore HTTP: {risposta.StatusCode}");
                return new List<Gioco>();
            }

            string json = await risposta.Content.ReadAsStringAsync();
            Console.WriteLine($"📄 Risposta ricevuta ({json.Length} caratteri)");

            if (json.Length < 50)
            {
                Console.WriteLine($"⚠️ Risposta troppo breve: {json}");
            }

            // Deserializza la risposta JSON
            var risultato = JsonSerializer.Deserialize<RispostaApi>(json);

            if (risultato == null || !risultato.successo)
            {
                Console.WriteLine($"❌ API non ha restituito successo");
                return new List<Gioco>();
            }

            Console.WriteLine($"✅ Trovate {risultato.giochi?.Count ?? 0} cacce");
            return risultato.giochi ?? new List<Gioco>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Errore in OttieniListaGiochiAsync: {ex.GetType().Name}: {ex.Message}");
            return new List<Gioco>();
        }
    }
}

/// <summary>
/// Modello per la risposta dell'API ListaGiochi
/// </summary>
public class RispostaApi
{
    public bool successo { get; set; }
    public int count { get; set; }
    public string? comuneFiltrato { get; set; }
    public string? statoFiltrato { get; set; }
    public string? lingua { get; set; }
    public List<Gioco>? giochi { get; set; }
}