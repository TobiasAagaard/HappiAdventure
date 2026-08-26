using NetTopologySuite.Geometries;
using HappiAdventure.Contract.Enums;

namespace HappiAdventure.Application.Models;

public class Place
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Point Location { get; set; }

    public PriceLevel PriceLevel { get; set; }
    public ICollection<Activity> Activities { get; set; } = [];
}
