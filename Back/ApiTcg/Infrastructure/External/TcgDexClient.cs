using Infrastructure.Dtos.Card;
using Infrastructure.Dtos.Set;

using System.Net.Http.Json;


namespace Infrastructure.External;

public class TcgDexClient
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://api.tcgdex.net/v2";

    public TcgDexClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<TcgDexSetBriefDto>> GetAllSetsAsync(string lang = "fr") 
    {
        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/{lang}/sets");
        response.EnsureSuccessStatusCode();

        List<TcgDexSetBriefDto>? sets = await response.Content
           .ReadFromJsonAsync<List<TcgDexSetBriefDto>>();

        return sets ?? new List<TcgDexSetBriefDto>();
    }

    public async Task<List<TcgDexCardBriefDto>> GetAllCardsAsync(string lang = "fr")
    {
        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/{lang}/cards");
        response.EnsureSuccessStatusCode(); 

        List<TcgDexCardBriefDto>? cards = await response.Content
           .ReadFromJsonAsync<List<TcgDexCardBriefDto>>();

        return cards ?? new List<TcgDexCardBriefDto>();
    }
}
