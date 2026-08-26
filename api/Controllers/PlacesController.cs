using HappiAdventure.Application.Mappings;
using HappiAdventure.Application.Db;
using HappiAdventure.Contract.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappiAdventure.Api.Controllers;

[ApiController]
[Route("api/places")]
public class PlacesController(HappiAdventureDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlaceResponse>>> GetPlaces([FromQuery] string? activity, CancellationToken ct)
    {
        var query = db.Places.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(activity))
        {
            query = query.Where(p => p.Activities.Any(a => a.Code == activity));
        }

        var places = await query
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

        return Ok(places.Select(p => p.ToResponse()).ToList());
    }
}