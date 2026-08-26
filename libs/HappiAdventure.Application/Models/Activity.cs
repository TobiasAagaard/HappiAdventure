namespace HappiAdventure.Application.Models;

public class Activity
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public ICollection<Place> Places { get; set; } = [];
}
