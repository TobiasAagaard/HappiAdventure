namespace HappiAdventure.Contract.Response;

public record GetActivitiesResponse(IReadOnlyList<ActivityResponse> Activities);
