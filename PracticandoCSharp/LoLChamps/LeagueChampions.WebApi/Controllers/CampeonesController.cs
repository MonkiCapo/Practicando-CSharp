using Microsoft.AspNetCore.Mvc;
using LeagueChampions.Data.Servicios;
using LeagueChampions.Core.Entidades;

namespace LeagueChampions.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampeonesController : ControllerBase
{
    private readonly RiotDataDragonServicio _servicio;
    
    public CampeonesController(RiotDataDragonServicio servicio)
    {
        _servicio = servicio;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Campeon>>> GetTodos()
    {
        var campeones = await _servicio.ObtenerTodosAsync();
        return Ok(campeones);
    }
}