using System.Net.Http.Json; 
using System.Text.Json;
using LeagueChampions.Core.Entidades;

namespace LeagueChampions.Data.Servicios;

public class RiotDataDragonServicio
{
    private readonly HttpClient _httpClient;
    
    public RiotDataDragonServicio(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<Campeon>> ObtenerTodosAsync()
    {
        var versionUrl = "https://ddragon.leagueoflegends.com/api/versions.json";
        var versionResponse = await _httpClient.GetAsync(versionUrl);
        var versiones = await versionResponse.Content.ReadFromJsonAsync<List<string>>();
        var ultimaVersion = versiones?.FirstOrDefault() ?? "14.10.1";
        
        var championsUrl = $"https://ddragon.leagueoflegends.com/cdn/{ultimaVersion}/data/es_MX/champion.json";
        var response = await _httpClient.GetAsync(championsUrl);
        var jsonString = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(jsonString);
        
        var dataElement = jsonDoc.RootElement.GetProperty("data");
        var campeones = new List<Campeon>();
        
        foreach (var propiedad in dataElement.EnumerateObject())
        {
            var campeonJson = propiedad.Value;
            var campeon = new Campeon
            {
                Id = propiedad.Name,  // "Ahri", "Ashe", etc.
                Nombre = campeonJson.GetProperty("name").GetString() ?? string.Empty,
                Titulo = campeonJson.GetProperty("title").GetString() ?? string.Empty,
                Blurb = campeonJson.GetProperty("blurb").GetString() ?? string.Empty,
                Tipos = campeonJson.GetProperty("tags").EnumerateArray().Select(t => t.GetString() ?? string.Empty).ToList(),
                Image = new ImageInfo
                {
                    Full = campeonJson.GetProperty("image").GetProperty("full").GetString() ?? string.Empty,
                    Sprite = campeonJson.GetProperty("image").GetProperty("sprite").GetString() ?? string.Empty
                },
                estadisticas = new Estadisticas
                {
                    Hp = campeonJson.GetProperty("stats").GetProperty("hp").GetDouble(),
                    HpPorNivel = campeonJson.GetProperty("stats").GetProperty("hpperlevel").GetDouble(),
                    Mr = campeonJson.GetProperty("stats").GetProperty("spellblock").GetDouble(),
                    MrPorNivel = campeonJson.GetProperty("stats").GetProperty("spellblockperlevel").GetDouble(),
                    Armadura = campeonJson.GetProperty("stats").GetProperty("armor").GetDouble(),
                    ArmaduraPorNivel = campeonJson.GetProperty("stats").GetProperty("armorperlevel").GetDouble(),
                    Danio = campeonJson.GetProperty("stats").GetProperty("attackdamage").GetDouble(),
                    DanioPorNivel = campeonJson.GetProperty("stats").GetProperty("attackdamageperlevel").GetDouble()
                }
            };
            campeones.Add(campeon);
        }
        
        return campeones;
    }
}