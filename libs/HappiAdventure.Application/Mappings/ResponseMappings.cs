using HappiAdventure.Application.Models;
using HappiAdventure.Contract.Response;

namespace HappiAdventure.Application.Mappings;
public static class ResponseMappings
{
    public static PlaceResponse ToResponse(this Place place) => new()
    {
        Id = place.Id,
        Name = place.Name,
        // NetTopologySuite stores longitude on X and latitude on Y.
        Location = new LocationResponse
        {
            Latitude = place.Location.Y,
            Longitude = place.Location.X
        },
        PriceLevel = place.PriceLevel
    };

    public static ActivityResponse ToResponse(this Activity activity) => new()
    {
        Id = activity.Id,
        Code = activity.Code,
        Name = activity.Name
    };
}
