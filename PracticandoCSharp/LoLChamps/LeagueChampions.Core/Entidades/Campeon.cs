// Campeon.cs
namespace LeagueChampions.Core.Entidades;

public class Campeon
{
    public string Id { get; set; } = string.Empty;  // IMPORTANTE: string, no int
    public string Nombre { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Blurb { get; set; } = string.Empty;
    public List<string> Tipos { get; set; } = new();
    public Estadisticas estadisticas { get; set; } = new();
    public ImageInfo Image { get; set; } = new();
}