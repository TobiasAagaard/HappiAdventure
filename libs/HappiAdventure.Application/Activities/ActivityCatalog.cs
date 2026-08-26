using HappiAdventure.Contract.Activities;
using HappiAdventure.Contract.Response;

namespace HappiAdventure.Application.Activities;

public static class ActivityCatalog
{
    public static readonly IReadOnlyList<ActivityResponse> All =
    [
        new ActivityResponse(ActivityCodes.Date, "Date"),
        new ActivityResponse(ActivityCodes.FamilyDay, "Family Day"),
        new ActivityResponse(ActivityCodes.PubCrawl, "Pub Crawl"),
        new ActivityResponse(ActivityCodes.TeamBuilding, "Team Building"),
        new ActivityResponse(ActivityCodes.WineTasting, "Wine Tasting"),
        new ActivityResponse(ActivityCodes.FoodTour, "Food Tour"),
        new ActivityResponse(ActivityCodes.BirthdayParty, "Birthday Party")
    ];
}
