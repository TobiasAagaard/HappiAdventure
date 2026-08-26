using HappiAdventure.Application.Mappings;
using HappiAdventure.Application.Db;
using HappiAdventure.Contract.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappiAdventure.Api.Controllers;

[ApiController]
[Route("api/activities")]
public class ActivitiesController(HappiAdventureDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ActivityResponse>>> GetActivities(CancellationToken ct)
    {
        var activities = await db.Activities
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .ToListAsync(ct);

        return Ok(activities.Select(a => a.ToResponse()).ToList());
    }
}