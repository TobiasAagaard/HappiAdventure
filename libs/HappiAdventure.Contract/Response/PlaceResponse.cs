using HappiAdventure.Contract.Enums;

namespace HappiAdventure.Contract.Response;

public class PlaceResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required LocationResponse Location { get; init; }
    public required PriceLevel PriceLevel { get; init; }
}
