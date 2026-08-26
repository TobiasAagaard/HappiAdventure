using HappiAdventure.Application.Models;
using HappiAdventure.Contract.Activities;
using HappiAdventure.Contract.Enums;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace HappiAdventure.Application.Db;

public static class SeedData
{
    private const int Wgs84 = 4326;

    private static readonly (string Code, string Name)[] ActivitySeeds =
    [
        (ActivityCodes.Date, "Date"),
        (ActivityCodes.FamilyDay, "Family day"),
        (ActivityCodes.PubCrawl, "Pub crawl"),
        (ActivityCodes.FoodTour, "Food tour"),
        (ActivityCodes.TeamBuilding, "Team building"),
        (ActivityCodes.WineTasting, "Wine tasting"),
        (ActivityCodes.BirthdayParty, "Birthday party")
    ];

    private static readonly PlaceSeed[] PlaceSeeds =
    [
        new("Utzon Center",           9.9236, 57.0446, PriceLevel.Moderate,  ActivityCodes.Date, ActivityCodes.FamilyDay),
        new("Musikkens Hus",          9.9302, 57.0525, PriceLevel.Expensive, ActivityCodes.Date),
        new("Nordkraft",              9.9310, 57.0489, PriceLevel.Cheap,     ActivityCodes.Date, ActivityCodes.FamilyDay, ActivityCodes.FoodTour, ActivityCodes.TeamBuilding),
        new("Kunsten Museum",         9.8977, 57.0400, PriceLevel.Moderate,  ActivityCodes.Date, ActivityCodes.FamilyDay),
        new("Aalborg Zoo",            9.8930, 57.0290, PriceLevel.Expensive, ActivityCodes.FamilyDay, ActivityCodes.BirthdayParty),
        new("Aalborgtårnet",          9.9047, 57.0396, PriceLevel.Cheap,     ActivityCodes.Date, ActivityCodes.FamilyDay),
        new("Lindholm Høje",          9.9080, 57.0770, PriceLevel.Cheap,     ActivityCodes.FamilyDay, ActivityCodes.TeamBuilding),
        new("Budolfi Kirke",          9.9195, 57.0479, PriceLevel.Free,      ActivityCodes.Date),
        new("Karolinelund",           9.9330, 57.0480, PriceLevel.Free,      ActivityCodes.FamilyDay, ActivityCodes.Date, ActivityCodes.BirthdayParty),
        new("Kildeparken",            9.9143, 57.0455, PriceLevel.Free,      ActivityCodes.FamilyDay, ActivityCodes.Date),
        new("Vestre Fjordpark",       9.8830, 57.0538, PriceLevel.Free,      ActivityCodes.FamilyDay, ActivityCodes.TeamBuilding, ActivityCodes.BirthdayParty),
        new("Aalborg Havnebad",       9.9280, 57.0510, PriceLevel.Free,      ActivityCodes.FamilyDay),
        new("Springeren",             9.9310, 57.0575, PriceLevel.Moderate,  ActivityCodes.FamilyDay, ActivityCodes.BirthdayParty),
        new("Søgaards Bryghus",       9.9165, 57.0483, PriceLevel.Moderate,  ActivityCodes.PubCrawl, ActivityCodes.Date, ActivityCodes.FoodTour, ActivityCodes.TeamBuilding),
        new("Studenterhuset",         9.9207, 57.0473, PriceLevel.Cheap,     ActivityCodes.PubCrawl, ActivityCodes.BirthdayParty),
        new("Jomfru Ane Gade",        9.9186, 57.0503, PriceLevel.Moderate,  ActivityCodes.PubCrawl, ActivityCodes.BirthdayParty),
        new("Skråen",                 9.9312, 57.0492, PriceLevel.Moderate,  ActivityCodes.PubCrawl),
        new("Penny Lane",             9.9200, 57.0475, PriceLevel.Moderate,  ActivityCodes.Date, ActivityCodes.FoodTour),
        new("Mortens Kro",            9.9179, 57.0476, PriceLevel.Expensive, ActivityCodes.Date, ActivityCodes.FoodTour, ActivityCodes.WineTasting),
        new("Street Food Aalborg",    9.9298, 57.0492, PriceLevel.Cheap,     ActivityCodes.FoodTour, ActivityCodes.FamilyDay, ActivityCodes.Date, ActivityCodes.BirthdayParty),
        new("Fusion Restaurant",      9.9198, 57.0470, PriceLevel.Expensive, ActivityCodes.Date, ActivityCodes.FoodTour, ActivityCodes.WineTasting),
        new("Vinstuen Aalborg",       9.9190, 57.0490, PriceLevel.Moderate,  ActivityCodes.WineTasting, ActivityCodes.Date),
        new("Havnefronten Promenade", 9.9270, 57.0500, PriceLevel.Free,      ActivityCodes.Date, ActivityCodes.FamilyDay)
    ];

    /// <summary>
    /// Adds any seed activity or place that is not in the database yet. Safe to run on every startup:
    /// rows are matched on their natural keys (activity code, place name) and existing rows are left alone.
    /// </summary>
    public static async Task EnsureSeededAsync(HappiAdventureDbContext db, CancellationToken ct = default)
    {
        var activities = await EnsureActivitiesAsync(db, ct);
        await EnsurePlacesAsync(db, activities, ct);
    }

    private static async Task<Dictionary<string, Activity>> EnsureActivitiesAsync(
        HappiAdventureDbContext db, CancellationToken ct)
    {
        var activities = await db.Activities.ToDictionaryAsync(a => a.Code, ct);

        foreach (var (code, name) in ActivitySeeds)
        {
            if (activities.ContainsKey(code)) continue;

            var activity = new Activity { Code = code, Name = name };
            db.Activities.Add(activity);
            activities[code] = activity;
        }

        if (db.ChangeTracker.HasChanges())
            await db.SaveChangesAsync(ct);

        return activities;
    }

    private static async Task EnsurePlacesAsync(
        HappiAdventureDbContext db, IReadOnlyDictionary<string, Activity> activities, CancellationToken ct)
    {
        var existingNames = (await db.Places.Select(p => p.Name).ToListAsync(ct)).ToHashSet();

        var missing = PlaceSeeds
            .Where(seed => !existingNames.Contains(seed.Name))
            .Select(seed => seed.ToPlace(activities))
            .ToList();

        if (missing.Count == 0) return;

        db.Places.AddRange(missing);
        await db.SaveChangesAsync(ct);
    }

    private sealed record PlaceSeed(
        string Name,
        double Longitude,
        double Latitude,
        PriceLevel PriceLevel,
        params string[] ActivityCodes)
    {
        public Place ToPlace(IReadOnlyDictionary<string, Activity> activities) => new()
        {
            Name = Name,
            // NetTopologySuite stores longitude on X and latitude on Y.
            Location = new Point(Longitude, Latitude) { SRID = Wgs84 },
            PriceLevel = PriceLevel,
            Activities = [.. ActivityCodes.Select(code => activities[code])]
        };
    }
}
